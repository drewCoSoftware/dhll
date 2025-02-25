using dhll.v1;
using drewCo.Tools;
using drewCo.Tools.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

  // ==============================================================================================================================
  internal class TypescriptEmitter : EmitterBase, IEmitter
  {
    private static Dictionary<string, string> TypeNameTable = new Dictionary<string, string>() {
      { "bool", "boolean" }
    };

    // --------------------------------------------------------------------------------------------------------------------------
    public TypescriptEmitter(Logger logger_) : base(logger_)
    { }

    // --------------------------------------------------------------------------------------------------------------------------
    public EmitterResults Emit(string outputDir, dhllFile file)
    {
      var res = new EmitterResults();

      Logger.Info($"File: {file.Path}");

      // Simple version.  We will emit one TS file per dhll input file.
      // It will use the same name.
      string fName = Path.GetFileNameWithoutExtension(file.Path);
      string outputPath = FileTools.GetRootedPath(Path.Combine(outputDir, fName + ".ts"));


      // Let's squirt some code for the declarations...
      const string TAB = "  ";      // Two-space tab, for now.

      var sb = new StringBuilder();
      foreach (var td in file.TypeDefs)
      {
        sb.Append($"class {td.Identifier} {{");

        foreach (var item in td.Declarations)
        {
          var useType = TranslateTypeName(item.TypeName);

          sb.Append($"{Environment.NewLine}{TAB}{item.Identifier}: {useType}");
          if (item.InitValue != null)
          {
            sb.Append($" = {item.InitValue}");
          }
          sb.Append(";");
        }

        sb.Append(Environment.NewLine);
        sb.Append("}");
      }

      string content = sb.ToString();
      File.WriteAllText(outputPath, content);

      Logger.Info($"Output: {outputPath}");

      return res;
    }


    // --------------------------------------------------------------------------------------------------------------------------
    public string TranslateTypeName(string typeName)
    {
      // TODO: Add lookup tables as needed.
      if (TypeNameTable.TryGetValue(typeName, out string res))
      {
        return res;
      }

      // Unknown, use input!
      return typeName;
    }
  }

}
