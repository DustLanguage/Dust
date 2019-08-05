namespace Dust.Compiler.Parser
{
  public enum AccessModifierKind
  {
    Public,
    Internal,
    Protected,
    Private,
    Static
  }

  public static class AccessModifierKindExtensions
  {
    public static string ToString(AccessModifierKind kind)
    {
      return kind.ToString().ToLower();
    }
  }
}