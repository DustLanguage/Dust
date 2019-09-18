using System;

namespace Dust.Compiler.Types
{
  public class DustObject : DustType
  {
    public DustObject()
      : base("object")
    {
    }

    protected DustObject(string typeName, DustType superType = null)
      : base(typeName, superType ?? DustTypes.Object)
    {
    }

    public virtual DustObject Add(DustObject other)
    {
      return null;
    }

    public virtual DustObject Subtract(DustObject other)
    {
      return null;
    }

    public virtual DustObject Multiply(DustObject other)
    {
      return null;
    }

    public virtual DustObject Divide(DustObject other)
    {
      return null;
    }

    public virtual DustInt ToInt()
    {
      throw new Exception($"Cannot convert type {TypeName} to type int.");
      // return null;
    }

    public virtual DustFloat ToFloat()
    {
      throw new Exception($"Cannot convert type {TypeName} to type float.");
      // return null;
    }

    public virtual DustDouble ToDouble()
    {
      throw new Exception($"Cannot convert type {TypeName} to type double.");
      // return null;
    }
  }
}