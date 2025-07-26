namespace dhll;


// ==============================================================================================================================
public class FormatPart
{
    public string Value { get; set; }
    public bool IsExpession { get; set; }
}

// ==============================================================================================================================
public class DynamicContent
{
  // Some way (function) to represent the interpolated string....
  public List<FormatPart> Parts { get; set; }

  /// <summary>
  /// All of the property names (identifiers) that appear in the expression.
  /// </summary>
  /// REFACTOR: Rename to 'identifiers' or something similar.
  public List<string> PropertyNames { get; set; } = new List<string>();
}

