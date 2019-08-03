using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler.Parser.Parsers
{
  public class VariableDeclarationParser : SyntaxParserExtension
  {
    public VariableDeclarationParser(SyntaxParser parser)
      : base(parser)
    {
    }

    public VariableDeclaration Parse(bool isMutable)
    {
      SyntaxToken letOrMutToken = Parser.ExpectToken(isMutable ? SyntaxTokenKind.MutKeyword : SyntaxTokenKind.LetKeyword);
      SyntaxToken nameToken = Parser.ExpectToken(SyntaxTokenKind.Identifier);
      SyntaxToken typeToken = Parser.ParseOptionalType();
      Expression initializer = null;

      if (Parser.MatchToken(SyntaxTokenKind.Equals))
      {
        initializer = Parser.ParseExpression();
      }

      return new VariableDeclaration(letOrMutToken, nameToken, isMutable, typeToken, initializer);
    }
  }
}