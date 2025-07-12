parser grammar t2Parser;

options {
    tokenVocab = t2Lexer;
}

expression : EXP_OPEN 
            value 
            | value OPERATOR value
            EXP_CLOSE;

value: str_literal | number;
number: INT | REAL;
str_literal: DOUBLE_QUOTED_STRING;


// TODO: Make a lexer file?

