using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using dhll.v1;
using drewCo.Tools;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace dhll.Grammars.v1;



// ==============================================================================================================================
public class Node
{
  /// <summary>
  /// Name of the tag, i.e. 'p', 'img', etc.
  /// If this represents text data, use: '&gt;text&lt;' for the name.
  /// </summary>
  public string Name { get; set; } = default!;
  public List<Attribute> Attributes { get; set; } = new List<Attribute>();
  public List<Node> Children { get; set; } = new List<Node>();

  /// <summary>
  /// Text/HTML content.  Used for text nodes.
  /// </summary>
  public string? Value { get; set; } = default;
}

// ==============================================================================================================================
public class Attribute
{
  public string Name { get; set; } = default!;
  public string? Value { get; set; } = default!;
}


// ==============================================================================================================================
/// <summary>
/// Some languages (like typescript) are allowed to have HTML type templates, and this
/// type captures their data.
/// </summary>
public class TemplateDefinition
{
  public string ForType { get; set; } = default!;
  public string? Name { get; set; } = default!;

  /// <summary>
  /// Node-Tree representation of the DOM.
  /// </summary>
  public Node DOM { get; set; } = default!;
}


// ==============================================================================================================================
internal class templatesVisitorImpl : templateParserBaseVisitor<object>
{
  private const string TEMPLATE = "template";

  public List<TemplateDefinition> TemplateDefs = new List<TemplateDefinition>();

  // --------------------------------------------------------------------------------------------------------------------------
  public override object VisitTemplates([NotNull] templateParser.TemplatesContext context)
  {
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

      TemplateDefinition def = ParseTemplate(elem);

      TemplateDefs.Add(def);
    }

    // NOTE: No clue what to return here....
    return null;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private TemplateDefinition ParseTemplate(templateParser.HtmlElementContext elem)
  {

    // Each template element MUST have a single child element!
    var content = elem.htmlContent();
    int childElemCount = content.htmlElement().Count();

    if (childElemCount != 1)
    {
      throw new Exception("Every template MUST have a single child element!");
    }
    var rootElem = content.htmlElement()[0];

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


    // Now in the rest of the template we have to find our prop strings.
    // These can be inside of elements, or as values of attributes.

    // Once we find them, we then have to associate them with some kind of selector....
    // We also have to get the property name....
    // We also need the DOM without the Prop strings so that we can create them in a DOM via javascript....

    // The DOM we can do like any other tree of tags....
    Node n = ComputeDOM(rootElem);


    var def = new TemplateDefinition()
    {
      ForType = forVal,
      Name = nameVal,
      DOM = n
    };

    return def;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Node ComputeDOM(templateParser.HtmlElementContext elem)
  {
    string tagName = elem.entityName().GetText();

    var attributes = ComputeAttributes(elem);
    List<Node> children = ComputeChildren(elem);

    var res = new Node()
    {
      Name = tagName,
      Attributes = attributes,
      Children = children
    };

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private List<Attribute> ComputeAttributes(templateParser.HtmlElementContext elem)
  {
    var res = new List<Attribute>();
    var attrs = elem.htmlAttribute();
    foreach (var attr in attrs)
    {
      string name = ExtractQuotedValue(attr.entityName().GetText())!;
      string? val = ExtractQuotedValue(attr.ATTVALUE_VALUE()?.GetText());

      if (val != null && IsPropString(val))
      {
        // This is where stuff gets interesting....
        int x = 10;
      }

      var toAdd = new Attribute()
      {
        Name = name,
        Value = val
      };

      res.Add(toAdd);

    }
    return res;
  }

  public override object VisitChildren(IRuleNode node)
  {
    return base.VisitChildren(node);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private List<Node> ComputeChildren(templateParser.HtmlElementContext elem)
  {
    List<Node> res = null!;

    // TODO: Implement me!
    var elems = elem.children;
    foreach (var child in elems)
    {
      var content = child as templateParser.HtmlContentContext;
      if (content != null)
      {
        res = GetChildNodesFromContent(content);
      }
      //else if (child is templateParser.HtmlElementContext)
      //{
      //  int z = 10;
      //}
      //else if (child is templateParser.HtmlChardataContext)
      //{
      //  int z = 10;
      //}
      ////// VisitChildren(
      ////int x = 10;
      ////if (child.
      //int x = 10;
      //string cName = child.GetType().Name;
      //Debug.WriteLine(cName);
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private List<Node> GetChildNodesFromContent(templateParser.HtmlContentContext child)
  {
    var res = new List<Node>();

    var kids = child.children;
    foreach (var kid in kids)
    {
      if (kid is templateParser.HtmlElementContext)
      {
        var n = ComputeDOM(kid as templateParser.HtmlElementContext); 
        res.Add(n);
      }
      else if (kid is templateParser.HtmlChardataContext)
      {
        var charData = kid as templateParser.HtmlChardataContext;
        var n = new Node()
        {
          Name = "<text>",
          Value = charData.GetText()
        };
        res.Add(n);
        int z = 10;
      }
    }

    // NOTE: We could combine contiguous text nodes at this point to simplify the tree a bit.

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public static bool StartsAndEndsWith(string input, string startsWith, string endsWith)
  {
    bool res = input.StartsWith(startsWith) && input.EndsWith(endsWith);
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public static bool IsPropString(string input)
  {
    bool res = StartsAndEndsWith(input, "{", "}");
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public static bool IsQuotedString(string input)
  {
    bool res = StartsAndEndsWith(input, "'", "'") || StartsAndEndsWith(input, "\"", "\"");
    return res;
  }


  // --------------------------------------------------------------------------------------------------------------------------
  private static string? ExtractQuotedValue(string? input, bool allowUnquotedStrings = true)
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
    else if (!allowUnquotedStrings)
    {
      throw new Exception("Expected a single or double quoted string!");
    }

    return input;
  }
}

