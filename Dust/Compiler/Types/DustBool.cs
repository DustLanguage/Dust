using System;

namespace Dust.Compiler.Types
{
  public class DustBool : DustType
  {
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