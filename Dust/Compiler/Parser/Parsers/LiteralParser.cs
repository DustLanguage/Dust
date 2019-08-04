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
          return ParseString(token);
        case SyntaxTokenKind.IntLiteral:
          return ParseInt(token);
        case SyntaxTokenKind.FloatLiteral:
          return ParseFloat(token);
        case SyntaxTokenKind.DoubleLiteral:
          return ParseDouble(token);
        default:
          Parser.Revert();

          return null;
      }
    }

    private static LiteralExpression<string> ParseString(SyntaxToken token)
    {
      return new LiteralExpression<string>(token, token.Text);
    }

    private static LiteralExpression<int> ParseInt(SyntaxToken token)
    {
      return new LiteralExpression<int>(token, int.Parse(token.Text));
    }

    private static LiteralExpression<float> ParseFloat(SyntaxToken token)
    {
      return new LiteralExpression<float>(token, float.Parse(token.Text));
    }

    private static LiteralExpression<double> ParseDouble(SyntaxToken token)
    {
      return new LiteralExpression<double>(token, double.Parse(token.Text));
    }
  }
}