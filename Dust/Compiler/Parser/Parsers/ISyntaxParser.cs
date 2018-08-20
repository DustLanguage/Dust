using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser.Parsers
{
  public abstract class SyntaxParserExtension
  {
    protected SyntaxParser Parser { get; }

    public SyntaxParserExtension(SyntaxParser parser)
    {
      Parser = parser;
    }

    public abstract Node Parse(SourcePosition startPosition);
  }
}