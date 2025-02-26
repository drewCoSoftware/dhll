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
    public virtual void WriteLine(string data, int breakCount = 1)
    {
      // HACK:  We are doing this because we have block output (if/then, etc.) that needs to write many lines with
      // formatting.  So we shouldn't have the 'emit' functions returning strings, they should all be writing directly to
      // a stream.  The test cases will have to be overhauled somewhat to support this.  We should do this sooner rather than later!
      if (data == null) { return; }

      SB.Append(TabFill);
      SB.Append(data);

      NextLine(breakCount);
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
    public void NextLine(int breakCount = 1)
    {
      for (int i = 0; i < breakCount; i++)
      {
        SB.Append(NewLine);
      }
    }



    // --------------------------------------------------------------------------------------------------------------------------
    public void Write(string text)
    {
      SB.Append(TabFill);
      SB.Append(text);
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public virtual void OpenBlock(bool breakBeforeBrace = false)
    {
      if (breakBeforeBrace)
      {
        NextLine();
      }
      SB.Append("{");

      TabDepth += 1;

      NextLine();
    }

    // --------------------------------------------------------------------------------------------------------------------------
    /// <param name="endline">Should a line ending character be included ?</param>
    public virtual void CloseBlock(int breakCount = 0)
    {
      TabDepth -= 1;

      SB.Append(TabFill);
      SB.Append("}");


      NextLine(breakCount);
    }


  }
}
