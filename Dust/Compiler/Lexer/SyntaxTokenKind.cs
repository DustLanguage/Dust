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
    Colon,
    Bang,
    Equal,
    NotEqual,
    EqualsEquals,
    Plus,
    PlusEquals,
    PlusPlus,
    Minus,
    MinusEquals,
    MinusMinus,
    Asterisk,
    AsteriskEquals,
    AsteriskAsterisk,
    AsteriskAsteriskEquals,
    Slash,
    SlashEquals,
    SlashSlash,
    Percent,
    PercentEquals,
    Ampersand,
    AmpersandAmpersand,
    Pipe,
    PipePipe,
    GreaterThan,
    GreaterThanEqual,
    LessThan,
    LessThanEqual,

    // Literals
    IntLiteral,
    FloatLiteral,
    DoubleLiteral,
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

    Invalid,
  }
}