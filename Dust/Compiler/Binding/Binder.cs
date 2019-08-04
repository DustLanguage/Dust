using System;
using Dust.Compiler.Binding.Tree;
using Dust.Compiler.Binding.Tree.Expressions;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Parser.SyntaxTree.Expressions;

namespace Dust.Compiler.Binding
{
  public class Binder
  {
    public BoundTree Bind(CodeBlockNode node)
    {
      BoundTree tree = new BoundTree();

      foreach (Statement statement in node.Children)
      {
        tree.Statements.Add(BindStatement(statement));
      }

      return tree;
    }

    private BoundStatement BindStatement(Statement statement)
    {
      if (statement is ExpressionStatement expressionStatement)
      {
        return BindExpressionStatement(expressionStatement);
      }

      return null;
    }

    private BoundExpressionStatement BindExpressionStatement(ExpressionStatement expressionStatement)
    {
      return new BoundExpressionStatement(BindExpression(expressionStatement.Expression));
    }

    private BoundExpression BindExpression(Expression expression)
    {
      if (expression is BinaryExpression binaryExpression)
      {
        return BindBinaryExpression(binaryExpression);
      }

      if (expression is LiteralExpression literalExpression)
      {
        return BindLiteralExpression(literalExpression);
      }

      return null;
    }

    private BoundBinaryExpression BindBinaryExpression(BinaryExpression binaryExpression)
    {
      BoundExpression left = BindExpression(binaryExpression.Left);
      BoundExpression right = BindExpression(binaryExpression.Right);

      BinaryOperatorKind kind = SyntaxFacts.GetBinaryOperatorKind(binaryExpression.OperatorToken);
      BoundBinaryOperator @operator = BoundBinaryOperator.Bind(left.Type, kind, right.Type);

      if (@operator == null)
      {
        // TODO: BoundErrorExpressions.
        // return new BoundErrorExpression(binaryExpression);

        throw new Exception($"{kind} operation not implemented for {left.Type} and {right.Type}.");
      }

      return new BoundBinaryExpression(left, @operator, right);
    }

    private BoundLiteralExpression BindLiteralExpression(LiteralExpression literalExpression)
    {
      return new BoundLiteralExpression(literalExpression.Value);
    }
  }
}