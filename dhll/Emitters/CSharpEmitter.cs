using dhll.CodeGen;
using dhll.v1;
using drewCo.Tools;

namespace dhll.Emitters
{

  // ==============================================================================================================================
  internal class CSharpEmitter : EmitterBase
  {
    private CodeFile CF = new CodeFile();

    // --------------------------------------------------------------------------------------------------------------------------
    public CSharpEmitter(CompilerContext context_)
      : base(context_)
    { }

    public override string TargetLanguage => "C#";

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

      var res = new EmitterResults();
      var outputFiles = new List<string>();

      // OPTIONS:
      // This is putting all types in the same file.
      // We should have the option to emit one file per type if we want.
      // If the language supports it, we can even use partials (one for def, one for templates), so
      // n-files per type.
      string fName = Path.GetFileNameWithoutExtension(file.Path);
      string outputPath = FileTools.GetRootedPath(Path.Combine(outputDir, fName + ".cs"));

      WriteCodeGenHeader(CF);

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
          EmitDeclaration(item, CF);
        }
        CF.NextLine();

        // Emit template elements that we will want to bind to during calls to setters....
        if (dynamics != null)
        {
          dynamics.EmitDOMDeclarations(CF);
          templateEmitter.EmitCreateDOMFunction(CF);

          templateEmitter.EmitBindFunction(CF, dynamics);

          dynamics.EmitDynamicFunctionDefs(CF, this);
        }

        // Now emit all of the getters / setters.
        foreach (var item in preProcResults.GetterSetters)
        {
          EmitGetterSetter(item, dynamics, CF);
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
    protected override void EmitGetterSetter(GetterSetter item, TemplateDynamics dynamics, CodeFile cf)
    {
      string scope = GetScopeWord(item.Scope);
      if (scope != string.Empty) { scope += " "; }

      // NOTE: C# can't directly interact with a template (DOM) like typescript can, at least not in
      // this iteration, so we can simply ignore our template dynamics....
      if (item.UseGetter && item.UseSetter)
      {
        // C# style auto-prop.
        cf.WriteLine($"{scope}{item.Identifier}{{get; set; }}");
      }
      else
      {
        EmitDeclaration(item.BackingMember, cf);
        cf.Write($"{scope}{item.Identifier}");
        cf.OpenBlock(true);
        if (item.UseGetter)
        {
          cf.WriteLine($"get {{ return {item.BackingMember.Identifier}; }}");
        }
        if (item.UseSetter)
        {
          cf.WriteLine($"set {{ {item.BackingMember.Identifier} = value; }}");
        }
        cf.CloseBlock(1);
      }
    }

    // --------------------------------------------------------------------------------------------------------------------------
    protected override void EmitDeclaration(Declare dec, CodeFile cf)
    {
      var useType = TranslateTypeName(dec.TypeName);

      string line = $"{dec.Identifier}: {useType}";
      if (dec.InitValue != null)
      {
        line += $" = {dec.InitValue}";
      }
      line += ";";

      cf.WriteLine(line);
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

        cf.Write($"{scope}{returnType} {def.Identifier}()");
        cf.OpenBlock(true);

        // NOTE: As we improve the capability of dhll, the function def's body will contain actual
        // dhll statements / expressions that can be emitted to the target language correctly.
        foreach (var item in def.Body)
        {
          cf.WriteLine(item);
        }
        cf.CloseBlock();
        cf.NextLine(2);
      }
    }

  }

}
