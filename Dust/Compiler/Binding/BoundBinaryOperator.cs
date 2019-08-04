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

    public BoundBinaryOperator(BinaryOperatorKind kind, DustType type)
      : this(kind, type, type, type)
    {
    }

    public BoundBinaryOperator(BinaryOperatorKind kind, DustType leftType, DustType rightType, DustType returnType)
    {
      Kind = kind;
      LeftType = leftType;
      RightType = rightType;
      ReturnType = returnType;
    }

    private static readonly List<BoundBinaryOperator> binaryOperators = new List<BoundBinaryOperator>
    {
      new BoundBinaryOperator(BinaryOperatorKind.Add, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Subtract, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Divide, DustTypes.Number),
      new BoundBinaryOperator(BinaryOperatorKind.Multiply, DustTypes.Number),
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
      return binaryOperators.Find((@operator) => @operator.Kind == kind && @operator.LeftType.IsAssignableFrom(leftType) && @operator.RightType.IsAssignableFrom(rightType));
    }
  }
}