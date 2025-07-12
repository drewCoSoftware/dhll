parser grammar t2Parser;

options {
    tokenVocab = t2Lexer;
}



expression : EXP_OPEN 
            value 
            | value OPERATOR value
            EXP_CLOSE;
    // : value 
    // | value OPERATOR value
    // ;

value: number;
number: INT | REAL;


// TODO: Make a lexer file?

