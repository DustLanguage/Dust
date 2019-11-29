using System;

namespace Dust.Compiler.Types
{
  public static class DustTypes
  {
    public static DustType Void => new DustVoid();
    public static DustType Object => new DustObject();
    public static DustType Int => new DustInt();
    public static DustType Float => new DustFloat();
    public static DustType Double => new DustDouble();
    public static DustType Number => new DustNumber();
    public static DustType String => new DustString();
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

    public static DustObject FromNative(object value)
    {
      if (value is int intValue)
      {
        return new DustInt(intValue);
      }

      if (value is float floatValue)
      {
        return new DustFloat(floatValue);
      }

      if (value is double doubleValue)
      {
        return new DustDouble(doubleValue);
      }

      if (value is bool boolValue)
      {
        return new DustBool(boolValue);
      }

      if (value is string stringValue)
      {
        return new DustString(stringValue);
      }

      throw new Exception($"{nameof(FromNative)} not implemented for `{value.GetType()}`");
    }
  }
}