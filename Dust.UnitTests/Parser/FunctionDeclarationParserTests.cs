using System.Collections.Generic;
using System.Linq;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.SyntaxTree;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class FunctionDeclarationParserTests : ParserTests
  {
    private List<SyntaxToken> identifiers;

    protected override void Setup(string code)
    {
      base.Setup(code);

      identifiers = tokens.FindAll((token) => token.Is(SyntaxTokenKind.Identifier));
    }

    [Theory]
    [InlineData("fn function() {}", true)]
    [InlineData("fn function()")]
    [InlineData("fn function {}", true)]
    [InlineData("fn function")]
    public void SimpleFunction(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode(identifiers.First(), hasBody: hasBody));
    }

    [Theory]
    [InlineData("fn function(): int {}", true)]
    [InlineData("fn function(): int")]
    [InlineData("fn function: int {}", true)]
    [InlineData("fn function: int")]
    public void SimpleFunctionWithReturnType(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode(identifiers.First(), null, identifiers.Last(), hasBody));
    }

    [Theory]
    [InlineData("fn function(param1: int) {}", true)]
    [InlineData("fn function(param1: int)")]
    public void FunctionWithOneParameter(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode(identifiers.First(), new List<FunctionParameter>
      {
        new FunctionParameter(identifiers[1], identifiers[2], false)
      }, hasBody: hasBody));
    }


    [Theory]
    [InlineData("fn function(mut param1: int, param2: string, param3: float, mut param4) {}", true)]
    [InlineData("fn function(mut param1: int, param2: string, param3: float, mut param4)")]
    public void FunctionWithMultipleParameters(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode(identifiers.First(), new List<FunctionParameter>
      {
        new FunctionParameter(identifiers[1], identifiers[2], true),
        new FunctionParameter(identifiers[3], identifiers[4], false),
        new FunctionParameter(identifiers[5], identifiers[6], false),
        new FunctionParameter(identifiers[7], null, true)
      }, hasBody: hasBody));
    }

    [Theory]
    [InlineData("fn function(mut param1: int, param2: string, param3: float, mut param4): int {}", true)]
    [InlineData("fn function(mut param1: int, param2: string, param3: float, mut param4): int")]
    public void FunctionWithMultipleParametersAndReturnType(string code, bool hasBody = false)
    {
      Setup(code);

      // TODO: Do this automagically
      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode(identifiers.First(), new List<FunctionParameter>
      {
        new FunctionParameter(identifiers[1], identifiers[2], true),
        new FunctionParameter(identifiers[3], identifiers[4], false),
        new FunctionParameter(identifiers[5], identifiers[6], false),
        new FunctionParameter(identifiers[7], null, true)
      }, identifiers.Last(), hasBody));
    }

    private CodeBlockNode CreateFunctionDeclarationNode(SyntaxToken functionNameToken, List<FunctionParameter> parameters = null, SyntaxToken returnTypeToken = null, bool hasBody = true)
    {
      if (parameters == null)
      {
        parameters = new List<FunctionParameter>();
      }

      root.Children.Add(new FunctionDeclaration(tokens.Find((token) => token.Is(SyntaxTokenKind.FnKeyword)), functionNameToken, parameters, tokens.Find((token) => token.Is(SyntaxTokenKind.CloseParenthesis)), hasBody
        ? new CodeBlockNode(tokens.Find((token) => token.Is(SyntaxTokenKind.OpenBrace))) {ClosingBrace = tokens.Find((token) => token.Is(SyntaxTokenKind.CloseBrace))}
        : null, returnTypeToken));

      return root;
    }
  }
}