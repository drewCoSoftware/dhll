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
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public partial class templateParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		SEA_WS=1, TAG_OPEN=2, HTML_TEXT=3, TAG_CLOSE=4, TAG_SLASH_CLOSE=5, TAG_SLASH=6, 
		TAG_EQUALS=7, TAG_NAME=8, TAG_WHITESPACE=9, ATTVALUE_VALUE=10, ATTRIBUTE=11;
	public const int
		RULE_templates = 0, RULE_htmlElements = 1, RULE_htmlMisc = 2, RULE_entityName = 3, 
		RULE_htmlElement = 4, RULE_htmlContent = 5, RULE_htmlAttribute = 6, RULE_htmlChardata = 7;
	public static readonly string[] ruleNames = {
		"templates", "htmlElements", "htmlMisc", "entityName", "htmlElement", 
		"htmlContent", "htmlAttribute", "htmlChardata"
	};

	private static readonly string[] _LiteralNames = {
		null, null, "'<'", null, "'>'", "'/>'", "'/'", "'='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "SEA_WS", "TAG_OPEN", "HTML_TEXT", "TAG_CLOSE", "TAG_SLASH_CLOSE", 
		"TAG_SLASH", "TAG_EQUALS", "TAG_NAME", "TAG_WHITESPACE", "ATTVALUE_VALUE", 
		"ATTRIBUTE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "templateParser.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static templateParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public templateParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public templateParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class TemplatesContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(templateParser.Eof, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public HtmlElementsContext[] htmlElements() {
			return GetRuleContexts<HtmlElementsContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlElementsContext htmlElements(int i) {
			return GetRuleContext<HtmlElementsContext>(i);
		}
		public TemplatesContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_templates; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterTemplates(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitTemplates(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTemplates(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TemplatesContext templates() {
		TemplatesContext _localctx = new TemplatesContext(Context, State);
		EnterRule(_localctx, 0, RULE_templates);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 19;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==SEA_WS || _la==TAG_OPEN) {
				{
				{
				State = 16;
				htmlElements();
				}
				}
				State = 21;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 22;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HtmlElementsContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public HtmlElementContext htmlElement() {
			return GetRuleContext<HtmlElementContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlMiscContext[] htmlMisc() {
			return GetRuleContexts<HtmlMiscContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlMiscContext htmlMisc(int i) {
			return GetRuleContext<HtmlMiscContext>(i);
		}
		public HtmlElementsContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_htmlElements; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterHtmlElements(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitHtmlElements(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHtmlElements(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HtmlElementsContext htmlElements() {
		HtmlElementsContext _localctx = new HtmlElementsContext(Context, State);
		EnterRule(_localctx, 2, RULE_htmlElements);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 27;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==SEA_WS) {
				{
				{
				State = 24;
				htmlMisc();
				}
				}
				State = 29;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 30;
			htmlElement();
			State = 34;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					State = 31;
					htmlMisc();
					}
					} 
				}
				State = 36;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HtmlMiscContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode SEA_WS() { return GetToken(templateParser.SEA_WS, 0); }
		public HtmlMiscContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_htmlMisc; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterHtmlMisc(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitHtmlMisc(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHtmlMisc(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HtmlMiscContext htmlMisc() {
		HtmlMiscContext _localctx = new HtmlMiscContext(Context, State);
		EnterRule(_localctx, 4, RULE_htmlMisc);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 37;
			Match(SEA_WS);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class EntityNameContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_NAME() { return GetToken(templateParser.TAG_NAME, 0); }
		public EntityNameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_entityName; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterEntityName(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitEntityName(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitEntityName(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public EntityNameContext entityName() {
		EntityNameContext _localctx = new EntityNameContext(Context, State);
		EnterRule(_localctx, 6, RULE_entityName);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 39;
			Match(TAG_NAME);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HtmlElementContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] TAG_OPEN() { return GetTokens(templateParser.TAG_OPEN); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_OPEN(int i) {
			return GetToken(templateParser.TAG_OPEN, i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public EntityNameContext entityName() {
			return GetRuleContext<EntityNameContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] TAG_CLOSE() { return GetTokens(templateParser.TAG_CLOSE); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_CLOSE(int i) {
			return GetToken(templateParser.TAG_CLOSE, i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_SLASH_CLOSE() { return GetToken(templateParser.TAG_SLASH_CLOSE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public HtmlAttributeContext[] htmlAttribute() {
			return GetRuleContexts<HtmlAttributeContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlAttributeContext htmlAttribute(int i) {
			return GetRuleContext<HtmlAttributeContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlContentContext htmlContent() {
			return GetRuleContext<HtmlContentContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_SLASH() { return GetToken(templateParser.TAG_SLASH, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_NAME() { return GetToken(templateParser.TAG_NAME, 0); }
		public HtmlElementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_htmlElement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterHtmlElement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitHtmlElement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHtmlElement(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HtmlElementContext htmlElement() {
		HtmlElementContext _localctx = new HtmlElementContext(Context, State);
		EnterRule(_localctx, 8, RULE_htmlElement);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 41;
			Match(TAG_OPEN);
			State = 42;
			entityName();
			State = 46;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==TAG_NAME) {
				{
				{
				State = 43;
				htmlAttribute();
				}
				}
				State = 48;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 59;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case TAG_CLOSE:
				{
				State = 49;
				Match(TAG_CLOSE);
				State = 56;
				ErrorHandler.Sync(this);
				switch ( Interpreter.AdaptivePredict(TokenStream,4,Context) ) {
				case 1:
					{
					State = 50;
					htmlContent();
					State = 51;
					Match(TAG_OPEN);
					State = 52;
					Match(TAG_SLASH);
					State = 53;
					Match(TAG_NAME);
					State = 54;
					Match(TAG_CLOSE);
					}
					break;
				}
				}
				break;
			case TAG_SLASH_CLOSE:
				{
				State = 58;
				Match(TAG_SLASH_CLOSE);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HtmlContentContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public HtmlChardataContext[] htmlChardata() {
			return GetRuleContexts<HtmlChardataContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlChardataContext htmlChardata(int i) {
			return GetRuleContext<HtmlChardataContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlElementContext[] htmlElement() {
			return GetRuleContexts<HtmlElementContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public HtmlElementContext htmlElement(int i) {
			return GetRuleContext<HtmlElementContext>(i);
		}
		public HtmlContentContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_htmlContent; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterHtmlContent(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitHtmlContent(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHtmlContent(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HtmlContentContext htmlContent() {
		HtmlContentContext _localctx = new HtmlContentContext(Context, State);
		EnterRule(_localctx, 10, RULE_htmlContent);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 62;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==SEA_WS || _la==HTML_TEXT) {
				{
				State = 61;
				htmlChardata();
				}
			}

			State = 70;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,8,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					{
					State = 64;
					htmlElement();
					}
					State = 66;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
					if (_la==SEA_WS || _la==HTML_TEXT) {
						{
						State = 65;
						htmlChardata();
						}
					}

					}
					} 
				}
				State = 72;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,8,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HtmlAttributeContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public EntityNameContext entityName() {
			return GetRuleContext<EntityNameContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_EQUALS() { return GetToken(templateParser.TAG_EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ATTVALUE_VALUE() { return GetToken(templateParser.ATTVALUE_VALUE, 0); }
		public HtmlAttributeContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_htmlAttribute; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterHtmlAttribute(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitHtmlAttribute(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHtmlAttribute(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HtmlAttributeContext htmlAttribute() {
		HtmlAttributeContext _localctx = new HtmlAttributeContext(Context, State);
		EnterRule(_localctx, 12, RULE_htmlAttribute);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 73;
			entityName();
			State = 76;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==TAG_EQUALS) {
				{
				State = 74;
				Match(TAG_EQUALS);
				State = 75;
				Match(ATTVALUE_VALUE);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HtmlChardataContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode HTML_TEXT() { return GetToken(templateParser.HTML_TEXT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode SEA_WS() { return GetToken(templateParser.SEA_WS, 0); }
		public HtmlChardataContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_htmlChardata; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.EnterHtmlChardata(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ItemplateParserListener typedListener = listener as ItemplateParserListener;
			if (typedListener != null) typedListener.ExitHtmlChardata(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ItemplateParserVisitor<TResult> typedVisitor = visitor as ItemplateParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHtmlChardata(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HtmlChardataContext htmlChardata() {
		HtmlChardataContext _localctx = new HtmlChardataContext(Context, State);
		EnterRule(_localctx, 14, RULE_htmlChardata);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 78;
			_la = TokenStream.LA(1);
			if ( !(_la==SEA_WS || _la==HTML_TEXT) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,11,81,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,
		7,7,1,0,5,0,18,8,0,10,0,12,0,21,9,0,1,0,1,0,1,1,5,1,26,8,1,10,1,12,1,29,
		9,1,1,1,1,1,5,1,33,8,1,10,1,12,1,36,9,1,1,2,1,2,1,3,1,3,1,4,1,4,1,4,5,
		4,45,8,4,10,4,12,4,48,9,4,1,4,1,4,1,4,1,4,1,4,1,4,1,4,3,4,57,8,4,1,4,3,
		4,60,8,4,1,5,3,5,63,8,5,1,5,1,5,3,5,67,8,5,5,5,69,8,5,10,5,12,5,72,9,5,
		1,6,1,6,1,6,3,6,77,8,6,1,7,1,7,1,7,0,0,8,0,2,4,6,8,10,12,14,0,1,2,0,1,
		1,3,3,82,0,19,1,0,0,0,2,27,1,0,0,0,4,37,1,0,0,0,6,39,1,0,0,0,8,41,1,0,
		0,0,10,62,1,0,0,0,12,73,1,0,0,0,14,78,1,0,0,0,16,18,3,2,1,0,17,16,1,0,
		0,0,18,21,1,0,0,0,19,17,1,0,0,0,19,20,1,0,0,0,20,22,1,0,0,0,21,19,1,0,
		0,0,22,23,5,0,0,1,23,1,1,0,0,0,24,26,3,4,2,0,25,24,1,0,0,0,26,29,1,0,0,
		0,27,25,1,0,0,0,27,28,1,0,0,0,28,30,1,0,0,0,29,27,1,0,0,0,30,34,3,8,4,
		0,31,33,3,4,2,0,32,31,1,0,0,0,33,36,1,0,0,0,34,32,1,0,0,0,34,35,1,0,0,
		0,35,3,1,0,0,0,36,34,1,0,0,0,37,38,5,1,0,0,38,5,1,0,0,0,39,40,5,8,0,0,
		40,7,1,0,0,0,41,42,5,2,0,0,42,46,3,6,3,0,43,45,3,12,6,0,44,43,1,0,0,0,
		45,48,1,0,0,0,46,44,1,0,0,0,46,47,1,0,0,0,47,59,1,0,0,0,48,46,1,0,0,0,
		49,56,5,4,0,0,50,51,3,10,5,0,51,52,5,2,0,0,52,53,5,6,0,0,53,54,5,8,0,0,
		54,55,5,4,0,0,55,57,1,0,0,0,56,50,1,0,0,0,56,57,1,0,0,0,57,60,1,0,0,0,
		58,60,5,5,0,0,59,49,1,0,0,0,59,58,1,0,0,0,60,9,1,0,0,0,61,63,3,14,7,0,
		62,61,1,0,0,0,62,63,1,0,0,0,63,70,1,0,0,0,64,66,3,8,4,0,65,67,3,14,7,0,
		66,65,1,0,0,0,66,67,1,0,0,0,67,69,1,0,0,0,68,64,1,0,0,0,69,72,1,0,0,0,
		70,68,1,0,0,0,70,71,1,0,0,0,71,11,1,0,0,0,72,70,1,0,0,0,73,76,3,6,3,0,
		74,75,5,7,0,0,75,77,5,10,0,0,76,74,1,0,0,0,76,77,1,0,0,0,77,13,1,0,0,0,
		78,79,7,0,0,0,79,15,1,0,0,0,10,19,27,34,46,56,59,62,66,70,76
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace dhll.v1
