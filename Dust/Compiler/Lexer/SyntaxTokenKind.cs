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
    EndOfFile,
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
    StaticKeyword
  }
}