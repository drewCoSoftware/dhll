using dhll.CodeGen;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;
using System.Diagnostics.Contracts;

namespace dhll.Emitters;

// ==============================================================================================================================
internal class TemplateEmitter
{
  private TemplateDynamics Dynamics = null!;

  private NamingContext NamingContext = new NamingContext();

  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateEmitter(TemplateDynamics dynamics_)
  {
    Dynamics = dynamics_;
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

    AddAttributes(cf, root, root.Symbol!);

    // Now we need to populate the child elements....
    CreateChildElements(cf, root, NamingContext);

    cf.NextLine(2);
    cf.WriteLine($"return {QualifyIdentifier(root.Symbol!)};");

    cf.CloseBlock();


    // Now spit out all of the formatting function defs....
    // Dynamics.EmitFunctionDefs(cf);


    //FileTools.CreateDirectory(outputDir);
    //string path = Path.Combine(outputDir, "test-output.ts");
    //cf.Save(path);

    //string res = cf.GetString();
    //return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string GetAssignSyntax(Node node)
  {
    string res = $"let {node.Symbol}";
    if (node.HasDynamicContent || node.Symbol == TemplateDynamics.ROOT_NODE_NAME)
    {
      res = $"this.{node.Symbol!}";
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
        useValue = $"{funcName}()";
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
          string funcName = item.DynamicFunction;
          useValue = $"{funcName}()";
        }

        if (useValue != null)
        {
          cf.WriteLine($"{QualifyIdentifier(parent.Symbol)}.insertAdjacentText('beforeend', {useValue});");
        }

        continue;
      }
      else
      {
        cf.NextLine(2);
        string assignTo = GetAssignSyntax(item);
        cf.WriteLine($"{assignTo} = document.createElement('{item.Name}');");

        // Attributes.
        AddAttributes(cf, item, item.Symbol);

        // Now its child elements too....
        CreateChildElements(cf, item, nameContext);

        // Add the child node to the parent....
        cf.WriteLine($"{QualifyIdentifier(parent.Symbol)}.append({QualifyIdentifier(item.Symbol)});");
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
    if (symbol.StartsWith("_"))
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
  public string Name { get; set; }
  public string ReturnType { get; set; }

  // TODO: Function args...

  public List<string> Body = new List<string>();
}

