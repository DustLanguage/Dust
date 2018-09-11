using System;
using System.Collections.Generic;
using Dust.Compiler;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Extensions;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class FunctionDeclarationParserTests : ParserTests
  {
    private SourceRange bodyRange;

    protected override void Setup(string code)
    {
      base.Setup(code);

      int firstBrace = code.IndexOf("{", StringComparison.Ordinal);

      if (firstBrace != -1)
      {
        bodyRange = SourceRange.FromText(code.SubstringRange(firstBrace, code.IndexOf("}", StringComparison.Ordinal) + 1), offset: firstBrace);
      }
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
    [InlineData("fn function(int param1) {}")]
    [InlineData("fn function(int param1)")]
    public void FunctionWithOneParameter(string code)
    {
      Setup(code);

      Expect(Parse(code)).To.ParseSuccessfully().And.AbstractSyntaxTreeOf(CreateFunctionDeclarationNode("function", new List<FunctionParameter>
      {
        new FunctionParameter("param1", null, true),
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

    private CodeBlockNode CreateFunctionDeclarationNode(string functionName, List<FunctionParameter> parameters = null)
    {
      if (parameters == null)
      {
        parameters = Lists.Empty<FunctionParameter>();
      }

      root.Children.Add(new FunctionDeclarationNode(functionName, Lists.Empty<AccessModifier>(), parameters, new CodeBlockNode(bodyRange), range));

      return root;
    }
  }
}