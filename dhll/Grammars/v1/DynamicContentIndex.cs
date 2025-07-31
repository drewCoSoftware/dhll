
namespace dhll.Grammars.v1;

// ==============================================================================================================================
/// <summary>
/// Describes the dynamic content in a DOM, and makes the relationships clear.
/// NOTE: This is basically V2 of 'template dynamics' but a bit cleaner.
/// </summary>
public class DynamicContentIndex
{
  // --------------------------------------------------------------------------------------------------------------------------
  public DynamicContentIndex(Node dom_)
  {
    DOM = dom_;
  }

  public Node DOM { get; private set; }

  /// <summary>
  /// All identifiers, and the nodes they are attached to.
  /// </summary>
  public Dictionary<string, List<Node>> IdentifiersToNodes { get; set; } = new Dictionary<string, List<Node>>();

  /// <summary>
  /// All identifiers, and each of the functions that they are associated with.
  /// </summary>
  public Dictionary<string, List<DynamicFunctionInfo>> IdentifiersToDynamicFunctions { get; set; } = new Dictionary<string, List<DynamicFunctionInfo>>();


  // --------------------------------------------------------------------------------------------------------------------------
  internal void AddDynamicFunctionData(DynamicFunctionInfo df)
  {
    foreach (var identifier in df.IdentifiersUsed)
    {
      if (!IdentifiersToDynamicFunctions.TryGetValue(identifier, out List<DynamicFunctionInfo>? funcs))
      {
        funcs = new List<DynamicFunctionInfo>();
        IdentifiersToDynamicFunctions.Add(identifier, funcs);
      }
      funcs.Add(df);
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal void AddNodeData(string identifier, Node node)
  {
    if (!IdentifiersToNodes.TryGetValue(identifier, out List<Node>? nodes))
    {
      nodes = new List<Node>();
      IdentifiersToNodes.Add(identifier, nodes);
    }
    nodes.Add(node);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public bool HasIdentifier(string name)
  {
    return IdentifiersToDynamicFunctions.ContainsKey(name);
  }


  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns true if the given name is used as an identifier on a DOM node.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool IsDOMIdentifier(string name)
  {
    return IdentifiersToNodes.ContainsKey(name);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal DynamicFunctionInfo[]? GetDynamicFunctions(string identifier)
  {
    if (IdentifiersToDynamicFunctions.TryGetValue(identifier, out var res))
    {
      return res.ToArray();
    }
    return null;
  }
}
