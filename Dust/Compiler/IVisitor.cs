using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler
{
  public interface IVisitor
  {
    void Accept(SyntaxNode node);
  }
}