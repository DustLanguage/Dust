using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler
{
  public interface IVisitor
  {
    void Accept(SyntaxNode node);
  }
}