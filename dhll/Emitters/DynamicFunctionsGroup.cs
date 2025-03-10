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
    // TODO: Some kind of option for how we want to handle leading / trailing whitespace in these
    // functions.  Since we are just targeting typescript for now, we are going to remove it all.

    const bool REMOVE_WHITESPACE = true;
    const bool REMOVE_NEWLINES = true;

    var useParts = new List<string>();
    foreach (var x in dc.Parts)
    {
      if (x.IsExpession) { 
        // HACK: We are shoving it in parens assuming that more complex expressions will be supported later.
        useParts.Add($"(this.{x.Value})");
      }
      else {
        string p = x.Value;
        if (REMOVE_NEWLINES) {
          p = StripNewlines(p);
        }
        if (REMOVE_WHITESPACE) { 
          p = p.Trim();
        }
        if (p != string.Empty) { 
          useParts.Add(p);
        }
      }
    }

    string joined = string.Join(" + ", useParts);

    // HACK: We are assuming that all return types are strings + doing a forced string coersion.
    string res = $"return ({joined}).toString();";

    // NOTE: We could certainly add some code to clean up the expressions a bit, but for the time being
    // we are just working with raw strings so.......
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private string StripNewlines(string input)
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



