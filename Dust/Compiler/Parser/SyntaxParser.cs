using System;
using System.Collections.Generic;
using System.Linq;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser
{
  public class SyntaxParser
  {
    public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

    private readonly List<SyntaxToken> tokens;
    private int position;

    private SyntaxToken CurrentToken => tokens[position];

    public SyntaxParser(List<SyntaxToken> tokens)
    {
      this.tokens = tokens;
      position = 0;
    }

    public Node Parse()
    {
      if (tokens.Count == 0)
      {
        return null;
      }

      CodeBlockNode module = new CodeBlockNode();

      SourcePosition start = CurrentToken.Position;

      while (position < tokens.Count - 1)
      {
        Node statement = ParseStatement();

        if (statement == null)
        {
          break;
        }

        module.Children.Add(statement);
      }

      module.Range = new SourceRange(start, CurrentToken.Position);

      return module;
    }
    
    private Node ParseStatement()
    {
      if (MatchToken(SyntaxTokenKind.FnKeyword, false))
      {
        Error(Errors.LetExpected, CurrentToken.Range);

        return null;
      }

      SourcePosition startPosition = null;

      while (IsAccessModifier())
      {
        if (startPosition == null)
        {
          startPosition = CurrentToken.Position;
        }

        Advance();
      }

      if (MatchNextToken(SyntaxTokenKind.FnKeyword, false))
      {
        return ParseFn(startPosition);
      }

      if (MatchToken(SyntaxTokenKind.LetKeyword))
      {
        return ParseLet(startPosition);
      }

      Error(Errors.UnexpectedToken, CurrentToken.Range);

      return null;
    }

    private Node ParseFn(SourcePosition startPosition)
    {
      if (MatchToken(SyntaxTokenKind.LetKeyword) == false)
      {
        Error(Errors.LetExpected, CurrentToken.Range);
      }

      if (MatchToken(SyntaxTokenKind.FnKeyword) == false)
      {
        Error(Errors.FnExpected, CurrentToken.Range);
      }

      List<AccessModifier> modifiers = new List<AccessModifier>();

      bool modifierSeen = false;

      for (int i = tokens.IndexOf(CurrentToken); i >= 0; i--)
      {
        AccessModifierKind? kind = AccessModifier.ParseKind(tokens[i].Kind);

        if (kind == null)
        {
          if (modifierSeen)
          {
            break;
          }

          continue;
        }

        AccessModifier modifier = new AccessModifier(tokens[i], kind.Value);

        modifiers.Add(modifier);

        modifierSeen = true;
      }

      if (MatchToken(SyntaxTokenKind.Identifier, false) == false)
      {
        Error(Errors.IdentifierExpected, CurrentToken.Range);
      }

      ValidateFunctionModifiers(modifiers, PeekBack().Range);

      string name = CurrentToken.Text;

      Advance();

      if (MatchToken(SyntaxTokenKind.OpenParenthesis) == false)
      {
        Error(Errors.OpenParenthesisExpected, CurrentToken.Range);
      }

      if (IsAtEnd())
      {
        Revert();

        return null;
      }

      SourcePosition bodyStartPosition = CurrentToken.Position;

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      if (MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
      {
        while (MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
        {
          bool isMutable = MatchToken(SyntaxTokenKind.MutKeyword);

          if (MatchToken(SyntaxTokenKind.Identifier))
          {
            parameters.Add(new FunctionParameter(CurrentToken.Text, null, isMutable));
          }
          else
          {
            Error(Errors.CloseParenthesisExpected, CurrentToken.Range);

            break;
          }
        }
      }

      if (MatchToken(SyntaxTokenKind.OpenBrace) == false)
      {
        Error(Errors.OpenBraceExpected, new SourceRange(CurrentToken.Position, CurrentToken.Position + 1));
      }

      CodeBlockNode bodyNode = new CodeBlockNode();

      SyntaxToken closeBrace = CurrentToken;

      while (MatchToken(SyntaxTokenKind.CloseBrace) == false)
      {
        if (IsAtEnd())
        {
          Error(Errors.CloseBraceExpected, new SourceRange(closeBrace.Position, closeBrace.Position + 1));

          break;
        }

        Node statement = ParseStatement();

        if (statement == null)
        {
          break;
        }

        bodyNode.Children.Add(statement);
      }

      bodyNode.Range = new SourceRange(bodyStartPosition, CurrentToken.Position);

      return new FunctionDeclarationNode(name, modifiers, parameters, bodyNode, new SourceRange(startPosition, CurrentToken.Position));
    }

    private PropertyDeclarationNode ParseLet(SourcePosition startPosition)
    {
      bool isMutable = MatchToken(SyntaxTokenKind.MutKeyword);

      if (CurrentToken.Kind != SyntaxTokenKind.Identifier)
      {
        // Syntax error
        Console.WriteLine("error");
      }

      PropertyDeclarationNode node = new PropertyDeclarationNode(CurrentToken.Text, isMutable)
      {
        Range = new SourceRange(startPosition, new SourcePosition(CurrentToken.Position.Line, CurrentToken.Position.Column + CurrentToken.Text.Length))
      };

      Advance();

      return node;
    }

    private bool IsAccessModifier(SyntaxToken token = null)
    {
      SyntaxTokenKind kind = token?.Kind ?? CurrentToken.Kind;

      return kind == SyntaxTokenKind.PublicKeyword || kind == SyntaxTokenKind.InternalKeyword || kind == SyntaxTokenKind.ProtectedKeyword || kind == SyntaxTokenKind.PrivateKeyword || kind == SyntaxTokenKind.StaticKeyword;
    }

    private void Error(Error error, SourceRange range, string arg0 = null, string arg1 = null)
    {
      error.Range = range;

      if (arg0 != null)
      {
        error.Format(arg0, arg1);
      }

      Diagnostics.Add(error);
    }

    private static (int publicCount, int internalCount, int protectedCount, int privateCount, int staticCount) CountModifiers(List<AccessModifier> modifiers)
    {
      return (modifiers.Count((modifier) => modifier.Kind == AccessModifierKind.Public), modifiers.Count((modifier) => modifier.Kind == AccessModifierKind.Internal), modifiers.Count((modifier) => modifier.Kind == AccessModifierKind.Protected), modifiers.Count((modifier) => modifier.Kind == AccessModifierKind.Private), modifiers.Count((modifier) => modifier.Kind == AccessModifierKind.Static));
    }

    private void ModifierError(Error error, AccessModifier modifier, string arg0 = null, string arg1 = null)
    {
      Error(error, modifier.Token.Range, arg0, arg1);
    }

    private void ValidateFunctionModifiers(List<AccessModifier> modifiers, SourceRange nameIdentifierRange)
    {
      if (modifiers.Count > 10)
      {
        Error(Errors.TooManyIncompatibleModifiers, nameIdentifierRange);

        return;
      }

      (int publicCount, int internalCount, int protectedCount, int privateCount, int staticCount) = CountModifiers(modifiers);

      bool containsPublic = publicCount > 0;
      bool containsInternal = internalCount > 0;
      bool containsProtected = protectedCount > 0;
      bool containsPrivate = privateCount > 0;

      foreach (AccessModifier modifier in modifiers.AsEnumerable().Reverse())
      {
        switch (modifier.Kind)
        {
          case AccessModifierKind.Public:
            if (containsInternal)
            {
              ModifierError(Errors.IncombinableModifier, modifier, "public", "internal");
            }

            if (containsProtected)
            {
              ModifierError(Errors.IncombinableModifier, modifier, "public", "protected");
            }

            if (containsPrivate)
            {
              ModifierError(Errors.IncombinableModifier, modifier, "public", "private");
            }

            if (publicCount > 1)
            {
              ModifierError(Errors.ModifierAlreadySeen, modifier, "public");
            }

            break;
          case AccessModifierKind.Internal:
            if (containsPublic)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "internal", "public");
            }

            if (containsProtected)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "internal", "protected");
            }

            if (containsPrivate)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "internal", "private");
            }

            if (internalCount > 1)
            {
              ModifierError(Errors.ModifierAlreadySeen, modifier, "internal");
            }

            break;
          case AccessModifierKind.Protected:
            if (containsPublic)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "protected", "public");
            }

            if (containsInternal)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "protected", "internal");
            }

            if (containsPrivate)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "protected", "private");
            }

            if (protectedCount > 1)
            {
              ModifierError(Errors.ModifierAlreadySeen, modifier, "protected");
            }

            break;
          case AccessModifierKind.Private:
            if (containsPublic)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "private", "public");
            }

            if (containsInternal)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "private", "internal");
            }

            if (containsProtected)
            {
              Error(Errors.IncombinableModifier, CurrentToken.Range, "private", "protected");
            }

            if (privateCount > 1)
            {
              ModifierError(Errors.ModifierAlreadySeen, modifier, "private");
            }

            break;
          case AccessModifierKind.Static:
            if (staticCount > 1)
            {
              ModifierError(Errors.ModifierAlreadySeen, modifier, "static");
            }

            break;
        }
      }
    }

    private bool MatchToken(SyntaxTokenKind kind, bool advance = true, int offset = 0)
    {
      if (IsAtEnd() || CurrentToken.Kind == SyntaxTokenKind.EndOfFile) return false;

      bool match = tokens[position + offset].Kind == kind;

      if (match && advance)
      {
        position++;
      }

      return match;
    }

    private bool MatchNextToken(SyntaxTokenKind kind, bool advance = true)
    {
      return MatchToken(kind, advance, 1);
    }

    private void Advance()
    {
      position++;
    }

    private SyntaxToken Peek()
    {
      return IsAtEnd() ? null : tokens[position + 1];
    }

    private SyntaxToken PeekBack()
    {
      return position == 0 ? null : tokens[position - 1];
    }

    private void Revert()
    {
      position--;
    }

    private bool IsAtEnd()
    {
      return position >= tokens.Count || CurrentToken.Kind == SyntaxTokenKind.EndOfFile;
    }
  }
}