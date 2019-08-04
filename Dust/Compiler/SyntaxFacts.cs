using Dust.Compiler.Lexer;

namespace Dust.Compiler
{
  public static class SyntaxFacts
  {
    public static bool IsValidIdentiferOrKeywordCharacter(char character)
    {
      return char.IsLetterOrDigit(character) || character == '_';
    }

    public static bool IsNumeric(char character)
    {
      return char.IsDigit(character) || character == '.';
    }

    public static bool IsUnaryOperator(SyntaxToken token)
    {
      return token.IsOr(SyntaxTokenKind.Plus, SyntaxTokenKind.Minus, SyntaxTokenKind.Bang, SyntaxTokenKind.PlusPlus, SyntaxTokenKind.MinusMinus, SyntaxTokenKind.AsteriskAsterisk,
        SyntaxTokenKind.SlashSlash);
    }

    public static bool IsBinaryOperator(SyntaxToken token)
    {
      return token.IsOr(SyntaxTokenKind.Plus, SyntaxTokenKind.PlusEquals, SyntaxTokenKind.Minus, SyntaxTokenKind.MinusEquals, SyntaxTokenKind.Asterisk,
               SyntaxTokenKind.AsteriskEquals, SyntaxTokenKind.AsteriskAsterisk, SyntaxTokenKind.AsteriskAsteriskEquals, SyntaxTokenKind.Percent,
               SyntaxTokenKind.PercentEquals) || IsBooleanOperator(token);
    }

    public static bool IsBooleanOperator(SyntaxToken token)
    {
      return token.IsOr(SyntaxTokenKind.EqualsEquals, SyntaxTokenKind.NotEqual, SyntaxTokenKind.AmpersandAmpersand, SyntaxTokenKind.PipePipe, SyntaxTokenKind.GreaterThan,
        SyntaxTokenKind.GreaterThanEqual, SyntaxTokenKind.LessThan, SyntaxTokenKind.LessThanEqual);
    }
  }
}