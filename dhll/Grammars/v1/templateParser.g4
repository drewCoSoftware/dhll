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

htmlAttribute
    : entityName (TAG_EQUALS ATTVALUE_VALUE)?
    ;

htmlChardata
    : HTML_TEXT
    | SEA_WS
    ;
