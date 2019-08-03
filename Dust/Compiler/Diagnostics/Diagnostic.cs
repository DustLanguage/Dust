namespace Dust.Compiler.Diagnostics
{
  public class Diagnostic
  {
    public string Message { get; }
    public DiagnosticSeverity Severity { get; }
    public SourceRange Range { get; }

    protected Diagnostic(string message, SourceRange range, DiagnosticSeverity severity)
    {
      Message = message;
      Range = range;
      Severity = severity;
    }

    public override string ToString()
    {
      return Message;
    }
  }
}