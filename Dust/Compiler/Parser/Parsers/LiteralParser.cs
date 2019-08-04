using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;

namespace Dust.Compiler.Parser.Parsers
{
  public class LiteralParser : SyntaxParserExtension
  {
    public LiteralParser(SyntaxParser parser)
      : base(parser)
    {
    }

    public Expression TryParse()
    {
      SyntaxToken token = Parser.CurrentToken;

      Parser.Advance();

      switch (token.Kind)
      {
        case SyntaxTokenKind.StringLiteral:
          return new LiteralExpression(token, token.Text);
        case SyntaxTokenKind.IntLiteral:
          return new LiteralExpression(token, int.Parse(token.Text));
        case SyntaxTokenKind.FloatLiteral:
          return new LiteralExpression(token, float.Parse(token.Text));
        case SyntaxTokenKind.DoubleLiteral:
          return new LiteralExpression(token, double.Parse(token.Text));
        default:
          Parser.Revert();

          return null;
      }
    }
  }
}