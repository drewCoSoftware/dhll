using drewCo.Curations;
using drewCo.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IOPath = System.IO.Path;

namespace dhll;
// ==============================================================================================================================
/// <summary>
/// A JSON file that describes a 'project' composed of many dhll files.
/// All paths use unix stlye separators and are relative to this file.
/// </summary>
public class dhlprojFile
{
  /// <summary>
  /// The path of this file.
  /// </summary>
  [JsonIgnore]
  public string Path { get; private set; } = null!;

  public List<string> Files { get; set; } = new List<string>();
  //ExternalReadonlyList<string> _Files = new ExternalReadonlyList<string>();
  //public ReadOnlyCollection<string> Files { get { return _Files.External; } }

  // --------------------------------------------------------------------------------------------------------------------------
  [JsonConstructor]
  private dhlprojFile() { }

  // --------------------------------------------------------------------------------------------------------------------------
  public dhlprojFile(string path_)
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
    FileTools.SaveJson<dhlprojFile>(this.Path, this);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void AddFile(string path)
  {
    if (this.Path == null)
    {
      throw new InvalidOperationException("Project file has no path set!");
    }

    string relPath = FileTools.GetRelativePath(this.Path, path);
    Files.Add(relPath);
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public static dhlprojFile Load(string path)
  {
    if (!File.Exists(path)) { throw new FileNotFoundException($"The file at path: {path} does not exist!"); }

    var res = FileTools.LoadJson<dhlprojFile>(path);
    if (res == null)
    {
      throw new InvalidOperationException($"The file at path: {path} could not be deserialized into an instance of: {typeof(dhlprojFile)}!");
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
}

// ==============================================================================================================================
public class dllprojFile
{
  public string Path { get; set; }
  public string FileType { get; set; }
}