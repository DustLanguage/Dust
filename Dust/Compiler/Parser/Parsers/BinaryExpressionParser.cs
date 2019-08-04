using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;

namespace Dust.Compiler.Parser.Parsers
{
  public class BinaryExpressionParser : SyntaxParserExtension
  {
    public BinaryExpressionParser(SyntaxParser parser) : base(parser)
    {
    }

    public BinaryExpression Parse(Expression left)
    {
      SyntaxToken operatorToken = Parser.CurrentToken;

      Parser.Advance();

      Expression right = Parser.ParseExpression();

      return new BinaryExpression(left, operatorToken, right);
    }
  }
}