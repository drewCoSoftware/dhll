
using Antlr4.Runtime;
using dhll.v1;
using drewCo.Tools;
using drewCo.Tools.Logging;
using static dhll.v1.TypeDefParser;

namespace dhll
{

  // ==============================================================================================================================
  internal class dhllCompiler
  {
    private static string[] SupportedLanguages = new[] {
      "typescript"
    };

    private CommandLineOptions Options;
    private Logger Logger = null;

    // --------------------------------------------------------------------------------------------------------------------------
    public dhllCompiler(CommandLineOptions ops_)
    {
      Options = ops_;
      Logger = new Logger();
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public int Compile()
    {
      ValidateOptions();

      string input = File.ReadAllText(Options.InputFile);

      AntlrInputStream s = new AntlrInputStream(input);
      var lexer = new TypeDefLexer(s);

      var ts = new CommonTokenStream(lexer);
      var parser = new TypeDefParser(ts);

      TypedefContext context = parser.typedef();
      var v = new TypeDefVisitorImpl();

      var td = (TypeDef)v.Visit(context);

      return 0;
    }


    // --------------------------------------------------------------------------------------------------------------------------
    private void ValidateOptions()
    {
      if (!SupportedLanguages.Contains(Options.OutputLang))
      {
        // TODO: Show valid
        string msg = $"The language: {Options.OutputLang} is not supported!";
        msg += (Environment.NewLine + "Valid Options are:" + Environment.NewLine);
        msg += string.Join(Environment.NewLine, SupportedLanguages);

        throw new InvalidOperationException(msg);
      }

      if (!File.Exists(Options.InputFile)) {
        string msg = $"The input file at path: {Options.InputFile} does not exist!";
        throw new FileNotFoundException(msg);
      }
      Options.InputFile = FileTools.GetRootedPath(Options.InputFile); 

    }


  }


}
