using System;
using System.Globalization;

namespace Dust.Compiler.Types
{
  public class DustDouble : DustNumber
  {
    public double Value { get; }
    public override int Rank => 3;

    public DustDouble(double value)
      : this()
    {
      Value = value;
    }

    public DustDouble()
      : base("double")
    {
    }

    public override DustObject Add(DustObject other)
    {
      return new DustDouble(Value + other.ToDouble());
    }

    public override DustObject Subtract(DustObject other)
    {
      return new DustDouble(Value - other.ToDouble());
    }

    public override DustObject Multiply(DustObject other)
    {
      return new DustDouble(Value * other.ToDouble());
    }

    public override DustObject Divide(DustObject other)
    {
      return new DustDouble(Value / other.ToDouble());
    }

    public override DustInt ToInt()
    {
      return new DustInt(Convert.ToInt32(Value));
    }

    public override DustFloat ToFloat()
    {
      return new DustFloat(Convert.ToSingle(Value));
    }

    public override DustDouble ToDouble()
    {
      return Value;
    }

    public override Type ToNativeType()
    {
      return typeof(double);
    }

    public override DustString ToString()
    {
      return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static implicit operator DustDouble(double value)
    {
      return new DustDouble(value);
    }

    public static implicit operator double(DustDouble dustDouble)
    {
      return dustDouble.Value;
    }
  }
}