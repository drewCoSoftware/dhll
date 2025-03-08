using dhll.CodeGen;
using dhll.Grammars.v1;

namespace dhll.Emitters;

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
        Identifier = name,
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


    // HACK:
    // We are assuming that all expressions are single, class level variables!
    string joined = string.Join(" + ", from x in dc.Parts
                                       select x.IsExpession ? $"(this.{x.Value})" : $"\"{StripNewlines(x.Value)}\"");

    string res = $"return {joined};";
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private object StripNewlines(string input)
  {
    string res = input.Replace("\r", "").Replace("\n", "");
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
      cf.WriteLine($"{scope} {def.Identifier}(): string ", 0);
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



