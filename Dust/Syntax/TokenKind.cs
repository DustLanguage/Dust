namespace Dust.Syntax
{
  public enum TokenKind
  {
    // Simple tokens
    OpenParentheses,
    CloseParentheses,
    OpenBrace,
    CloseBrace,
    OpenBracket,
    CloseBracket,
    Plus,
    PlusEquals,
    PlusPlus,
    Minus,
    MinusEquals,
    MinusMinus,
    Asterisk,
    AsteriskEquals,
    AsteriskAsterisk,
    Slash,
    SlashEquals,
    SlashSlash,
    // Literals
    NumericLiteral,
    StringLiteral,
    Identifier,
    // Keywords
    LetKeyword,
    FnKeyword,
    MutKeyword,
    ReturnKeyword,
    TypeOfKeyword,
    TrueKeyword,
    FalseKeyword,
    IfKeyword,
    ElseKeyword,
    ElifKeyword,
    NullKeyword
  }
}