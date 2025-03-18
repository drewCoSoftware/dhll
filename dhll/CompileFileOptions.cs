using CommandLine;

namespace dhll
{

  // ==============================================================================================================================
  // NOTE: There is a bug in 'CommandLineParser' that won't let us use the verb 'compile-project'
  // As the verb 'compile' is detected first....
  [Verb("compile-project", HelpText = "Compile a dhll project.")]
  public class CompileProjectOptions
  {
    [Option("file", Required = false, HelpText = "Path to project file to compile.")]
    public string? InputFile { get; set; } = null;

    // TODO: Support for logging options?
  }

  // ==============================================================================================================================
  [Verb("compile", HelpText = "Compile a single dhll file.")]
  public class CompileFileOptions
  {
    const string DEFAULT_OUTPUT_DIR = "Output";

    [Option("file", Required = true, HelpText = "Path to .dhll file to compile.")]
    public string InputFile { get; set; } = default!;

    [Option("to", Required = true, HelpText = "Comma delimited list of language(s) to compile to.")]
    public string TargetLanguage { get; set; } = default!;

    [Option("outputdir", Required = false, HelpText = $"Directory to output compiled files to.  Defaults to: {DEFAULT_OUTPUT_DIR}")]
    public string OutputDir { get; set; } = DEFAULT_OUTPUT_DIR;

    // TODO: Support for logging options?
  }


}
