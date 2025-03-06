using dhll.CodeGen;
using dhll.Grammars.v1;
using dhll.v1;
using drewCo.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhll.Emitters
{
  // ==============================================================================================================================
  /// <summary>
  /// Helps keep track of names, etc. so that we can create unique symbols while generating code.
  /// </summary>
  internal class NamingContext
  {

    private Dictionary<string, int> NamesToCounts = new Dictionary<string, int>();
    private object DataLock = new object();

    // --------------------------------------------------------------------------------------------------------------------------
    public string GetUniqueNameFor(string symbol)
    {

      lock (DataLock)
      {
        int count = 0;
        if (NamesToCounts.TryGetValue(symbol, out count))
        {
          count++;
        }
        NamesToCounts[symbol] = count;

        if (count == 0)
        {
          return symbol;
        }

        string res = $"{symbol}{count}";
        return res;
      }

    }
  }

  // ==============================================================================================================================
  internal class CreateDOMEmitter : IEmitter
  {
    // --------------------------------------------------------------------------------------------------------------------------
    public string GetCreateDOMFunction(TemplateDefinition def, string outputDir)
    {
      var nameContext = new NamingContext();

      CodeFile cf = new CodeFile();

      // Maybe a way to do the dynamic strings?
      // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals

      // Walk the tree and create elements + children as we go....
      Node root = def.DOM;

      cf.WriteLine($"function CreateDOM(): HTMLElement ");
      cf.OpenBlock();

      string nodeSymbol = nameContext.GetUniqueNameFor("node");
      root.Symbol = nodeSymbol;

      cf.WriteLine($"let {nodeSymbol} = document.createElement('{root.Name}');");

      AddAttributes(cf, root, nodeSymbol);

      // Now we need to populate the child elements....
      CreateChildElements(cf, root, nameContext);

      cf.NextLine();
      cf.WriteLine("return node;");

      cf.CloseBlock();


      FileTools.CreateDirectory(outputDir);
      string path = Path.Combine(outputDir, "test-output.ts");
      cf.Save(path);

      string res = cf.GetString();
      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private static void AddAttributes(CodeFile cf, Node toNode, string nodeSymbol)
    {
      foreach (var item in toNode.Attributes)
      {
        cf.WriteLine($"{nodeSymbol}.setAttribute('{item.Name}', '{item.Value}');");
      }
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private void CreateChildElements(CodeFile cf, Node parent, NamingContext nameContext)
    {
      foreach (var item in parent.Children)
      {
        if (item.Name == "<text>")
        {
          // NOTE: This may have dynamic text attached....
          string? useValue = FormatText(item.Value);
          if (!string.IsNullOrWhiteSpace(useValue))
          {
            cf.WriteLine($"{parent.Symbol}.insertAdjacentText('beforeend', '{useValue}');");
          }

          continue;
        }
        else {
          string symbol = nameContext.GetUniqueNameFor("child");
          item.Symbol = symbol;

          cf.NextLine();
          cf.WriteLine($"let {symbol} = document.createElement('{item.Name}');");

          // Attributes.
          AddAttributes(cf, item, symbol);

          // Now its child elements too....
          CreateChildElements(cf, item, nameContext);

          // Add the child node to the parent....
          cf.WriteLine($"{parent.Symbol}.append({symbol});");
        }

      }

      // throw new NotImplementedException();
    }

    // --------------------------------------------------------------------------------------------------------------------------
    private string? FormatText(string? value)
    {
      if (string.IsNullOrEmpty(value)) { return value; }
      
      string res = value.Replace("\r", " ");
      res = res.Replace("\n", " ");

      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public EmitterResults Emit(string outputDir, dhllFile file)
    {
      throw new NotImplementedException();
    }

    // --------------------------------------------------------------------------------------------------------------------------
    public string TranslateTypeName(string typeName)
    {
      throw new NotImplementedException();
    }

  }
}
