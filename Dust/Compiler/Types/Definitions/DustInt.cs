using System;

namespace Dust.Compiler.Types
{
  public class DustInt : DustNumber
  {
    public int Value { get; }
    public override int Rank => 1;

    public DustInt(int value)
      : this()
    {
      Value = value;
    }

    public DustInt()
      : base("int")
    {
    }

    public override DustObject Add(DustObject other)
    {
      return new DustInt(Value + other.ToInt());
    }

    public override DustObject Subtract(DustObject other)
    {
      return new DustInt(Value - other.ToInt());
    }

    public override DustObject Multiply(DustObject other)
    {
      return new DustInt(Value * other.ToInt());
    }

    public override DustObject Divide(DustObject other)
    {
      return new DustFloat(Value / other.ToFloat());
    }

    public override DustInt ToInt()
    {
      return this;
    }

    public override DustFloat ToFloat()
    {
      return (float) Value;
    }

    public override DustDouble ToDouble()
    {
      return (double) Value;
    }

    public override Type ToNativeType()
    {
      return typeof(int);
    }

    public override string ToString()
    {
      return Value.ToString();
    }

    public static implicit operator DustInt(int value)
    {
      return new DustInt(value);
    }

    public static implicit operator int(DustInt dustInt)
    {
      return dustInt.Value;
    }
  }
}