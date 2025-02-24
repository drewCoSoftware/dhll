using CommandLine;

namespace dhll
{

  // ==============================================================================================================================
  [Verb("compile")]
  public class CommandLineOptions
  {

    [Option("to", Required = true, HelpText = "Language to compile to.")]
    public string OutputLang { get; set; } = default!;


    [Option("file", Required = true, HelpText = "Path to file to compile.")]
    public string InputFile { get; set; } = default!;
  }

}
