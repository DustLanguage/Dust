using Dust.Compiler.Binding;
using Dust.Compiler.Binding.Tree;
using Dust.Compiler.Binding.Tree.Expressions;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Types;

namespace Dust.Compiler.Interpreter
{
  public class Interpreter
  {
    public DustObject Interpret(CodeBlockNode root)
    {
      BoundTree tree = new Binder().Bind(root);

      /*foreach (Statement statement in root.Children)
      {*/
      return EvaluateStatement(tree.Statements[0]);
      /*}*/
    }

    private DustObject EvaluateStatement(BoundStatement statement)
    {
      if (statement is BoundExpressionStatement expressionStatement)
      {
        return EvaluateExpressionStatement(expressionStatement);
      }

      return null;
    }

    private DustObject EvaluateExpression(BoundExpression expression)
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

    private DustObject EvaluateExpressionStatement(BoundExpressionStatement expressionStatement)
    {
      return EvaluateExpression(expressionStatement.Expression);
    }

    private DustObject EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
    {
      DustObject left = EvaluateExpression(binaryExpression.Left);
      DustObject right = EvaluateExpression(binaryExpression.Right);

      /*
       * int   + double   -> double
       *   DoubleType.Add(int, double)  -> double
       * double + int     -> double
       *   DoubleType.Add(double, int)  -> double
       * int   + string   -> string
       *   StringType.Add(int, string)  -> string
       * Left <op> Right  -> ReturnType
       *   ReturnType.<op>(Left, Right) -> ReturnType
       */


/*      if (binaryExpression.Right.Type == binaryExpression.Type)
      {
        DustObject t = left;

        left = right;
        right = t;
      }*/

      switch (binaryExpression.Operator.Kind)
      {
        case BinaryOperatorKind.Add:
          return left.Add(right);
        case BinaryOperatorKind.Subtract:
          return left.Subtract(right);
        case BinaryOperatorKind.Multiply:
          return left.Multiply(right);
        case BinaryOperatorKind.Divide:
          return left.Divide(right);
        /*case BinaryOperatorKind.Modulo:
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
          return Convert.ToDouble(left) <= Convert.ToDouble(right);*/
        default:
          return null;
      }
    }

    private DustObject EvaluateLiteralExpression(BoundLiteralExpression literalExpression)
    {
      return literalExpression.Value;
    }
  }
}