namespace Dust.Extensions
{
  public static class StringExtension
  {
    public static string SubstringRange(this string value, int start, int end)
    {
      return value.Substring(start, end - start);
    }
  }
}