using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser
{
  public class FunctionModifier
  {
    public SyntaxToken Token { get; }
    public FunctionModifierKind Kind { get; }

    public FunctionModifier(SyntaxToken token, FunctionModifierKind kind)
    {
      Token = token;
      Kind = kind;
    }

    public static FunctionModifierKind? ParseKind(SyntaxTokenKind kind)
    {
      switch (kind)
      {
        case SyntaxTokenKind.PublicKeyword:
          return FunctionModifierKind.Public;
        case SyntaxTokenKind.InternalKeyword:
          return FunctionModifierKind.Internal;
        case SyntaxTokenKind.ProtectedKeyword:
          return FunctionModifierKind.Protected;
        case SyntaxTokenKind.PrivateKeyword:
          return FunctionModifierKind.Private;
        case SyntaxTokenKind.StaticKeyword:
          return FunctionModifierKind.Static;
      }

      return null;
    }
  }
}