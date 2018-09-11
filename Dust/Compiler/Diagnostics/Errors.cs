namespace Dust.Compiler.Diagnostics
{
  public static class Errors
  {
    public static Error OpenBraceExpected => CreateError("{ expected");
    public static Error CloseBraceExpected => CreateError("} expected");
    public static Error OpenParenthesisExpected => CreateError("( expected");
    public static Error CloseParenthesisExpected => CreateError(") expected");
    public static Error FnExpected => CreateError("fn expected in function declaration");
    public static Error IdentifierExpected => CreateError("identifier expected in {0}");
    public static Error ModifierAlreadySeen => CreateError("modifier '{0}' already seen");
    public static Error IncombinableModifier => CreateError("modifier '{0}' can't be combined with modifier '{1}'");
    public static Error UnexpectedToken => CreateError("unexpected token '{0}' in {1}");
    public static Error UnexpectedTokenGlobal => CreateError("unexpected token '{0}'");
    public static Error TooManyIncompatibleModifiers => CreateError("too many incompatible modifiers");
    public static Error ExpectedAfter => CreateError("{0} expected after {1}");

    private static Error CreateError(string message)
    {
      return new Error(message);
    }
  }
}