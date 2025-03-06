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

      string inputPath = GetTestInputPath("Template1.dhlt");

      TemplateDefinition[] defs = compiler.ParseTemplates(inputPath);
      Assert.That(defs.Length, Is.EqualTo(1), "There should be one template def!");

      // TODO: We can expand upon this test by adding more checks for DOM structure, attributes, etc.
      // Assert.True(false, "Please finish this test!");




    }

  }
}
