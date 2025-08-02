using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace dhll.CodeGen
{

  // ============================================================================================================================
  /// <summary>
  /// Lovingly copied from another project, and modified to fit in this one.
  /// It is used to make it easier to write code to a stream in a nicely formatted way.
  /// </summary>
  public class CodeFile
  {
    private StringBuilder SB = new StringBuilder();

    /// <summary>
    /// The current tab depth.
    /// </summary>
    private int _TabDepth = 0;
    private int TabDepth
    {
      get { return _TabDepth; }
      set
      {
        _TabDepth = value;
        _TabFill = null;
      }
    }

    // OPTIONS:
    private char TabChar = ' ';
    private int TabCharCount = 2;
    private string NewLine = Environment.NewLine;

    private string? _TabFill = null;
    private string TabFill
    {
      get { return _TabFill ?? (_TabFill = new string(TabChar, TabDepth * TabCharCount)); }
    }
    // --------------------------------------------------------------------------------------------------------------------------
    public CodeFile()
    { }

    // --------------------------------------------------------------------------------------------------------------------------
    public void Save(string path)
    {
      string toWrite = SB.ToString();
      File.WriteAllText(path, toWrite);
    }


    // --------------------------------------------------------------------------------------------------------------------------
    public virtual CodeFile WriteLine(string data, int breakCount = 1)
    {
      // HACK:  We are doing this because we have block output (if/then, etc.) that needs to write many lines with
      // formatting.  So we shouldn't have the 'emit' functions returning strings, they should all be writing directly to
      // a stream.  The test cases will have to be overhauled somewhat to support this.  We should do this sooner rather than later!
      if (data != null)
      {
        SB.Append(TabFill);
        SB.Append(data);

        NextLine(breakCount);
      }
      return this;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private string AddTabFill(string data)
    {
      return TabFill + data + Environment.NewLine;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Add extra line breaks to the current output stream.
    /// </summary>
    public CodeFile NextLine(int breakCount = 1)
    {
      for (int i = 0; i < breakCount; i++)
      {
        SB.Append(NewLine);
      }

      return this;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public CodeFile Write(string text)
    {
      SB.Append(TabFill);
      SB.Append(text);

      return this;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public virtual CodeFile OpenBlock(bool breakBeforeBrace = false)
    {
      if (breakBeforeBrace)
      {
        if (!IsOnNewline())
        {
          NextLine();
        }
        SB.Append(TabFill);
      }
      else
      {
        RemoveNewline();
      }

      SB.Append("{");

      TabDepth += 1;

      NextLine();

      return this;
    }


    // --------------------------------------------------------------------------------------------------------------------------
    /// <param name="endline">Should a line ending character be included ?</param>
    public virtual CodeFile CloseBlock(int breakCount = 0)
    {
      TabDepth -= 1;

      SB.Append(TabFill);
      SB.Append("}");

      NextLine(breakCount);

      return this;
    }


    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Tells us if the last character(s) in the string builder represent a newline.
    /// </summary>
    /// <returns></returns>
    private bool IsOnNewline()
    {
      bool res = SB[SB.Length - 1] == '\n';
      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// If the current buffer ends with a newline, this code will remove it.
    /// Remove multiple newlines by calling this function again.
    /// Returns a boolean value indicating how many newlines were removed.
    /// </summary>
    private bool RemoveNewline()
    {
      int toRemove = 0;
      if (SB[SB.Length - 1] == '\n')
      {
        ++toRemove;
        if (SB[SB.Length - 2] == '\r')
        {
          ++toRemove;
        }
      }

      if (toRemove > 0)
      {
        SB.Remove(SB.Length - toRemove, toRemove);
        return true;
      }
      return false;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    internal string GetString()
    {
      string res = SB.ToString();
      return res;
    }
  }
}
