parser grammar t2Parser;

options {
    tokenVocab = t2Lexer;
}

expression : EXP_OPEN 
            exp
            EXP_CLOSE;

exp: MINUS exp              #NEGATE
    | exp OPERATOR exp      #OPERATED
    | constant              #CONST
    ;

constant: str_literal | number;

number: INT | REAL;
str_literal: DOUBLE_QUOTED_STRING;


