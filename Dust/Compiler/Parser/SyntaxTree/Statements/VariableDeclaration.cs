using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public class VariableDeclaration : Statement
  {
    public SyntaxToken LetOrMutToken { get; }
    public SyntaxToken NameToken { get; }
    public bool IsMutable { get; }
    public SyntaxToken TypeToken { get; }
    public Expression Initializer { get; }

    public VariableDeclaration(SyntaxToken letOrMutToken, SyntaxToken nameToken, bool isMutable, SyntaxToken typeToken, Expression initializer)
    {
      LetOrMutToken = letOrMutToken;
      NameToken = nameToken;
      IsMutable = isMutable;
      TypeToken = typeToken;
      Initializer = initializer;
    }
  }
}