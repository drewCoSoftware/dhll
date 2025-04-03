using dhll.v1;
using Antlr4.Runtime;
using static dhll.v1.dhllParser;

namespace dhllTesters
{

  // ==============================================================================================================================
  public class ParserTests : TestBase
  {

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Shows that we can parse all kinds of typedefs.
    /// </summary>
    [Test]
    public void CanParseTypeDefs()
    {
      string input = LoadTestInput("BasicTypeDef.dhll");

      var context = GetTypeDefContext(input);
      var v = new dhllVisitorImpl();
      var td = (TypeDef)v.Visit(context);

      Assert.That(td.Identifier, Is.EqualTo("ModalWindow"));
      Assert.That(td.Members.Count, Is.EqualTo(2));
      Assert.That(td.Members[0].Identifier, Is.EqualTo("IsVisible"));
      Assert.That(td.Members[0].InitValue, Is.EqualTo("false"));
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private TypedefContext GetTypeDefContext(string input)
    {
      AntlrInputStream s = new AntlrInputStream(input);
      var lexer = new dhllLexer(s);

      var ts = new CommonTokenStream(lexer);
      var parser = new dhllParser(ts);

      TypedefContext context = parser.typedef();
      return context;
    }

  }


}