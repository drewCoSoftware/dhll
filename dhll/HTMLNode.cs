using System.Text;

// NOTE: I don't think this namespace is all that great....
namespace drewCo.Web;

// ==============================================================================================================================
/// <summary>
/// Represents a node in an HTML dom.
/// NOTE: Maybe we can finally create some kind of 'tree' abstract class....
/// TODO: This is going to go live in some shared lib somewhere....
/// </summary>
public class HTMLNode
{
  /// <summary>
  /// List of node names that should be rendered as empty tags.
  /// </summary>
  private HashSet<string> EmptyNodeNames = new HashSet<string>(){
    "img"
  };

  public const string TEXT_NAME = "<text>";
  public const string EXPRESSION_NAME = "<expression>";

  public HTMLNode? Parent { get; set; } = null;

  public string Name { get; private set; }

  /// <summary>
  /// This is for text nodes.
  /// </summary>
  public string? InnerText { get; private set; } = null;

  public List<HTMLNode> Children { get; private set; } = new List<HTMLNode>();
  private Dictionary<string, string> Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

  public bool IsTextNode { get { return Name == TEXT_NAME; } }

  // --------------------------------------------------------------------------------------------------------------------------
  public static HTMLNode CreateTextNode(string content)
  {
    var res = new HTMLNode(TEXT_NAME);
    res.InnerText = content;
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public HTMLNode(string name_)
  {
    this.Name = name_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void AddChild(HTMLNode child)
  {
    Children.Add(child);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void SetAttribute(string name, string value)
  {
    Attributes[name] = value;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public string ToHTMLString()
  {
    var sb = new StringBuilder();
    RenderNode(sb);

    return sb.ToString();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void RenderNode(StringBuilder sb, int indentLevel = 0)
  {

    if (indentLevel != 0)
    {
      // NOTE: We don't have any notion of tabs at this time.
      throw new NotSupportedException("Indented formatting is not supported at this time.  Deal with it!");
    }

    sb.Append($"<{this.Name}");
    foreach (var key in Attributes.Keys)
    {
      string val = Attributes[key];
      string useVal = string.IsNullOrWhiteSpace(val) ? string.Empty : $"=\"{val}\"";
      sb.Append($" {key}{val}");
    }

    // NOTE: Some tags are empties....
    bool isEmpty = EmptyNodeNames.Contains(Name);
    if (isEmpty)
    {
      sb.Append(" />");
    }
    else
    {
      sb.Append(">");
    }

    if (!isEmpty)
    {
      if (Children.Count > 0)
      {
        throw new InvalidOperationException("Empty tags should not have any children!");
      }
      foreach (var child in Children)
      {
        RenderNode(sb);
      }

      // Close the tag.
      sb.Append($"</{Name}>");
    }


  }
}

// ==============================================================================================================================
public class HTMLAttribute
{
  // --------------------------------------------------------------------------------------------------------------------------
  public HTMLAttribute(string name_, string value_)
  {
    Name = name_;
    Value = value_;
  }

  public string Name { get; private set; }
  public string Value { get; private set; }
}