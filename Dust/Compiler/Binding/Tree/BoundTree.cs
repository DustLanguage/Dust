using System.Collections.Generic;

namespace Dust.Compiler.Binding.Tree
{
  public class BoundTree
  {
    public List<BoundStatement> Statements { get; }

    public BoundTree()
    {
      Statements = new List<BoundStatement>();
    }
  }
}