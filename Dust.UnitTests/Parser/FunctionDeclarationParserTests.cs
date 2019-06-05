using System.Collections.Generic;
using System.Linq;
using Dust.Compiler;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.SyntaxTree;
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

      SyntaxToken openBrace = tokens.Find((token) => token.Is(SyntaxTokenKind.OpenBrace));

      if (openBrace != null)
      {
        bodyRange = new SourceRange(openBrace.Position, tokens.Find((token) => token.Is(SyntaxTokenKind.CloseBrace)).Position);
      }

      modifiers = tokens.Select((token) =>
      {
        AccessModifierKind? modifierKind = AccessModifier.ParseKind(token.Kind);

        return modifierKind != null ? new AccessModifier(token, modifierKind.Value) : null;
      }).Where((token) => token != null).ToList();
    }

    [Theory]
    [InlineData("fn function() {}", true)]
    [InlineData("fn function()")]
    [InlineData("fn function")]
    public void SimpleFunction(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", hasBody: hasBody));
    }

    [Theory]
    [InlineData("fn int function() {}", true)]
    [InlineData("fn int function()")]
    [InlineData("fn int function")]
    public void SimpleFunctionWithReturnType(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", returnType: DustTypes.Int, hasBody: hasBody));
    }

    [Theory]
    [InlineData("public fn int function() {}", true)]
    [InlineData("private fn int function()")]
    [InlineData("static internal fn int function")]
    public void SimpleFunctionWithAccessModifiers(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", returnType: DustTypes.Int, hasBody: hasBody));
    }

    [Theory]
    [InlineData("fn function(int param1) {}", true)]
    [InlineData("fn function(int param1)")]
    public void FunctionWithOneParameter(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", DustTypes.Int, false)
      }, hasBody: hasBody));
    }

    [Theory]
    // TODO: More parameters once Rider fixes this
    [InlineData("fn function(mut int param1, string param2) {}", true)]
    [InlineData("fn function(mut int param1, string param2)")]
    public void FunctionWithMultipleParameters(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", DustTypes.Int, true),
        new FunctionParameter("param2", null, false)
      }, hasBody: hasBody));
    }

    [Theory]
    // TODO: More parameters once Rider fixes this
    [InlineData("fn int function(mut int param1, string param2) {}", true)]
    [InlineData("fn int function(mut int param1, string param2)")]
    public void FunctionWithMultipleParametersAndReturnType(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", DustTypes.Int, true),
        new FunctionParameter("param2", null, false)
      }, DustTypes.Int, hasBody));
    }

    [Theory]
    [InlineData("fn int function(mut int param1, string param2) {}", true)]
    [InlineData("fn int function(mut int param1, string param2)")]
    public void FunctionWithMultipleParametersAndAccessModifiers(string code, bool hasBody = false)
    {
      Setup(code);

      Expect(Parse()).To.ParseSuccessfully().And.SyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", DustTypes.Int, true),
        new FunctionParameter("param2", null, false)
      }, DustTypes.Int, hasBody));
    }

    private CodeBlockNode CreateFunctionDeclarationNode(string functionName, List<FunctionParameter> parameters = null, DustType returnType = null, bool hasBody = true)
    {
      if (parameters == null)
      {
        parameters = new List<FunctionParameter>();
      }

      if (returnType == null)
      {
        returnType = DustTypes.Void;
      }

      root.Children.Add(new FunctionDeclaration(functionName, modifiers, parameters, hasBody ? new CodeBlockNode(bodyRange) : null, returnType, range));

      return root;
    }
  }
}