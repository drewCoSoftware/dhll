parser grammar templateParser;

options {
    tokenVocab = templateLexer;
}


templates: htmlElements* EOF;

htmlElements
    : htmlMisc* htmlElement htmlMisc*
    ;

htmlMisc: SEA_WS;

entityName: TAG_NAME;

htmlElement: TAG_OPEN entityName htmlAttribute* (TAG_CLOSE
     (htmlContent TAG_OPEN TAG_SLASH TAG_NAME TAG_CLOSE)?
        | TAG_SLASH_CLOSE)
;


htmlContent
    : htmlChardata? ((htmlElement) htmlChardata?)*
    ;

// NOTE: Because we are in a certain mode 
htmlAttribute
    : entityName (TAG_EQUALS attrValue)?
    ;

htmlChardata
    : HTML_TEXT
    | SEA_WS
    ;

attrValue
    : TAG_DBL_QUOTE EXP_OPEN expr EXP_CLOSE TAG_DBL_QUOTE
    | EXP_OPEN expr EXP_CLOSE
    | TAG_DQ_STR;

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
    | EXP_DQ_STR                            # MAGIC_STRING
    | ID                                    # VARIABLE
    | ID OPEN_PAREN argList? CLOSE_PAREN    # CALL
    | OPEN_PAREN expr CLOSE_PAREN           # PARENS
    ;

// comma-separated arguments
argList
    : expr (COMMA expr)*                    # ARGS
    ;

number: INT | REAL;
