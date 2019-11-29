using System.Collections.Generic;
using Dust.Compiler.Parser;
using Dust.Compiler.Types;

namespace Dust.Compiler.Binding
{
  public class BoundBinaryOperator
  {
    public BinaryOperatorKind Kind { get; }
    public DustType LeftType { get; }
    public DustType RightType { get; }
    public DustType ReturnType { get; }
    public bool IsInterchangeable { get; }

    public BoundBinaryOperator(BinaryOperatorKind kind, DustType type)
      : this(kind, type, type, type)
    {
    }

    public BoundBinaryOperator(BinaryOperatorKind kind, DustType leftType, DustType rightType, DustType returnType, bool isInterchangeable = true)
    {
      Kind = kind;
      LeftType = leftType;
      RightType = rightType;
      ReturnType = returnType;
      IsInterchangeable = isInterchangeable;
    }

    private static readonly List<BoundBinaryOperator> binaryOperators = new List<BoundBinaryOperator>
    {
      new BoundBinaryOperator(BinaryOperatorKind.Add, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Add, DustTypes.String),
      new BoundBinaryOperator(BinaryOperatorKind.Add, DustTypes.String, DustTypes.Number, DustTypes.String),
      new BoundBinaryOperator(BinaryOperatorKind.Subtract, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Subtract, DustTypes.String),
      new BoundBinaryOperator(BinaryOperatorKind.Divide, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Divide, DustTypes.String),
      new BoundBinaryOperator(BinaryOperatorKind.Divide, DustTypes.String, DustTypes.Int, DustTypes.String),
      new BoundBinaryOperator(BinaryOperatorKind.Multiply, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Multiply, DustTypes.String, DustTypes.Int, DustTypes.String),
      new BoundBinaryOperator(BinaryOperatorKind.Modulo, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Exponentiate, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Equal, DustTypes.Bool),
      new BoundBinaryOperator(BinaryOperatorKind.Equal, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.NotEqual, DustTypes.Bool),
      new BoundBinaryOperator(BinaryOperatorKind.NotEqual, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.And, DustTypes.Bool),
      new BoundBinaryOperator(BinaryOperatorKind.Or, DustTypes.Bool),
      new BoundBinaryOperator(BinaryOperatorKind.GreaterThan, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.GreaterThanEqual, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.LessThan, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.LessThanEqual, DustTypes.Number),
    };

    public static BoundBinaryOperator Bind(DustType leftType, BinaryOperatorKind kind, DustType rightType)
    {
      foreach (BoundBinaryOperator @operator in binaryOperators)
      {
        if (@operator.Kind == kind)
        {
          if (@operator.LeftType.IsAssignableFrom(leftType) && @operator.RightType.IsAssignableFrom(rightType))
          {
            return @operator;
          }
          
          if (@operator.IsInterchangeable && @operator.LeftType.IsAssignableFrom(rightType) && @operator.RightType.IsAssignableFrom(leftType))
          {
            return @operator;
          }
        }
      }
      
      return null;
    }
  }
}