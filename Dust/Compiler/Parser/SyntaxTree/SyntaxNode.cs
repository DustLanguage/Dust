using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public abstract class SyntaxNode
  {
    public virtual SourceRange Range
    {
      get
      {
        if (range != null)
        {
          return range;
        }

        SyntaxNode[] children = GetChildren().ToArray();

        range = new SourceRange(children.First().Range.Start, children.Last().Range.End);

        return range;
      }
      protected set => range = value;
    }

    private SourceRange range;

    public virtual IEnumerable<SyntaxNode> GetChildren()
    {
      PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

      foreach (PropertyInfo property in properties)
      {
        if (typeof(SyntaxNode).IsAssignableFrom(property.PropertyType))
        {
          SyntaxNode child = (SyntaxNode) property.GetValue(this);

          if (child != null)
          {
            yield return child;
          }
        }
        else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType))
        {
          foreach (SyntaxNode child in (IEnumerable<SyntaxNode>) property.GetValue(this))
          {
            if (child != null)
            {
              yield return child;
            }
          }
        }
      }
    }
  }
}