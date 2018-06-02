namespace Dust.Compiler.Diagnostics
{
  public class Errors
  {
    public static Diagnostic OpenBraceExpected => CreateError("{ expected");
    public static Diagnostic CloseBraceExpected => CreateError("} expected");
    public static Diagnostic OpenParenthesisExpected => CreateError("( expected");
    public static Diagnostic CloseParenthesisExpected => CreateError(") expected");
    public static Diagnostic LetExpected => CreateError("let expected in function declaration");
    public static Diagnostic FnExpected => CreateError("fn expected in function declaration");
    public static Diagnostic IdentifierExpected => CreateError("identifier expected in function declaration");
    public static Diagnostic DuplicateModifier =>CreateError("duplicate '{0}' modifier");
    public static Diagnostic IncombinableModifier =>CreateError("modifier '{0}' can't be combined with modifier '{1}'");
    public static Diagnostic UnexpectedToken =>CreateError("unexpected token");
    
    private static Diagnostic CreateError(string message)
    {
      return new Diagnostic(message, DiagnosticSeverity.Error);
    }
  }
}