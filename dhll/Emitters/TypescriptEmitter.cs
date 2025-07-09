using Antlr4.Runtime;
using dhll.CodeGen;
using dhll.v1;
using drewCo.Tools;
using drewCo.Tools.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace dhll.Emitters
{


  // ==============================================================================================================================
  internal class TypescriptEmitter : EmitterBase
  {

    public override string TargetLanguage => "typescript";

    // --------------------------------------------------------------------------------------------------------------------------
    public TypescriptEmitter(CompilerContext context_)
      : base(context_)
    { }

    // --------------------------------------------------------------------------------------------------------------------------
    protected override Dictionary<string, string> LoadTypeNameTable()
    {
      var res = new Dictionary<string, string>() {
        { "bool", "boolean" },
        { "int", "number" },
        { "float", "number" },
        { "double", "number" },
      };
      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public override EmitterResults Emit(string outputDir, dhllFile file)
    {
      var cf = new CodeFile();


      var res = new EmitterResults();
      var outputFiles = new List<string>();


      // OPTIONS:
      // This is putting all types in the same file.
      // We should have the option to emit one file per type if we want.
      // If the language supports it, we can even use partials (one for def, one for templates), so
      // n-files per type.
      string fName = Path.GetFileNameWithoutExtension(file.Path);
      string outputPath = FileTools.GetRootedPath(Path.Combine(outputDir, fName + ".ts"));

      WriteCodeGenHeader(cf);

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

        cf.Write($"class {td.Identifier} ");
        cf.OpenBlock();
        cf.NextLine();

        foreach (var item in preProcResults.Declares)
        {
          EmitDeclaration(item, cf);
        }
        cf.NextLine();

        // Emit template elements that we will want to bind to during calls to setters....
        if (dynamics != null)
        {
          dynamics.EmitDOMDeclarations(cf);
          templateEmitter.EmitCreateDOMFunctionForTypescript(cf);

          templateEmitter.EmitBindFunction(cf, dynamics);

          dynamics.EmitDynamicFunctionDefs(cf, this);
        }

        // Now emit all of the getters / setters.
        foreach (var item in preProcResults.GetterSetters)
        {
          EmitGetterSetter(item, dynamics, cf);
        }

        cf.CloseBlock();
        cf.NextLine();
      }

      cf.Save(outputPath);

      outputFiles.Add(outputPath);
      res.OutputFiles = outputFiles.ToArray();

      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    protected override void EmitGetterSetter(GetterSetter item, TemplateDynamics dynamics, CodeFile cf)
    {

      if (item.UseGetter)
      {
        cf.Write($"public get {item.Identifier}() ");
        cf.OpenBlock();
        cf.WriteLine($"return this.{item.BackingMember.Identifier};");
        cf.CloseBlock(2);
      }

      if (item.UseSetter)
      {
        string typeName = TranslateTypeName(item.BackingMember.TypeName);
        string bid = ConvertToArgumentName(item.Identifier) + "_";

        cf.Write($"public set {item.Identifier}({bid}: {typeName}) ");
        cf.OpenBlock();
        cf.WriteLine($"this.{item.BackingMember.Identifier} = {bid};");

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
              cf.WriteLine($"const {valId} = this.{t.FunctionName}();");
              cf.WriteLine($"this.{t.TargetNode.Identifier}.setAttribute('{t.Attr.Name}', {valId});");

              ++attrIndex;
            }
            else
            {
              // We are setting content for this item.
              cf.WriteLine($"this.{t.TargetNode.Identifier}.innerText = this.{t.FunctionName}();");
            }
          }
        }

        cf.CloseBlock(2);
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
    protected override void EmitDeclaration(Declare dec, CodeFile cf)
    {
      var useType = TranslateTypeName(dec.TypeName);

      string line = $"{dec.Identifier}: {useType}";

      // OPTION? --> AlwaysInitialize == true
      string useInitial = dec.InitValue ?? GetInitialFor(useType);
      if (useInitial != null)
      {
        line += $" = {useInitial}";
      }
      line += ";";

      cf.WriteLine(line);
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private string? GetInitialFor(string useType)
    {
      // OPTION: Check the option here.....
      const bool ALWAYS_INITIALIZE = true;
      if (ALWAYS_INITIALIZE)
      {
        switch (useType)
        {
          case "string": return "\"\"";
          case "number": return "0";
          default:
            throw new NotSupportedException($"The value: {useType} is not supported!");
        }
      }
      else
      {
        throw new NotImplementedException();
      }

    }

    // --------------------------------------------------------------------------------------------------------------------------
    public override void EmitFunctionDefs(IEnumerable<FunctionDef> defs, CodeFile cf)
    {
      cf.NextLine(2);
      foreach (var def in defs)
      {
        // TODO: We should have a typescript emitter squirt all of this out.
        // We just don't have a fully functional DHLL implementation at this time to make that work.
        string scope = GetScopeWord(def.Scope);
        string returnType = TranslateTypeName(def.ReturnType);

        cf.WriteLine($"{scope}{def.Identifier}(): {returnType}", 0);
        cf.OpenBlock(false);
        foreach (var item in def.Body)
        {
          cf.WriteLine(item);
        }
        cf.CloseBlock();
        cf.NextLine(2);
      }
    }

  }


  // ==============================================================================================================================
  // NOTE: This is very much like a function, but not quite.....
  // Depends on the language really....
  // Perhaps there will be a way to unify them at some point?
  class GetterSetter
  {
    public EScope Scope { get; set; }
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
