using System;
using System.Linq;
using Dust.Compiler;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;
using Dust.Compiler.Types;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class VariableDeclarationParserTests : ParserTests
  {
    private bool isMutable;

    protected override void Setup(string code)
    {
      base.Setup(code);

      isMutable = tokens.Any((token) => token.Is(SyntaxTokenKind.MutKeyword));
    }

    [Theory]
    [InlineData("let int var")]
    [InlineData("mut int var")]
    public void SimpleVariable(string code)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateVariableDeclaration("var", DustTypes.Int));
    }

    [Theory]
    [InlineData("let int var = 10", 10)]
    [InlineData("let int var = 'string'", "string")]
    [InlineData("mut int var = 'string'", "string")]
    public void VariableWithInitializer(string code, object value)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateVariableDeclaration("var", DustTypes.Int, CreateLiteralExpression(value, tokens[tokens.Count - 2].Range)));
    }

    private CodeBlockNode CreateVariableDeclaration(string name, DustType type, Expression initializer = null)
    {
      if (root == null)
      {
        throw new Exception("You forgot to call Setup(code)");
      }

      ExpressionStatement initializerStatement = initializer != null ? new ExpressionStatement(initializer) : null;

      root.Children.Add(new VariableDeclaration(name, isMutable, type, initializerStatement, range));

      return root;
    }

    private static Expression CreateLiteralExpression(object value, SourceRange range)
    {
      Expression expression = (Expression) Activator.CreateInstance(typeof(LiteralExpression<>).MakeGenericType(value.GetType()), value);

      expression.Range = range;

      return expression;
    }
  }
}