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
  /// <summary>
  /// All of the property names (identifiers) that appear in the expression.
  /// </summary>
  /// REFACTOR: Rename to 'identifiers' or something similar.
  public List<string> Identifiers { get; set; } = new List<string>();
}

