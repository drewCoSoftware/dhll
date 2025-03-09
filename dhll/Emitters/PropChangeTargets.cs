using dhll.Grammars.v1;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace dhll.Emitters;

// ==============================================================================================================================
internal class PropChangeTargets
{

  /// <summary>
  /// Key = Property Name
  /// Value = All nodes that contain dynamic content that the property provides.
  /// </summary>
  private Dictionary<string, List<PropTargetInfo>> PropsToTargets = new Dictionary<string, List<PropTargetInfo>>();
  private object DataLock = new object();


  // --------------------------------------------------------------------------------------------------------------------------
  public PropTargetInfo[]? GetTargetsForProperty(string propertyName)
  {
    if (PropsToTargets.TryGetValue(propertyName, out List<PropTargetInfo> targets))
    {
      return targets.ToArray();
    }
    return null;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public string[] GetAllTargetNodeIdentifiers(IList<string>? extraSymbols = null)
  {
    var allNodes = GetAllTargetNodes();
    var res = (from x in allNodes select x.Identifier).ToList();

    if (extraSymbols != null)
    {
      foreach (var s in extraSymbols)
      {
        if (res.Any(x => x == s)) { continue; }
        res.Insert(0, s);
      }
    }

    return res.ToArray();
  }
  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns an array of all nodes that are marked as targets of property changes.
  /// </summary>
  /// <param name="extraNodesBySymbol">A set of additional node names that you wish to include.</param>
  public Node[] GetAllTargetNodes()
  {
    var res = new List<Node>();

    foreach (var item in PropsToTargets)
    {
      res.AddRange((from x in item.Value select x.TargetNode).DistinctBy(x => x));
    }

    return res.ToArray();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Adds an entry to the propery change table.  This is how we assign the correct content to the
  /// node (text) or the named attribute if any.  When the associated property value is set, we can
  /// update the DOM accordingly.
  /// </summary>
  internal void AddPropChangeTarget(string funcName, Node node, DynamicContent dynamicContent, Grammars.v1.Attribute? attr = null)
  {
    if (dynamicContent == null)
    {
      throw new InvalidOperationException("PropChange targets can't be set if there is no dynamic content!");
    }

    lock (DataLock)
    {
      foreach (var item in dynamicContent.PropertyNames)
      {
        if (!PropsToTargets.TryGetValue(item, out var targets))
        {
          targets = new List<PropTargetInfo>();
          PropsToTargets.Add(item, targets);
        }
        targets.Add(new PropTargetInfo()
        {
          FunctionName = funcName,
          Attr = attr,
          TargetNode = node
        });
      }
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns a flag indicating that the given node is targeted by at least one property.
  /// </summary>
  internal bool HasNode(Node node)
  {
    foreach (var item in PropsToTargets.Values)
    {
      foreach (var n in item)
      {
        if (n.TargetNode == node) { return true; }
      }
    }
    return false;
  }
}

/// <summary>
/// Tells us more about property targets and their interactions.
/// </summary>
internal class PropTargetInfo
{
  public Node TargetNode { get; set; } = default!;

  /// <summary>
  /// Name of the function that generates the dynamic content.
  /// </summary>
  public string FunctionName { get; set; } = default!;

  /// <summary>
  /// If the dynamic content targets an attribute.
  /// </summary>
  public Grammars.v1.Attribute? Attr { get; set; }
}


