using System.Collections.Generic;
using System.Linq;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Types;
using Dust.Extensions;

namespace Dust.Compiler.Parser.Parsers
{
  public class FunctionDeclarationParser : SyntaxParserExtension
  {
    public FunctionDeclarationParser(SyntaxParser parser)
      : base(parser)
    {
    }

    public FunctionDeclaration Parse(SourcePosition startPosition)
    {
      if (Parser.MatchToken(SyntaxTokenKind.FnKeyword) == false)
      {
        Parser.Error(Errors.FnExpected, Parser.CurrentToken);
      }

      DustType returnType = null;
      string typeName = Parser.CurrentToken.Text;

      if (Parser.MatchToken(SyntaxTokenKind.Identifier, false))
      {
        if (Parser.MatchNextToken(SyntaxTokenKind.Identifier, false))
        {
          returnType = DustTypes.GetType(typeName);

          Parser.Advance();
        }
        else
        {
          returnType = DustTypes.Void;
        }
      }

      if (returnType == null)
      {
        Parser.Error(Errors.UnknownType, Parser.CurrentToken, typeName);
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
        Parser.Error(Errors.IdentifierExpected, Parser.CurrentToken, "function declaration");
      }

      ValidateFunctionModifiers(modifiers, Parser.PeekBack().Range);

      string name = Parser.CurrentToken.Text;

      Parser.Advance();

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      bool parentheses = false;

      if (Parser.MatchToken(SyntaxTokenKind.OpenParenthesis))
      {
        if (Parser.IsAtEnd())
        {
          Parser.Revert();

          return null;
        }

        bool comma = false;

        if (Parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
        {
          while (Parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
          {
            if (Parser.MatchToken(SyntaxTokenKind.Comma))
            {
              comma = true;

              continue;
            }

            bool isMutable = Parser.MatchToken(SyntaxTokenKind.MutKeyword);

            if (Parser.MatchToken(SyntaxTokenKind.Identifier))
            {
              DustType type = DustTypes.GetType(Parser.PeekBack().Text);

              if (Parser.MatchToken(SyntaxTokenKind.Identifier))
              {
                parameters.Add(new FunctionParameter(Parser.PeekBack().Text, type, isMutable));

                comma = false;
              }
              else if (Parser.MatchToken(SyntaxTokenKind.MutKeyword))
              {
                Parser.Error(Errors.ExpectedBefore, Parser.PeekBack(), "mut", "parameter name");

                Parser.Advance();
              }
              else
              {
                Parser.Error(Errors.ExpectedBefore, Parser.PeekBack(), "type", "parameter name");
              }
            }
            else
            {
              Parser.Error(Errors.CloseParenthesisExpected, Parser.CurrentToken);

              break;
            }
          }
        }

        parentheses = true;

        if (comma)
        {
          Parser.Error(Errors.ExpectedAfter, Parser.CurrentToken, "parameter", "comma");
        }
      }

      SourcePosition bodyStartPosition = Parser.CurrentToken?.Position;

      CodeBlockNode bodyNode = null;

      if (Parser.MatchToken(SyntaxTokenKind.OpenBrace))
      {
        if (parentheses == false)
        {
          Parser.Error(Errors.OpenParenthesisExpected, Parser.CurrentToken);
          Parser.Error(Errors.CloseParenthesisExpected, Parser.CurrentToken);

          Parser.ConsumeIf((token) => token.Is(SyntaxTokenKind.CloseBrace));
        }
        else
        {
          SyntaxToken closeBrace = Parser.CurrentToken;

          bodyNode = new CodeBlockNode();

          while (Parser.MatchToken(SyntaxTokenKind.CloseBrace) == false)
          {
            if (Parser.IsAtEnd())
            {
              Parser.Error(Errors.CloseBraceExpected, new SourceRange(closeBrace.Position, closeBrace.Position + 1));

              break;
            }

            SyntaxNode statement = Parser.ParseStatement();

            if (statement == null)
            {
              break;
            }

            bodyNode.Children.Add(statement);
          }

          bodyNode.Range = new SourceRange(bodyStartPosition, Parser.CurrentToken.Position);
        }
      }

      return new FunctionDeclaration(name, modifiers, parameters, bodyNode, returnType, new SourceRange(startPosition, Parser.CurrentToken.Position));
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