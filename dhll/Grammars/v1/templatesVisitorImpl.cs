using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using dhll.v1;
using drewCo.Tools;
using System.Collections.Specialized;
using System.ComponentModel;

namespace dhll.Grammars.v1;


// ==============================================================================================================================
/// <summary>
/// Some languages (like typescript) are allowed to have HTML type templates, and this
/// type captures their data.
/// </summary>
public class TemplateDefinition
{
  public string ForType { get; set; } = default!;
  public string? Name { get; set; } = default!;
}

// ==============================================================================================================================
internal class templatesVisitorImpl : templateParserBaseVisitor<object>
{
  private const string TEMPLATE = "template";

  public List<TemplateDefinition> TemplateDefs = new List<TemplateDefinition>();

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitTemplates([NotNull] templateParser.TemplatesContext context)
  {
    // Example of some kind of tree traversal + getting the names of stuff...
    //templateParser.HtmlElementsContext x = context.htmlElements()[0];
    //string nameOfThing = x.htmlElement().entityName().GetText();

    // I want to get every top-level element that is 'template'.
    // Anything else should be marked as an error.....
    var elems = context.htmlElements();
    foreach (var e in elems)
    {
      var elem = e.htmlElement();
      string tagName = elem.entityName().GetText();
      if (tagName != TEMPLATE)
      {
        throw new Exception($"Top level element must be '{TEMPLATE}'!");
      }

      var attrToVal = new Dictionary<string, string?>()
      {
        { "for", null },
        { "name", null },
      };

      var attrs = elem.htmlAttribute();
      foreach (var item in attrs)
      {
        string attrName = item.entityName().GetText();
        string? attrVal = item.ATTVALUE_VALUE()?.GetText();
        attrToVal[attrName] = attrVal;
      }

      if (attrToVal["for"] == null)
      {
        throw new Exception("template MUST have 'for' attribute!");
      }


      string forVal = ExtractQuotedValue(attrToVal["for"])!;
      string? nameVal = ExtractQuotedValue(attrToVal["name"]);

      var def = new TemplateDefinition()
      {
        ForType = forVal,
        Name = nameVal,
      };

      TemplateDefs.Add(def);
    }

    return VisitChildren(context);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private static string? ExtractQuotedValue(string? input)
  {
    if (input == null) { return null; }

    if (input.StartsWith("'"))
    {
      // TODO: Update 'StringTools' so that it can 'unquote' anything be it single, double, parens, etc.
      // I think that 'extract string' is the best thing to call it.
      input = input.Substring(1, input.Length - 2);
    }
    else if (input.StartsWith('\"'))
    {
      input = StringTools.Unquote(input);
    }
    else
    {
      throw new Exception("Expected a single or double quoted string!");
    }

    return input;
  }
}

