
using Antlr4.Runtime;
using dhll.Emitters;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;
using drewCo.Tools.Logging;
using static dhll.v1.dhllParser;
using static dhll.v1.templateParser;

namespace dhll;

// ==============================================================================================================================
public class dhllCompiler
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
    var lexer = new dhllLexer(s);

    var ts = new CommonTokenStream(lexer);
    var parser = new dhllParser(ts);

    FileContext context = parser.file();
    var v = new dhllVisitorImpl();

    var dFile = (dhllFile)v.VisitFile(context);
    dFile.Path = Options.InputFile;

    // TODO: Check for parse errors, etc.


    string outputDir = FileTools.GetLocalDir(Options.OutputDir);
    FileTools.CreateDirectory(outputDir);

    // Load the emitter...
    IEmitter emitter = LoadEmitter();

    // Run the emitter...
    EmitterResults results = emitter.Emit(outputDir, dFile);


    return 0;
  }


  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateDefinition[] CompileTemplates(string inputFilePath)
  {

    string input = File.ReadAllText(inputFilePath);

    AntlrInputStream s = new AntlrInputStream(input);
    var lexer = new templateLexer(s);

    var ts = new CommonTokenStream(lexer);
    var parser = new templateParser(ts);

    TemplatesContext context = parser.templates();
    var v = new templatesVisitorImpl();

    v.VisitTemplates(context);

    return v.TemplateDefs.ToArray();

  }

  // --------------------------------------------------------------------------------------------------------------------------
  private IEmitter LoadEmitter()
  {
    // NOTE: We will use a registration type approach in the future.
    // That will allow for all kinds of plugins + overrides if we wanted.
    switch (Options.OutputLang)
    {
      case "typescript":
        var res = new TypescriptEmitter(Logger);
        return res;

      default:
        throw new NotSupportedException();
    }
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

    string usePath = FileTools.GetRootedPath(Options.InputFile);
    if (!File.Exists(usePath))
    {
      string msg = $"The input file at path: {Options.InputFile} does not exist!";
      throw new FileNotFoundException(msg);
    }
    Options.InputFile = FileTools.GetRootedPath(Options.InputFile);

  }
}
