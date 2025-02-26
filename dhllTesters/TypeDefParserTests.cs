
using drewCo.Tools;
using dhll.v1;
using Antlr4.Runtime;
using static dhll.v1.dhllParser;

namespace dhllTesters
{
  // ==============================================================================================================================
  public class TestBase
  {
    protected string TestDir = null!;

    // --------------------------------------------------------------------------------------------------------------------------
    public TestBase()
    {
      TestDir = FileTools.GetLocalDir("TestInputs");
    }


    // --------------------------------------------------------------------------------------------------------------------------
    protected string LoadTestInput(string name)
    {
      if (!name.EndsWith(".dhll")) { name += ".dhll"; }
      string inputPath = Path.Combine(TestDir, name);

      if (!File.Exists(inputPath))
      {
        throw new FileNotFoundException($"The input file at path: {inputPath} does not exist!");
      }

      string data = File.ReadAllText(inputPath);
      return data;
    }
  }

  // ==============================================================================================================================
  public class Tests : TestBase
  {


    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Shows that we can parse all kinds of typedefs.
    /// </summary>
    [Test]
    public void CanParseTypeDefs()
    {
      string input = LoadTestInput("BasicTypeDef");

      var context =  GetTypeDefContext(input);
      var v = new dhllVisitorImpl();
      var td = (TypeDef)v.Visit(context);

      Assert.That(td.Identifier, Is.EqualTo("TypeDef1"));
      Assert.That(td.Declarations.Count, Is.EqualTo(1));
      Assert.That(td.Declarations[0].Identifier, Is.EqualTo("x"));
      Assert.That(td.Declarations[0].InitValue, Is.EqualTo("10"));
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