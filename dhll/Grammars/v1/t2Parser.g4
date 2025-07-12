parser grammar t2Parser;

options {
    tokenVocab = t2Lexer;
}

// TEMP:
file: expression* EOF;


expression : EXP_OPEN 
            exp
            EXP_CLOSE;

exp: MINUS exp                          #NEGATE
    | OPEN_PAREN exp CLOSE_PAREN        #PARENTHETICAL
    | exp PLUS exp                      #ADD
    | constant                          #CONST
    ;

// addExp: (exp PLUS exp);
// minusExp: (exp MINUS exp);

// OPERATOR: PLUS 

constant: str_literal | number;

number: INT | REAL;
str_literal: DOUBLE_QUOTED_STRING;


