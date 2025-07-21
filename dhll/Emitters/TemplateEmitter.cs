using Antlr4.Runtime.Tree;
using dhll.CodeGen;
using dhll.Grammars.v1;
using drewCo.Tools;
using System.ComponentModel.DataAnnotations;



namespace dhll.Emitters;


// ==============================================================================================================================
// REFACTOR:  This may need a base class as well, but it should certainly be called 'TypescriptTemplateEmitter!'
// NOTE: The create DOM functions might have to create some kind of dhll construct which then get transpiled...
internal class TemplateEmitter
{
  const string DEFAULT_VAL_ID = "val";
  const string DEFAULT_TEXT_NODE_ID = "textNode";

  private TemplateDynamics Dynamics = null!;

  private NamingContext NamingContext = new NamingContext();
  private CompilerContext Context = null!;

  /// <summary>
  /// Name of the type that this template represents.
  /// </summary>
  private string TypeIdentifier = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  // This version is mostly meant for test cases....
  internal TemplateEmitter() { }

  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateEmitter(string typeIdentifier_, TemplateDynamics dynamics_, CompilerContext context_)
  {
    TypeIdentifier = typeIdentifier_;
    Dynamics = dynamics_;
    Context = context_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  // HACK: I don't really have a way to emit templates to different language targets at this time, so this will have to do.
  public void EmitCreateDOMFunctionForCSharp(CodeFile cf)
  {

    const string TEMPLATE_TYPE = "string";
    cf.WriteLine($"public {TEMPLATE_TYPE} CreateDOM()");
    cf.OpenBlock(true);

    // We are going to use the 'HTMLNode' syntax since I know that code already works.

    const string ROOT_NAME = "root";

    Node root = Dynamics.DOM;
    cf.WriteLine($"var {ROOT_NAME} = new HTMLNode(\"{root.Name}\");");
    AddAttributesForCsharp(cf, root, ROOT_NAME);

    CreateChildElementsForCsharp(cf, root, ROOT_NAME);

    cf.WriteLine($"{TEMPLATE_TYPE} res = {ROOT_NAME}.ToHTMLString();");


    cf.WriteLine("return res;");

    cf.CloseBlock();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void CreateChildElementsForCsharp(CodeFile cf, Node parent, string parentVarId)
  {
    foreach (var item in parent.Children)
    {
      if (item.IsTextNode)
      {

        string? useText = FormatText(item.Value);
        string? valLine = !string.IsNullOrWhiteSpace(useText) ? $"\"{useText}\"" : null;


        if (item.DynamicContent != null)
        {
          string valId = NamingContext.GetUniqueNameFor(DEFAULT_VAL_ID);
          // We need to know all of the variable names....
          // HACK: We are assuming that the dynamic functions are all at class level!
          string funcName = item.DynamicFunction;
          valLine = $"var {valId} = this.{funcName}();";
          cf.WriteLine(valLine);

          string nodeName = NamingContext.GetUniqueNameFor(DEFAULT_TEXT_NODE_ID);
          cf.WriteLine($"var {nodeName} = HTMLNode.CreateTextNode({valId});");
          cf.WriteLine($"{parentVarId}.AddChild({nodeName});");

          continue;
        }

      }
      else
      {
        cf.NextLine();

        string nodeId = NamingContext.GetUniqueNameFor("node");
        cf.WriteLine($"var {nodeId} = new HTMLNode(\"{item.Name}\");");
        cf.WriteLine($"{parentVarId}.AddChild({nodeId});");

        // Attributes.
        AddAttributesForCsharp(cf, item, nodeId);

        // Now its child elements too....
        CreateChildElementsForCsharp(cf, item, nodeId);

        // Add the child node to the parent....
        //cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.append({QualifyIdentifier(item.Identifier)});");
      }

    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void EmitCreateDOMFunctionForTypescript(CodeFile cf)
  {

    // Walk the tree and create elements + children as we go....
    Node root = Dynamics.DOM;

    cf.WriteLine($"CreateDOM(): HTMLElement ");
    cf.OpenBlock();

    // Special name.

    string assignTo = GetTypescriptAssignSyntax(root);
    cf.WriteLine($"{QualifyIdentifier(assignTo)} = document.createElement('{root.Name}');");

    AddAttributes(cf, root, root.Identifier!);

    // Now we need to populate the child elements....
    CreateChildElements(cf, root, NamingContext);

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
  public void EmitBindFunction(CodeFile cf, TemplateDynamics dynamics)
  {
    Node root = Dynamics.DOM;

    const string BIND_ID = "dom";
    cf.Write($"Bind({BIND_ID}:HTMLElement) ");
    cf.OpenBlock();

    cf.WriteLine("// NOTE: A correctly formed DOM for this type is assumed!");

    // Bind all of the nodes with dynamic content.
    var boundNodes = BindNode(root, BIND_ID, cf);
    cf.NextLine(1);

    // Set values for all nodes:
    SetPropertyValues(boundNodes, cf, dynamics);

    cf.CloseBlock(1);


  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void SetPropertyValues(List<Node> boundNodes, CodeFile cf, TemplateDynamics dynamics)
  {

    // NOTE: TemplateDynamics could probably compute the selectors / paths for binding when we first
    // walk the tree looking for dynamics.
    string[] propNames = dynamics.PropTargets.GetNames();
    var targetNodes = dynamics.PropTargets.GetAllTargetNodes();
    foreach (var p in propNames)
    {
      // Get the name plus any options!
      string[] pParts = p.Split(':');
      string useName = pParts[0];

      if (pParts.Length > 1)
      {
        throw new NotSupportedException("Property options in templates are not yet supported!");
      }

      var targets = dynamics.PropTargets.GetTargetsForProperty(p);
      foreach (var t in targets)
      {



        // HACK: This is typescript specific!  We will have to come up with a better way later.
        // Best way is to probably ask the current emitter directly.
        string useId = QualifyIdentifier(t.TargetNode.Identifier);
        string getBy = $"{useId}" + (t.Attr != null ? $".getAttribute('{t.Attr.Name}')"
                                                                      : $".innerText");

        // NOTE: This data should probably be available in 'dynamics.PropTargets'!"
        string propType = Context.TypeIndex.GetMemberDataType(this.TypeIdentifier, useName);

        // Some extra coercion so we produce typesafe code....
        // 'getAttribute' returns (string | null) in typescript scenarios, which can cause errors.
        // HACK: This is typescript specific!  We will have to come up with a better way later.
        // Best way is to probably ask the current emitter directly.
        if (t.Attr != null)
        {
          if (IsNumberType(propType))
          {
            getBy += " ?? \"0\"";
          }
          else if (propType == "string")
          {
            getBy += " ?? \"\"";
          }
        }

        if (propType != "string")
        {
          if (IsNumberType(propType))
          {
            // Cast to number type!
            getBy = $"Number({getBy})";
          }
          else
          {
            throw new NotSupportedException($"There is no supported cast for type: {propType}!");
          }
        }

        cf.WriteLine($"this._{p} = {getBy};");
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
    if (Dynamics.IdentifierIsClassLevel(node.Identifier))
    {
      res.Add(node);
      cf.WriteLine($"{elementId} = <HTMLElement>{bindTo};");
    }

    int index = 0;
    foreach (var c in node.Children)
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
    string res = $"let {node.Identifier}";
    if (Dynamics.IdentifierIsClassLevel(node.Identifier))
    {
      res = $"this.{node.Identifier!}";
    }
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void AddAttributesForCsharp(CodeFile cf, Node srcNode, string parentSymbol)
  {
    foreach (var item in srcNode.Attributes)
    {

      if (item.DynamicContent != null)
      {
        string useValId = NamingContext.GetUniqueNameFor(DEFAULT_VAL_ID);
        string valLine = $"var {useValId} = \"{item.Value}\";";

        // The attribute value is created via expression.
        string funcName = item.DynamicFunction;
        valLine = $"var {useValId} = {funcName}();";

        cf.WriteLine(valLine);
        cf.WriteLine($"{parentSymbol}.SetAttribute(\"{item.Name}\", {useValId});");
      }

    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void AddAttributes(CodeFile cf, Node toNode, string parentSymbol)
  {
    foreach (var item in toNode.Attributes)
    {

      string useValue = FormatValue(item);


      cf.WriteLine($"{QualifyIdentifier(parentSymbol)}.setAttribute('{item.Name}', {useValue});");
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal string FormatValue(Grammars.v1.Attribute item)
  {
    if (item.Value.Type != EAttrValType.String)
    {
      throw new InvalidOperationException("Can't format a non-string attribute value!");
    }
    string useValue = $"'{item.Value.StringVal}'";

    if (item.DynamicContent != null)
    {
      // The attribute value is created via expression.
      string funcName = item.DynamicFunction;
      useValue = $"this.{funcName}()";
    }
    else
    {
      useValue = UnescapeBackslash(useValue);
    }

    return useValue;
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
    foreach (var item in parent.Children)
    {
      if (item.Name == "<text>")
      {
        string? useText = FormatText(item.Value);
        string? useValue = !string.IsNullOrWhiteSpace(useText) ? $"'{useText}'" : null;

        if (item.DynamicContent != null)
        {
          // We need to know all of the variable names....
          // HACK: We are assuming that the dynamic functions are all at class level!
          string funcName = item.DynamicFunction;
          useValue = $"this.{funcName}()";
        }

        if (useValue != null)
        {
          cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.insertAdjacentText('beforeend', {useValue});");
        }

        continue;
      }
      else
      {
        cf.NextLine(2);
        string assignTo = GetTypescriptAssignSyntax(item);
        cf.WriteLine($"{assignTo} = document.createElement('{item.Name}');");

        // Attributes.
        AddAttributes(cf, item, item.Identifier);

        // Now its child elements too....
        CreateChildElements(cf, item, nameContext);

        // Add the child node to the parent....
        cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.append({QualifyIdentifier(item.Identifier)});");
      }

    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Determines which identifiers belong to the class and should be accessed as 'this.xxx'
  /// We use a simple hueristic at this time....
  /// </summary>
  private string QualifyIdentifier(string symbol)
  {
    if (Dynamics.IdentifierIsClassLevel(symbol))
    {
      return $"this.{symbol}";
    }
    return symbol;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string? FormatText(string? value)
  {
    if (string.IsNullOrEmpty(value)) { return value; }

    string res = value.Replace("\r", " ");
    res = res.Replace("\n", " ");

    return res;
  }
}


// ==============================================================================================================================
internal class FunctionDef
{
  public EScope Scope { get; set; } = EScope.Default;
  public string Identifier { get; set; }
  public string ReturnType { get; set; }

  // TODO: Function args...

  public List<string> Body = new List<string>();
}

