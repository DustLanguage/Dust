using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser.Parsers
{
  public class PropertyDeclarationParser : ISyntaxParserExtension
  {
    public Node Parse(SyntaxParser parser, SourcePosition startPosition)
    {
      bool isMutable = parser.MatchToken(SyntaxTokenKind.MutKeyword);

      if (parser.CurrentToken.Kind != SyntaxTokenKind.Identifier)
      {
        parser.Error(Errors.IdentifierExpected, parser.CurrentToken, "property declaration");
      }

      PropertyDeclarationNode node = new PropertyDeclarationNode(parser.CurrentToken.Text, isMutable, new SourceRange(startPosition, new SourcePosition(parser.CurrentToken.Position.Line, parser.CurrentToken.Position.Column + parser.CurrentToken.Text.Length)));

      parser.Advance();

      return node;
    }
  }
}