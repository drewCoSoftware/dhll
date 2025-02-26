//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./v1/dhll.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace dhll.v1 {

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IdhllListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class dhllBaseListener : IdhllListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.file"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFile([NotNull] dhllParser.FileContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.file"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFile([NotNull] dhllParser.FileContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.inlineComment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInlineComment([NotNull] dhllParser.InlineCommentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.inlineComment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInlineComment([NotNull] dhllParser.InlineCommentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.typedef"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypedef([NotNull] dhllParser.TypedefContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.typedef"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypedef([NotNull] dhllParser.TypedefContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.decl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDecl([NotNull] dhllParser.DeclContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.decl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDecl([NotNull] dhllParser.DeclContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.initializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInitializer([NotNull] dhllParser.InitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.initializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInitializer([NotNull] dhllParser.InitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.prop"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProp([NotNull] dhllParser.PropContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.prop"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProp([NotNull] dhllParser.PropContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.value"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterValue([NotNull] dhllParser.ValueContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.value"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitValue([NotNull] dhllParser.ValueContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.identifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdentifier([NotNull] dhllParser.IdentifierContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.identifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdentifier([NotNull] dhllParser.IdentifierContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.typename"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypename([NotNull] dhllParser.TypenameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.typename"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypename([NotNull] dhllParser.TypenameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.scope"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterScope([NotNull] dhllParser.ScopeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.scope"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitScope([NotNull] dhllParser.ScopeContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace dhll.v1
