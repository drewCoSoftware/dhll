parser grammar t2Parser;

options {
    tokenVocab = t2Lexer;
}

// TEMP:
file: expression* EOF;


expression : EXP_OPEN 
            expr
            EXP_CLOSE;

// NOTE: Aventerra's answer here :https://stackoverflow.com/questions/41017948/antlr4-the-following-sets-of-rules-are-mutually-left-recursive
// Is useful to undertand how the rule are actually interpreted by the parser....
// exp:
// 	 NEGATIVE exp                   # NEGATE
// 	| OPEN_PAREN exp CLOSE_PAREN	# PARENTHETICAL
//  | exp OPERATOR exp              # OPERATOR 
// 	| constant						# CONST
//  ;   


// Newer version with proper nesting:
// Taken from + corrected/modified from some LLM output.
expr: additiveExpr;

// + and -
additiveExpr
    : additiveExpr PLUS multiplicativeExpr   # ADD
    | additiveExpr MINUS multiplicativeExpr  # SUBTRACT
    | multiplicativeExpr                     # TO_MULT
    ;

// * and /
multiplicativeExpr
    : multiplicativeExpr MULT unaryExpr      # MULTIPLY
    | multiplicativeExpr DIVIDE unaryExpr    # DIVIDE
    | unaryExpr                              # TO_UNARY
    ;

// unary negation
unaryExpr
    : MINUS unaryExpr                       # NEGATE
    | primaryExpr                           # TO_PRIMARY
    ;

// numbers, variables, parentheses, function calls
primaryExpr
    : number                                # MAGIC_NUMBER
    | DOUBLE_QUOTED_STRING                  # MAGIC_STRING
    | ID                                    # VARIABLE
    | ID OPEN_PAREN argList? CLOSE_PAREN    # CALL
    | OPEN_PAREN expr CLOSE_PAREN           # PARENS
    ;

// comma-separated arguments
argList
    : expr (COMMA expr)*                    # ARGS
    ;

number: INT | REAL;


