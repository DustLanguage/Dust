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

    private LiteralExpression<int> ParseInt(int multiplier)
    {
      return new LiteralExpression<int>(int.Parse(Parser.CurrentToken.Text) * multiplier)
      {
        Range = Parser.CurrentToken.Range
      };
    }

    private LiteralExpression<float> ParseFloat(int multiplier)
    {
      return new LiteralExpression<float>(float.Parse(Parser.CurrentToken.Text) * multiplier)
      {
        Range = Parser.CurrentToken.Range
      };
    }

    private LiteralExpression<double> ParseDouble(int multiplier)
    {
      return new LiteralExpression<double>(double.Parse(Parser.CurrentToken.Text) * multiplier)
      {
        Range = Parser.CurrentToken.Range
      };
    }

    public Expression ParseLiteral()
    {
      bool isNegative = Parser.CurrentToken.Is(SyntaxTokenKind.Minus);

      int multiplier = isNegative ? -1 : 1;
      
      Parser.Advance();
      
      switch (Parser.CurrentToken.Kind)
      {
        case SyntaxTokenKind.StringLiteral:
          return ParseString();
        case SyntaxTokenKind.IntLiteral:
          return ParseInt(multiplier);
        case SyntaxTokenKind.FloatLiteral:
          return ParseFloat(multiplier);
        case SyntaxTokenKind.DoubleLiteral:
          return ParseDouble(multiplier);
        default:
          // TODO: This is not a literal, what do we do now?

          return null;
      }
    }
  }
}