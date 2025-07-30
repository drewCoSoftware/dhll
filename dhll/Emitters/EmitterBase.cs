using dhll.CodeGen;
using dhll.Expressions;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools.Logging;

namespace dhll.Emitters;


// ==============================================================================================================================
internal abstract class EmitterBase
{
  protected CompilerContext Context { get; set; } = default!;

  protected Dictionary<string, string> TypeNameTable = null!;


  /// <summary>
  /// the language that this emitter outputs code to.
  /// </summary>
  public abstract string TargetLanguage { get; }

  protected abstract Dictionary<string, string> LoadTypeNameTable();
  protected abstract void EmitGetterSetter(GetterSetter item, TemplateInfo dynamics, CodeFile cf);
  protected abstract void EmitDeclaration(Declare dec, CodeFile cf);

  public abstract void EmitFunctionDefs(IEnumerable<FunctionDef> defs, CodeFile cf);
  public abstract EmitterResults Emit(string outputDir, dhllFile file);
  
  // public abstract void EmitExpression(Expression expression, CodeFile cf);

  // HACK: The 'onPrimaryCallback' is being used so that we can inject 'this.' into some typescript statements.
  // In reality the code emitter should have some kind of notion of 'scope' that covers when it is in a 
  // class and is emitting class members....
  public abstract string RenderExpression(Expression expression, Func<string, string>? onIdentifierCallback = null);

  // --------------------------------------------------------------------------------------------------------------------------
  public EmitterBase(CompilerContext context_)
  {
    Context = context_;
    TypeNameTable = LoadTypeNameTable();
  }


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
  protected virtual Grammars.v1.TemplateInfo? GetTemplateInfoForType(TypeDef td)
  {
    Context.TemplateInfoIndex.TryGetValue(td.Identifier, out Grammars.v1.TemplateInfo? def);
    return def;
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
  protected virtual ProcessDeclareResults PreProcessDeclarations(TypeDef td)
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


// ==============================================================================================================================
public class EmitterResults
{
  public string[] OutputFiles { get; set; } = null!;
}
