using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser
{
  public enum BinaryOperatorKind
  {
    Add = SyntaxTokenKind.Plus,
    Subtract = SyntaxTokenKind.Minus,
    Multiply = SyntaxTokenKind.Asterisk,
    Divide = SyntaxTokenKind.Slash,
    Modulo = SyntaxTokenKind.Percent,
    Exponentiate = SyntaxTokenKind.AsteriskAsterisk,
    Equal = SyntaxTokenKind.EqualsEquals,
    NotEqual = SyntaxTokenKind.NotEqual,
    And = SyntaxTokenKind.AmpersandAmpersand,
    Or = SyntaxTokenKind.PipePipe,
    GreaterThan = SyntaxTokenKind.GreaterThan,
    GreaterThanEqual = SyntaxTokenKind.GreaterThanEqual,
    LessThan = SyntaxTokenKind.LessThan,
    LessThanEqual = SyntaxTokenKind.LessThanEqual,
    Assign = SyntaxTokenKind.Equal,
    AddAssign = SyntaxTokenKind.PlusEquals,
    SubtractAssign = SyntaxTokenKind.MinusEquals,
    MultiplyAssign = SyntaxTokenKind.AsteriskEquals,
    DivideAssign = SyntaxTokenKind.SlashEquals,
    ModuloAssign = SyntaxTokenKind.PercentEquals,
    ExponentiateAssign = SyntaxTokenKind.AsteriskAsteriskEquals,
  }
}