using CommandLine;

namespace dhll
{

  // ==============================================================================================================================
  [Verb("compile")]
  public class CommandLineOptions
  {
    const string DEFAULT_OUTPUT_DIR = "Output";

    [Option("file", Required = true, HelpText = "Path to file to compile.  This can be a single .dhll file, or a .dhlproj file.")]
    public string InputFile { get; set; } = default!;

    [Option("to", Required = true, HelpText = "Language to compile to.")]
    public string OutputLang { get; set; } = default!;

    [Option("outputdir", Required = false, HelpText = $"Directory to output compiled files to.  Defaults to: {DEFAULT_OUTPUT_DIR}")]
    public string OutputDir { get; set; } = DEFAULT_OUTPUT_DIR;
  }

}
