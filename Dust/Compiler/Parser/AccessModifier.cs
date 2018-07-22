using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser
{
  public class AccessModifier
  {
    public SyntaxToken Token { get; }
    public AccessModifierKind Kind { get; }

    public AccessModifier(SyntaxToken token, AccessModifierKind kind)
    {
      Token = token;
      Kind = kind;
    }

    public static AccessModifierKind? ParseKind(SyntaxTokenKind kind)
    {
      switch (kind)
      {
        case SyntaxTokenKind.PublicKeyword:
          return AccessModifierKind.Public;
        case SyntaxTokenKind.InternalKeyword:
          return AccessModifierKind.Internal;
        case SyntaxTokenKind.ProtectedKeyword:
          return AccessModifierKind.Protected;
        case SyntaxTokenKind.PrivateKeyword:
          return AccessModifierKind.Private;
        case SyntaxTokenKind.StaticKeyword:
          return AccessModifierKind.Static;
      }

      return null;
    }
  }
}