using System;
using System.Linq;

namespace Dust.Compiler.Types
{
  public class DustString : DustObject
  {
    public string Value { get; }

    public DustString(string value)
      : this()
    {
      Value = value;
    }

    public DustString()
      : base("string")
    {
    }

    public override DustObject Add(DustObject other)
    {
      return new DustString(Value + other.ToString());
    }

    public override DustObject Subtract(DustObject other)
    {
      if (other is DustString)
      {
        return new DustString(Value.Replace(other.ToString(), ""));
      }

      return base.Subtract(other);
    }

    public override DustObject Multiply(DustObject other)
    {
      if (other is DustInt)
      {
        return new DustString(string.Concat(Enumerable.Repeat(Value, other.ToInt())));
      }

      return base.Multiply(other);
    }

    public override DustObject Divide(DustObject other)
    {
      if (other is DustInt)
      {
        throw new NotImplementedException("Arrays are not implemented");
      }

      if (other is DustString)
      {
        throw new NotImplementedException("Arrays are not implemented");
      }

      return base.Divide(other);
    }

    public override DustString ToString()
    {
      return Value;
    }
    

    public static implicit operator DustString(string value)
    {
      return new DustString(value);
    }

    public static implicit operator string(DustString dustString)
    {
      return dustString.Value;
    }
  }
}