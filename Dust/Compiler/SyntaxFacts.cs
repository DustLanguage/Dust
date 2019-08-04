using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;

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
      return IsBinaryArithmeticOperator(token) || IsBinaryBooleanOperator(token) || IsAssignmentOperator(token);
    }

    public static bool IsBinaryArithmeticOperator(SyntaxToken token)
    {
      return token.IsOr(SyntaxTokenKind.Plus, SyntaxTokenKind.Minus, SyntaxTokenKind.Asterisk, SyntaxTokenKind.Slash, SyntaxTokenKind.AsteriskAsterisk, SyntaxTokenKind.Percent);
    }

    public static bool IsBinaryBooleanOperator(SyntaxToken token)
    {
      return token.IsOr(SyntaxTokenKind.EqualsEquals, SyntaxTokenKind.NotEqual, SyntaxTokenKind.AmpersandAmpersand, SyntaxTokenKind.PipePipe, SyntaxTokenKind.GreaterThan,
        SyntaxTokenKind.GreaterThanEqual, SyntaxTokenKind.LessThan, SyntaxTokenKind.LessThanEqual);
    }

    public static bool IsAssignmentOperator(SyntaxToken token)
    {
      return token.IsOr(SyntaxTokenKind.Equal, SyntaxTokenKind.PlusEquals, SyntaxTokenKind.MinusEquals, SyntaxTokenKind.AsteriskEquals, SyntaxTokenKind.SlashEquals,
        SyntaxTokenKind.AsteriskAsteriskEquals, SyntaxTokenKind.PercentEquals);
    }

    public static BinaryOperatorKind GetBinaryOperatorKind(SyntaxToken operatorToken)
    {
      return (BinaryOperatorKind) operatorToken.Kind;
    }
  }
}