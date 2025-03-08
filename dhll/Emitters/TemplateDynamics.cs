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
  public const string ROOT_NODE_NAME = "_Root";

  private NamingContext NamingContext = null!;
  private DynamicFunctionsGroup DynamicFunctions = null!;
  private PropChangeTargets PropTargets = null!;

  private TemplateDefinition Def = null!;

  public Node DOM { get { return Def.DOM; } }

  // --------------------------------------------------------------------------------------------------------------------------
  public TemplateDynamics(TemplateDefinition def_)
  {
    Def = def_;

    NamingContext = new NamingContext();
    DynamicFunctions = new DynamicFunctionsGroup(NamingContext);
    PropTargets = new PropChangeTargets();

    Def.DOM.Symbol = NamingContext.GetUniqueNameFor(ROOT_NODE_NAME);
    PreProcessNode(Def.DOM);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void EmitFunctionDefs(CodeFile cf)
  {
    DynamicFunctions.EmitFunctionDefs(cf);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private void PreProcessNode(Node node)
  {
    bool isTextNode = node.Name == "<text>";

    if (node.Symbol == null)
    {
      string baseName = node.HasDynamicContent ? "_Node" : "node";
      node.Symbol = isTextNode ? null : NamingContext.GetUniqueNameFor(baseName);
    }

    if (node.Name == "<text>" && node.DynamicContent != null)
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
      PreProcessNode(child);
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

    var nodes = PropTargets.GetAllTargetNodes();

    // Always include the root node!
    if (!nodes.Any(x => x.Symbol == ROOT_NODE_NAME))
    {
      cf.WriteLine($"{ROOT_NODE_NAME}: HTMLElement");
    }

    foreach (var node in nodes)
    {
      cf.WriteLine($"{node.Symbol}: HTMLElement;");
    }

    cf.NextLine(1);
  }
}

