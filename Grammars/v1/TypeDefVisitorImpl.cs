
namespace dhll.v1;

// ==============================================================================================================================
public class Declare
{
  public string TypeName { get; set; }
  public string Identifier { get; set; }
  public string? InitValue { get; set; }
}

// ==============================================================================================================================
public class TypeDef
{
  // --------------------------------------------------------------------------------------------------------------------------
  public TypeDef(string identifier_)
  {
    Identifier = identifier_;
    Declarations = new List<Declare>();
  }
  public string Identifier { get; set; }
  public List<Declare> Declarations { get; set; }
}

// ==============================================================================================================================
public class TypeDefVisitorImpl : TypeDefBaseVisitor<object>
{

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitTypedef([Antlr4.Runtime.Misc.NotNull] TypeDefParser.TypedefContext context)
  {
    var id = context.identifier().GetText();

    var res = new TypeDef(id);

    // Get the declarations:
    foreach (var item in context.decl())
    {
      var dr = (Declare)VisitDecl(item);
      res.Declarations.Add(dr);
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitDecl([Antlr4.Runtime.Misc.NotNull] TypeDefParser.DeclContext context)
  {
    var res = new Declare();

    res.TypeName = context.typename().GetText();
    res.Identifier = context.identifier().GetText();

    var initializer = context.initializer();
    if (initializer != null)
    {
      res.InitValue = initializer.expr().GetText();
    }
    else {
      Console.WriteLine("no initializer!");
    }
    return res;
  }
}


// public class IdentifierVisitorImpl : Identifier