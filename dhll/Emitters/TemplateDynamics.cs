using dhll.CodeGen;
using dhll.Grammars.v1;
using dhll.Emitters;

namespace dhll;

// ==============================================================================================================================
// TODO: Think of a better name for this class.
/// <summary>
/// Kepps track of all of the dynamic functions, property targets, etc. for a TemplateDefinition.
/// </summary>
internal class TemplateDynamics
{
  public const string ROOT_NODE_SYMBOL = "_Root";

  private NamingContext NamingContext = null!;
  private DynamicFunctionsGroup DynamicFunctions = null!;
  private PropChangeTargets PropTargets = null!;

  private TemplateDefinition Def = null!;

  public Node DOM { get { return Def.DOM; } }

  /// <summary>
  /// The set of all symbols that we have identified that are at class level.
  /// This is used so that we can keep persistent references to nodes that have dynamic content.
  /// </summary>
  private HashSet<string> ClassLevelNodeSymbols = null!;

  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateDynamics(TemplateDefinition def_)
  {
    Def = def_;

    NamingContext = new NamingContext();
    DynamicFunctions = new DynamicFunctionsGroup(NamingContext);
    PropTargets = new PropChangeTargets();

    Def.DOM.Symbol = NamingContext.GetUniqueNameFor(ROOT_NODE_SYMBOL);

    PreProcessNodes();

    ClassLevelNodeSymbols = PropTargets.GetAllTargetNodeSymbols(new[] { ROOT_NODE_SYMBOL }).ToHashSet();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void PreProcessNodes()
  {
    SetDynamicContent(Def.DOM);
    SetNodeSymbols(Def.DOM);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public bool SymbolIsClassLevel(string symbol)
  {
    return ClassLevelNodeSymbols.Contains(symbol);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void EmitFunctionDefs(CodeFile cf)
  {
    DynamicFunctions.EmitFunctionDefs(cf);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// After dynamic content on nodes has been detected, this will assign each node a unique symbol.
  /// These symbols are used to indicate which nodes are 'class-level' in which case we will keep a
  /// persistent reference to them.
  /// </summary>
  private void SetNodeSymbols(Node node)
  {
    bool isTextNode = node.Name == "<text>";

    if (node.Symbol == null && !isTextNode)
    {

      // NOTE: In a future version we could probably use some kind of 'hinting' system to have
      // more meaningful names for the nodes.
      string baseName = PropTargets.HasNode(node) ? "_Node" : "node";
      node.Symbol = NamingContext.GetUniqueNameFor(baseName);
    }

    foreach (var child in node.Children)
    {
      SetNodeSymbols(child);
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void SetDynamicContent(Node node)
  {
    bool isTextNode = node.Name == "<text>";

    if (isTextNode && node.DynamicContent != null)
    {
      // NOTE: This check should probably happen elsewhere....
      if (node.Parent == null)
      {
        throw new InvalidOperationException("<text> nodes must have a parent!");
      }

      string funcName = DynamicFunctions.AddDynamicFunction(node.DynamicContent);
      node.DynamicFunction = funcName;

      // We need to make note that this is a target...
      // So if every content function is unique, then we can make a 1:1 association with a DOM element....
      // soo....  --> elem.innerText = contentFunc();
      // Every time we set a value on the associated list of properties, then we need to call this func....
      // --> We already have a unique name for the DOM element as it is created....
      // So then each function is associated with a unique target....
      PropTargets.AddPropChangeTarget(funcName, node.Parent!, node.DynamicContent);
    }

    foreach (var attr in node.Attributes)
    {
      if (attr.DynamicContent != null)
      {
        string funcName = DynamicFunctions.AddDynamicFunction(attr.DynamicContent);
        attr.DynamicFunction = funcName;

        PropTargets.AddPropChangeTarget(funcName, node, attr.DynamicContent, attr);
      }
    }

    foreach (var child in node.Children)
    {
      SetDynamicContent(child);
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Emit declarations for DOM related elements into the given CodeFile instance.
  /// </summary>
  internal void EmitDOMDeclarations(CodeFile cf)
  {
    // HACK: We are assuming that we are outputting to typescript syntax!

    cf.NextLine();
    cf.WriteLine("// ---- DOM Elements ------");

    foreach (var s in ClassLevelNodeSymbols)
    {
      cf.WriteLine($"{s}: HTMLElement;");
    }

    cf.NextLine(1);
  }
}

