using System;
using Dust.Compiler.Binding;
using Dust.Compiler.Binding.Tree;
using Dust.Compiler.Binding.Tree.Expressions;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler.Interpreter
{
  public class Interpreter
  {
    public object Interpret(CodeBlockNode root)
    {
      BoundTree tree = new Binder().Bind(root);

      /*foreach (Statement statement in root.Children)
      {*/
      return EvaluateStatement(tree.Statements[0]);
      /*}*/
    }

    private object EvaluateStatement(BoundStatement statement)
    {
      if (statement is BoundExpressionStatement expressionStatement)
      {
        return EvaluateExpressionStatement(expressionStatement);
      }

      return null;
    }

    private object EvaluateExpression(BoundExpression expression)
    {
      if (expression is BoundBinaryExpression binaryExpression)
      {
        return EvaluateBinaryExpression(binaryExpression);
      }

      if (expression is BoundLiteralExpression literalExpression)
      {
        return EvaluateLiteralExpression(literalExpression);
      }

      return null;
    }

    private object EvaluateExpressionStatement(BoundExpressionStatement expressionStatement)
    {
      return EvaluateExpression(expressionStatement.Expression);
    }

    private object EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
    {
      object left = Convert.ChangeType(EvaluateExpression(binaryExpression.Left), binaryExpression.Type.ToNativeType());
      object right = Convert.ChangeType(EvaluateExpression(binaryExpression.Right), binaryExpression.Type.ToNativeType());

      switch (binaryExpression.Operator.Kind)
      {
        case BinaryOperatorKind.Add:
          if (left is double || right is double)
          {
            return (double) left + (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left + (float) right;
          }

          return (int) left + (int) right;
        case BinaryOperatorKind.Subtract:
          if (left is double || right is double)
          {
            return (double) left - (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left - (float) right;
          }

          return (int) left - (int) right;
        case BinaryOperatorKind.Multiply:
          if (left is double || right is double)
          {
            return (double) left * (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left * (float) right;
          }

          return (int) left * (int) right;
        case BinaryOperatorKind.Divide:
          if (left is double || right is double)
          {
            return (double) left / (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left / (float) right;
          }

          return (int) left / (int) right;
        case BinaryOperatorKind.Modulo:
          if (left is double || right is double)
          {
            return (double) left % (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left % (float) right;
          }

          return (int) left % (int) right;
        case BinaryOperatorKind.Exponentiate:
          return Convert.ChangeType(Math.Pow(Convert.ToDouble(left), Convert.ToDouble(right)), binaryExpression.Type.ToNativeType());
        case BinaryOperatorKind.Equal:
        {
          if (left is bool leftBoolValue && right is bool rightBoolValue)
          {
            return leftBoolValue == rightBoolValue;
          }

          if (left is double || right is double)
          {
            return (double) left == (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left == (float) right;
          }

          if (left is int || right is int)
          {
            return (int) left == (int) right;
          }

          throw new Exception("Equal only implemented for booleans.");
        }
        case BinaryOperatorKind.NotEqual:
        {
          if (left is bool leftBoolValue && right is bool rightBoolValue)
          {
            return leftBoolValue != rightBoolValue;
          }

          if (left is double || right is double)
          {
            return (double) left != (double) right;
          }

          if (left is float || right is float)
          {
            return (float) left != (float) right;
          }

          if (left is int || right is int)
          {
            return (int) left != (int) right;
          }

          throw new Exception("NotEqual only implemented for booleans.");
        }
        case BinaryOperatorKind.And:
          return (bool) left && (bool) right;
        case BinaryOperatorKind.Or:
          return (bool) left || (bool) right;
        case BinaryOperatorKind.GreaterThan:
          return Convert.ToDouble(left) > Convert.ToDouble(right);
        case BinaryOperatorKind.GreaterThanEqual:
          return Convert.ToDouble(left) >= Convert.ToDouble(right);
        case BinaryOperatorKind.LessThan:
          return Convert.ToDouble(left) < Convert.ToDouble(right);
        case BinaryOperatorKind.LessThanEqual:
          return Convert.ToDouble(left) <= Convert.ToDouble(right);
        default:
          return null;
      }
    }

    private object EvaluateLiteralExpression(BoundLiteralExpression literalExpression)
    {
      return literalExpression.Value;
    }
  }
}