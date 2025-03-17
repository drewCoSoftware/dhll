
using Antlr4.Runtime;
using dhll.Emitters;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;
using drewCo.Tools.Logging;
using System.Linq.Expressions;
using static dhll.v1.dhllParser;
using static dhll.v1.templateParser;


namespace dhll;

// ==============================================================================================================================
public class dhllCompiler
{
  /// <summary>
  /// Extension for DHLL code files.
  /// </summary>
  public const string DHLL_EXT = ".dhll";

  /// <summary>
  /// Extension for DHLL template files.
  /// </summary>
  public const string DHLT_EXT = ".dhlt";

  /// <summary>
  /// Extensions for DHLL project.  Projects are lists of files + options.
  /// </summary>
  public const string DHLPROJ_EXT = ".dhlproj";

  private static string[] SupportedLanguages = new[] {
      "typescript"
  };

  private CommandLineOptions Options;

  private CompilerContext Context;

  private Logger Logger { get { return Context.Logger; } }


  // --------------------------------------------------------------------------------------------------------------------------
  public dhllCompiler(CommandLineOptions ops_)
  {
    Options = ops_;

    Context = new CompilerContext()
    {
      Logger = new Logger(),
      TypeIndex = new TypeIndex(),
      TemplateIndex = new TemplateIndex(),
    };
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public int Compile()
  {
    ValidateOptions();

    if (Options.InputFile.EndsWith(DHLPROJ_EXT))
    {
      CompileDhllProject(Options.InputFile);
    }
    else if (Options.InputFile.EndsWith(DHLL_EXT))
    {
      EmitterBase emitter = CreateEmitter();
      ParseAndCompileDhllFile(Options.InputFile, emitter);
    }
    else
    {
      Logger.Error($"Invalid file type!  Valid extensions are: {Environment.NewLine}{string.Join(Environment.NewLine, new[] { DHLL_EXT, DHLPROJ_EXT })}");
      return -1;
    }

    return 0;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void CompileDhllProject(string projectFilePath)
  {
    Logger.Info("loading project file...");
    var projFile = dhlprojFile.Load(projectFilePath);

    Logger.Info("Compiling dhll Project.");
    Logger.Info("Cleaning previous output.");
    string outDir = FileTools.GetRootedPath(Options.OutputDir);
    FileTools.CreateDirectory(outDir);
    FileTools.EmptyDirectory(outDir);

    Dictionary<string, List<string>> filesByType = ComputeFileGroups(projFile);

    var codeFiles = filesByType[DHLL_EXT];
    if (codeFiles.Count == 0)
    {
      string msg = $"There must be at least one code file ({DHLL_EXT}) in the project!";
      throw new InvalidOperationException(msg);
    }

    Logger.Verbose("Parsing dhll files...");
    // We need a pre-pass on the code files so that we can determine the type information
    // for the properties that may appear in the generated template code.
    //
    //This is also the kind of pre-pass where we will end up detecting invalid/missing type names, etc.
    // Basically, things that are legal syntax, but wouldn't actually compile...
    List<dhllFile> parsed = ParseAllFiles(codeFiles);

    CreateTypeIndex(parsed);

    var emitter = CreateEmitter();  

    var templateFiles = filesByType[DHLT_EXT];
    ProcessTemplateFiles(templateFiles, emitter);

    // Now that all of the template information is computed, we can find a way to hook this up to the
    // code file emitter....
    foreach (var f in parsed)
    {
      CompileAndEmitFiles(f, emitter);
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void CreateTypeIndex(List<dhllFile> parsed)
  {
    // Now that we have parsed all input files, we can build a type index, which will be
    // used for lookups, later.
    using (var token = Context.TypeIndex.BeginUpdate())
    {
      foreach (var item in parsed)
      {
        foreach (var td in item.TypeDefs)
        {
          Context.TypeIndex.AddOrUpdateType(td);
        }
      }
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private List<dhllFile> ParseAllFiles(List<string> codeFiles)
  {
    var parsed = new List<dhllFile>();
    foreach (var itemPath in codeFiles)
    {
      var f = ParseDhllFile(itemPath);
      parsed.Add(f);
    }

    return parsed;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void ProcessTemplateFiles(List<string> templateFiles, EmitterBase emitter_)
  {
    // NOTE: Not all target languages can have templates, and we should probably validate this, or
    // at least spit out warnings.  At time of writing (3.7.2025) only typescript targets are able
    // to utilize templates.

    if (templateFiles.Count > 0)
    {
      // We want to process each of the template files + associate them with a certain data type.
      // NOTE that we don't want to emit any code at this time as that will happen during the
      // code emitting step of the type definitions into the target language...
      foreach (var path in templateFiles)
      {
        Logger.Info($"Processing template at path: {path}");

        var templateDefs = ParseTemplates(path);

        foreach (var item in templateDefs)
        {
          string useName = item.ForType;

          // NOTE: We can't really support named templates at this time (not practical while prototyping)
          // But maybe later.. I guess that would mean that our typedefs would have to be a bit more involved
          // to render / bind against the different versions....
          // if (item.Name != null) { useName += ("." + item.Name); }

          // NOTE: I don't think that we actually need the emitter here.....
          // It proabably doesn't need to be stored in dynamics, rather it is invoked when we 
          // doing the code emit step....
          var dynamics = new TemplateDynamics(item, emitter_);

          Context.TemplateIndex.Add(useName, dynamics);
        }
      }
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private static Dictionary<string, List<string>> ComputeFileGroups(dhlprojFile projFile)
  {
    // Scan all of the files, and sort them by extension.  We will want to process all of the
    // template files first.
    // Multiple passes may be required to resolve + verify all symbols.
    var filesByType = Partition(projFile.Files, x => Path.GetExtension(x), x => projFile.GetFullPath(x));

    // NOTE: The empty lists are being added as a convenience so that we can have cleaner code downstream.
    var allTypes = new[] { DHLT_EXT, DHLL_EXT };
    foreach (var type in allTypes)
    {
      if (!filesByType.ContainsKey(type))
      {
        filesByType.Add(type, new List<string>());
      }
    }

    return filesByType;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Partitions the list of inputs into a dictionary using a function to derive a key from those inputs.
  /// </summary>
  [Obsolete("Use version from drewco.Tools > 1.3.3.6!")]
  private static Dictionary<TKey, List<TInput>> Partition<TInput, TKey>(List<TInput> input, Func<TInput, TKey> keyGenerator)
  {
    var res = new Dictionary<TKey, List<TInput>>();
    foreach (var item in input)
    {
      TKey useKey = keyGenerator(item);
      if (!res.TryGetValue(useKey, out List<TInput> list))
      {
        list = new List<TInput>();
        res[useKey] = list;
      }

      list.Add(item);
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Partitions the list of values, computed from the inputs, into a dictionary using a function to derive a key from those inputs.
  /// </summary>
  [Obsolete("Use version from drewco.Tools > 1.3.3.6!")]
  private static Dictionary<TKey, List<TValue>> Partition<TInput, TKey, TValue>(List<TInput> input, Func<TInput, TKey> keyGenerator, Func<TInput, TValue> valGenerator)
  {
    var res = new Dictionary<TKey, List<TValue>>();
    foreach (var item in input)
    {
      TKey useKey = keyGenerator(item);
      if (!res.TryGetValue(useKey, out List<TValue> list))
      {
        list = new List<TValue>();
        res[useKey] = list;
      }

      TValue useval = valGenerator(item);
      list.Add(useval);
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Partitions the inputs into lists grouped by property value.
  /// </summary>
  public static Dictionary<TKey, List<TInput>> PartitionIntoDictionary<TInput, TKey>(List<TInput> input, Expression<Func<TInput, TKey>> keyPropExp)
  {
    var res = new Dictionary<TKey, List<TInput>>();

    foreach (var item in input)
    {
      TKey key = (TKey)ReflectionTools.GetPropertyValue(item, keyPropExp);
      if (!res.TryGetValue(key, out var list))
      {
        list = new List<TInput>();
        res[key] = list;
      }
      list.Add(item);
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void ParseAndCompileDhllFile(string inputFile, EmitterBase emitter)
  {
    Logger.Info($"Compiling dhll at path: {inputFile}");

    dhllFile parsed = ParseDhllFile(inputFile);
    CompileAndEmitFiles(parsed, emitter);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void CompileAndEmitFiles(dhllFile file, EmitterBase emitter)
  {
    string outputDir = FileTools.GetLocalDir(Options.OutputDir);
    FileTools.CreateDirectory(outputDir);


    // Run the emitter...
    EmitterResults results = emitter.Emit(outputDir, file);

    LogResults(results);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private dhllFile ParseDhllFile(string path)
  {
    string inputText = File.ReadAllText(path);

    AntlrInputStream s = new AntlrInputStream(inputText);
    var lexer = new dhllLexer(s);

    var ts = new CommonTokenStream(lexer);
    var parser = new dhllParser(ts);

    FileContext context = parser.file();
    var v = new dhllVisitorImpl();

    var dFile = (dhllFile)v.VisitFile(context);
    dFile.Path = path;

    return dFile;
  }


  // --------------------------------------------------------------------------------------------------------------------------
  private void LogResults(EmitterResults results)
  {
    if (results.OutputFiles.Length == 0)
    {
      Logger.Warning("There are no output files!");
    }

    foreach (var item in results.OutputFiles)
    {
      Logger.Info($"-> File: {item}");
    }
  }


  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateDefinition[] ParseTemplates(string inputFilePath)
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
  private EmitterBase CreateEmitter()
  {
    // NOTE: We will use a registration type approach in the future.
    // That will allow for all kinds of plugins + overrides if we wanted.
    switch (Options.OutputLang)
    {
      case "typescript":
        {
          var res = new TypescriptEmitter(this.Context);
          return res;
        }

      case "C#":
        {
          var res = new CSharpEmitter(this.Context);
          return res;
        }

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


// ==============================================================================================================================
internal class CompilerContext
{
  public Logger Logger { get; set; } = default!;
  public TemplateIndex TemplateIndex { get; set; } = default!;
  public TypeIndex TypeIndex { get; set; } = default!;
}