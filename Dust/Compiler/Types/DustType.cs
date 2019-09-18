using System;

namespace Dust.Compiler.Types
{
  public class DustType
  {
    public string TypeName { get; }
    public DustType SuperType { get; }
    public virtual int Rank => 0;

    protected DustType(string typeName, DustType superType = null)
    {
      TypeName = typeName;
      SuperType = superType;
    }

    public virtual Type ToNativeType()
    {
      return null;
    }

    public bool IsAssignableFrom(DustType type)
    {
      // TODO: Support multiple levels of inheritance
      return this == type || this == type.SuperType;
    }

    public override string ToString()
    {
      return TypeName;
    }

    public static bool operator ==(DustType left, DustType right)
    {
      if ((object) left == null && (object) right == null)
      {
        return true;
      }

      return left?.Equals(right) ?? false;
    }

    public static bool operator !=(DustType left, DustType right)
    {
      return !(left == right);
    }

    public bool Equals(DustType other)
    {
      return TypeName == other.TypeName && SuperType == other.SuperType;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;

      return Equals((DustType) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int hashCode = (TypeName != null ? TypeName.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (SuperType != null ? SuperType.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ Rank;

        return hashCode;
      }
    }
  }
}