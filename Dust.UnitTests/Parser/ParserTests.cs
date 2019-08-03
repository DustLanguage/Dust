using System.Collections.Generic;
using System.Linq;
using Dust.Compiler;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.SyntaxTree;
using JetBrains.Annotations;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class ParserTests : ExpectTests<SyntaxParseResult, ParserTests.ExceptToFunctions>
  {
    protected CodeBlockNode root;
    protected SourceRange range;
    protected List<SyntaxToken> tokens;

    private readonly SyntaxLexer lexer = new SyntaxLexer();
    private readonly SyntaxParser parser = new SyntaxParser();
    private static readonly CompareLogic comparer = new CompareLogic();

    protected virtual void Setup(string code)
    {
      range = SourceRange.FromText(code);
      root = new CodeBlockNode(range: range);
      tokens = lexer.Lex(code);
    }

    protected SyntaxParseResult Parse()
    {
      return parser.Parse(tokens);
    }

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ExceptToFunctions
    {
      private readonly SyntaxParseResult result;

      public ExceptToFunctions(SyntaxParseResult result)
      {
        this.result = result;
      }

      public ExpectFunctions ParseSuccessfully()
      {
        Assert.True(result.Diagnostics.Any((diagnostic) => diagnostic.Severity == DiagnosticSeverity.Error) == false, $"Failed to parse\n{string.Join('\n', result.Diagnostics)}");

        return new ExpectFunctions(result);
      }

      public ExpectFunctions SyntaxTreeOf(SyntaxNode node)
      {
        ComparisonResult comparisonResult = comparer.Compare(result.Node, node);

        string[] differences = comparisonResult.Differences.Select((difference) => $"{difference.ExpectedName}.{difference.PropertyName} ({difference.Object2Value}) != {difference.ActualName}.{difference.PropertyName} ({(difference.Object1)})").ToArray();

        Assert.True(comparisonResult.AreEqual, $"Syntax tree mismatch\n{string.Join("\n", differences.ToArray())}");

        return new ExpectFunctions(result);
      }
    }
  }
}