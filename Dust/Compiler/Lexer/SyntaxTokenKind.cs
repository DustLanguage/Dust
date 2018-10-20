namespace Dust.Compiler.Lexer
{
  public enum SyntaxTokenKind
  {
    // Simple tokens
    OpenParenthesis,
    CloseParenthesis,
    OpenBrace,
    CloseBrace,
    OpenBracket,
    CloseBracket,
    Dot,
    Comma,
    Equals,
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
    NullKeyword,
    PublicKeyword,
    InternalKeyword,
    ProtectedKeyword,
    PrivateKeyword,
    StaticKeyword,

    // Invisible tokens
    EndOfLine,
    EndOfFile,

    Invalid
  }
}