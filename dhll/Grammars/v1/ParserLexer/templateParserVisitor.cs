//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./v1/templateParser.g4 by ANTLR 4.13.2

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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="templateParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface ItemplateParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.templates"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTemplates([NotNull] templateParser.TemplatesContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.htmlElements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHtmlElements([NotNull] templateParser.HtmlElementsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.htmlMisc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHtmlMisc([NotNull] templateParser.HtmlMiscContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.entityName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEntityName([NotNull] templateParser.EntityNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.htmlElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHtmlElement([NotNull] templateParser.HtmlElementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.htmlContent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHtmlContent([NotNull] templateParser.HtmlContentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] templateParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.htmlAttribute"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHtmlAttribute([NotNull] templateParser.HtmlAttributeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.htmlChardata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHtmlChardata([NotNull] templateParser.HtmlChardataContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>RAW_EXPRESSION</c>
	/// labeled alternative in <see cref="templateParser.attrValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRAW_EXPRESSION([NotNull] templateParser.RAW_EXPRESSIONContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DBL_QUOTE_STRING</c>
	/// labeled alternative in <see cref="templateParser.attrValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDBL_QUOTE_STRING([NotNull] templateParser.DBL_QUOTE_STRINGContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.tag_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_expression([NotNull] templateParser.Tag_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpr([NotNull] templateParser.ExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_SUB</c>
	/// labeled alternative in <see cref="templateParser.addExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_SUB([NotNull] templateParser.TO_SUBContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ADD</c>
	/// labeled alternative in <see cref="templateParser.addExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitADD([NotNull] templateParser.ADDContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_MULT</c>
	/// labeled alternative in <see cref="templateParser.subExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_MULT([NotNull] templateParser.TO_MULTContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SUBTRACT</c>
	/// labeled alternative in <see cref="templateParser.subExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSUBTRACT([NotNull] templateParser.SUBTRACTContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_DIV</c>
	/// labeled alternative in <see cref="templateParser.multExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_DIV([NotNull] templateParser.TO_DIVContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MULTIPLY</c>
	/// labeled alternative in <see cref="templateParser.multExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMULTIPLY([NotNull] templateParser.MULTIPLYContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_UNARY</c>
	/// labeled alternative in <see cref="templateParser.divExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_UNARY([NotNull] templateParser.TO_UNARYContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DIVIDE</c>
	/// labeled alternative in <see cref="templateParser.divExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDIVIDE([NotNull] templateParser.DIVIDEContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>NEGATE</c>
	/// labeled alternative in <see cref="templateParser.unaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNEGATE([NotNull] templateParser.NEGATEContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_PARENS</c>
	/// labeled alternative in <see cref="templateParser.unaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_PARENS([NotNull] templateParser.TO_PARENSContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>PARENS</c>
	/// labeled alternative in <see cref="templateParser.parensExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPARENS([NotNull] templateParser.PARENSContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_CALL</c>
	/// labeled alternative in <see cref="templateParser.parensExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_CALL([NotNull] templateParser.TO_CALLContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>CALL</c>
	/// labeled alternative in <see cref="templateParser.callExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCALL([NotNull] templateParser.CALLContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TO_PRIMARY</c>
	/// labeled alternative in <see cref="templateParser.callExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTO_PRIMARY([NotNull] templateParser.TO_PRIMARYContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MAGIC_NUMBER</c>
	/// labeled alternative in <see cref="templateParser.primaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMAGIC_NUMBER([NotNull] templateParser.MAGIC_NUMBERContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MAGIC_STRING</c>
	/// labeled alternative in <see cref="templateParser.primaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMAGIC_STRING([NotNull] templateParser.MAGIC_STRINGContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>VARIABLE</c>
	/// labeled alternative in <see cref="templateParser.primaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVARIABLE([NotNull] templateParser.VARIABLEContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ARGS</c>
	/// labeled alternative in <see cref="templateParser.argList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitARGS([NotNull] templateParser.ARGSContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="templateParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] templateParser.NumberContext context);
}
} // namespace dhll.v1
