using dhll.CodeGen;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;

namespace dhll.Emitters;


// ==============================================================================================================================
internal class CreateDOMEmitter
{
  private NamingContext NamingContext = null!;
  private DynamicFunctionsGroup DynamicFunctions = null!;
  private PropChangeTargets PropTargets = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  public CreateDOMEmitter()
  {
    NamingContext = new NamingContext();
    DynamicFunctions = new DynamicFunctionsGroup(NamingContext);
    PropTargets = new PropChangeTargets();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public string GetCreateDOMFunction(TemplateDefinition def, string outputDir)
  {
    PreProcessDynamicContent(def.DOM);

    CodeFile cf = new CodeFile();

    // Walk the tree and create elements + children as we go....
    Node root = def.DOM;

    cf.WriteLine($"function CreateDOM(): HTMLElement ");
    cf.OpenBlock();

    cf.WriteLine($"let {root.Symbol} = document.createElement('{root.Name}');");

    AddAttributes(cf, root, root.Symbol!);

    // Now we need to populate the child elements....
    CreateChildElements(cf, root, NamingContext);

    cf.NextLine(2);
    cf.WriteLine("return node;");

    cf.CloseBlock();


    // Now spit out all of the formatting function defs....
    DynamicFunctions.EmitFunctionDefs(cf);


    FileTools.CreateDirectory(outputDir);
    string path = Path.Combine(outputDir, "test-output.ts");
    cf.Save(path);

    string res = cf.GetString();
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Find all dynamic content, and create functions, etc. for them.
  /// </summary>
  private void PreProcessDynamicContent(Node node)
  {
    PreProcessNode(node);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void PreProcessNode(Node node)
  {
    bool isTextNode = node.Name == "<text>";

    node.Symbol = isTextNode ? null : NamingContext.GetUniqueNameFor("node");

    if (node.Name == "<text>" && node.DynamicContent != null)
    {
      // NOTE: This check should probably happen elsewhere....
      if (node.Parent == null) { 
        throw new InvalidOperationException("<text> nodes must have a parent!");
      }

      string funcName = DynamicFunctions.AddDynamicFunction(node.DynamicContent);
      node.DynamicFunction = funcName;

      // We need to make note that this is a target...
      // So if every content function is unique, then we can make a 1:1 association with a DOM element....
      // soo....  --> elem.innerText = contentFunc();
      // Every time we set a value on the associated list of properties, then we need to call this func....
      // --> We already have a unique name for the DOM element as it is created....
      // So then each function is associated with a unique target....
      PropTargets.AddPropChangeTarget(funcName, node.Parent!, node.DynamicContent);
    }

    foreach (var attr in node.Attributes)
    {
      if (attr.DynamicContent != null)
      {
        string funcName = DynamicFunctions.AddDynamicFunction(attr.DynamicContent);
        attr.DynamicFunction = funcName;

        PropTargets.AddPropChangeTarget(funcName, node, attr.DynamicContent, attr);
      }
    }

    foreach (var child in node.Children)
    {
      PreProcessNode(child);
    }
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

      cf.WriteLine($"{parentSymbol}.setAttribute('{item.Name}', {useValue});");
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
          cf.WriteLine($"{parent.Symbol}.insertAdjacentText('beforeend', {useValue});");
        }

        continue;
      }
      else
      {
        cf.NextLine(2);
        cf.WriteLine($"let {item.Symbol} = document.createElement('{item.Name}');");

        // Attributes.
        AddAttributes(cf, item, item.Symbol);

        // Now its child elements too....
        CreateChildElements(cf, item, nameContext);

        // Add the child node to the parent....
        cf.WriteLine($"{parent.Symbol}.append({item.Symbol});");
      }

    }

    // throw new NotImplementedException();
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

