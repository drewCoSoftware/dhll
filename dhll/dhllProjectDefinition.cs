using drewCo.Tools;
using System.Text.Json.Serialization;
using IOPath = System.IO.Path;

namespace dhll;

// ==============================================================================================================================
/// <summary>
/// A JSON file that describes a 'project' composed of many dhll files.
/// All paths use unix stlye separators and are relative to this file.
/// </summary>
public class dhllProjectDefinition
{
  public const string DEFAULT_OUTPUT_DIR = "build";

  /// <summary>
  /// The path of this file.
  /// </summary>
  [JsonIgnore]
  public string Path { get; private set; } = null!;

  public IList<string> InputFiles { get; set; } = new List<string>();

  /// <summary>
  /// Global output directory for all OutputTargets that don't specify an OutputDir.
  /// </summary>
  public string OutputDir { get; set; } = DEFAULT_OUTPUT_DIR;

  /// <summary>
  /// We need a way to target multiple output targets...
  /// </summary>
  public Dictionary<string, OutputTarget> OutputTargets { get; set; } = new Dictionary<string, OutputTarget>();


  // --------------------------------------------------------------------------------------------------------------------------
  [JsonConstructor]
  public dhllProjectDefinition() { }

  // --------------------------------------------------------------------------------------------------------------------------
  public dhllProjectDefinition(string path_)
  {
    if (path_ == null) { throw new ArgumentNullException(nameof(path_)); }

    this.Path = FileTools.GetRootedPath(path_);
    Save();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void Save()
  {
    if (this.Path == null)
    {
      throw new InvalidOperationException("Project file has no path set!");
    }

    string? dir = System.IO.Path.GetDirectoryName(this.Path);
    if (dir == null) { throw new InvalidOperationException($"Could not resolve the directory from path: {Path}!"); }

    FileTools.CreateDirectory(dir);
    FileTools.SaveJson<dhllProjectDefinition>(this.Path, this);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void AddFile(string path)
  {
    if (this.Path == null)
    {
      throw new InvalidOperationException("Project file has no path set!");
    }

    string relPath = FileTools.GetRelativePath(this.Path, path);
    InputFiles.Add(relPath);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public static dhllProjectDefinition Load(string path)
  {
    if (!File.Exists(path)) { throw new FileNotFoundException($"The file at path: {path} does not exist!"); }

    var res = FileTools.LoadJson<dhllProjectDefinition>(path);
    if (res == null)
    {
      throw new InvalidOperationException($"The file at path: {path} could not be deserialized into an instance of: {typeof(dhllProjectDefinition)}!");
    }
    res.Path = path;

    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public string GetFullPath(string relPath)
  {
    if (this.Path == null) { throw new InvalidOperationException("There is no base path!"); }

    string dir = IOPath.GetDirectoryName(this.Path);
    string res = IOPath.GetFullPath(IOPath.Combine(dir, relPath));

    return res;
    // throw new NotImplementedException();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Compute the output directory for the given output target.
  /// </summary>
  internal string ComputeOutputDir(OutputTarget target)
  {
    string res = target.OutputDir;
    if (res == null)
    {
      res = IOPath.GetFullPath(IOPath.Combine(Directory.GetCurrentDirectory(), this.OutputDir));
      res = IOPath.Combine(res, target.Name);
    }

    return res;
  }
}


// ==============================================================================================================================
public class OutputTarget
{
  /// <summary>
  /// Unique name for this output target.
  /// </summary>
  // NOTE: Name and corresponding dictionary key in Project Def must match!
  public string Name { get; set; } = default!;

  public string TargetLanguage { get; set; } = default!;

  /// <summary>
  /// Optional output directory.  If this is not set, a default/global one will be used.
  /// </summary>
  public string? OutputDir { get; set; }
}

// ==============================================================================================================================
public class dllprojFile
{
  public string Path { get; set; }
  public string FileType { get; set; }
}