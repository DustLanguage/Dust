using System;

namespace Dust.Extensions
{
  public static class TypeExtension
  {
    public static bool Extends(this Type type, Type other)
    {
      return other.IsAssignableFrom(type) && type != other;
    }
  }
}