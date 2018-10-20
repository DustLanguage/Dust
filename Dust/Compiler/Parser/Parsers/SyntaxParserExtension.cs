namespace Dust.Compiler.Parser.Parsers
{
  public class SyntaxParserExtension
  {
    protected SyntaxParser Parser { get; }

    public SyntaxParserExtension(SyntaxParser parser)
    {
      Parser = parser;
    }
  }
}