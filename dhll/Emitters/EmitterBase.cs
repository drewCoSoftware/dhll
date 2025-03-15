using dhll.v1;
using drewCo.Tools.Logging;

namespace dhll.Emitters
{

  // ==============================================================================================================================
  public interface IEmitter
  {
    EmitterResults Emit(string outputDir, dhllFile file);
    string TranslateTypeName(string typeName);
  }

  // ==============================================================================================================================
  public class EmitterResults
  {
    public string[] OutputFiles { get; set; } = null!;
  }

  // ==============================================================================================================================
  internal abstract class EmitterBase
  {
    protected Logger Logger { get { return Context.Logger; } }
    protected CompilerContext Context { get; set; } = default!;

    // --------------------------------------------------------------------------------------------------------------------------
    public EmitterBase(CompilerContext context_)
    {
      Context = context_;
    }


  }

}
