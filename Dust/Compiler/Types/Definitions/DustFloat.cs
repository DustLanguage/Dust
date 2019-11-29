using System;
using System.Globalization;

namespace Dust.Compiler.Types
{
  public class DustFloat : DustNumber
  {
    public float Value { get; }
    public override int Rank => 2;

    public DustFloat(float value)
      : base("float")
    {
      Value = value;
    }

    public DustFloat()
      : base("float")
    {
    }

    public override DustObject Add(DustObject other)
    {
      return new DustFloat(Value + other.ToFloat());
    }

    public override DustObject Subtract(DustObject other)
    {
      return new DustFloat(Value - other.ToFloat());
    }

    public override DustObject Multiply(DustObject other)
    {
      return new DustFloat(Value * other.ToFloat());
    }

    public override DustObject Divide(DustObject other)
    {
      return new DustFloat(Value / other.ToFloat());
    }

    public override DustInt ToInt()
    {
      return (int) Value;
    }

    public override DustFloat ToFloat()
    {
      return this;
    }

    public override DustDouble ToDouble()
    {
      return (double) Value;
    }

    public override Type ToNativeType()
    {
      return typeof(float);
    }

    public override DustString ToString()
    {
      return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static implicit operator DustFloat(float value)
    {
      return new DustFloat(value);
    }

    public static implicit operator float(DustFloat dustFloat)
    {
      return dustFloat.Value;
    }
  }
}