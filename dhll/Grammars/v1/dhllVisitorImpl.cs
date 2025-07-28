
using Antlr4.Runtime.Misc;
using dhll.Emitters;

namespace dhll.v1;

// ==============================================================================================================================
/// <summary>
/// Used to describe where a particular element appears in the raw text (file).
/// This data can be used to provide context in error situations, language server features (maybe),
/// and so on.
/// </summary>
public class SourceMetadata
{
  /// <summary>
  /// Path to the file where this element is defined.
  /// </summary>
  public string FilePath { get; set; }

  /// <summary>
  /// The line where the element appears.
  /// </summary>
  public int LineNumber { get; set; }

  /// <summary>
  /// The column where the element appears.
  /// </summary>
  public int ColNumber { get; set; }
}

// ==============================================================================================================================
public interface ISourceMetadata
{
  SourceMetadata SourceMeta { get; }
}

// ==============================================================================================================================
public class TypeDef : ISourceMetadata
{
  // --------------------------------------------------------------------------------------------------------------------------
  public TypeDef(string identifier_, SourceMetadata sourceMeta_)
  {
    Identifier = identifier_;
    Members = new List<Declare>();
    SourceMeta = sourceMeta_;
  }

  public string Identifier { get; set; }
  public List<Declare> Members { get; set; }

  public SourceMetadata SourceMeta { get; private set; }

  // --------------------------------------------------------------------------------------------------------------------------
  internal Declare? GetMember(string name)
  {
    var match = (from x in Members
                 where x.Identifier == name
                 select x).SingleOrDefault();

    return match;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public bool HasMember(string name) {
    var res = GetMember(name);
    return res != null;
  }

}

// ==============================================================================================================================
public class Declare
{
  public EScope Scope { get; set; } = EScope.Public;

  public string TypeName { get; set; } = default!;
  public string Identifier { get; set; } = default!;
  public string? InitValue { get; set; }

  public bool IsProperty { get; set; }

  // FUTURE:
  // public bool IsAtomic { get; set; }
}


// ==============================================================================================================================
public class dhllFile
{
  /// <summary>
  /// The original file path...
  /// </summary>
  public string Path { get; set; } = default!;

  /// <summary>
  /// Every typedef that is contained in this file.
  /// </summary>
  public List<TypeDef> TypeDefs { get; set; } = new List<TypeDef>();
}


// ==============================================================================================================================
public class dhllVisitorImpl : dhllBaseVisitor<object>
{

  private List<TypeDef> TypeDefs = new List<TypeDef>();

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitFile([NotNull] dhllParser.FileContext context)
  {
    foreach (var item in context.typedef())
    {
      var td = (TypeDef)VisitTypedef(item);
      this.TypeDefs.Add(td);
    }

    var res = new dhllFile();
    res.TypeDefs = this.TypeDefs;


    // TODO: Check for errors, multiple defs, etc.

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitTypedef([Antlr4.Runtime.Misc.NotNull] dhllParser.TypedefContext context)
  {
    var id = context.identifier();
    string idText = id.GetText();

    // TODO: We can care about metadata later.
    var meta = new SourceMetadata();
    var res = new TypeDef(idText, meta);

    // Get the declarations:
    foreach (var item in context.decl())
    {
      var dr = (Declare)VisitDecl(item);
      res.Members.Add(dr);
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitDecl([Antlr4.Runtime.Misc.NotNull] dhllParser.DeclContext context)
  {
    var res = new Declare();

    res.TypeName = context.typename().GetText();
    res.Identifier = context.identifier().GetText();

    var initializer = context.initializer();
    if (initializer != null)
    {
      res.InitValue = initializer.value().GetText();
    }
    else
    {
      Console.WriteLine("no initializer!");
    }

    res.IsProperty = context.prop()?.GetText() == "prop";
    return res;
  }
}


// public class IdentifierVisitorImpl : Identifier