
using drewCo.Tools;

namespace dhllTesters
{
  // ==============================================================================================================================
  public class TestBase
  {
    protected string TestDir = null!;

    // --------------------------------------------------------------------------------------------------------------------------
    public TestBase()
    {
      TestDir = FileTools.GetLocalDir("TestInputs");
    }

    // --------------------------------------------------------------------------------------------------------------------------
    protected string GetTestInputPath(string name)
    {
      string res = Path.Combine(TestDir, name);

      return res;
    }

    // --------------------------------------------------------------------------------------------------------------------------
    protected string LoadTestInput(string name)
    {
      string inputPath = GetTestInputPath(name);  
      if (!File.Exists(inputPath))
      {
        throw new FileNotFoundException($"The input file at path: {inputPath} does not exist!");
      }

      string data = File.ReadAllText(inputPath);
      return data;
    }
  }


}