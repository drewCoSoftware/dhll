using dhll.Grammars.v1;

namespace dhll.Emitters;

// ==============================================================================================================================
internal class PropChangeTargets
{
  /// <summary>
  /// Tells us more about property targets and their interactions.
  /// </summary>
  class PropTargetInfo
  {
    public Node TargetNode { get; set; } = default!;
    public string FunctionName { get; set; } = default!;
    public Grammars.v1.Attribute? Attr { get; set; }
  }

  private Dictionary<string, List<PropTargetInfo>> PropsToTargets = new Dictionary<string, List<PropTargetInfo>>();
  private object DataLock = new object();

  // --------------------------------------------------------------------------------------------------------------------------
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
}



