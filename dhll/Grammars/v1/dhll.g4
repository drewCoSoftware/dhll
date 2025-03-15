grammar dhll;

file: (COMMENT | typedef)+? EOF;


// NOTE: Inline comments can't be used in typedefs... I think we probably need to fix that somehow so that they can go
// anywhere.....
typedef: 'typedef' identifier OBRACE (decl+)? CBRACE;

decl: prop? typename identifier initializer? EOS;

initializer: ASSIGN value;
prop:PROP;

value: INT | TRUE | FALSE | STRING;

identifier: ID;
typename: identifier;

scope: PUBLIC | PRIVATE;

ASSIGN: '=';
INT: [0-9]+;

FALSE: 'false';
TRUE: 'true';
PUBLIC: 'public';
PRIVATE: 'private';

PROP: 'prop';           // Indicates a declaration should be implemented as a property.

ID: FIRSTCHAR ([a-zA-Z0-9_]*);     // FIX: This is making it so that we need at least two chars.  making the second part optional (?) makes it so no ids are detected....
fragment FIRSTCHAR: [a-zA-Z_];


WORD: [a-zA-Z]+;

fragment SLASH: '/';
fragment QUOTE: '"';
OBRACE: '{';
CBRACE: '}';
EOS: ';';       // end of statement (line)

COMMENT: SLASH SLASH (.)*? '\n' ->channel(HIDDEN);
STRING: (QUOTE)(.*?)(QUOTE);


LF: '\n' -> skip;
WS: [ \r\n\t] -> skip;