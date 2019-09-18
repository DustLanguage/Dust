using System;

namespace Dust.Compiler.Types
{
  public class DustBool : DustObject
  {
    public bool Value { get; }

    public DustBool(bool value)
      : this()
    {
      Value = value;
    }

    public DustBool()
      : base("bool")
    {
    }

    public override Type ToNativeType()
    {
      return typeof(bool);
    }
  }
}