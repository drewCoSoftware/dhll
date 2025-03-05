using dhll;
using dhll.Grammars.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhllTesters
{

  // ==============================================================================================================================
  public class TemplateParserTests : TestBase
  {

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Show that we can parse out a basic template, and that the features are working how we might
    /// expect them too.
    /// </summary>
    [Test]
    public void CanParseBasicTemplate()
    {
      var compiler = new dhllCompiler(null);

      string inputPath = GetTestInputPath("Template1.dhll");
      TemplateDefinition[] defs = compiler.CompileTemplates(inputPath);

      Assert.That(defs.Length, Is.EqualTo(1), "There should be one template def!");

      Assert.True(false, "Please finish this test!");
    }

  }
}
