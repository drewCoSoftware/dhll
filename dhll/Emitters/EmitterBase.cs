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
    protected Logger Logger = default!;
    public EmitterBase(Logger logger_) { Logger = logger_; }
  }

}
