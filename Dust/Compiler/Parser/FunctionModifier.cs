using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser
{
  public class FunctionModifier
  {
    public static FunctionModifier Public => new FunctionModifier();
    public static FunctionModifier Internal => new FunctionModifier();
    public static FunctionModifier Protected => new FunctionModifier();
    public static FunctionModifier Private => new FunctionModifier();
    public static FunctionModifier Static => new FunctionModifier();

    public static FunctionModifier Parse(SyntaxTokenKind kind)
    {
      switch (kind)
      {
        case SyntaxTokenKind.PublicKeyword:
          return Public;
        case SyntaxTokenKind.InternalKeyword:
          return Internal;
        case SyntaxTokenKind.ProtectedKeyword:
          return Protected;
        case SyntaxTokenKind.PrivateKeyword:
          return Private;
        case SyntaxTokenKind.StaticKeyword:
          return Static;
        default:
          return null;
      }
    }
  }
}