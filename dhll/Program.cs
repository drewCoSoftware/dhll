using Antlr4.Runtime;
using CommandLine;
using dhll.v1;
using drewCo.Tools.Logging;
using System.Runtime.CompilerServices;
using CmdParser = CommandLine.Parser;

namespace dhll
{
  // ==============================================================================================================================
  internal class Program
  {

    // --------------------------------------------------------------------------------------------------------------------------
    static int Main(string[] args)
    {
      int res = CmdParser.Default.ParseArguments<CommandLineOptions>(args)
                                 .MapResult((CommandLineOptions ops) => Compile(ops),
                                 errs => -1);
      return res;

    }

    // --------------------------------------------------------------------------------------------------------------------------
    private static int Compile(CommandLineOptions ops)
    {
      var compiler = new dhllCompiler(ops);
      int res = compiler.Compile();
      return res;
    }

  }
}
