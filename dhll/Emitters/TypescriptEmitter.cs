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
  internal class TypescriptEmitter : EmitterBase, IEmitter
  {
    private CodeFile CF = new CodeFile();

    private static Dictionary<string, string> TypeNameTable = new Dictionary<string, string>() {
      { "bool", "boolean" },
      { "int", "number" },
      { "float", "number" },
      { "double", "number" },
    };

    // --------------------------------------------------------------------------------------------------------------------------
    public TypescriptEmitter(CompilerContext context_)
      : base(context_)
    { }

    // --------------------------------------------------------------------------------------------------------------------------
    public EmitterResults Emit(string outputDir, dhllFile file)
    {
      var res = new EmitterResults();
      var outputFiles = new List<string>();

      // Simple version.  We will emit one TS file per dhll input file.
      // It will use the same name.
      string fName = Path.GetFileNameWithoutExtension(file.Path);
      string outputPath = FileTools.GetRootedPath(Path.Combine(outputDir, fName + ".ts"));

      WriteCodeGenHeader();

      foreach (var td in file.TypeDefs)
      {
        // Let's check to see if there is an associated template...
        Logger.Verbose("Resolving template data...");
        TemplateDynamics? dynamics = GetTemplateDynamics(td);
        TemplateEmitter? templateEmitter = null;
        if (dynamics != null)
        {
          Logger.Verbose($"Resolved template for type: {td.Identifier}");
          templateEmitter = new TemplateEmitter(td.Identifier, dynamics, Context);
        }
        else
        {
          Logger.Verbose($"There is no template for type: {td.Identifier}");
        }

        var preProcResults = PreProcessDeclarations(td);

        CF.Write($"class {td.Identifier} ");
        CF.OpenBlock();
        CF.NextLine();

        foreach (var item in preProcResults.Declares)
        {
          EmitDeclaration(item);
        }
        CF.NextLine();

        // Emit template elements that we will want to bind to during calls to setters....
        if (dynamics != null)
        {
          dynamics.EmitDOMDeclarations(CF);
          templateEmitter.EmitCreateDOMFunction(CF);

          templateEmitter.EmitBindFunction(CF, dynamics);

          dynamics.EmitDynamicFunctionDefs(CF);
        }

        // Now emit all of the getters / setters.
        foreach (var item in preProcResults.GetterSetters)
        {
          EmitGetterSetter(item, dynamics);
        }

        CF.CloseBlock();
        CF.NextLine();
      }

      CF.Save(outputPath);

      outputFiles.Add(outputPath);
      res.OutputFiles = outputFiles.ToArray();

      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private TemplateDynamics? GetTemplateDynamics(TypeDef td)
    {
      base.Context.TemplateIndex.TryGetValue(td.Identifier, out TemplateDynamics? template);
      return template;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public static string GetScopeWord(EScope scope)
    {
      switch (scope)
      {
        case EScope.Default:
          return string.Empty;
        case EScope.Public:
          return "public";
        case EScope.Private:
          return "private";

        default:
          throw new NotSupportedException();
      }
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private void WriteCodeGenHeader()
    {
      CF.WriteLine("// -------------------------------------------------------- ");
      CF.WriteLine("// -------------------- CODE GEN WARNING ------------------ ");
      CF.WriteLine("// This file was created by a code generator.  You may edit ");
      CF.WriteLine("// it but be aware that your changes may disappear suddenly ");
      CF.WriteLine("// when the generator program runs next!                    ");
      CF.WriteLine("// -------------------------------------------------------- ");
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

      foreach (var dec in td.Members)
      {
        if (dec.IsProperty)
        {
          // Create a new backing member for this declaration.
          string useId = GetInternalIdentifier(dec.Identifier);
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
    public static string GetInternalIdentifier(string identifier)
    {
      string res = "_" + identifier;
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
    private void EmitGetterSetter(GetterSetter item, TemplateDynamics dynamics)
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

        // This is where we do property target stuff...
        var propTargets = dynamics.PropTargets.GetTargetsForProperty(item.Identifier);
        if (propTargets != null)
        {
          int attrIndex = 0;
          foreach (var t in propTargets)
          {
            if (t.Attr != null)
            {
              string valId = $"val{attrIndex}";
              CF.WriteLine($"const {valId} = this.{t.FunctionName}();");
              CF.WriteLine($"this.{t.TargetNode.Identifier}.setAttribute('{t.Attr.Name}', {valId});");

              ++attrIndex;
            }
            else
            {
              // We are setting content for this item.
              CF.WriteLine($"this.{t.TargetNode.Identifier}.innerText = this.{t.FunctionName}();");
            }
          }
        }

        CF.CloseBlock(2);
      }

    }

    // --------------------------------------------------------------------------------------------------------------------------
    private string ConvertToArgumentName(string identifier)
    {
      string asWords = StringTools.DeCamelCase(identifier);
      string[] parts = asWords.Split(' ');
      parts[0] = LowerFirst(parts[0]);

      string res = string.Join("", parts);
      return res;
    }


    // --------------------------------------------------------------------------------------------------------------------------
    [Obsolete("Use version from drewCo.Tools.StringTools > 1.3.3.6!")]
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
    Default = 0,
    Public,
    Private
  }

}
