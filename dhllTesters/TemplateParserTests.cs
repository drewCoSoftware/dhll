using dhll;
using dhll.Expressions;
using dhll.Grammars.v1;
using drewCo.Web;

namespace dhllTesters
{

  // ==============================================================================================================================
  public class TemplateParserTests : TestBase
  {
    // --------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// This test case was added to show that we can parse one or more expressions in tag content.
    /// </summary>
    [Test]
    public void CanParseExpressionsFromContent()
    {
      var compiler = new dhllCompiler();
      string input = """
        <template for="x">
          <div>
            This is some text but I want to have {dynamic} sections in that text, and even {computed + values}.
          </div>
        </template>
      """;

      var defs = compiler.ParseTemplates(input);
      Assert.That(defs.Length, Is.EqualTo(1), "There should be one template def!");

      var d = defs[0];
      var dom = d.DOM;

      // We should have three text, and two expression nodes interleaved:
      // text - exp - text - exp - text
      Assert.That(dom.Children.Count, Is.EqualTo(5));

      var c1=  dom.Children[0];
      var c2 = dom.Children[1];
      var c3 = dom.Children[2];
      var c4 = dom.Children[3];
      var c5 = dom.Children[4];

      Assert.IsTrue(c1.Name == HTMLNode.TEXT_NAME);
      Assert.IsTrue(c2.Name == HTMLNode.EXPRESSION_NAME);
      Assert.IsTrue(c3.Name == HTMLNode.TEXT_NAME);
      Assert.IsTrue(c4.Name == HTMLNode.EXPRESSION_NAME);
      Assert.IsTrue(c5.Name == HTMLNode.TEXT_NAME);

      int x = 10;
    }



    // --------------------------------------------------------------------------------------------------------------------
    [Test]
    public void CanParseExpressionsInAttributes()
    {
      var compiler = new dhllCompiler();
      string input = """
                <template for="X">
                    <div a="string_val" style={x + 123 + "abc"} class={MyClass}>ABC</div>
                </template>
            """;

      var defs = compiler.ParseTemplates(input);
      Assert.That(defs.Length, Is.EqualTo(1), "There should be one template def!");


      var d = defs[0];
      var c = d.DOM;

      // Make sure we have attributes, and that their values are expressions.
      Assert.That(c.Attributes.Count, Is.EqualTo(3), "There should be three attributes");


      var attr1 = c.Attributes[0];
      var v1 = attr1.Value;
      Assert.That(attr1.Value.Type, Is.EqualTo(EAttrValType.String));

      var attr2 = c.Attributes[1];
      var v2 = attr2.Value;
      Assert.That(attr2.Value.Type, Is.EqualTo(EAttrValType.Expression), "This attribute should be an expression!");

      {
        // Just show that we parsed this out ok....
        var b = attr2.Value.Expression as BinaryExpression;
        Assert.IsNotNull(b);

        var left = b.Left as BinaryExpression;
        Assert.IsNotNull(left);
        Assert.That(b.OperatorType, Is.EqualTo(EOperator.Add));

        var right = b.Right as PrimaryExpression;
        Assert.IsNotNull(right);
      }

      var attr3 = c.Attributes[2];
      var v3 = attr3.Value;
      Assert.That(attr3.Value.Type, Is.EqualTo(EAttrValType.Expression), "This attribute should be an expression!");
      {
        var p = attr2.Value.Expression as BinaryExpression;
        Assert.IsNotNull(p);
      }


      // TODO: Add more cases as we go to make sure that everything is how we expect....
      // Assert.Fail("Please finish this test! --> dig in an check out the attribute for a proper expression tree.");
    }

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// This test case shows that templates with more than one 'root' will not be parsed!
    /// </summary>
    [Test]
    public void TemplateCanOnlyHaveOneRoot()
    {
      var compiler = new dhllCompiler();
      string inputPath = GetTestInputPath("MultiRootTemplate.dhlt");

      Assert.Throws<TemplateParseException>(() =>
      {
        TemplateDefinition[] defs = compiler.ParseTemplatesFromFile(inputPath);
      }, $"A {nameof(TemplateParseException)} should have been thrown!");
    }

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Show that we can parse out a basic template, and that the features are working how we might
    /// expect them too.
    /// </summary>
    [Test]
    public void CanParseBasicTemplate()
    {
      var compiler = new dhllCompiler();
      string inputPath = GetTestInputPath("Template1.dhlt");

      TemplateDefinition[] defs = compiler.ParseTemplatesFromFile(inputPath);
      Assert.That(defs.Length, Is.EqualTo(1), "There should be one template def!");

      // TODO: We can expand upon this test by adding more checks for DOM structure, attributes, etc.
      // Assert.True(false, "Please finish this test!");
      // var def = defs[0];
      // Assert.That(def.DOM.Children.Count, Is.EqualTo(2), "There should two DOM children!");
    }


    // --------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// This test case was provided to solve a bug where empty content of the child element for template would crash the parser.
    /// </summary>
    [Test]
    public void CanParseTemplateWithEmptyContent()
    {
      var compiler = new dhllCompiler();
      string input = """
                <template for="X">
                    <div></div>
                </template>
            """;

      var defs = compiler.ParseTemplates(input);
      Assert.That(defs.Length, Is.EqualTo(1), "There should be one template def!");

      // If we didn't crash, we are good to go!

    }

  }
}
