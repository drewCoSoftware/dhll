using dhll;
using drewCo.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhllTesters
{
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
      // We will create + compile a quick project...
      // TODO: We can probably wrap this up into a single function that will generate a test project file for us.....
      dhllProjectDefinition pd = new dhllProjectDefinition();
      pd.InputFiles.Add("EscapedPropString.dhll");
      pd.InputFiles.Add("EscapedPropString.dhlt");
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

      var ops = new CompileProjectOptions()
      {
        InputFile = projPath,
      };

      var c = new dhllCompiler(ops);
      int cRes = c.CompileProject();
      Assert.That(0, Is.EqualTo(cRes), "Invalid return code!");



      throw new NotSupportedException("we need to update this test to show that any generated template code properly renders the string literals!");
    }

  }
}
