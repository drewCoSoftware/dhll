using dhll;
using dhll.Emitters;
using dhll.Grammars.v1;
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
    [Test]
    public void CanCompileBasicTemplate()
    {
      dhllProjectDefinition def = CreateProjectDef(new[] {
        "./TestInputs/Template1.dhlt",
        "./TestInputs/BasicTypeDef.dhll"
      },
      $"./{nameof(CanCompileBasicTemplate)}{dhllCompiler.DHLPROJ_EXT}");


      var compiler = new dhllCompiler(new CompileProjectOptions()
      {
        InputFile = def.Path
      });
      int compRes = compiler.CompileProject();
      Assert.That(compRes, Is.EqualTo(0), "Compilation failed!");

    }



    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Shows that the characters that represent a property string can be escaped for literal values.
    /// i.e. {MyProp} as a literal would be: \{MyProp\}
    /// </summary>
    [Test]
    public void CanCompileTypeAndTemplateWithEscapePropertyStrings()
    {
      Assert.Inconclusive("I'm not sure what I want to do about this test.... I think the best thing is to advise users to just use &lbrace; &rbrace; stuff in their 'HTML' content?");

      var def = CreateProjectDef(new[] {
        "EscapedPropStringdhll",
        "EscapedPropString.dhlt"
      },
      $"./{nameof(CanCompileBasicTemplate)}{dhllCompiler.DHLPROJ_EXT}");

      var ops = new CompileProjectOptions()
      {
        InputFile = def.Path,
      };

      var c = new dhllCompiler(ops);
      int cRes = c.CompileProject();
      Assert.That(0, Is.EqualTo(cRes), "Invalid return code!");

    }


    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a project def that can target C# and typescript.
    /// </summary>
    private dhllProjectDefinition CreateProjectDef(string[] inputFiles, string savetoPath)
    {
      var def = new dhllProjectDefinition()
      {
        InputFiles = inputFiles,
        OutputDir = "./test-output",
        OutputTargets = new Dictionary<string, OutputTarget>() {
          {"typescript", new OutputTarget() {
            Name = "typescript",
            TargetLanguage = "typescript"
          }},
          {"C#", new OutputTarget() {
            Name = "C#",
            TargetLanguage = "C#"
          }}
        }
      };

      string defPath = savetoPath;
      FileTools.SaveJson(defPath, def);

      return def;
    }
  }
}
