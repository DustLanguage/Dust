using System.Linq;
using Dust.Compiler;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using JetBrains.Annotations;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace Dust.UnitTests.Parser
{
  public class ParserTests : ExpectTests<SyntaxParseResult, ParserTests.ExceptToFunctions>
  {
    protected CodeBlockNode root;
    protected SourceRange range;

    protected static readonly SyntaxLexer lexer = new SyntaxLexer();
    private static readonly SyntaxParser parser = new SyntaxParser();

    protected virtual void Setup(string code)
    {
      range = SourceRange.FromText(code);
      root = new CodeBlockNode(range);
    }

    protected static SyntaxParseResult Parse(string code)
    {
      return parser.Parse(lexer.Lex(code));
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
        Assert.True(result.Diagnostics.Any((diagnostic) => diagnostic.Severity == DiagnosticSeverity.Error) == false, $"failed to parse\n{string.Join('\n', result.Diagnostics)}");

        return new ExpectFunctions(result);
      }

      public ExpectFunctions AbstractSyntaxTreeOf(Node node)
      {
        ComparisonResult comparisonResult = new CompareLogic().Compare(result.Node, node);

        string[] differences = comparisonResult.Differences.Select((difference) => $"{difference.ExpectedName}.{difference.PropertyName} ({difference.Object1Value}) != {difference.ActualName}.{difference.PropertyName} ({(difference.Object2Value == "(null)" ? "null" : difference.Object2Value)})").ToArray();

        Assert.True(result.Node.Equals(node), $"syntax tree mismatch\n{string.Join("\n", differences.ToArray())}");

        return new ExpectFunctions(result);
      }
    }
  }
}