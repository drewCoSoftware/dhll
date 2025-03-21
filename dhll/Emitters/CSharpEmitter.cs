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
        { "bool", "bool" },
        { "int", "int" },
        { "float", "float" },
        { "double", "double" },
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

        CF.Write($"class {td.Identifier} ");
        CF.OpenBlock(true);
        CF.NextLine();

        foreach (var dec in td.Members)
        {
          EmitDeclaration(dec, CF);
        }
        CF.NextLine();

        // Emit template elements that we will want to bind to during calls to setters....
        if (dynamics != null)
        {
          // NOTE: Currently for C# we don't bind to any kind of template, so I am just going
          // to skip this step.  Future versions of the code will of course have to care (say we
          // want to bind to WPF or something...)

          //dynamics.EmitDOMDeclarations(CF);
          templateEmitter.EmitCreateDOMFunctionForCSharp(CF);
          //templateEmitter.EmitBindFunction(CF, dynamics);
          dynamics.EmitDynamicFunctionDefs(CF, this);
        }


        //// Now emit all of the getters / setters.
        //foreach (var item in preProcResults.GetterSetters)
        //{
        //  EmitGetterSetter(item, dynamics, CF);
        //}

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
      string scope = GetScopeWord(dec.Scope);

      string getset = string.Empty;
      if (dec.IsProperty)
      {
        getset = " { get; set; }";
      }
      string line = $"{scope}{useType} {dec.Identifier}{getset}";
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

        string scope = GetScopeWord(def.Scope);
        string returnType = TranslateTypeName(def.ReturnType);

        cf.Write($"{scope}{returnType} {def.Identifier}()");
        cf.OpenBlock(true);

        // NOTE: As we improve the capability of dhll, the function def's body will contain actual
        // dhll statements / expressions that can be emitted to the target language correctly.
        foreach (var text in def.Body)
        {
          // HACK:  I am doing a find + replace to patch up the code while we generalize the 'FunctionDef' type + add expression support.
          string useText = text;
          useText = useText.Replace("toString()", "ToString()");
          cf.WriteLine(useText);
        }

        cf.CloseBlock();
        cf.NextLine(2);
      }
    }

  }

}
