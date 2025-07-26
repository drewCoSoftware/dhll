using dhll.CodeGen;
using dhll.Grammars.v1;
using dhll.Emitters;
using drewCo.Tools.Logging;

namespace dhll;

// ==============================================================================================================================
// TODO: Think of a better name for this class.
/// <summary>
/// Kepps track of all of the dynamic functions, property targets, etc. for a TemplateDefinition.
/// </summary>
internal class TemplateDynamics
{
  public const string ROOT_NODE_IDENTIFIER = "_Root";

  private NamingContext NamingContext = null!;
  private DynamicFunctionsGroup DynamicFunctions = null!;
  public PropChangeTargets PropTargets { get; private set; } = null!;

  private TemplateDefinition Def = null!;

  // NOTE: We are assuming that our template is represented as an HTML/browser type DOM!
  public Node DOM { get { return Def.DOM; } }

  /// <summary>
  /// The set of all identifiers that we have identified that are at class level.
  /// This is used so that we can keep persistent references to nodes that have dynamic content.
  /// </summary>
  private HashSet<string> ClassLevelNodeIdentifiers = null!;
  private CompilerContext Context = null!;


  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateDynamics(TemplateDefinition def_)
  {

    Def = def_;
    NamingContext = new NamingContext();
    DynamicFunctions = new DynamicFunctionsGroup(NamingContext);
    PropTargets = new PropChangeTargets();

    Def.DOM.Identifier = NamingContext.GetUniqueNameFor(ROOT_NODE_IDENTIFIER);

    PreProcessNodes();

    ClassLevelNodeIdentifiers = PropTargets.GetAllTargetNodeIdentifiers(new[] { ROOT_NODE_IDENTIFIER }).ToHashSet();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void PreProcessNodes()
  {
    SetDynamicContent(Def.DOM);
    SetNodeIdentifiers(Def.DOM);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public bool IdentifierIsClassLevel(string identifier)
  {
    return ClassLevelNodeIdentifiers.Contains(identifier);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void EmitDynamicFunctionDefs(CodeFile cf, EmitterBase emitter)
  {
    DynamicFunctions.EmitFunctionDefs(cf, emitter);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// After dynamic content on nodes has been detected, this will assign each node a unique identifier.
  /// These identifiers are used to indicate which nodes are 'class-level' in which case we will keep a
  /// persistent reference to them.
  /// </summary>
  private void SetNodeIdentifiers(Node node)
  {
    throw new NotImplementedException();

    //bool isTextNode = node.Name == "<text>";

    //if (node.Identifier == null && !isTextNode)
    //{

    //  // NOTE: In a future version we could probably use some kind of 'hinting' system to have
    //  // more meaningful names for the nodes.
    //  string baseName = PropTargets.HasNode(node) ? "_Node" : "node";
    //  node.Identifier = NamingContext.GetUniqueNameFor(baseName);
    //}

    //foreach (var child in node.Children)
    //{
    //  SetNodeIdentifiers(child);
    //}
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void SetDynamicContent(Node node)
  {
    throw new NotImplementedException();

    //bool isTextNode = node.Name == "<text>";

    //if (isTextNode && node.DynamicContent != null)
    //{
    //  // NOTE: This check should probably happen elsewhere....
    //  if (node.Parent == null)
    //  {
    //    throw new InvalidOperationException("<text> nodes must have a parent!");
    //  }

    //  string funcName = DynamicFunctions.AddDynamicFunction(node.DynamicContent);
    //  node.DynamicFunction = funcName;

    //  // We need to make note that this is a target...
    //  // So if every content function is unique, then we can make a 1:1 association with a DOM element....
    //  // soo....  --> elem.innerText = contentFunc();
    //  // Every time we set a value on the associated list of properties, then we need to call this func....
    //  // --> We already have a unique name for the DOM element as it is created....
    //  // So then each function is associated with a unique target....
    //  PropTargets.AddPropChangeTarget(funcName, node.Parent!, node.DynamicContent);
    //}

    //foreach (var attr in node.Attributes)
    //{
    //  if (attr.DynamicContent != null)
    //  {
    //    string funcName = DynamicFunctions.AddDynamicFunction(attr.DynamicContent);
    //    attr.DynamicFunction = funcName;

    //    PropTargets.AddPropChangeTarget(funcName, node, attr.DynamicContent, attr);
    //  }
    //}

    //foreach (var child in node.Children)
    //{
    //  SetDynamicContent(child);
    //}
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

    foreach (var s in ClassLevelNodeIdentifiers)
    {
      cf.WriteLine($"{s}: HTMLElement;");
    }

    cf.NextLine(1);
  }
}

