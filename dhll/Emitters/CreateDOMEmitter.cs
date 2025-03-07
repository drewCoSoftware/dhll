using CommandLine;
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

  // --------------------------------------------------------------------------------------------------------------------------
  public CreateDOMEmitter()
  {
    NamingContext = new NamingContext();
    DynamicFunctions = new DynamicFunctionsGroup(NamingContext);
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

    //string nodeSymbol = NamingContext.GetUniqueNameFor("node");
    //root.Symbol = nodeSymbol;

    cf.WriteLine($"let {root.Symbol} = document.createElement('{root.Name}');");

    AddAttributes(cf, root, root.Symbol);

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
      string funcName = DynamicFunctions.AddDynamicFunction(node.DynamicContent);
      node.DynamicFunction = funcName;

      // We need to make note that this is a target...
      // So if every content function is unique, then we can make a 1:1 association with a DOM element....
      // soo....  --> elem.innerText = contentFunc();
      // Every time we set a value on the associated list of properties, then we need to call this func....
      // --> We already have a unique name for the DOM element as it is created....
    }

    foreach (var attr in node.Attributes)
    {
      if (attr.DynamicContent != null)
      {
        string funcName = DynamicFunctions.AddDynamicFunction(attr.DynamicContent);
        attr.DynamicFunction = funcName;
      }
    }

    foreach (var child in node.Children)
    {
      PreProcessNode(child);
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void AddAttributes(CodeFile cf, Node toNode, string nodeSymbol)
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

      cf.WriteLine($"{nodeSymbol}.setAttribute('{item.Name}', {useValue});");
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

  // --------------------------------------------------------------------------------------------------------------------------
  public EmitterResults Emit(string outputDir, dhllFile file)
  {
    throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public string TranslateTypeName(string typeName)
  {
    throw new NotImplementedException();
  }

}


// ==============================================================================================================================
/// <summary>
/// Helps keep track of names, etc. so that we can create unique symbols while generating code.
/// </summary>
internal class NamingContext
{

  private Dictionary<string, int> NamesToCounts = new Dictionary<string, int>();
  private object DataLock = new object();

  // --------------------------------------------------------------------------------------------------------------------------
  public string GetUniqueNameFor(string symbol)
  {

    lock (DataLock)
    {
      int count = 0;
      if (NamesToCounts.TryGetValue(symbol, out count))
      {
        count++;
      }
      NamesToCounts[symbol] = count;

      if (count == 0)
      {
        return symbol;
      }

      string res = $"{symbol}{count}";
      return res;
    }

  }
}

// ==============================================================================================================================
/// <summary>
/// Tracks our dynamic functions, names + their associated property names.
/// </summary>
internal class DynamicFunctionsGroup
{
  private Dictionary<string, List<FunctionDef>> PropsToFunctions = new Dictionary<string, List<FunctionDef>>();
  private List<FunctionDef> UniqueFunctions = new List<FunctionDef>();

  private object DataLock = new object();

  private NamingContext NamingContext = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  public DynamicFunctionsGroup(NamingContext namingContext_)
  {
    NamingContext = namingContext_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Create internal internal entries based on the dynamic content....
  /// Returns the name of the generated function!
  /// </summary>
  public string AddDynamicFunction(DynamicContent dc)
  {
    lock (DataLock)
    {
      string name = NamingContext.GetUniqueNameFor("getValue");

      // We need to create the function definition....
      // Simple function def, that takes zero arguments...
      var funcDef = new FunctionDef()
      {
        Name = name,
        ReturnType = "string"
      };

      funcDef.Body.Add(ComputeStringFunction(dc));

      // Now we will associate the dynamic function with all of the implicated properties.
      foreach (var item in dc.PropertyNames)
      {
        if (!PropsToFunctions.TryGetValue(item, out var funcs))
        {
          funcs = new List<FunctionDef>();
          PropsToFunctions[item] = funcs;
        }

        funcs.Add(funcDef);
      }

      UniqueFunctions.Add(funcDef);

      return name;
    }

  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string ComputeStringFunction(DynamicContent dc)
  {
    // Maybe a way to do the dynamic strings?
    // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals

    string joined = string.Join(" + ", from x in dc.Parts
                                       select x.IsExpession ? $"({x.Value})" : $"\"{x.Value}\"");

    string res = $"return {joined};";
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal void EmitFunctionDefs(CodeFile cf)
  {
    cf.NextLine(2);
    foreach (var def in UniqueFunctions)
    {
      // TODO: We should have a typescript emitter squirt all of this out.
      // We just don't have a fully functional DHLL implementation at this time to make that work.
      string scope = TypescriptEmitter.GetScopeWord(def.Scope);
      cf.WriteLine($"{scope} function {def.Name}(): string ", 0);
      cf.OpenBlock(false);
      foreach (var item in def.Body)
      {
        cf.WriteLine(item);
      }
      cf.CloseBlock();
      cf.NextLine(2);
    }

    //// TEST:
    //foreach (var item in PropsToFunctions.Keys)
    //{
    //  cf.WriteLine($"PROP: {item}");
    //}
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



