using System;
using System.Linq;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class BinaryExpressionParserTests : ParserTests
  {
    [Theory]
    [InlineData("11+11")]
    [InlineData("11 + 11")]
    [InlineData("11 - 11")]
    [InlineData("11 * 11")]
    [InlineData("11 / 11")]
    [InlineData("11 % 11")]
    public void SimpleArithmeticBinaryExpressions(string code)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateLiteralBinaryExpression(tokens.First(), 11, tokens[1], tokens[2], 11));
    }

    [Theory]
    [InlineData("11+=11")]
    [InlineData("11 += 11")]
    [InlineData("11 -= 11")]
    [InlineData("11 *= 11")]
    [InlineData("11 /= 11")]
    [InlineData("11 %= 11")]
    [InlineData("11 ** 11")]
    public void TwoCharArithmeticBinaryExpressions(string code)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateLiteralBinaryExpression(tokens.First(), 11, tokens[1], tokens[2], 11));
    }

    [Theory]
    [InlineData("11 == 11")]
    [InlineData("11 != 11")]
    [InlineData("11 && 11")]
    [InlineData("11 || 11")]
    [InlineData("11 > 11")]
    [InlineData("11 >= 11")]
    [InlineData("11 < 11")]
    [InlineData("11 <= 11")]
    public void BooleanBinaryExpressions(string code)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateLiteralBinaryExpression(tokens.First(), 11, tokens[1], tokens[2], 11));
    }

    [Theory]
    [InlineData("11**=11")]
    [InlineData("11 **= 11")]
    public void ThreeCharBinaryExpressions(string code)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateLiteralBinaryExpression(tokens.First(), 11, tokens[1], tokens[2], 11));
    }

    private CodeBlockNode CreateLiteralBinaryExpression(SyntaxToken leftToken, int leftValue, SyntaxToken operatorToken, SyntaxToken rightToken, int rightValue)
    {
      BinaryExpression expression = new BinaryExpression(CreateLiteralExpression(leftToken, leftValue), operatorToken, CreateLiteralExpression(rightToken, rightValue));

      root.Children.Add(new ExpressionStatement(expression));

      return root;
    }

    private static Expression CreateLiteralExpression(SyntaxToken token, object value)
    {
      Expression expression = (Expression) Activator.CreateInstance(typeof(LiteralExpression<>).MakeGenericType(value.GetType()), token, value);

      return expression;
    }
  }
}