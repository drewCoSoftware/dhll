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
      int res = CmdParser.Default.ParseArguments(args).MapResult<CommandLineOptions, int>((CommandLineOptions ops) => Compile(ops), errs => -1);
      return res;

      //Console.WriteLine("Hello, World!");


      //string input = "typedef MyType { bool x = false; int y = 10; float z; }";

      //AntlrInputStream s = new AntlrInputStream(input);
      //var lexer = new TypeDefLexer(s);

      //var ts = new CommonTokenStream(lexer);
      //var parser = new TypeDefParser(ts);

      //var context = parser.typedef();
      //var v = new TypeDefVisitorImpl();

      //// NOTE: We can also leave all of the typedefs inside of our visitor class......
      //var td = (TypeDef)v.Visit(context);

      //Console.WriteLine($"typedef: {td.Identifier}");
      //if (td.Declarations.Count > 0)
      //{
      //  Console.WriteLine("The member names are:");
      //  foreach (var item in td.Declarations)
      //  {
      //    Console.WriteLine(item.Identifier);
      //  }
      //}
      //else
      //{
      //  Console.WriteLine("There are no declarations in this typedef!");
      //}
      //Console.WriteLine("All done!");

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
