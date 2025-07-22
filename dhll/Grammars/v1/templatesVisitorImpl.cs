using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using dhll.Emitters;
using dhll.Expressions;
using dhll.v1;
using drewCo.Tools;
using drewCo.Web;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using static dhll.v1.templateParser;

namespace dhll.Grammars.v1;

// ==============================================================================================================================
public class TemplateParseException : Exception
{
  public TemplateParseException(string message) : base(message) { }
  public TemplateParseException(string message, Exception innerException) : base(message, innerException) { }
}

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

  /// <summary>
  /// All of the property names (identifiers) that appear in the expression.
  /// </summary>
  /// REFACTOR: Rename to 'identifiers' or something similar.
  public List<string> PropertyNames { get; set; } = new List<string>();
}


// ==============================================================================================================================
/// <summary>
/// This attribute is really only used to make it clear that a certain property is only used for codegen
/// purposes, and shouldn't be messed with.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class CodeGenAttribute : System.Attribute
{ }

// ==============================================================================================================================
public partial class Node
{

  public bool IsTextNode { get { return Name == HTMLNode.TEXT_NAME; } }
  public bool IsExpressionNode { get { return Name == HTMLNode.EXPRESSION_NAME; } }

  /// <summary>
  /// The parent node, or null if this is the root node.
  /// </summary>
  public Node? Parent { get; set; } = null;

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

  public bool HasDynamicContent
  {
    get
    {
      bool res = this.DynamicContent != null ||
      this.Attributes.Any(x => x.DynamicContent != null);
      return res;
    }
  }


  /// <summary>
  /// Used during codegen.
  /// Text nodes are never named and can therefore be null.
  /// </summary>
  [CodeGen]
  internal string? Identifier { get; set; } = null;

  /// <summary>
  /// Used during codegen.
  /// </summary>
  [CodeGen]
  internal string DynamicFunction { get; set; } = null!;

}

// ==============================================================================================================================
public enum EAttrValType
{
  Invalid = 0,
  String,
  Expression
}


// ==============================================================================================================================
public class AttributeValue
{
  // --------------------------------------------------------------------------------------------------------------------------
  public AttributeValue(Expression expresion_)
  {
    Type = EAttrValType.Expression;
    Expression = expresion_;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public AttributeValue(string value)
  {
    Type = EAttrValType.String;
    StringVal = value;
  }

  public EAttrValType Type { get; set; }
  public string? StringVal { get; set; }
  public Expression? Expression { get; set; }
}

// ==============================================================================================================================
public class Attribute
{
  public string Name { get; set; } = default!;
  public AttributeValue Value { get; set; } = default!;

  public DynamicContent? DynamicContent { get; set; } = null;

  /// <summary>
  /// Used during codegen.
  /// </summary>
  [CodeGen]
  internal string DynamicFunction { get; set; }
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


// internal class attributeVisitorImpl : a

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
      throw new TemplateParseException("Every template MUST have a single child element!");
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
      string? attrVal = item.attrValue()?.GetText(); // ATTVALUE_VALUE()?.GetText();
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

    var res = new Node()
    {
      Parent = null,
      Name = tagName,
      Attributes = attributes,
    };

    List<Node> children = ComputeChildren(elem, res);

    res.Children = children;

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


      var atv = attr.attrValue();
      AttributeValue val = VisitAttribute(atv);
      // atv.children[0].

      // string? val = ExtractQuotedValue(attr.attrValue()?.GetText());


      // We need to figure this out...
      // throw new NotImplementedException();

      // DynamicContent? dc = null;
      // List<string> propNames = new List<string>();



      //if (val != null && HasPropString(val))
      //{
      DynamicContent? dc = ParseDynamicContent(val);

      //  // Any value, be it class or text must be representable as a string.
      //  // This means that any time an implicated property changes, then we will compute a string (like sprintf) and then slap in into the DOM at the correct place.
      //  // Correct places can be:
      //  // - An attribute
      //  // - Inner text/html of an element.
      //  // - That's about it!
      //  // so we need:
      //  // --> The selector for the element.
      //  // --> what property we are affecting....
      //}

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
  private AttributeValue VisitAttribute(templateParser.AttrValueContext atv)
  {
    var expr = atv as RAW_EXPRESSIONContext;
    if (expr != null)
    {
      var child = expr.children[0];
      var exp = child as Tag_expressionContext;
      if (exp == null)
      {
        throw new InvalidOperationException("unknown expression sub-type!");
      }

      Expression e = ParseTagExpression(exp);
      var res = new AttributeValue(e);
      return res;

    }
    else
    {
      if (!(atv is DBL_QUOTE_STRINGContext))
      {
        throw new InvalidOperationException("Expressin value is not supported!  Can't parse!");
      }
      string text = atv.GetText();
      var res = new AttributeValue(text);
      return res;
    }


    // We should not get here!
    throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseTagExpression(Tag_expressionContext tagExpr)
  {

    // We have to walk down the treee......
    ExprContext expression = tagExpr.expr();

    var res = ParseExpresion(expression);
    return res;

    throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseExpresion(ExprContext expression)
  {
    var add = expression.addExp();

    Expression res = ParseIt(add);
    return res;

  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseIt(IParseTree rule)
  {
    var useRule = rule as ParserRuleContext;
    return ParseIt(useRule!);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseIt(ParserRuleContext rule)
  {
    if (rule == null)
    {
      throw new ArgumentNullException($"{nameof(rule)} may not be null!");
    }

    if (rule is AddExpContext ||
        rule is SubExpContext ||
        rule is MultExpContext ||
        rule is DivExpContext)
    {
      if (rule.ChildCount == 3)
      {
        var res = ParseBinaryExpression(rule);
        return res;
      }
      else if (rule.ChildCount == 1)
      {
        var res = ParseIt((rule.children[0] as ParserRuleContext)!);
        return res;
      }
      else
      {
        throw new NotSupportedException("The add expression appears to be malformed...");
      }
    }
    else
    {
      // This will be a primary expression...
      var parens = rule as TO_PARENSContext;
      if (parens == null)
      {
        throw new InvalidOperationException($"This should be an {nameof(TO_PARENSContext)}!");
      }

      return ParseParens(parens);
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseParens(TO_PARENSContext parens)
  {
    var p = parens.children[0];
    var call = p as TO_CALLContext;
    if (call != null)
    {
      return ParseCall(call);
    }

    // Deal with the children of parenthesis....

    throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseCall(TO_CALLContext call)
  {
    var c = call.children[0];
    var primary = c as TO_PRIMARYContext;
    if (primary != null)
    {
      return ParsePrimaryExpression(primary);
    }

    // Deal with the calling of functions.
    throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParsePrimaryExpression(TO_PRIMARYContext primaryExp)
  {
    int x = 10;

    var p = primaryExp.children[0] as PrimaryExprContext;
    if (p == null)
    {
      throw new InvalidOperationException("Why isn't this a primary expression?");
    }

    var vc = p as VARIABLEContext;
    if (vc != null)
    {
      return new PrimaryExpression(vc);
    }

    var ms = p as MAGIC_STRINGContext;
    if (ms != null)
    {
      return new PrimaryExpression(ms);
    }

    var mn = p as MAGIC_NUMBERContext;
    if (mn != null)
    {
      return new PrimaryExpression(mn);
    }

    throw new NotSupportedException("This is some other kind of primary that we don't have support for!");
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private Expression ParseBinaryExpression(ParserRuleContext context)
  {
    // This is a binary expression!
    var left = ParseIt(context.children[0]);

    // TODO: We could test this, but who cares?
    string opString = context.children[1].GetText();
    EOperator useOp = ComputeOperator(opString);

    var right = ParseIt(context.children[2]);
    var ae = new BinaryExpression(left, right, EOperator.Add);

    return ae;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private EOperator ComputeOperator(string opString)
  {
    switch (opString)
    {
      case "+":
        return EOperator.Add;
      case "-":
        return EOperator.Subtract;
      case "*":
        return EOperator.Multiply;
      case "/":
        return EOperator.Divide;

      default:
        throw new NotSupportedException($"The operator value: {opString} is not supported!");
    }
    throw new NotImplementedException();
  }


  // --------------------------------------------------------------------------------------------------------------------------
  private DynamicContent ParseDynamicContent(AttributeValue val)
  {
    return ParseDynamicContent(val.Expression);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private DynamicContent ParseDynamicContent(Expression val)
  {
    Debug.WriteLine("DYNAMIC CONTENT SUPPORT PLEASE!  TEST CASES TOO!");
    return null;
  }

  //// --------------------------------------------------------------------------------------------------------------------------
  //private DynamicContent ParseDynamicContent(AttributeValue val)
  //{
  //  return null;

  //  //// OLD: String only based approach....
  //  //  int start = 0;

  //  //  var parts = new List<FormatPart>();
  //  //  var propNames = new List<string>();

  //  //  MatchCollection matches = Regex.Matches(val, "\\{.*\\}");
  //  //  foreach (Match m in matches)
  //  //  {
  //  //    int index = m.Captures[0].Index;
  //  //    string prevPart = val.Substring(start, index - start);
  //  //    if (prevPart != string.Empty)
  //  //    {
  //  //      parts.Add(new FormatPart()
  //  //      {
  //  //        Value = prevPart,
  //  //      });
  //  //    }


  //  //    string expValue = m.Value.Substring(1, m.Value.Length - 2);
  //  //    // NOTE: If we have some kind of expression for our properties, then we have to have a way
  //  //    // to extract all of the named properties..  At this time we don't have a way to really do that
  //  //    // outside of updating the grammar, so for now, we will just assume that the expression is a single
  //  //    // property name....
  //  //    propNames.Add(expValue.Trim());

  //  //    parts.Add(new FormatPart()
  //  //    {
  //  //      Value = expValue,
  //  //      IsExpession = true
  //  //    });

  //  //    start = index + m.Length;
  //  //  }

  //  //  // Leftover string?
  //  //  if (val.Length > start)
  //  //  {
  //  //    parts.Add(new FormatPart()
  //  //    {
  //  //      Value = val.Substring(start)
  //  //    });
  //  //  }

  //  //  var res = new DynamicContent()
  //  //  {
  //  //    Parts = parts,
  //  //    PropertyNames = propNames.Distinct().ToList()
  //  //  };

  //  //  if (res.PropertyNames.Count == 0)
  //  //  {
  //  //    // NOTE: A future version could allow for const expressions....
  //  //    throw new Exception("There are no named properties in the expression!");
  //  //  }

  //  //  return res;
  //}

  // --------------------------------------------------------------------------------------------------------------------------
  private List<Node> ComputeChildren(templateParser.HtmlElementContext elem, Node parentNode)
  {
    List<Node> res = new List<Node>();

    var elems = elem.children;
    foreach (var child in elems)
    {
      var content = child as templateParser.HtmlContentContext;
      if (content != null)
      {
        res = GetChildNodesFromContent(content, parentNode);
      }
    }

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  private List<Node> GetChildNodesFromContent(templateParser.HtmlContentContext parent, Node parentNode)
  {
    var res = new List<Node>();

    var kids = parent.children;
    if (kids != null)
    {
      foreach (var kid in kids)
      {
        if (kid is templateParser.HtmlElementContext)
        {
          var n = ComputeDOM((kid as templateParser.HtmlElementContext)!);
          res.Add(n);
        }
        else if (kid is templateParser.HtmlChardataContext)
        {
          var charData = (kid as templateParser.HtmlChardataContext)!;

          string text = charData.GetText();

          var n = new Node()
          {
            Parent = parentNode,
            Name = HTMLNode.TEXT_NAME,
            Value = text,
          };
          res.Add(n);
          int z = 10;
        }
        else if (kid is templateParser.ExpressionContext)
        {
          var context = kid as templateParser.ExpressionContext;
          Expression expr = ParseExpresion(context.expr());

          // NOTE / TODO:
          // If this expression is just a string literal, then we should convert it to a text node,
          // and then have a second pass to combine all adjacent text nodes into one.
          // NOTE: --> Such a feature should be an option that we enable by default.

          var dc = ParseDynamicContent(expr);
          var n = new Node()
          {
            Parent = parentNode,
            Name = HTMLNode.EXPRESSION_NAME,
            Value = null,
            DynamicContent = dc,
          };
          res.Add(n);

          int x = 10;
        }
        else
        {
          var t = kid.GetType();
          throw new NotImplementedException($"A handler for the parser rule (type: {t}) is not implemented!");
        }
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
    // input = input.Replace("\\{", "_").Replace("\\}", "_"))
    const string PAT = @"(?<!\\)\{(.*?)(?<!\\)\}";
    //const string PAT = @"\{.*\}";

    bool res = Regex.IsMatch(input, PAT);
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

