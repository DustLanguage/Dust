using System;
using System.Collections.Generic;
using System.Linq;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class VariableDeclarationParserTests : ParserTests
  {
    private bool isMutable;
    private List<SyntaxToken> identifiers;

    protected override void Setup(string code)
    {
      base.Setup(code);

      isMutable = tokens.Any((token) => token.Is(SyntaxTokenKind.MutKeyword));
      identifiers = tokens.FindAll((token) => token.Is(SyntaxTokenKind.Identifier));
    }

    [Theory]
    [InlineData("let var: int")]
    [InlineData("mut var: int")]
    public void SimpleVariable(string code)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateVariableDeclaration(identifiers.First(), identifiers[1]));
    }

    [Theory]
    [InlineData("let var: int = 10", 10)]
    [InlineData("let var: string = 'string'", "string")]
    [InlineData("mut var: string = 'string'", "string")]
    [InlineData("mut var = 'string'", "string", false)]
    [InlineData("let var = 'string'", "string", false)]
    public void VariableWithInitializer(string code, object value, bool hasType = true)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateVariableDeclaration(identifiers.First(), hasType ? identifiers[1] : null, CreateLiteralExpression(tokens[tokens.Count - 2], value)));
    }

    private CodeBlockNode CreateVariableDeclaration(SyntaxToken nameToken, SyntaxToken typeToken, Expression initializer = null)
    {
      if (root == null)
      {
        throw new Exception("You forgot to call Setup(code)");
      }

      root.Children.Add(new VariableDeclaration(tokens.First(), nameToken, isMutable, typeToken, initializer));

      return root;
    }

    private static Expression CreateLiteralExpression(SyntaxToken token, object value)
    {
      Expression expression = (Expression) Activator.CreateInstance(typeof(LiteralExpression<>).MakeGenericType(value.GetType()), token, value);

      return expression;
    }
  }
}