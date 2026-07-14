using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhll
{
  // ==============================================================================================================================
  // REFACTOR: Relocate.
  public class ThrowingErrorListener_Parser : IAntlrErrorListener<IToken>
  {
    public static readonly ThrowingErrorListener_Parser Instance = new ThrowingErrorListener_Parser();

    // --------------------------------------------------------------------------------------------------------------------------
    public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
      throw new Exception($"Syntax error at line {line}:{charPositionInLine} - {msg}");
    }
  }
  // ==============================================================================================================================
  // REFACTOR: Relocate.
  public class ThrowingErrorListener_Lexer : IAntlrErrorListener<int>
  {
    public static readonly ThrowingErrorListener_Lexer Instance = new ThrowingErrorListener_Lexer();

    // --------------------------------------------------------------------------------------------------------------------------
    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
      throw new Exception($"Syntax error at line {line}:{charPositionInLine} - {msg}");
    }

  }
}
