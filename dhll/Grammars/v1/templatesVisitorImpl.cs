using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using dhll.v1;
using drewCo.Tools;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace dhll.Grammars.v1;


// ==============================================================================================================================
public class FormatPart
{
  public string Value { get; set; }
  public bool IsExpession { get; set; }
}

// ==============================================================================================================================
public class DynamicContent
{
  // Some way (function) to represent the interpolated string....
  public List<FormatPart> Parts { get; set; }
  public List<string> PropertyNames { get; set; } = new List<string>();
}


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

  /// <summary>
  /// If there is any dynamic content / prop expressions in the content, we will use this.
  /// </summary>
  public DynamicContent? DynamicContent { get; set; } = null;


  /// <summary>
  /// Used during codegen.
  /// </summary>
  internal string Symbol { get; set; } = null!;
}

// ==============================================================================================================================
public class Attribute
{
  public string Name { get; set; } = default!;
  public string? Value { get; set; } = default!;

  public DynamicContent? DynamicContent { get; set; } = null;
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

    // NOTE: No clue what to return here....  Null is the default return value FWIW.
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

      DynamicContent? dc = null;
      List<string> propNames = new List<string>();
      if (val != null && HasPropString(val))
      {
        dc = ParseDynamicContent(val);

        // Any value, be it class or text must be representable as a string.
        // This means that any time an implicated property changes, then we will compute a string (like sprintf) and then slap in into the DOM at the correct place.
        // Correct places can be:
        // - An attribute
        // - Inner text/html of an element.
        // - That's about it!
        // so we need:
        // --> The selector for the element.
        // --> what property we are affecting....
      }

      var toAdd = new Attribute()
      {
        Name = name,
        Value = val,
        DynamicContent = dc
      };

      res.Add(toAdd);

    }
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private DynamicContent ParseDynamicContent(string val)
  {
    int start = 0;

    var parts = new List<FormatPart>();
    var propNames = new List<string>();

    MatchCollection matches = Regex.Matches(val, "\\{.*\\}");
    foreach (Match m in matches)
    {
      int index = m.Captures[0].Index;
      string prevPart = val.Substring(start, index - start);
      if (prevPart != string.Empty)
      {
        parts.Add(new FormatPart()
        {
          Value = prevPart,
        });
      }


      string expValue = m.Value.Substring(1, m.Value.Length - 2);
      // NOTE: If we have some kind of expression for our properties, then we have to have a way
      // to extract all of the named properties..  At this time we don't have a way to really do that
      // outside of updating the grammar, so for now, we will just assume that the expression is a single
      // property name....
      propNames.Add(expValue.Trim());

      parts.Add(new FormatPart()
      {
        Value = expValue,
        IsExpession = true
      });

      start = index + m.Length;
    }

    // Leftover string?
    if (val.Length > start)
    {
      parts.Add(new FormatPart()
      {
        Value = val.Substring(start)
      });
    }

    var res = new DynamicContent()
    {
      Parts = parts,
      PropertyNames = propNames.Distinct().ToList()
    };

    if (res.PropertyNames.Count == 0) {
      // NOTE: A future version could allow for const expressions....
      throw new Exception("There are no named properties in the expression!");
    }

    return res;
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
        var charData = (kid as templateParser.HtmlChardataContext)!;

        string text = charData.GetText();
        bool hasDynamic = HasPropString(text);
        DynamicContent? dc = null;
        if (hasDynamic)
        {
          dc = ParseDynamicContent(text);
        }

        var n = new Node()
        {
          Name = "<text>",
          Value = text,
          DynamicContent = dc
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
  /// <summary>
  /// Tells us if the given input string has one or more prop strings...
  /// </summary>
  public static bool HasPropString(string input)
  {
    bool res = Regex.IsMatch(input, "\\{.*\\}");
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

