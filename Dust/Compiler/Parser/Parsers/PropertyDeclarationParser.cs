using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser.Parsers
{
  public class PropertyDeclarationParser : SyntaxParserExtension
  {
    public PropertyDeclarationParser(SyntaxParser parser) : base(parser)
    {
    }

    public override Node Parse(SourcePosition startPosition)
    {
      bool isMutable = Parser.MatchToken(SyntaxTokenKind.MutKeyword);

      if (Parser.CurrentToken.Kind != SyntaxTokenKind.Identifier)
      {
        Parser.Error(Errors.IdentifierExpected, Parser.CurrentToken.Range, "property declaration");
      }

      PropertyDeclarationNode node = new PropertyDeclarationNode(Parser.CurrentToken.Text, isMutable, new SourceRange(startPosition, new SourcePosition(Parser.CurrentToken.Position.Line, Parser.CurrentToken.Position.Column + Parser.CurrentToken.Text.Length)));

      Parser.Advance();

      return node;
    }
  }
}