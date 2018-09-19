using System.Collections.Generic;
using System.Linq;
using Dust.Compiler;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Types;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class FunctionDeclarationParserTests : ParserTests
  {
    private SourceRange bodyRange;
    private List<AccessModifier> modifiers;

    protected override void Setup(string code)
    {
      base.Setup(code);

      List<SyntaxToken> tokens = lexer.Lex(code);

      SyntaxToken openBrace = tokens.Find((token) => token.Kind == SyntaxTokenKind.OpenBrace);

      if (openBrace != null)
      {
        bodyRange = new SourceRange(openBrace.Position, tokens.Find((token) => token.Kind == SyntaxTokenKind.CloseBrace).Position);
      }

      modifiers = tokens.Select((token) =>
      {
        AccessModifierKind? modifierKind = AccessModifier.ParseKind(token.Kind);

        return modifierKind != null ? new AccessModifier(token, modifierKind.Value) : null;
      }).Where((token) => token != null).ToList();
    }

    [Theory]
    [InlineData("fn function() {}")]
    [InlineData("fn function()")]
    [InlineData("fn function")]
    public void SimpleFunction(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function"));
    }

    [Theory]
    [InlineData("fn int function() {}")]
    [InlineData("fn int function()")]
    [InlineData("fn int function")]
    public void SimpleFunctionWithReturnType(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", returnType: DustTypes.Int));
    }

    [Theory]
    [InlineData("public fn function() {}")]
    [InlineData("private fn int function()")]
    [InlineData("static internal fn int function")]
    public void SimpleFunctionWithAccessModifiers(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", returnType: DustTypes.Int));
    }

    [Theory]
    [InlineData("fn function(int param1) {}")]
    [InlineData("fn function(int param1)")]
    public void FunctionWithOneParameter(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", null, true)
      }));
    }

    [Theory]
    // TODO: More parameters once Rider fixes this
    [InlineData("fn function(mut int param1, string param2) {}")]
    [InlineData("fn function(mut int param1, string param2)")]
    public void FunctionWithMultipleParameters(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", null, true),
        new FunctionParameter("param2", null, false)
      }));
    }

    [Theory]
    // TODO: More parameters once Rider fixes this
    [InlineData("fn int function(mut int param1, string param2) {}")]
    [InlineData("fn int function(mut int param1, string param2)")]
    public void FunctionWithMultipleParametersAndReturnType(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", null, true),
        new FunctionParameter("param2", null, false)
      }, DustTypes.Int));
    }

    [Theory]
    [InlineData("fn int function(mut int param1, string param2) {}")]
    [InlineData("fn int function(mut int param1, string param2)")]
    public void FunctionWithMultipleParametersAndAccessModifiers(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", null, true),
        new FunctionParameter("param2", null, false)
      }, DustTypes.Int));
    }

    private CodeBlockNode CreateFunctionDeclarationNode(string functionName, List<FunctionParameter> parameters = null, DustType returnType = null)
    {
      if (parameters == null)
      {
        parameters = Lists.Empty<FunctionParameter>();
      }

      if (returnType == null)
      {
        returnType = DustTypes.Void;
      }

      root.Children.Add(new FunctionDeclarationNode(functionName, modifiers, parameters, new CodeBlockNode(bodyRange), returnType, range));

      return root;
    }
  }
}