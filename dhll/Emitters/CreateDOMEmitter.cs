using dhll.CodeGen;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhll.Emitters
{

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
      CodeFile cf = new CodeFile();

      // Maybe a way to do the dynamic strings?
      // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals

      // Walk the tree and create elements + children as we go....
      Node root = def.DOM;

      cf.WriteLine($"function CreateDOM(): HTMLElement ");
      cf.OpenBlock();

      string nodeSymbol = NamingContext.GetUniqueNameFor("node");
      root.Symbol = nodeSymbol;

      cf.WriteLine($"let {nodeSymbol} = document.createElement('{root.Name}');");

      AddAttributes(cf, root, nodeSymbol);

      // Now we need to populate the child elements....
      CreateChildElements(cf, root, NamingContext);

      cf.NextLine();
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
    private void AddAttributes(CodeFile cf, Node toNode, string nodeSymbol)
    {
      foreach (var item in toNode.Attributes)
      {
        string useValue = $"'{item.Value}'";

        if (item.DynamicContent != null)
        {
          // The attribute value is created via expression.
          string funcName = DynamicFunctions.AddDynamicFunction(item.DynamicContent);
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
            string funcName = DynamicFunctions.AddDynamicFunction(item.DynamicContent);
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
          string symbol = nameContext.GetUniqueNameFor("child");
          item.Symbol = symbol;

          cf.NextLine();
          cf.WriteLine($"let {symbol} = document.createElement('{item.Name}');");

          // Attributes.
          AddAttributes(cf, item, symbol);

          // Now its child elements too....
          CreateChildElements(cf, item, nameContext);

          // Add the child node to the parent....
          cf.WriteLine($"{parent.Symbol}.append({symbol});");
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


}
