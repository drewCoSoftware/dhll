using dhll.v1;
using System.Runtime.Serialization;

namespace dhll;

// ==============================================================================================================================  
/// <summary>
/// Helps when adding/updating many typedefs in a TypeIndex instance.
/// </summary>
internal class UpdateToken : IDisposable
{
  private TypeIndex Parent;

  // --------------------------------------------------------------------------------------------------------------------------
  public UpdateToken(TypeIndex parent_)
  {
    Parent = parent_;
    Parent.SetUpdateFlag();
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public void Dispose()
  {
    Parent.RebuildIndex();
  }
}

// ==============================================================================================================================  
/// <summary>
/// This class keeps track of information for all types that are currently in the system.
/// This data is used for references, validation, etc.
/// </summary>
internal class TypeIndex
{
  /// <summary>
  /// All typedefs that we are aware of, keyed by name.
  /// </summary>
  private Dictionary<string, TypeDef> IdsToTypes = new Dictionary<string, TypeDef>();
  private object DataLock = new object();

  private bool IsInUpdateMode = false;
  public void SetUpdateFlag()
  {
    if (IsInUpdateMode)
    {
      throw new InvalidOperationException("Already in update mode!");
    }
    IsInUpdateMode = true;
  }


  // --------------------------------------------------------------------------------------------------------------------------
  public UpdateToken BeginUpdate()
  {
    var res = new UpdateToken(this);
    return res;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// If the given typedef is not in the system, it will be added.  Otherwise, the older version of the def
  /// will be updated.  Validation happens at this step as well.
  /// </summary>
  public void AddOrUpdateType(TypeDef td)
  {
    lock (DataLock)
    {
      if (IdsToTypes.TryGetValue(td.Identifier, out TypeDef typeDef))
      {
        // This is a basic implementation where we just remove it, and then re-add.
        // Not the most efficient, especially if there are a lot of types, but this will
        // do for now.
        IdsToTypes.Remove(td.Identifier);
      }
      IdsToTypes.Add(td.Identifier, td);

      if (!IsInUpdateMode)
      {
        RebuildIndex();
      }
    }
  }

  // --------------------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Build all useful index information + validate types + refs.
  /// </summary>
  internal void RebuildIndex()
  {
    // TODO: Setup any indexes / internal lookup tables as needed.

    // TODO:
    // Here we can validate the types.  This pretty much makes sure that all of the
    // members, etc. also have valid / detectable type names.

    IsInUpdateMode = false;

  }

  // --------------------------------------------------------------------------------------------------------------------------
  internal string GetMemberDataType(string typeId, string memberId)
  {
    // NOTE: This is the kind of function that can/could memoize certain results + return them
    // vs. having to dig back into the TypeDef instances themselves?
    if (!IdsToTypes.TryGetValue(typeId, out TypeDef typeDef))
    {
      throw new InvalidOperationException($"The type identifier: {typeId} does not exist in this index!");
    }
    var declare = typeDef.GetMember(memberId);
    if (declare == null)
    {
      throw new InvalidOperationException($"The member with identifier: {memberId} does not exist on type: {typeId}!");
    }

    return declare.TypeName;
  }

  // --------------------------------------------------------------------------------------------------------------------------
  public TypeDef? GetDataType(string forType)
  {
    if (this.IdsToTypes.TryGetValue(forType, out TypeDef res))
    {
      return res;
    }
    return null;
  }
}


