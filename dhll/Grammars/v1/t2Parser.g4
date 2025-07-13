parser grammar t2Parser;

options {
    tokenVocab = t2Lexer;
}

// TEMP:
file: expression* EOF;


expression : EXP_OPEN 
            exp
            EXP_CLOSE;

// NOTE: Aventerra's answer here :https://stackoverflow.com/questions/41017948/antlr4-the-following-sets-of-rules-are-mutually-left-recursive
// Is useful to undertand how the rule are actually interpreted by the parser....

negate: MINUS exp;

exp:
	 MINUS exp                      # NEGATE
	| OPEN_PAREN exp CLOSE_PAREN	# PARENTHETICAL
	| exp PLUS exp					# ADD
	| exp MINUS exp					# SUBTRACT
	| exp MULT exp					# MULTIPLY
	| exp DIVIDE exp				# DIVIDE
	| constant						# CONST
    ;   

addExp: exp PLUS exp;
// minusExp: (exp MINUS exp);

// OPERATOR: PLUS 

constant: str_literal | number;

number: INT | REAL;
str_literal: DOUBLE_QUOTED_STRING;


