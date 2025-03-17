using dhll.CodeGen;
using dhll.Grammars.v1;

namespace dhll.Emitters;

// ==============================================================================================================================
// REFACTOR:  This may need a base class as well, but it should certainly be called 'TypescriptTemplateEmitter!'
internal class TemplateEmitter
{
  private TemplateDynamics Dynamics = null!;

  private NamingContext NamingContext = new NamingContext();
  private CompilerContext Context = null!;

  /// <summary>
  /// Name of the type that this template represents.
  /// </summary>
  private string TypeIdentifier = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateEmitter(string typeIdentifier_, TemplateDynamics dynamics_, CompilerContext context_)
  {
    TypeIdentifier = typeIdentifier_;
    Dynamics = dynamics_;
    Context = context_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void EmitCreateDOMFunction(CodeFile cf)
  {
    // Walk the tree and create elements + children as we go....
    Node root = Dynamics.DOM;

    cf.WriteLine($"CreateDOM(): HTMLElement ");
    cf.OpenBlock();

    // Special name.

    string assignTo = GetAssignSyntax(root);
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
  // throw new NotSupportedException("Convert this to look for data-* type values vs. reading them directly.  Otherwise, we will not be able to support more complex expressions in the future!  (NOTE: simple expressions that are property only will still work, so don't destroy the code outright, we can support both in the future..... actually, only support the data-* type expressions to keep it all consistent.  NOTE: Bindiners will destroy the data-* values on bind!  Errors for incompatible data / types!");

    // NOTE: TemplateDynamics could probably compute the selectors / paths for binding when we first
    // walk the tree looking for dynamics.
    string[] propNames = dynamics.PropTargets.GetNames();
    var targetNodes = dynamics.PropTargets.GetAllTargetNodes();
    foreach (var p in propNames)
    {
      var targets = dynamics.PropTargets.GetTargetsForProperty(p);
      foreach (var t in targets)
      {
        string useId = QualifyIdentifier(t.TargetNode.Identifier);
        string getBy = $"{useId}" + (t.Attr != null ? $".getAttribute('{t.Attr.Name}')"
                                                                      : $".innerText");

        // TODO: We need some way to cast to correct data type here....
        string propType = Context.TypeIndex.GetMemberDataType(this.TypeIdentifier, p);

        // HACK: This is typescript specific!  We will have to come up with a better way later.
        // Also, we should come up with a generalized function to 'cast' to correct type in all cases.
        if (propType != "string")
        {
          if (propType == "int" || propType == "float" || propType == "double")
          {
            // Cast to number type!
            getBy = $"Number({getBy})"; 
          }
          else {
            throw new NotSupportedException($"There is no supported cast for type: {propType}!");
          }
        }

        cf.WriteLine($"this._{p} = {getBy};");
      }
    }
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
  private string GetAssignSyntax(Node node)
  {
    string res = $"let {node.Identifier}";
    if (Dynamics.IdentifierIsClassLevel(node.Identifier))
    {
      res = $"this.{node.Identifier!}";
    }
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void AddAttributes(CodeFile cf, Node toNode, string parentSymbol)
  {
    foreach (var item in toNode.Attributes)
    {
      string useValue = $"'{item.Value}'";

      if (item.DynamicContent != null)
      {
        // The attribute value is created via expression.
        string funcName = item.DynamicFunction;
        useValue = $"this.{funcName}()";
      }

      cf.WriteLine($"{QualifyIdentifier(parentSymbol)}.setAttribute('{item.Name}', {useValue});");
    }
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
        string assignTo = GetAssignSyntax(item);
        cf.WriteLine($"{assignTo} = document.createElement('{item.Name}');");

        // Attributes.
        AddAttributes(cf, item, item.Identifier);

        // Now its child elements too....
        CreateChildElements(cf, item, nameContext);

        // Add the child node to the parent....
        cf.WriteLine($"{QualifyIdentifier(parent.Identifier)}.append({QualifyIdentifier(item.Identifier)});");
      }

    }

    // throw new NotImplementedException();
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

