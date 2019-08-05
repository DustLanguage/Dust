using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;

namespace Dust.Compiler.Diagnostics
{
  public static class Errors
  {
    public static Error ModifierAlreadySeen(SyntaxToken modifierToken) => CreateError($"modifier '{AccessModifier.ParseKind(modifierToken.Kind)}' already seen", modifierToken);
    public static Error IncombinableModifier(SyntaxToken modiferToken, AccessModifierKind otherKind) => CreateError($"modifier '{AccessModifier.ParseKind(modiferToken.Kind)}' can't be combined with modifier '{otherKind}'", modiferToken);
    public static Error UnexpectedToken(SyntaxToken actualToken, SyntaxTokenKind expectedKind) => CreateError($"unexpected token <{actualToken.Kind}>, expected <{expectedKind}>", actualToken);
    public static Error ExpectedToken(SyntaxToken token, SyntaxTokenKind expectedKind) => CreateError($"expected token <{expectedKind}> but couldn't find it", token);
    public static Error UnexpectedTokenGlobal(SyntaxToken token) => CreateError($"unexpected token '{token.Kind}'", token);
    public static Error UnknownType(SyntaxToken typeToken) => CreateError($"unknown type '{typeToken.Text}'", typeToken);

    private static Error CreateError(string message, SyntaxToken token)
    {
      return new Error(message, token.Range);
    }
  }
}