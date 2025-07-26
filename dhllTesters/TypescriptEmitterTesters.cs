using dhll.Emitters;
using dhll.Grammars.v1;

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

      string formatted = te.FormatValue(new dhll.Grammars.v1.Attribute()
      {
        Value = new AttributeValue(TEST_STRING),
      });

      Assert.That(formatted, Is.EqualTo(EXPECTED), "The formatted value is not correct!");
    }

  }
}
