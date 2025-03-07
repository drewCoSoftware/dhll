namespace dhll.Emitters;

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



