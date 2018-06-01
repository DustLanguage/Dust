namespace Dust.Compiler.Diagnostics
{
  public class Errors
  {
    public static Diagnostic OpenBraceExpected { get; }
    public static Diagnostic CloseBraceExpected { get; }
    public static Diagnostic OpenParenthesisExpected { get; }
    public static Diagnostic CloseParenthesisExpected { get; }
    public static Diagnostic LetExpected { get; }
    public static Diagnostic FnExpected { get; }
    public static Diagnostic IdentifierExpected { get; }
    public static Diagnostic UnexpectedToken { get; }

    private static readonly int startErrorCode = 1000;
    private static int currentErrorCode = startErrorCode - 1;

    static Errors()
    {
      OpenBraceExpected = CreateError("{ expected");
      CloseBraceExpected = CreateError("} expected");
      LetExpected = CreateError("let expected in function declaration");
      FnExpected = CreateError("fn expected in function declaration");
      IdentifierExpected = CreateError("identifier expected in function declaration");
      OpenParenthesisExpected = CreateError("( expected");
      CloseParenthesisExpected = CreateError(") expected");
      UnexpectedToken = CreateError("unexpected token");
    }

    private static Diagnostic CreateError(string message)
    {
      currentErrorCode++;

      return new Diagnostic($"DS{currentErrorCode.ToString()}", message, DiagnosticSeverity.Error);
    }
  }
}