namespace Dust.Compiler.Diagnostics
{
  public class Diagnostic
  {
    public string Code { get; }
    public string Message { get; }
    public DiagnosticSeverity Severity { get; }
    public SourceRange Range { get; set; }

    public Diagnostic(string code, string message, DiagnosticSeverity severity)
    {
      Code = code;
      Message = message;
      Severity = severity;
    }
  }
}