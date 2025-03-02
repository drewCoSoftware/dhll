parser grammar templateParser;

options {
    tokenVocab = templateLexer;
}

template: htmlElements* EOF;

htmlElements
    : htmlMisc* htmlElement htmlMisc*
    ;

htmlMisc: SEA_WS;


htmlElement: TAG_OPEN TAG_NAME htmlAttribute* (TAG_CLOSE
     (htmlContent TAG_OPEN TAG_SLASH TAG_NAME TAG_CLOSE)?
        | TAG_SLASH_CLOSE)
;


htmlContent
    : htmlChardata? ((htmlElement) htmlChardata?)*
    ;

htmlAttribute
    : TAG_NAME (TAG_EQUALS ATTVALUE_VALUE)?
    ;

htmlChardata
    : HTML_TEXT
    | SEA_WS
    ;
