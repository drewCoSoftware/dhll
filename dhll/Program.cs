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
      int res = CmdParser.Default.ParseArguments<CompileFileOptions, CompileProjectOptions>(args)
                                 .MapResult((CompileFileOptions ops) => Compile(ops),
                                           (CompileProjectOptions ops) => CompileProject(ops),
                                 errs => -1);
      return res;

    }

    // --------------------------------------------------------------------------------------------------------------------------
    private static int Compile(CompileFileOptions ops)
    {
      InitLogger();
      try
      {
        var compiler = new dhllCompiler(ops);
        int res = compiler.CompileProject();
        return res;
      }
      catch (Exception ex)
      {
        Log.Exception(ex);
#if DEBUG
        throw;
#endif
        return -1;
      }
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private static int CompileProject(CompileProjectOptions ops)
    {
      InitLogger();
      try
      {
        var compiler = new dhllCompiler(ops);
        int res = compiler.CompileProject();
        return res;
      }
      catch (Exception ex)
      {
        Log.Exception(ex);
#if DEBUG
        throw;
#endif
        return -1;
      }
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private static void InitLogger() {
    
      var consoleLogger = new ConsoleLogger();
      Log.AddLogger(consoleLogger);

      var levels = new[] { ELogLevel.EXCEPTION.ToString() };
      var ops = new FileLoggerOptions(levels, "./logs", "runlog", "./logs/exceptions", EFileLoggerMode.Overwrite);
      Log.AddLogger(new FileLogger(ops));
    }

  }
}
