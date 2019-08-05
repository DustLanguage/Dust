using System;
using System.Linq;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class LiteralParserTests : ParserTests
  {
    [Theory]
    [InlineData("\"\"", "")]
    [InlineData("\"a string\"", "a string")]
    [InlineData("'single quote string'", "single quote string")]
    public void StringLiteral(string code, string expectedValue)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateLiteralExpression(tokens.First(), expectedValue));
    }
    
    [Theory]
    [InlineData("12345678", 12345678)]
    [InlineData("12.33", 12.33f)]
    [InlineData("420.69f", 420.69f)]
    [InlineData("420.69d", 420.69)]
    public void NumericLiteral(string code, object expectedValue)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateLiteralExpression(tokens.First(), expectedValue));
    }
    
    private CodeBlockNode CreateLiteralExpression(SyntaxToken token, object value)
    {
      Expression expression = (Expression) Activator.CreateInstance(typeof(LiteralExpression), token, value);

      root.Children.Add(new ExpressionStatement(expression));
      
      return root;
    }
  }
}