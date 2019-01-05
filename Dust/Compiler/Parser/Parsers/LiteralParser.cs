using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Parser.AbstractSyntaxTree.Expressions;

namespace Dust.Compiler.Parser.Parsers
{
  public class LiteralParser : SyntaxParserExtension
  {
    public LiteralParser(SyntaxParser parser)
      : base(parser)
    {
    }

    private LiteralExpression<string> ParseString()
    {
      return new LiteralExpression<string>(Parser.CurrentToken.Text)
      {
        Range = Parser.CurrentToken.Range
      };
    }

    private LiteralExpression<int> ParseInt()
    {
      return new LiteralExpression<int>(int.Parse(Parser.CurrentToken.Text))
      {
        Range = Parser.CurrentToken.Range
      };
    }

    private LiteralExpression<float> ParseFloat()
    {
      return new LiteralExpression<float>(float.Parse(Parser.CurrentToken.Text))
      {
        Range = Parser.CurrentToken.Range
      };
    }

    private LiteralExpression<double> ParseDouble()
    {
      return new LiteralExpression<double>(double.Parse(Parser.CurrentToken.Text))
      {
        Range = Parser.CurrentToken.Range
      };
    }

    public Expression ParseLiteral()
    {
      switch (Parser.CurrentToken.Kind)
      {
        case SyntaxTokenKind.StringLiteral:
          return ParseString();
        case SyntaxTokenKind.IntLiteral:
          return ParseInt();
        case SyntaxTokenKind.FloatLiteral:
          return ParseFloat();
        case SyntaxTokenKind.DoubleLiteral:
          return ParseDouble();
        default:
          // TODO: This is not a literal, what do we do now?

          return null;
      }
    }
  }
}