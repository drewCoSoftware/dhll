using dhll.CodeGen;
using dhll.v1;
using drewCo.Tools.Logging;

namespace dhll.Emitters
{

  // ==============================================================================================================================
  [Obsolete("I think that we can just use 'EmitterBase' instead!")]
  public interface IEmitter
  {
    EmitterResults Emit(string outputDir, dhllFile file);
    string TranslateTypeName(string typeName);
    string GetScopeWord(EScope scope);
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

    protected Dictionary<string, string> TypeNameTable = null!;

    // --------------------------------------------------------------------------------------------------------------------------
    public EmitterBase(CompilerContext context_)
    {
      Context = context_;
      TypeNameTable = LoadTypeNameTable();
    }

    // --------------------------------------------------------------------------------------------------------------------------
    
    protected abstract Dictionary<string, string> LoadTypeNameTable();
    protected abstract void EmitGetterSetter(GetterSetter item, TemplateDynamics dynamics, CodeFile cf);
    protected abstract void EmitDeclaration(Declare dec, CodeFile cf);

    public abstract void EmitFunctionDefs(IEnumerable<FunctionDef> defs, CodeFile cf);
    public abstract EmitterResults Emit(string outputDir, dhllFile file);

    // --------------------------------------------------------------------------------------------------------------------------
    protected void WriteCodeGenHeader(CodeFile cf)
    {
      cf.WriteLine("// -------------------------------------------------------- ");
      cf.WriteLine("// -------------------- CODE GEN WARNING ------------------ ");
      cf.WriteLine("// This file was created by a code generator.  You may edit ");
      cf.WriteLine("// it but be aware that your changes may disappear suddenly ");
      cf.WriteLine("// when the generator program runs next!                    ");
      cf.WriteLine("// -------------------------------------------------------- ");
      cf.NextLine(1);
    }

    // --------------------------------------------------------------------------------------------------------------------------
    protected TemplateDynamics? GetTemplateDynamics(TypeDef td)
    {
      Context.TemplateIndex.TryGetValue(td.Identifier, out TemplateDynamics? template);
      return template;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public virtual string GetScopeWord(EScope scope, bool addSpace = true)
    {
      string res = string.Empty;
      switch (scope)
      {
        case EScope.Default:
          break;
        case EScope.Public:
          res = "public";
          break;
        case EScope.Private:
          res = "private";
          break;

        default:
          throw new NotSupportedException();

      }

      res = (addSpace && res != string.Empty) ? res + " " : res;
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


    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Compute the identifier that is used for backing members of properties.
    /// </summary>
    public virtual string ComputeBackingIdentifier(string identifier)
    {
      string res = "_" + identifier;
      return res;
    }



    // --------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Scans all declarations in the given typedef.  In particular, we want to generate the correct code for
    /// getters/setters + setup the correct backing members, etc.
    /// </summary>
    protected ProcessDeclareResults PreProcessDeclarations(TypeDef td)
    {
      var declares = new List<Declare>();
      var getterSetters = new List<GetterSetter>();

      foreach (var dec in td.Members)
      {
        if (dec.IsProperty)
        {
          // Create a new backing member for this declaration.
          string useId = ComputeBackingIdentifier(dec.Identifier);
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



  }

}
