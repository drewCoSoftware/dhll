using Antlr4.Runtime.Tree;
using CommandLine;
using dhll.CodeGen;
using dhll.Emitters;
using dhll.Expressions;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.SymbolStore;
using System.Text.RegularExpressions;



namespace dhll.Emitters;


// ==============================================================================================================================
// REFACTOR:  This may need a base class as well, but it should certainly be called 'TypescriptTemplateEmitter!'
// NOTE: The create DOM functions might have to create some kind of dhll construct which then get transpiled...
internal class TemplateEmitter
{
  const string DEFAULT_VAL_ID = "val";
  const string DEFAULT_TEXT_NODE_ID = "textNode";

  private CompilerContext Context = null!;

  /// <summary>
  /// Name of the type that this template represents.
  /// </summary>
  private string ForType = null!;
  private TypeDef TemplateType = null!;
  private EmitterBase Emitter = null!;


  /// <summary>
  /// All of the dynamic functions + their defs that we have found.
  /// We keep a list of them so we can emit them to the file later.
  /// </summary>
  private Dictionary<string, FunctionDef> DynamicFunctions = new Dictionary<string, FunctionDef>();


  private Node DOM = null!;
  private TemplateInfo TemplateInfo = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  // This version is mostly meant for test cases....
  internal TemplateEmitter() { }

  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateEmitter(TypeDef templateType_, TemplateInfo templateInfo_, CompilerContext context_, EmitterBase emitter_)
  {
    TemplateType = templateType_;
    ForType = TemplateType.Identifier;

    TemplateInfo = templateInfo_;
    DOM = TemplateInfo.DOM;

    Context = context_;

    if (TemplateType == null)
    {
      throw new InvalidOperationException($"There is no typedef for: {this.ForType} in the type index!");
    }

    Emitter = emitter_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  // HACK: I don't really have a way to emit templates to different language targets at this time, so this will have to do.
  public void EmitCreateDOMFunctionForCSharp(CodeFile cf)
  {
    var nameContext = new NamingContext();

    const string TEMPLATE_TYPE = "string";
    cf.WriteLine($"public {TEMPLATE_TYPE} CreateDOM()");
    cf.OpenBlock(true);

    // We are going to use the 'HTMLNode' syntax since I know that code already works.
    const string ROOT_NAME = "root";

    Node root = DOM;
    cf.WriteLine($"var {ROOT_NAME} = new HTMLNode(\"{root.Name}\");");
    AddAttributesForCsharp(cf, root, ROOT_NAME, nameContext);

    CreateChildElementsForCsharp(cf, root, ROOT_NAME, nameContext);

    cf.NextLine();
    cf.WriteLine($"{TEMPLATE_TYPE} res = {ROOT_NAME}.ToHTMLString();");
    cf.WriteLine("return res;");

    cf.CloseBlock();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void CreateChildElementsForCsharp(CodeFile cf, Node parent, string parentNodeName, NamingContext nameContext)
  {
    if (parent.ChildContent == null || parent.ChildContent.Nodes.Count == 0)
    {
      return;
    }

    // Attributes:
    // NOTE: Should we be calling 'AddAttributesForCSharp**' here?
    // NOTE: Should  the code in 'AddAttributesForCSharp' be responsible for this step?
    foreach (var attr in parent.Attributes)
    {
      if (attr.IsExpression)
      {
        string funcName = CreateDynamicContentFunction(attr);
      }
    }

    if (parent.HasDynamicContent)
    {
      // We will have a function that creates the content for the node.
      string funcName = CreateDynamicContentFunction(parent);

      var nodeId = nameContext.GetUniqueNameFor(DEFAULT_TEXT_NODE_ID);
      string valName = nameContext.GetUniqueNameFor(DEFAULT_VAL_ID);

      cf.WriteLine($"string {valName} = {funcName}();");
      cf.WriteLine($"var {nodeId} = HTMLNode.CreateTextNode({valName});");
      cf.WriteLine($"{parentNodeName}.AddChild({nodeId});");
    }
    else
    {
      foreach (var item in parent.ChildContent.Nodes)
      {
        if (item.IsTextNode)
        {
          string? useText = FormatText(item.Value);
          string? useValue = !string.IsNullOrWhiteSpace(useText) ? $"'{useText}'" : null;
          if (useValue == null) { continue; }

          var nodeId = nameContext.GetUniqueNameFor(DEFAULT_TEXT_NODE_ID);
          cf.WriteLine($"var {nodeId} = HTMLNode.CreateTextNode(\"{useText}\");");
        }
        else
        {
          cf.NextLine(2);
          string nodeId = nameContext.GetUniqueNameFor("node");
          cf.WriteLine($"var {nodeId} = new HTMLNode(\"{item.Name}\");");

          // Attributes.
          AddAttributesForCsharp(cf, item, nodeId, nameContext);

          // Now its child elements too....
          CreateChildElementsForCsharp(cf, item, nodeId, nameContext);

          // Add the child node to the parent....
          cf.WriteLine($"{parentNodeName}.AddChild({nodeId});");
        }

      }
    }
  }


  // --------------------------------------------------------------------------------------------------------------------------
  public void EmitCreateDOMFunctionForTypescript(CodeFile cf)
  {
    var nameContext = new NamingContext();

    // Walk the tree and create elements + children as we go....
    Node root = DOM;

    cf.WriteLine($"CreateDOM(): HTMLElement ");
    cf.OpenBlock();

    // Special name.

    string assignTo = GetTypescriptAssignSyntax(root);
    cf.WriteLine($"{assignTo} = document.createElement('{root.Name}');");

    AddAttributes(cf, root, root.Identifier!, nameContext);

    // Now we need to populate the child elements....
    CreateChildElements(cf, root, nameContext);

    cf.NextLine(2);
    cf.WriteLine($"return {QualifyIdentifier(root.Identifier!)};");

    cf.CloseBlock();
    cf.NextLine(2);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// For existing DOM content we want to be able to bind a specific instance.
  /// That means that we need to be able to get our DOM elements that are defined in the type
  /// via selectors....
  /// Since we already have the tree, we can proabbly just use some simple array index syntax...
  /// --> The generated function should be able to warn / throw errors when certain elements can't be found.
  /// --> We also need a way to read the propery values in from the DOM too.  This could get weird for
  /// properties that are computed as we would have to have some kind of inverse value?
  /// Actually, we should enforce some kind of 'data-propname-value' attribute as not all expressions are invertable!
  /// NOTE: Since we are doing a frikkin code generator, why not generate some C# functions that can also create + print + set
  /// data for the binding?
  /// 
  /// NOTE: Do we need to generate another function to find all instances to bind to?  Might be nice...
  /// </summary>
  public void EmitBindFunction(CodeFile cf, TemplateInfo templateInfo)
  {
    Node root = DOM;

    const string BIND_ID = "dom";
    cf.Write($"Bind({BIND_ID}:HTMLElement) ");
    cf.OpenBlock();

    cf.WriteLine("// NOTE: A correctly formed DOM for this type is assumed!");

    // Bind all of the nodes with dynamic content.
    var boundNodes = BindNode(root, BIND_ID, cf);
    cf.NextLine(1);

    // Set values for all nodes:
    SetPropertyValues(boundNodes, cf, templateInfo);

    cf.CloseBlock(1);


  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void SetPropertyValues(List<Node> boundNodes, CodeFile cf, TemplateInfo templateInfo)
  {
    // throw new NotSupportedException();

    // NOTE: TemplateDynamics could probably compute the selectors / paths for binding when we first
    // walk the tree looking for dynamics.
    DynamicContentIndex dci = templateInfo.DynamicContentIndex;
    string[] propNames = dci.IdentifiersToNodes.Keys.ToArray(); //; // PropTargets.GetNames();
    // Node[] targetNodes = dci.IdentifiersToNodes.Values.ToArray(); //   templateInfo.PropTargets.GetAllTargetNodes();

    foreach (var p in propNames)
    {

      // For a given identifer in the class, we want to find all of the dynamic functions
      // that contain it.
      var nodes = dci.IdentifiersToNodes[p];

      // var targets = templateInfo.PropTargets.GetTargetsForProperty(p);
      foreach (var node in nodes)
      {
        // Every node will either have content or attributes that are created from functions.
        // For non-trivial expressions, we may need to come up with some kind of data-* attributes
        // to supoport proper binding...

        var id = QualifyIdentifier(node.Identifier);

        foreach (var attr in node.Attributes)
        {
          if (attr.IsExpression)
          {
            var primary = attr.Value.Expression as PrimaryExpression;
            if (primary == null)
            {
              throw new InvalidOperationException("only primary expressions are supported in binding functions at this time!");
            }
          }
          if (attr.DynamicFunction != null)
          {

            var funcids = attr.DynamicFunction.IdentifiersUsed;
            if (funcids

            int xxxx = 10;
          }
        }



        int x = 10;
        //// HACK: This is typescript specific!  We will have to come up with a better way later.
        //// Best way is to probably ask the current emitter directly.
        //string useId = QualifyIdentifier(t.TargetNode.Identifier);
        //string getBy = $"{useId}" + (t.Attr != null ? $".getAttribute('{t.Attr.Name}')"
        //                                                              : $".innerText");

        //// NOTE: This data should probably be available in 'dynamics.PropTargets'!"
        //string propType = Context.TypeIndex.GetMemberDataType(this.ForType, useName);

        //// Some extra coercion so we produce typesafe code....
        //// 'getAttribute' returns (string | null) in typescript scenarios, which can cause errors.
        //// HACK: This is typescript specific!  We will have to come up with a better way later.
        //// Best way is to probably ask the current emitter directly.
        //if (t.Attr != null)
        //{
        //  if (IsNumberType(propType))
        //  {
        //    getBy += " ?? \"0\"";
        //  }
        //  else if (propType == "string")
        //  {
        //    getBy += " ?? \"\"";
        //  }
        //}

        //if (propType != "string")
        //{
        //  if (IsNumberType(propType))
        //  {
        //    // Cast to number type!
        //    getBy = $"Number({getBy})";
        //  }
        //  else
        //  {
        //    throw new NotSupportedException($"There is no supported cast for type: {propType}!");
        //  }
        //}

        //cf.WriteLine($"this._{p} = {getBy};");
      }
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  // HACK: This is typescript specific!  We will have to come up with a better way later.
  // Best way is to probably ask the current emitter directly.
  private bool IsNumberType(string propType)
  {
    bool res = propType == "int" || propType == "float" || propType == "double";
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Generate code to bind nodes to the DOM.
  /// </summary>
  private List<Node> BindNode(Node node, string bindTo, CodeFile cf)
  {
    var res = new List<Node>();

    string elementId = QualifyIdentifier(node.Identifier);
    if (IsClassMember(node.Identifier))
    {
      res.Add(node);
      cf.WriteLine($"{elementId} = <HTMLElement>{bindTo};");
    }

    int index = 0;
    foreach (var c in node.ChildContent.Nodes)
    {
      if (c.Name == "<text>") { continue; }
      var kids = BindNode(c, bindTo + $".children[{index}]", cf);
      res.AddRange(kids);
      ++index;
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string GetTypescriptAssignSyntax(Node node)
  {
    if (string.IsNullOrWhiteSpace(node.Identifier))
    {
      throw new InvalidOperationException("This node is missing an identifier!");
    }

    string res = $"{QualifyIdentifier(node.Identifier)}";
    if (!res.StartsWith("this"))
    {
      res = "let " + res;
    }
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void AddAttributesForCsharp(CodeFile cf, Node srcNode, string parentSymbol, NamingContext nameContext)
  {
    foreach (var item in srcNode.Attributes)
    {
      if (item.IsExpression)
      {
        string useValId = nameContext.GetUniqueNameFor(DEFAULT_VAL_ID);
        string valLine = $"var {useValId} = \"{item.Value}\";";

        // The attribute value is created via expression.
        string funcName = item.DynamicFunction.Name;
        valLine = $"var {useValId} = {funcName}();";

        cf.WriteLine(valLine);
        cf.WriteLine($"{parentSymbol}.SetAttribute(\"{item.Name}\", {useValId});");
      }
      else
      {
        // Const, string value.
        cf.WriteLine($"{parentSymbol}.SetAttribute(\"{item.Name}\", {item.Value.StringVal});");
      }

    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void AddAttributes(CodeFile cf, Node toNode, string parentSymbol, NamingContext nameContext)
  {

    foreach (var item in toNode.Attributes)
    {
      if (item.IsExpression)
      {
        string useValId = nameContext.GetUniqueNameFor(DEFAULT_VAL_ID);
        //string valLine = $"var {useValId} = \"{item.Value}\";";

        // The attribute value is created via expression.
        // NOTE: We are assuming the qualification of the function:
        // --> In the real world we would have a way to do so.
        string funcName = item.DynamicFunction.Name;
        funcName = "this." + funcName;

        string valLine = $"var {useValId} = {funcName}();";

        cf.WriteLine(valLine);
        cf.WriteLine($"{QualifyIdentifier(parentSymbol)}.setAttribute(\"{item.Name}\", {useValId});");
      }
      else
      {
        cf.WriteLine($"{QualifyIdentifier(parentSymbol)}.setAttribute(\"{item.Name}\", {item.Value.StringVal});");
      }

    }

  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal string FormatValue(Grammars.v1.Attribute item)
  {
    string res = null!;

    if (item.Value.Type == EAttrValType.String)
    {
      res = item.Value.StringVal!;
      return res;
    }

    if (item.IsExpression)
    {
      res = RenderExpression(item.Value.Expression);
      //var exp = item.Value.Expression;
      //var primary = exp as PrimaryExpression;
      //if (primary != null)
      //{
      //  switch (primary.Type)

      //  {
      //    case EPrimaryType.Identifier:
      //      string useName = primary.Content;
      //      res = QualifyIdentifier(primary.Content);
      //      break;

      //    case EPrimaryType.Number:
      //    case EPrimaryType.String:
      //      res = primary.Content;
      //      break;

      //    default:
      //      throw new NotImplementedException();
      //  }
      //}

      // NOTE: In the absence of data type, we will just convert everything to a string:
      // This can be improved upon later.
      res = $"({res}).toString()";

      return res;

    }

    throw new InvalidOperationException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  // TODO: Share in tools lib!
  public static string UnescapeBackslash(string useValue)
  {
    string res = useValue.Replace("\\", "");
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void CreateChildElements(CodeFile cf, Node parent, NamingContext nameContext)
  {
    if (parent.ChildContent == null || parent.ChildContent.Nodes.Count == 0)
    {
      return;
    }

    // Attributes:
    foreach (var attr in parent.Attributes)
    {
      if (attr.IsExpression)
      {
        string funcName = CreateDynamicContentFunction(attr);
      }
    }

    if (parent.HasDynamicContent)
    {
      // We will have a function that creates the content for the node.
      string funcName = CreateDynamicContentFunction(parent);
      cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.insertAdjacentText('beforeend', this.{funcName}());");
    }
    else
    {
      foreach (var item in parent.ChildContent.Nodes)
      {
        if (item.IsTextNode)
        {
          string? useText = FormatText(item.Value);
          string? useValue = !string.IsNullOrWhiteSpace(useText) ? $"'{useText}'" : null;
          if (useValue == null) { continue; }

          cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.insertAdjacentText('beforeend', {useValue});");
        }
        else
        {
          cf.NextLine(2);
          string assignTo = GetTypescriptAssignSyntax(item);
          cf.WriteLine($"{assignTo} = document.createElement('{item.Name}');");

          // Attributes.
          AddAttributes(cf, item, item.Identifier, nameContext);

          // Now its child elements too....
          CreateChildElements(cf, item, nameContext);

          // Add the child node to the parent....
          cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.append({QualifyIdentifier(item.Identifier)});");
        }

      }
    }

  }

  // --------------------------------------------------------------------------------------------------------------------------
  public string CreateDynamicContentFunction(dhll.Grammars.v1.Attribute attr)
  {
    if (!attr.IsExpression)
    {
      throw new InvalidOperationException("attribute is not an expression!");
    }

    string res = attr.DynamicFunction.Name;
    var funcDef = new FunctionDef()
    {
      Identifier = res,
      ReturnType = "string"
    };

    string func = RenderExpression(attr.Value.Expression!);

    // HACK: Extra parens + string coersion to get us over the hump.  we can care about optimizing this later....
    func = "return (" + func + ").toString();";

    funcDef.Body.Add(func);

    DynamicFunctions.Add(res, funcDef);
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Create internal entries based on the dynamic content....
  /// Returns the name of the generated function!
  /// </summary>
  public string CreateDynamicContentFunction(Node node) // ChildContent childContent)
  {
    if (!node.HasDynamicContent)
    {
      throw new InvalidOperationException("no dynamic content is listed!");
    }

    string functionName = node.DynamicFunction.Name; // NamingContext.GetUniqueNameFor("getValue");

    // We need to create the function definition....
    // Simple function def, that takes zero arguments...
    var funcDef = new FunctionDef()
    {
      Identifier = functionName,
      ReturnType = "string"
    };

    // NOTE: We should just be composing some dhll constructs here, and emitting them later...
    // For now we will just use a functor....
    string func = GenerateComputeStringFunction(node.ChildContent);
    funcDef.Body.Add(func);

    // We will emit these all at once, later.
    DynamicFunctions.Add(functionName, funcDef);

    return functionName;

  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string GenerateComputeStringFunction(ChildContent dc)
  {
    // Maybe a way to do the dynamic strings?
    // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals


    // HACK:
    // We are assuming that all expressions are single, class level variables!
    // TODO: Some kind of option for how we want to handle leading / trailing whitespace in these
    // functions.  Since we are just targeting typescript for now, we are going to remove it all.

    const bool REMOVE_EXCESS_WHITESPACE = true;
    const bool REMOVE_NEWLINES = true;

    var useParts = new List<string>();
    foreach (var x in dc.Nodes)
    {
      if (x.IsExpressionNode)
      {
        string expr = RenderExpression(x);
        // HACK: We are shoving it in parens assuming that more complex expressions will be supported later.
        expr = $"({expr})";

        useParts.Add(expr);
      }
      else if (x.IsTextNode)
      {
        string p = x.Value;
        if (REMOVE_NEWLINES)
        {
          p = StringTools.StripNewlines(p);
        }
        if (REMOVE_EXCESS_WHITESPACE)
        {
          p = Regex.Replace(p, "[ ]+", " ");
          p = StringTools.Quote(p);
        }
        if (p != string.Empty)
        {
          useParts.Add(p);
        }
      }
      else
      {
        // I think that we just render this as normal........
        // Actually, I don't think that we will actually get here?
        throw new NotImplementedException();
      }
    }

    string joined = string.Join(" + ", useParts);

    // HACK: We are assuming that all return types are strings + doing a forced string coersion.
    // In the future, we could look at the typedef to decide if this is necessary.
    // We might even be able to figure out some kind of a way to remove excess parens?
    string res = $"return ({joined}).toString();";

    // NOTE: We could certainly add some code to clean up the expressions a bit, but for the time being
    // we are just working with raw strings so.......
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string RenderExpression(Node node)
  {
    if (!node.IsExpressionNode)
    {
      throw new InvalidOperationException("This is not an expression node!");
    }
    return RenderExpression(node.Expression!);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string RenderExpression(Expression expr)
  {
    string res = this.Emitter.RenderExpression(expr!);
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Determines which identifiers belong to the class and should be accessed as 'this.xxx'
  /// We use a simple hueristic at this time....
  /// </summary>
  private string QualifyIdentifier(string symbol)
  {
    if (IsClassMember(symbol))
    {
      return $"this.{symbol}";
    }
    return symbol;

  }

  // --------------------------------------------------------------------------------------------------------------------------
  private bool IsClassMember(string symbol)
  {
    bool res = TemplateType.HasMember(symbol) || IsDOMVariable(symbol);
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private bool IsDOMVariable(string symbol)
  {
    bool res = TemplateInfo.DynamicContentIndex.IsDOMIdentifier(symbol);
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string? FormatText(string? value)
  {
    if (string.IsNullOrEmpty(value)) { return value; }

    string res = value.Replace("\r", " ");
    res = res.Replace("\n", " ");
    res = res.Trim();

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal void EmitDynamicFunctions(CodeFile cf)
  {
    this.Emitter.EmitFunctionDefs(this.DynamicFunctions.Values, cf);

    //foreach (var item in this.DynamicFunctions.Values)
    //{
    //}

    //throw new NotImplementedException();
  }
}



