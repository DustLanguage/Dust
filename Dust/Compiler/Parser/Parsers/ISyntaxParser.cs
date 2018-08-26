using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser.Parsers
{
  public interface ISyntaxParserExtension
  {
    Node Parse(SyntaxParser parser, SourcePosition startPosition);
  }
}