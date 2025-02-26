using Antlr4.Runtime;
using dhll.CodeGen;
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
    private CodeFile CF = new CodeFile();


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

      WriteCodeGenHeader();


      foreach (var td in file.TypeDefs)
      {
        var preProcResults = PreProcessDeclarations(td);

        CF.Write($"class {td.Identifier} ");
        CF.OpenBlock();
        CF.NextLine();

        foreach (var item in preProcResults.Declares)
        {
          EmitDeclaration(item);
        }
        CF.NextLine();

        // Now emit all of the getters / setters.
        foreach (var item in preProcResults.GetterSetters)
        {
          EmitGetterSetter(item);
        }

        CF.CloseBlock();
        CF.NextLine();
      }

      CF.Save(outputPath);
      Logger.Info($"Output: {outputPath}");

      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private void WriteCodeGenHeader()
    {
      CF.WriteLine("// -------------------------------------------------------- ");
      CF.WriteLine("// -------------------- CODE GEN WARNING ------------------ ");
    //  CF.WriteLine("//                                                        //");
      CF.WriteLine("// This file was created by a code generator.  You may edit ");
      CF.WriteLine("// it but be aware that your changes may disappear suddenly ");
      CF.WriteLine("// when the generator program runs next!                    ");
      CF.WriteLine("//                                                          ");
      CF.WriteLine("// -------------------------------------------------------- ");
   //   CF.WriteLine("// ---------------------------------------------------------- //");
      CF.NextLine(1);
    }


    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Scans all declarations in the given typedef.  In particular, we want to generate the correct code for
    /// getters/setters + setup the correct backing members, etc.
    /// </summary>
    private ProcessDeclareResults PreProcessDeclarations(TypeDef td)
    {
      var declares = new List<Declare>();
      var getterSetters = new List<GetterSetter>();

      foreach (var dec in td.Declarations)
      {
        if (dec.IsProperty)
        {
          // Create a new backing member for this declaration.
          string useId = "_" + dec.Identifier;
          var useDec = new Declare()
          {
            TypeName = dec.TypeName,
            InitValue = dec.InitValue,
            Identifier = useId,
            IsProperty = false,
          };
          declares.Add(useDec);

          // And the getter setters!
          getterSetters.Add(new GetterSetter()
          {
            BackingMember = useDec,
            Identifier = dec.Identifier,
            UseGetter = true,
            UseSetter = true,
          });
        }
        else
        {
          declares.Add(dec);
        }
      }

      var res = new ProcessDeclareResults()
      {
        Declares = declares,
        GetterSetters = getterSetters,
      };

      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private void EmitDeclaration(Declare item)
    {
      var useType = TranslateTypeName(item.TypeName);

      string line = $"{item.Identifier}: {useType}";
      if (item.InitValue != null)
      {
        line += $" = {item.InitValue}";
      }
      line += ";";

      CF.WriteLine(line);
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private void EmitGetterSetter(GetterSetter item)
    {
      if (item.UseGetter)
      {
        CF.Write($"public get {item.Identifier}() ");
        CF.OpenBlock();
        CF.WriteLine($"return this.{item.BackingMember.Identifier};");
        CF.CloseBlock(2);
      }

      if (item.UseSetter)
      {
        string typeName = TranslateTypeName(item.BackingMember.TypeName);
        string bid = ConvertToArgumentName(item.Identifier) + "_";

        CF.Write($"public set {item.Identifier}({bid}: {typeName}) ");
        CF.OpenBlock();
        CF.WriteLine($"this.{item.BackingMember.Identifier} = {bid};");
        CF.CloseBlock(2);
      }

    }

    // --------------------------------------------------------------------------------------------------------------------------
    // TODO: This entire function could be wrapped up into something in StringTools.
    private string ConvertToArgumentName(string identifier)
    {
      string asWords = StringTools.DeCamelCase(identifier);
      string[] parts = asWords.Split(' ');
      parts[0] = LowerFirst(parts[0]);

      string res = string.Join("", parts);
      return res;
    }


    // --------------------------------------------------------------------------------------------------------------------------
    // TODO: StringTools!
    // Convert the first character of the given input to lowercase.
    public static string LowerFirst(string input)
    {
      if (input.Length == 0) { return input; }

      uint val = (uint)input[0];
      if (val >= 65 & val <= 90)
      {
        val += 32;
      }

      // Too bad we can't just set the stupid character.  Might be useful to do so in an
      // unsafe context tho!
      string res = (char)val + input.Substring(1);
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


  // ==============================================================================================================================
  // NOTE: This is very much like a function, but not quite.....
  // Depends on the language really....
  // Perhaps there will be a way to unify them at some point?
  class GetterSetter
  {
    public Declare BackingMember { get; set; }
    public string Identifier { get; set; }
    public bool UseGetter { get; set; }
    public bool UseSetter { get; set; }
  }

  // ==============================================================================================================================
  class ProcessDeclareResults
  {
    public List<Declare> Declares { get; set; }
    public List<GetterSetter> GetterSetters { get; set; } = new List<GetterSetter>();
  }

  // ==============================================================================================================================
  public enum EScope
  {
    Public = 0,
    Private
  }

}
