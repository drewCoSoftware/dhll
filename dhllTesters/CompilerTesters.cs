using dhll;
using dhll.Emitters;
using drewCo.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhllTesters
{
  // ==============================================================================================================================
  /// <summary>
  /// Some test cases to handle typescript specifc situations.
  /// </summary>
  public class TypescriptEmitterTesters : TestBase
  {
    /// <summary>
    /// Shows that escaped strings are unescaped correctly when being emitted as literals.
    /// This is something that all languages should do by default.
    /// </summary>
    [Test]
    public void CanEmitEscapedStringAsLiteral()
    {
      const string TEST_STRING = @"\{escaped-value\}";
      const string EXPECTED = "'{escaped-value}'";        // Note that it is single quoted + curly braces are escaped!
      var te = new TemplateEmitter();

      string formatted = te.FormatValue(new dhll.Grammars.v1.Attribute() {
       Value = TEST_STRING,
      });

      Assert.That(formatted, Is.EqualTo(EXPECTED), "The formatted value is not correct!");
    }

  }

  // ==============================================================================================================================
  public class CompilerTesters : TestBase
  {

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Shows that the characters that represent a property string can be escaped for literal values.
    /// i.e. {MyProp} as a literal would be: \{MyProp\}
    /// </summary>
    [Test]
    public void CanCompileTypeAndTemplateWithEscapePropertyStrings()
    {

      string projPath = CreateDHLLProjectFromFiles(new[] { "EscapedPropString.dhll", "EscapedPropString.dhlt" });
      var ops = new CompileProjectOptions()
      {
        InputFile = projPath,
      };

      var c = new dhllCompiler(ops);
      int cRes = c.CompileProject();
      Assert.That(0, Is.EqualTo(cRes), "Invalid return code!");



      //throw new NotSupportedException("we need to update this test to show that any generated template code properly renders the string literals!");
    }

    // --------------------------------------------------------------------------------------------------------------------------
    protected string CreateDHLLProjectFromFiles(IList<string> fileNames)
    {
      // We will create + compile a quick project...
      // TODO: We can probably wrap this up into a single function that will generate a test project file for us.....
      dhllProjectDefinition pd = new dhllProjectDefinition();
      foreach (string name in fileNames)
      {
        pd.InputFiles.Add(name);
      }
      pd.OutputTargets.Add("typescript", new OutputTarget()
      {
        Name = "typescript",
        TargetLanguage = "typescript"
      });
      pd.OutputTargets.Add("csharp", new OutputTarget()
      {
        Name = "csharp",
        TargetLanguage = "C#"
      });
      string projPath = Path.Combine(TestDir, nameof(CanCompileTypeAndTemplateWithEscapePropertyStrings) + dhllCompiler.DHLPROJ_EXT);
      FileTools.SaveJson(projPath, pd);

      return projPath;

    }
  }
}
