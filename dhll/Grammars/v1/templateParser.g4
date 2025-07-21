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
    : htmlChardata? ((htmlElement | expression) htmlChardata?)*
    ;

expression: EXP_OPEN expr EXP_CLOSE;

htmlAttribute
    : entityName (TAG_EQUALS attrValue)?
    ;

htmlChardata
    : HTML_TEXT
    | SEA_WS
    ;

attrValue
    : tag_expression                                #RAW_EXPRESSION
    | TAG_DQ_STR                                    #DBL_QUOTE_STRING
    ;

tag_expression: TAG_EXP_OPEN expr EXP_CLOSE;

expr: addExp;

// Addition
addExp
    : addExp PLUS subExp   # ADD
    | subExp               # TO_SUB
    ;

// Subtraction
subExp
    : subExp MINUS multExp  # SUBTRACT
    | multExp               # TO_MULT
    ;

// Multiply
multExp
    : multExp MULT divExp # MULTIPLY
    | divExp              # TO_DIV
    ;

// Divide
divExp
    : divExp DIVIDE unaryExpr   # DIVIDE
    | unaryExpr                 # TO_UNARY
    ;

// unary negation
unaryExpr
    : MINUS unaryExpr                       # NEGATE
    | parensExp                             # TO_PARENS
    ;

parensExp
    : OPEN_PAREN expr CLOSE_PAREN           # PARENS 
    | callExp                               # TO_CALL
    ;

callExp
    : ID OPEN_PAREN argList? CLOSE_PAREN    # CALL
    | primaryExpr                           # TO_PRIMARY
    ;


// numbers, variables, parentheses, function calls
primaryExpr
    : number                                # MAGIC_NUMBER
    | EXP_DQ_STR                            # MAGIC_STRING
    | ID                                    # VARIABLE
    ;

// comma-separated arguments
argList
    : expr (COMMA expr)*                    # ARGS
    ;

number: INT | REAL;
