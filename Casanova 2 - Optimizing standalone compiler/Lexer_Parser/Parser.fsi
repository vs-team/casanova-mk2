// Signature file for parser generated by fsyacc
module CasanovaCompiler.Parser
type token = 
  | HASH_IF of (Position * string * LexerWhitespaceContinuation)
  | HASH_ELSE of (Position * string * LexerWhitespaceContinuation)
  | HASH_ENDIF of (Position * string * LexerWhitespaceContinuation)
  | COMMENT of (LexerWhitespaceContinuation)
  | WHITESPACE of (LexerWhitespaceContinuation)
  | HASH_LINE of (LexerWhitespaceContinuation)
  | HASH_LIGHT of (LexerWhitespaceContinuation)
  | INACTIVECODE of (LexerWhitespaceContinuation)
  | LINE_COMMENT of (LexerWhitespaceContinuation)
  | STRING_TEXT of (LexerWhitespaceContinuation)
  | EOF of (LexerWhitespaceContinuation)
  | LEX_FAILURE of (string)
  | ODUMMY of (token)
  | OINTERFACE_MEMBER
  | OBLOCKEND
  | OBLOCKEND_COMING_SOON
  | OBLOCKEND_IS_HERE
  | ORIGHT_BLOCK_END
  | ODECLEND
  | OEND
  | OBLOCKSEP
  | OBLOCKBEGIN
  | ORESET
  | OFUN
  | OFUNCTION
  | OWITH
  | OELSE
  | OBODY_OPERATOR
  | OTHEN
  | ODO_BANG
  | ODO
  | OSELECT
  | OWHERE
  | OYIELD
  | OWHEN
  | OPARALLEL_OPERATOR
  | OWAIT
  | OBINDER of (string)
  | OLET of (bool)
  | HIGH_PRECEDENCE_TYAPP
  | HIGH_PRECEDENCE_PAREN_APP
  | HIGH_PRECEDENCE_BRACK_APP
  | TYPE_COMING_SOON
  | TYPE_IS_HERE
  | MODULE_COMING_SOON
  | MODULE_IS_HERE
  | RULE
  | CREATE
  | WAIT
  | WAIT_UNTIL
  | YIELD
  | SELECT_OPERATOR
  | PARALLEL_OPERATOR
  | INT_SELECT_OPERATOR
  | EXTERN
  | VOID
  | PUBLIC
  | PRIVATE
  | INTERNAL
  | GLOBAL
  | STATIC
  | MEMBER
  | CLASS
  | ABSTRACT
  | OVERRIDE
  | DEFAULT
  | CONSTRUCTOR
  | INHERIT
  | GREATER_RBRACK
  | STRUCT
  | SIG
  | BAR
  | RBRACK
  | RBRACE
  | RBRACE_COMING_SOON
  | RBRACE_IS_HERE
  | MINUS
  | DOLLAR
  | LBRACE_LESS
  | BAR_RBRACK
  | GREATER_RBRACE
  | UNDERSCORE
  | SEMICOLON_SEMICOLON
  | LARROW
  | EQUALS
  | LBRACK
  | LBRACK_BAR
  | LBRACK_LESS
  | LBRACE
  | QMARK
  | QMARK_QMARK
  | DOT
  | COLON
  | COLON_COLON
  | COLON_GREATER
  | COLON_QMARK_GREATER
  | COLON_QMARK
  | COLON_EQUALS
  | SEMICOLON
  | WHEN
  | WHILE
  | WITH
  | HASH
  | AMP
  | AMP_AMP
  | QUOTE
  | LPAREN
  | RPAREN
  | RPAREN_COMING_SOON
  | RPAREN_IS_HERE
  | STAR
  | COMMA
  | RARROW
  | GREATER_BAR_RBRACK
  | LPAREN_STAR_RPAREN
  | IMPORT
  | OPEN
  | OR
  | REC
  | THEN
  | BODY_OPERATOR
  | TO
  | TRUE
  | TRY
  | TYPE
  | VAL
  | INLINE
  | INTERFACE
  | INSTANCE
  | CONST
  | WORLD_ENTITY
  | MASTER
  | SLAVE
  | CONNECT
  | DISCONNECT
  | VIRTUAL
  | ENTITY
  | LAZY
  | OLAZY
  | MATCH
  | MUTABLE
  | REFERENCE
  | NEW
  | OF
  | EXCEPTION
  | FALSE
  | FOR
  | FUN
  | FUNCTION
  | IF
  | IN
  | JOIN_IN
  | FINALLY
  | DO_BANG
  | AND
  | AS
  | ASSERT
  | OASSERT
  | ASR
  | BEGIN
  | DO
  | DONE
  | DOWNTO
  | ELSE
  | ELIF
  | END
  | DOT_DOT
  | BAR_BAR
  | UPCAST
  | DOWNCAST
  | NULL
  | RESERVED
  | MODULE
  | NAMESPACE
  | DELEGATE
  | CONSTRAINT
  | BASE
  | LQUOTE of (string * bool)
  | RQUOTE of (string * bool)
  | RQUOTE_DOT of (string * bool)
  | PERCENT_OP of (string)
  | BINDER of (string)
  | LESS of (bool)
  | GREATER of (bool)
  | LET of (bool)
  | YIELD_BANG of (bool)
  | BIGNUM of ((string * string))
  | DECIMAL of (System.Decimal)
  | CHAR of (char)
  | IEEE64 of (double)
  | IEEE32 of (single)
  | NATIVEINT of (int64)
  | UNATIVEINT of (uint64)
  | UINT64 of (uint64)
  | UINT32 of (uint32)
  | UINT16 of (uint16)
  | UINT8 of (byte)
  | INT64 of (int64 * bool)
  | INT32 of (int32 * bool)
  | INT32_DOT_DOT of (int32 * bool)
  | INT16 of (int16 * bool)
  | INT8 of (sbyte * bool)
  | FUNKY_OPERATOR_NAME of (string)
  | ADJACENT_PREFIX_OP of (string)
  | PLUS_MINUS_OP of (string)
  | INFIX_AMP_OP of (string)
  | INFIX_STAR_DIV_MOD_OP of (string)
  | PREFIX_OP of (string)
  | INFIX_BAR_OP of (string)
  | INFIX_AT_HAT_OP of (string)
  | INFIX_COMPARE_OP of (string)
  | INFIX_STAR_STAR_OP of (string)
  | IDENT of (string)
  | KEYWORD_STRING of (string)
  | STRING of (string)
  | BYTEARRAY of (byte[])
  | A
  | B
  | C
type tokenId = 
    | TOKEN_HASH_IF
    | TOKEN_HASH_ELSE
    | TOKEN_HASH_ENDIF
    | TOKEN_COMMENT
    | TOKEN_WHITESPACE
    | TOKEN_HASH_LINE
    | TOKEN_HASH_LIGHT
    | TOKEN_INACTIVECODE
    | TOKEN_LINE_COMMENT
    | TOKEN_STRING_TEXT
    | TOKEN_EOF
    | TOKEN_LEX_FAILURE
    | TOKEN_ODUMMY
    | TOKEN_OINTERFACE_MEMBER
    | TOKEN_OBLOCKEND
    | TOKEN_OBLOCKEND_COMING_SOON
    | TOKEN_OBLOCKEND_IS_HERE
    | TOKEN_ORIGHT_BLOCK_END
    | TOKEN_ODECLEND
    | TOKEN_OEND
    | TOKEN_OBLOCKSEP
    | TOKEN_OBLOCKBEGIN
    | TOKEN_ORESET
    | TOKEN_OFUN
    | TOKEN_OFUNCTION
    | TOKEN_OWITH
    | TOKEN_OELSE
    | TOKEN_OBODY_OPERATOR
    | TOKEN_OTHEN
    | TOKEN_ODO_BANG
    | TOKEN_ODO
    | TOKEN_OSELECT
    | TOKEN_OWHERE
    | TOKEN_OYIELD
    | TOKEN_OWHEN
    | TOKEN_OPARALLEL_OPERATOR
    | TOKEN_OWAIT
    | TOKEN_OBINDER
    | TOKEN_OLET
    | TOKEN_HIGH_PRECEDENCE_TYAPP
    | TOKEN_HIGH_PRECEDENCE_PAREN_APP
    | TOKEN_HIGH_PRECEDENCE_BRACK_APP
    | TOKEN_TYPE_COMING_SOON
    | TOKEN_TYPE_IS_HERE
    | TOKEN_MODULE_COMING_SOON
    | TOKEN_MODULE_IS_HERE
    | TOKEN_RULE
    | TOKEN_CREATE
    | TOKEN_WAIT
    | TOKEN_WAIT_UNTIL
    | TOKEN_YIELD
    | TOKEN_SELECT_OPERATOR
    | TOKEN_PARALLEL_OPERATOR
    | TOKEN_INT_SELECT_OPERATOR
    | TOKEN_EXTERN
    | TOKEN_VOID
    | TOKEN_PUBLIC
    | TOKEN_PRIVATE
    | TOKEN_INTERNAL
    | TOKEN_GLOBAL
    | TOKEN_STATIC
    | TOKEN_MEMBER
    | TOKEN_CLASS
    | TOKEN_ABSTRACT
    | TOKEN_OVERRIDE
    | TOKEN_DEFAULT
    | TOKEN_CONSTRUCTOR
    | TOKEN_INHERIT
    | TOKEN_GREATER_RBRACK
    | TOKEN_STRUCT
    | TOKEN_SIG
    | TOKEN_BAR
    | TOKEN_RBRACK
    | TOKEN_RBRACE
    | TOKEN_RBRACE_COMING_SOON
    | TOKEN_RBRACE_IS_HERE
    | TOKEN_MINUS
    | TOKEN_DOLLAR
    | TOKEN_LBRACE_LESS
    | TOKEN_BAR_RBRACK
    | TOKEN_GREATER_RBRACE
    | TOKEN_UNDERSCORE
    | TOKEN_SEMICOLON_SEMICOLON
    | TOKEN_LARROW
    | TOKEN_EQUALS
    | TOKEN_LBRACK
    | TOKEN_LBRACK_BAR
    | TOKEN_LBRACK_LESS
    | TOKEN_LBRACE
    | TOKEN_QMARK
    | TOKEN_QMARK_QMARK
    | TOKEN_DOT
    | TOKEN_COLON
    | TOKEN_COLON_COLON
    | TOKEN_COLON_GREATER
    | TOKEN_COLON_QMARK_GREATER
    | TOKEN_COLON_QMARK
    | TOKEN_COLON_EQUALS
    | TOKEN_SEMICOLON
    | TOKEN_WHEN
    | TOKEN_WHILE
    | TOKEN_WITH
    | TOKEN_HASH
    | TOKEN_AMP
    | TOKEN_AMP_AMP
    | TOKEN_QUOTE
    | TOKEN_LPAREN
    | TOKEN_RPAREN
    | TOKEN_RPAREN_COMING_SOON
    | TOKEN_RPAREN_IS_HERE
    | TOKEN_STAR
    | TOKEN_COMMA
    | TOKEN_RARROW
    | TOKEN_GREATER_BAR_RBRACK
    | TOKEN_LPAREN_STAR_RPAREN
    | TOKEN_IMPORT
    | TOKEN_OPEN
    | TOKEN_OR
    | TOKEN_REC
    | TOKEN_THEN
    | TOKEN_BODY_OPERATOR
    | TOKEN_TO
    | TOKEN_TRUE
    | TOKEN_TRY
    | TOKEN_TYPE
    | TOKEN_VAL
    | TOKEN_INLINE
    | TOKEN_INTERFACE
    | TOKEN_INSTANCE
    | TOKEN_CONST
    | TOKEN_WORLD_ENTITY
    | TOKEN_MASTER
    | TOKEN_SLAVE
    | TOKEN_CONNECT
    | TOKEN_DISCONNECT
    | TOKEN_VIRTUAL
    | TOKEN_ENTITY
    | TOKEN_LAZY
    | TOKEN_OLAZY
    | TOKEN_MATCH
    | TOKEN_MUTABLE
    | TOKEN_REFERENCE
    | TOKEN_NEW
    | TOKEN_OF
    | TOKEN_EXCEPTION
    | TOKEN_FALSE
    | TOKEN_FOR
    | TOKEN_FUN
    | TOKEN_FUNCTION
    | TOKEN_IF
    | TOKEN_IN
    | TOKEN_JOIN_IN
    | TOKEN_FINALLY
    | TOKEN_DO_BANG
    | TOKEN_AND
    | TOKEN_AS
    | TOKEN_ASSERT
    | TOKEN_OASSERT
    | TOKEN_ASR
    | TOKEN_BEGIN
    | TOKEN_DO
    | TOKEN_DONE
    | TOKEN_DOWNTO
    | TOKEN_ELSE
    | TOKEN_ELIF
    | TOKEN_END
    | TOKEN_DOT_DOT
    | TOKEN_BAR_BAR
    | TOKEN_UPCAST
    | TOKEN_DOWNCAST
    | TOKEN_NULL
    | TOKEN_RESERVED
    | TOKEN_MODULE
    | TOKEN_NAMESPACE
    | TOKEN_DELEGATE
    | TOKEN_CONSTRAINT
    | TOKEN_BASE
    | TOKEN_LQUOTE
    | TOKEN_RQUOTE
    | TOKEN_RQUOTE_DOT
    | TOKEN_PERCENT_OP
    | TOKEN_BINDER
    | TOKEN_LESS
    | TOKEN_GREATER
    | TOKEN_LET
    | TOKEN_YIELD_BANG
    | TOKEN_BIGNUM
    | TOKEN_DECIMAL
    | TOKEN_CHAR
    | TOKEN_IEEE64
    | TOKEN_IEEE32
    | TOKEN_NATIVEINT
    | TOKEN_UNATIVEINT
    | TOKEN_UINT64
    | TOKEN_UINT32
    | TOKEN_UINT16
    | TOKEN_UINT8
    | TOKEN_INT64
    | TOKEN_INT32
    | TOKEN_INT32_DOT_DOT
    | TOKEN_INT16
    | TOKEN_INT8
    | TOKEN_FUNKY_OPERATOR_NAME
    | TOKEN_ADJACENT_PREFIX_OP
    | TOKEN_PLUS_MINUS_OP
    | TOKEN_INFIX_AMP_OP
    | TOKEN_INFIX_STAR_DIV_MOD_OP
    | TOKEN_PREFIX_OP
    | TOKEN_INFIX_BAR_OP
    | TOKEN_INFIX_AT_HAT_OP
    | TOKEN_INFIX_COMPARE_OP
    | TOKEN_INFIX_STAR_STAR_OP
    | TOKEN_IDENT
    | TOKEN_KEYWORD_STRING
    | TOKEN_STRING
    | TOKEN_BYTEARRAY
    | TOKEN_A
    | TOKEN_B
    | TOKEN_C
    | TOKEN_end_of_input
    | TOKEN_error
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_stringOrKeywordString
    | NONTERM_cnvModuleIntro
    | NONTERM_casanovaModuleImpl
    | NONTERM_moduleDefnsOrExprPossiblyEmptyOrBlock
    | NONTERM_moduleDefnsOrExprPossiblyEmpty
    | NONTERM_moduleDefnsOrExpr
    | NONTERM_moduleDefns
    | NONTERM_moduleDefnOrDirective
    | NONTERM_opt_attributes
    | NONTERM_attributes
    | NONTERM_attributeList
    | NONTERM_attributeListElements
    | NONTERM_attribute
    | NONTERM_attributeTarget
    | NONTERM_moduleDefn
    | NONTERM_namedModuleAbbrevBlock
    | NONTERM_namedModuleDefnBlock
    | NONTERM_wrappedNamedModuleDefn
    | NONTERM_opt_signature
    | NONTERM_tyconDefnAugmentation
    | NONTERM_opt_literalValue
    | NONTERM_memberFlags
    | NONTERM_memberSpecFlags
    | NONTERM_classMemberSpfnGetSet
    | NONTERM_classMemberSpfnGetSetElements
    | NONTERM_typeNameInfo
    | NONTERM_tyconDefnList
    | NONTERM_tyconDefn
    | NONTERM_tyconDefnRhsBlock
    | NONTERM_cnvTyconDefnRhs
    | NONTERM_tyconDefnRhs
    | NONTERM_classDefnBlock
    | NONTERM_classDefnMembers
    | NONTERM_classDefnMembersAtLeastOne
    | NONTERM_memberCore
    | NONTERM_createMemberCoreWithParams
    | NONTERM_createMemberCore
    | NONTERM_abstractMemberFlags
    | NONTERM_classDefnMember
    | NONTERM_opt_typ
    | NONTERM_atomicPatternLongIdent
    | NONTERM_opt_declVisibility
    | NONTERM_opt_classDefn
    | NONTERM_optAsSpec
    | NONTERM_asSpec
    | NONTERM_optBaseSpec
    | NONTERM_baseSpec
    | NONTERM_memberOrOverride
    | NONTERM_tyconDefnOrSpfnSimpleRepr
    | NONTERM_tyconDefnOrSpfnCnvRepr
    | NONTERM_braceFieldOrRuleDeclList
    | NONTERM_fieldOrRuleList
    | NONTERM_topReturnTypeWithTypeConstraints
    | NONTERM_cnvFieldInitCore
    | NONTERM_fieldOrRule
    | NONTERM_inherit_idents
    | NONTERM_braceFieldDeclList
    | NONTERM_interfaceMember
    | NONTERM_tyconNameAndTyparDecls
    | NONTERM_typarAlts
    | NONTERM_unionRepr
    | NONTERM_attrUnionCaseDecls
    | NONTERM_attrUnionCaseDecl
    | NONTERM_unionCaseName
    | NONTERM_firstUnionCaseDeclOfMany
    | NONTERM_firstUnionCaseDecl
    | NONTERM_unionCaseRepr
    | NONTERM_recdFieldDeclList
    | NONTERM_recdFieldDecl
    | NONTERM_fieldDecl
    | NONTERM_importDecl
    | NONTERM_importLibrary
    | NONTERM_defnBindings
    | NONTERM_doBinding
    | NONTERM_hardwhiteLetBindings
    | NONTERM_hardwhiteDoBinding
    | NONTERM_classDefnBindings
    | NONTERM_hardwhiteDefnBindingsTerminator
    | NONTERM_localBindings
    | NONTERM_moreLocalBindings
    | NONTERM_attr_localBinding
    | NONTERM_localBinding
    | NONTERM_typedExprWithStaticOptimizationsBlock
    | NONTERM_typedExprWithStaticOptimizations
    | NONTERM_opt_staticOptimizations
    | NONTERM_staticOptimization
    | NONTERM_staticOptimizationConditions
    | NONTERM_staticOptimizationCondition
    | NONTERM_rawConstant
    | NONTERM_anonLambdaExpr
    | NONTERM_constant
    | NONTERM_bindingPattern
    | NONTERM_createBindingPattern
    | NONTERM_headBindingPattern
    | NONTERM_tuplePatternElements
    | NONTERM_conjPatternElements
    | NONTERM_constrPattern
    | NONTERM_opt_explicitValTyparDecls2
    | NONTERM_atomicPatterns
    | NONTERM_atomicPattern
    | NONTERM_parenPatternBody
    | NONTERM_parenPattern
    | NONTERM_tupleParenPatternElements
    | NONTERM_conjParenPatternElements
    | NONTERM_recordPatternElements
    | NONTERM_recordPatternElementsAux
    | NONTERM_recordPatternElement
    | NONTERM_listPatternElements
    | NONTERM_fromAssignmentExprBlock
    | NONTERM_typedSeqExprBlock
    | NONTERM_declExprBlock
    | NONTERM_typedSeqExprBlockR
    | NONTERM_typedSeqExpr
    | NONTERM_seqExpr
    | NONTERM_recover
    | NONTERM_choices
    | NONTERM_intChoices
    | NONTERM_choiceBody
    | NONTERM_parallel
    | NONTERM_declExpr
    | NONTERM_dynamicArg
    | NONTERM_withClauses
    | NONTERM_withPatternClauses
    | NONTERM_patternAndGuard
    | NONTERM_patternClauses
    | NONTERM_patternGuard
    | NONTERM_patternResult
    | NONTERM_fromExprCondition
    | NONTERM_ifExprCases
    | NONTERM_ifExprThen
    | NONTERM_ifExprElifs
    | NONTERM_tuplePat
    | NONTERM_tupleExpr
    | NONTERM_minusExpr
    | NONTERM_appExpr
    | NONTERM_argExpr
    | NONTERM_atomicExpr
    | NONTERM_atomicExprQualification
    | NONTERM_optRange
    | NONTERM_atomicExprAfterType
    | NONTERM_beginEndExpr
    | NONTERM_arrayExpr
    | NONTERM_parenExpr
    | NONTERM_parenExprBody
    | NONTERM_staticallyKnownHeadTypars
    | NONTERM_staticallyKnownHeadTyparAlts
    | NONTERM_braceExpr
    | NONTERM_braceExprBody
    | NONTERM_monadicExprBody
    | NONTERM_listExprElements
    | NONTERM_monadicExprInitial
    | NONTERM_rangeSequenceExpr
    | NONTERM_monadicSingleLineQualifiersThenArrowThenExprR
    | NONTERM_forLoopBinder
    | NONTERM_fromLoopBinder
    | NONTERM_fromTupleExpr
    | NONTERM_forLoopRange
    | NONTERM_opt_curriedArgExprs
    | NONTERM_opt_atomicExprAfterType
    | NONTERM_recdExpr
    | NONTERM_opt_seps_recd
    | NONTERM_seps_recd
    | NONTERM_pathOrUnderscore
    | NONTERM_recdExprBindings
    | NONTERM_recdBinding
    | NONTERM_forLoopDirection
    | NONTERM_typeWithTypeConstraints
    | NONTERM_topTypeWithTypeConstraints
    | NONTERM_opt_topReturnTypeWithTypeConstraints
    | NONTERM_topType
    | NONTERM_topTupleType
    | NONTERM_topTupleTypeElements
    | NONTERM_topAppType
    | NONTERM_typ
    | NONTERM_tupleType
    | NONTERM_tupleOrQuotTypeElements
    | NONTERM_tupleTypeElements
    | NONTERM_appTypeCon
    | NONTERM_appTypeConPower
    | NONTERM_appType
    | NONTERM_arrayTypeSuffix
    | NONTERM_appTypePrefixArguments
    | NONTERM_typeArgListElements
    | NONTERM_powerType
    | NONTERM_appTypeNonAtomicDeprecated
    | NONTERM_powerTypeNonAtomicDeprecated
    | NONTERM_atomType
    | NONTERM_typeArgsNoHpaDeprecated
    | NONTERM_typeArgsActual
    | NONTERM_typeArgActual
    | NONTERM_typeArgActualOrDummyIfEmpty
    | NONTERM_measureTypeArg
    | NONTERM_measureTypeAtom
    | NONTERM_measureTypePower
    | NONTERM_measureTypeSeq
    | NONTERM_measureTypeExpr
    | NONTERM_typar
    | NONTERM_staticallyKnownHeadTypar
    | NONTERM_ident
    | NONTERM_path
    | NONTERM_opName
    | NONTERM_operatorName
    | NONTERM_barName
    | NONTERM_barNames
    | NONTERM_identOrOp
    | NONTERM_pathOp
    | NONTERM_nameop
    | NONTERM_topSeparator
    | NONTERM_topSeparators
    | NONTERM_interactiveSeparator
    | NONTERM_interactiveSeparators
    | NONTERM_opt_interactiveSeparators
    | NONTERM_opt_topSeparators
    | NONTERM_seps
    | NONTERM_declEnd
    | NONTERM_opt_declEnd
    | NONTERM_opt_ODECLEND
    | NONTERM_deprecated_opt_equals
    | NONTERM_opt_OBLOCKSEP
    | NONTERM_opt_seps
    | NONTERM_opt_rec
    | NONTERM_opt_bar
    | NONTERM_opt_inline
    | NONTERM_opt_mutable
    | NONTERM_opt_ref
    | NONTERM_doToken
    | NONTERM_doneDeclEnd
    | NONTERM_structOrBegin
    | NONTERM_sigOrBegin
    | NONTERM_colonOrEquals
    | NONTERM_opt_HASH
    | NONTERM_opt_HIGH_PRECEDENCE_APP
    | NONTERM_opt_HIGH_PRECEDENCE_TYAPP
    | NONTERM_type_keyword
    | NONTERM_module_keyword
    | NONTERM_rbrace
    | NONTERM_rparen
    | NONTERM_oblockend
    | NONTERM_ends_other_than_rparen_coming_soon_or_recover
    | NONTERM_ends_coming_soon_or_recover
/// This function maps integers indexes to symbolic token ids
val tagOfToken: token -> int

/// This function maps integers indexes to symbolic token ids
val tokenTagToTokenId: int -> tokenId

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
val prodIdxToNonTerminal: int -> nonTerminalId

/// This function gets the name of a token as a string
val token_to_string: token -> string
val start : (Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> token) -> Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> (Program) 
