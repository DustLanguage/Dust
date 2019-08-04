using System;

namespace Dust.Compiler.Types
{
  public static class DustTypes
  {
    public static DustType Void => new DustVoid();
    public static DustType Int => new DustInt();
    public static DustType Float => new DustFloat();
    public static DustType Double => new DustDouble();
    public static DustType Number => new DustNumber();
    public static DustType Bool => new DustBool();

    public static DustType TypeOf(object value)
    {
      if (value is int)
      {
        return Int;
      }

      if (value is float)
      {
        return Float;
      }

      if (value is double)
      {
        return Double;
      }

      if (value is bool)
      {
        return Bool;
      }

      throw new Exception($"{nameof(TypeOf)} not implemented for `{value.GetType()}`");
    }

    public static DustType BestTypeFor(DustType type1, DustType type2)
    {
      return type1.Rank > type2.Rank ? type1 : type2;
    }
  }
}