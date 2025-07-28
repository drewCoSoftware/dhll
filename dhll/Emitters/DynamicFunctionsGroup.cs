using dhll.CodeGen;
using dhll.Grammars.v1;
using drewCo.Tools;
using System.Text.RegularExpressions;

namespace dhll.Emitters;

// ==============================================================================================================================
/// <summary>
/// Tracks the dynamic functions, names + their associated property names.
/// </summary>
internal class DynamicFunctionsGroup
{
  private Dictionary<string, List<FunctionDef>> PropsToFunctions = new Dictionary<string, List<FunctionDef>>();
  private List<FunctionDef> UniqueFunctions = new List<FunctionDef>();

  private object DataLock = new object();

  private NamingContext NamingContext = null!;
  private EmitterBase CodeEmitter = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  public DynamicFunctionsGroup(NamingContext namingContext_, EmitterBase codeEmitter_)
  {
    NamingContext = namingContext_;
    CodeEmitter = codeEmitter_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Create internal internal entries based on the dynamic content....
  /// Returns the name of the generated function!
  /// </summary>
  [Obsolete("This has been moved to the TemplateEmitter.")]
  public string AddDynamicContentFunction(ChildContent childContent)
  {
    if (childContent.DynamicContent == null)
    {
      throw new InvalidOperationException("no dynamic content is listed!");
    }

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

      // NOTE: We should just be composing some dhll constructs here, and emitting them later...
      // For now we will just use a functor....
      string func = GenerateComputeStringFunction(childContent);
      funcDef.Body.Add(func);

      // Now we will associate the dynamic function with all of the implicated identifiers(properties).
      foreach (var item in childContent.DynamicContent.Identifiers)
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
  /// <summary>
  /// Returns a function that will create + return a string from the given dynamic content...
  /// </summary>
  /// <param name="dc"></param>
  /// <returns></returns>
  [Obsolete("This has been moved to the TemplateEmitter.")]
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
        // HACK: We are shoving it in parens assuming that more complex expressions will be supported later.
        string expressionText = RenderExpression(x);
        useParts.Add($"(this.{x.Expression})");
      }
      else
      {
        string p = x.Value;
        if (REMOVE_NEWLINES)
        {
          p = StripNewlines(p);
        }
        if (REMOVE_EXCESS_WHITESPACE)
        {
          p = Regex.Replace(p, "[ ]*", " ");
          p = StringTools.Quote(p);
        }
        if (p != string.Empty)
        {
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
  [Obsolete("This has been moved to the TemplateEmitter.")]
  private string RenderExpression(Node x)
  {
   // if (!x.IsExpressionNode) { 
   //   throw new InvalidOperationException("This is not an expression node!");
   // }

   // this.CodeEmitter.EmitExpression(x.Expression, cf);
   //// this.EmitFunctionDefs

    throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  [Obsolete("Use drewco.tools.stringtools version > 1.3.3.6!")]
  public static string StripNewlines(string input)
  {
    string res = input.Replace("\r", "").Replace("\n", "");
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal void EmitFunctionDefs(CodeFile cf, EmitterBase emitter)
  {
    emitter.EmitFunctionDefs(UniqueFunctions, cf);
  }
}



