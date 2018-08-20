using System.Collections.Generic;
using System.Linq;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Extensions;

namespace Dust.Compiler.Parser.Parsers
{
  public class FunctionDeclarationParser : SyntaxParserExtension
  {
    public FunctionDeclarationParser(SyntaxParser parser) : base(parser)
    {
    }

    public override Node Parse(SourcePosition startPosition)
    {
      if (Parser.MatchToken(SyntaxTokenKind.FnKeyword) == false)
      {
        Parser.Error(Errors.FnExpected, Parser.CurrentToken.Range);
      }

      List<AccessModifier> modifiers = new List<AccessModifier>();

      bool modifierSeen = false;

      for (int i = Parser.tokens.IndexOf(Parser.CurrentToken); i >= 0; i--)
      {
        AccessModifierKind? kind = AccessModifier.ParseKind(Parser.tokens[i].Kind);

        if (kind == null)
        {
          if (modifierSeen)
          {
            break;
          }

          continue;
        }

        AccessModifier modifier = new AccessModifier(Parser.tokens[i], kind.Value);

        modifiers.Add(modifier);

        modifierSeen = true;
      }

      if (Parser.MatchToken(SyntaxTokenKind.Identifier, false) == false)
      {
        Parser.Error(Errors.IdentifierExpected, Parser.CurrentToken.Range, "function declaration");
      }

      ValidateFunctionModifiers(modifiers, Parser.PeekBack().Range);

      string name = Parser.CurrentToken.Text;

      Parser.Advance();

      if (Parser.MatchToken(SyntaxTokenKind.OpenParenthesis) == false)
      {
        Parser.Error(Errors.OpenParenthesisExpected, Parser.CurrentToken.Range);
      }

      if (Parser.IsAtEnd())
      {
        Parser.Revert();

        return null;
      }

      SourcePosition bodyStartPosition = Parser.CurrentToken.Position;

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      if (Parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
      {
        while (Parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
        {
          bool isMutable = Parser.MatchToken(SyntaxTokenKind.MutKeyword);

          if (Parser.MatchToken(SyntaxTokenKind.Identifier))
          {
            parameters.Add(new FunctionParameter(Parser.CurrentToken.Text, null, isMutable));
          }
          else
          {
            Parser.Error(Errors.CloseParenthesisExpected, Parser.CurrentToken.Range);

            break;
          }
        }
      }

      if (Parser.MatchToken(SyntaxTokenKind.OpenBrace) == false)
      {
        Parser.Error(Errors.OpenBraceExpected, new SourceRange(Parser.CurrentToken.Position, Parser.CurrentToken.Position + 1));
      }

      CodeBlockNode bodyNode = new CodeBlockNode();

      SyntaxToken closeBrace = Parser.CurrentToken;

      while (Parser.MatchToken(SyntaxTokenKind.CloseBrace) == false)
      {
        if (Parser.IsAtEnd())
        {
          Parser.Error(Errors.CloseBraceExpected, new SourceRange(closeBrace.Position, closeBrace.Position + 1));

          break;
        }

        Node statement = Parser.ParseStatement();

        if (statement == null)
        {
          break;
        }

        bodyNode.Children.Add(statement);
      }

      bodyNode.Range = new SourceRange(bodyStartPosition, Parser.CurrentToken.Position);

      return new FunctionDeclarationNode(name, modifiers, parameters, bodyNode, new SourceRange(startPosition, Parser.CurrentToken.Position));
    }

    private void ValidateFunctionModifiers(List<AccessModifier> modifiers, SourceRange nameIdentifierRange)
    {
      if (modifiers.Count > 10)
      {
        Parser.Error(Errors.TooManyIncompatibleModifiers, nameIdentifierRange);

        return;
      }

      modifiers.Reverse();

      List<AccessModifier> distinctModifiers = modifiers.DistinctBy((modifier) => modifier.Kind).ToList();

      if (distinctModifiers.Count != modifiers.Count)
      {
        foreach (AccessModifier modifier in modifiers.Except(distinctModifiers).Skip(1))
        {
          Parser.ModifierError(Errors.ModifierAlreadySeen, modifier, modifier.ToString());
        }
      }

      foreach (AccessModifier modifier in modifiers)
      {
        // Static is an exception, it can be used alongside any other modifier
        if (modifier.Kind == AccessModifierKind.Static)
        {
          continue;
        }

        foreach (AccessModifierKind kind in new[] {AccessModifierKind.Public, AccessModifierKind.Internal, AccessModifierKind.Protected, AccessModifierKind.Private})
        {
          List<AccessModifier> incompatibleModifiers = modifiers.FindAll((m) => m.Kind == kind && m != modifier);

          foreach (AccessModifier incompatibleModifier in incompatibleModifiers)
          {
            if (modifier.Kind != incompatibleModifier.Kind)
            {
              Parser.ModifierError(Errors.IncombinableModifier, modifier, modifier.ToString(), incompatibleModifier.ToString());

              break;
            }
          }
        }
      }
    }
  }
}