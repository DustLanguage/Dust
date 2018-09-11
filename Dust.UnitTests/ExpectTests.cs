using System;

namespace Dust.UnitTests
{
  public class ExpectTests<T, TExpectFunctions>
  {
    public ExpectFunctions Expect(T result)
    {
      return new ExpectFunctions(result);
    }

    public class ExpectFunctions
    {
      private readonly T result;

      public ExpectFunctions(T result)
      {
        this.result = result;
      }

      public TExpectFunctions To => (TExpectFunctions) Activator.CreateInstance(typeof(TExpectFunctions), result);
      public TExpectFunctions And => (TExpectFunctions) Activator.CreateInstance(typeof(TExpectFunctions), result);
    }
  }
}