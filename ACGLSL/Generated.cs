using System;
using ACC;
using ACGLSL.Parse.Structure;
using static ACGLSL.Parse.Parser;
using static ACGLSL.Tokenizer.TT;
using static ACGLSL.Debug.Debugger;
//These code are generate by AsixaCompilerCompiler
namespace ACC.Generated
{
    /*
    variable_identifier →
         | IDENTIFIER 

    */
    public class variable_identifier : Node
    {
        public static bool Check() => CheckToken(IDENTIFIER);

        public static variable_identifier Match()
        {
            var obj = new variable_identifier();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    primary_expression →
         | variable_identifier 
         | INTCONSTANT 
         | UINTCONSTANT 
         | FLOATCONSTANT 
         | BOOLCONSTANT 
         | LEFT_PAREN expression RIGHT_PAREN 

    */
    public class primary_expression : Node
    {
        public static bool Check() => variable_identifier.Check() || CheckToken(INTCONSTANT) ||
                                      CheckToken(UINTCONSTANT) || CheckToken(FLOATCONSTANT) ||
                                      CheckToken(BOOLCONSTANT) || CheckToken(LEFT_PAREN);

        public static primary_expression Match()
        {
            var obj = new primary_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(variable_identifier.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(INTCONSTANT)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(UINTCONSTANT)))) goto production0_4; // normal goto & last go
                return obj;
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(FLOATCONSTANT)))) goto production0_5; // normal goto & last go
                return obj;
                production0_5: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BOOLCONSTANT)))) goto production0_6; // normal goto & last go
                return obj;
                production0_6: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) return null; // fail goto
                {
                    // start of node LEFT_PAREN
                    if (!obj.Try(expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node expression
                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    postfix_expression →
         | primary_expression _postfix_expression 
         | function_call _postfix_expression 

    */
    public class postfix_expression : Node
    {
        public static bool Check() => primary_expression.Check() || function_call.Check();

        public static postfix_expression Match()
        {
            var obj = new postfix_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(primary_expression.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node primary_expression
                    if (!obj.Try(_postfix_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_call.Match())) return null; // fail goto
                {
                    // start of node function_call
                    if (!obj.Try(_postfix_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    _postfix_expression →
         | LEFT_BRACKET integer_expression RIGHT_BRACKET _postfix_expression 
         | DOT FIELD_SELECTION _postfix_expression 
         | INC_OP _postfix_expression 
         | DEC_OP _postfix_expression 
         | E 

    */
    public class _postfix_expression : Node
    {
        public static bool Check() => CheckToken(LEFT_BRACKET) || CheckToken(DOT) || CheckToken(INC_OP) ||
                                      CheckToken(DEC_OP) || true;

        public static _postfix_expression Match()
        {
            var obj = new _postfix_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET)))) goto production0_2; // normal goto & last go
                {
                    // start of node LEFT_BRACKET
                    if (!obj.Try(integer_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node integer_expression
                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                            goto production2_END; // normal goto & last go
                        {
                            // start of node RIGHT_BRACKET
                            if (!obj.Try(_postfix_expression.Match())) goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DOT)))) goto production0_3; // normal goto & last go
                {
                    // start of node DOT
                    if (!obj.Try(new TokenNode(MatchToken(FIELD_SELECTION))))
                        goto production1_END; // normal goto & last go
                    {
                        // start of node FIELD_SELECTION
                        if (!obj.Try(_postfix_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(INC_OP)))) goto production0_4; // normal goto & last go
                {
                    // start of node INC_OP
                    if (!obj.Try(_postfix_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DEC_OP)))) goto production0_5; // normal goto & last go
                {
                    // start of node DEC_OP
                    if (!obj.Try(_postfix_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_5: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    integer_expression →
         | expression 

    */
    public class integer_expression : Node
    {
        public static bool Check() => expression.Check();

        public static integer_expression Match()
        {
            var obj = new integer_expression();
            {
                // start of node 
                if (!obj.Try(expression.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    function_call →
         | function_call_or_method 

    */
    public class function_call : Node
    {
        public static bool Check() => function_call_or_method.Check();

        public static function_call Match()
        {
            var obj = new function_call();
            {
                // start of node 
                if (!obj.Try(function_call_or_method.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    function_call_or_method →
         | function_call_generic 
         | postfix_expression DOT function_call_generic 

    */
    public class function_call_or_method : Node
    {
        public static bool Check() => function_call_generic.Check() || postfix_expression.Check();

        public static function_call_or_method Match()
        {
            var obj = new function_call_or_method();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_call_generic.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(postfix_expression.Match())) return null; // fail goto
                {
                    // start of node postfix_expression
                    if (!obj.Try(new TokenNode(MatchToken(DOT)))) goto production1_END; // normal goto & last go
                    {
                        // start of node DOT
                        if (!obj.Try(function_call_generic.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    function_call_generic →
         | function_call_header_with_parameters RIGHT_PAREN 
         | function_call_header_no_parameters RIGHT_PAREN 

    */
    public class function_call_generic : Node
    {
        public static bool Check() =>
            function_call_header_with_parameters.Check() || function_call_header_no_parameters.Check();

        public static function_call_generic Match()
        {
            var obj = new function_call_generic();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_call_header_with_parameters.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node function_call_header_with_parameters
                    if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_call_header_no_parameters.Match())) return null; // fail goto
                {
                    // start of node function_call_header_no_parameters
                    if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    function_call_header_no_parameters →
         | function_call_header VOID 
         | function_call_header 

    */
    public class function_call_header_no_parameters : Node
    {
        public static bool Check() => function_call_header.Check();

        public static function_call_header_no_parameters Match()
        {
            var obj = new function_call_header_no_parameters();
            {
                // start of node 
                if (!obj.Try(function_call_header.Match())) return null; // fail goto
                {
                    // start of node function_call_header
                    if (!obj.Try(new TokenNode(MatchToken(VOID)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    function_call_header_with_parameters →
         | function_call_header assignment_expression _function_call_header_with_parameters 

    */
    public class function_call_header_with_parameters : Node
    {
        public static bool Check() => function_call_header.Check();

        public static function_call_header_with_parameters Match()
        {
            var obj = new function_call_header_with_parameters();
            {
                // start of node 
                if (!obj.Try(function_call_header.Match())) return null; // fail goto
                {
                    // start of node function_call_header
                    if (!obj.Try(assignment_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node assignment_expression
                        if (!obj.Try(_function_call_header_with_parameters.Match()))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _function_call_header_with_parameters →
         | COMMA assignment_expression _function_call_header_with_parameters 
         | E 

    */
    public class _function_call_header_with_parameters : Node
    {
        public static bool Check() => CheckToken(COMMA) || true;

        public static _function_call_header_with_parameters Match()
        {
            var obj = new _function_call_header_with_parameters();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(COMMA)))) goto production0_2; // normal goto & last go
                {
                    // start of node COMMA
                    if (!obj.Try(assignment_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node assignment_expression
                        if (!obj.Try(_function_call_header_with_parameters.Match()))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    function_call_header →
         | function_identifier LEFT_PAREN 

    */
    public class function_call_header : Node
    {
        public static bool Check() => function_identifier.Check();

        public static function_call_header Match()
        {
            var obj = new function_call_header();
            {
                // start of node 
                if (!obj.Try(function_identifier.Match())) return null; // fail goto
                {
                    // start of node function_identifier
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    function_identifier →
         | type_specifier 
         | IDENTIFIER 
         | FIELD_SELECTION 

    */
    public class function_identifier : Node
    {
        public static bool Check() => type_specifier.Check() || CheckToken(IDENTIFIER) || CheckToken(FIELD_SELECTION);

        public static function_identifier Match()
        {
            var obj = new function_identifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_specifier.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(FIELD_SELECTION)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    unary_expression →
         | postfix_expression 
         | INC_OP unary_expression 
         | DEC_OP unary_expression 
         | unary_operator unary_expression 

    */
    public class unary_expression : Node
    {
        public static bool Check() => postfix_expression.Check() || CheckToken(INC_OP) || CheckToken(DEC_OP) ||
                                      unary_operator.Check();

        public static unary_expression Match()
        {
            var obj = new unary_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(postfix_expression.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(INC_OP)))) goto production0_3; // normal goto & last go
                {
                    // start of node INC_OP
                    if (!obj.Try(unary_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DEC_OP)))) goto production0_4; // normal goto & last go
                {
                    // start of node DEC_OP
                    if (!obj.Try(unary_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(unary_operator.Match())) return null; // fail goto
                {
                    // start of node unary_operator
                    if (!obj.Try(unary_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    unary_operator →
         | PLUS 
         | DASH 
         | BANG 
         | TILDE 

    */
    public class unary_operator : Node
    {
        public static bool Check() => CheckToken(PLUS) || CheckToken(DASH) || CheckToken(BANG) || CheckToken(TILDE);

        public static unary_operator Match()
        {
            var obj = new unary_operator();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(PLUS)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DASH)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BANG)))) goto production0_4; // normal goto & last go
                return obj;
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(TILDE)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    multiplicative_expression →
         | unary_expression _multiplicative_expression 

    */
    public class multiplicative_expression : Node
    {
        public static bool Check() => unary_expression.Check();

        public static multiplicative_expression Match()
        {
            var obj = new multiplicative_expression();
            {
                // start of node 
                if (!obj.Try(unary_expression.Match())) return null; // fail goto
                {
                    // start of node unary_expression
                    if (!obj.Try(_multiplicative_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _multiplicative_expression →
         | STAR unary_expression _multiplicative_expression 
         | SLASH unary_expression _multiplicative_expression 
         | PERCENT unary_expression _multiplicative_expression 
         | E 

    */
    public class _multiplicative_expression : Node
    {
        public static bool Check() => CheckToken(STAR) || CheckToken(SLASH) || CheckToken(PERCENT) || true;

        public static _multiplicative_expression Match()
        {
            var obj = new _multiplicative_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(STAR)))) goto production0_2; // normal goto & last go
                {
                    // start of node STAR
                    if (!obj.Try(unary_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node unary_expression
                        if (!obj.Try(_multiplicative_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SLASH)))) goto production0_3; // normal goto & last go
                {
                    // start of node SLASH
                    if (!obj.Try(unary_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node unary_expression
                        if (!obj.Try(_multiplicative_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(PERCENT)))) goto production0_4; // normal goto & last go
                {
                    // start of node PERCENT
                    if (!obj.Try(unary_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node unary_expression
                        if (!obj.Try(_multiplicative_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_4: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    additive_expression →
         | multiplicative_expression _additive_expression 

    */
    public class additive_expression : Node
    {
        public static bool Check() => multiplicative_expression.Check();

        public static additive_expression Match()
        {
            var obj = new additive_expression();
            {
                // start of node 
                if (!obj.Try(multiplicative_expression.Match())) return null; // fail goto
                {
                    // start of node multiplicative_expression
                    if (!obj.Try(_additive_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _additive_expression →
         | PLUS multiplicative_expression _additive_expression 
         | DASH multiplicative_expression _additive_expression 
         | E 

    */
    public class _additive_expression : Node
    {
        public static bool Check() => CheckToken(PLUS) || CheckToken(DASH) || true;

        public static _additive_expression Match()
        {
            var obj = new _additive_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(PLUS)))) goto production0_2; // normal goto & last go
                {
                    // start of node PLUS
                    if (!obj.Try(multiplicative_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node multiplicative_expression
                        if (!obj.Try(_additive_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DASH)))) goto production0_3; // normal goto & last go
                {
                    // start of node DASH
                    if (!obj.Try(multiplicative_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node multiplicative_expression
                        if (!obj.Try(_additive_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    shift_expression →
         | additive_expression _shift_expression 

    */
    public class shift_expression : Node
    {
        public static bool Check() => additive_expression.Check();

        public static shift_expression Match()
        {
            var obj = new shift_expression();
            {
                // start of node 
                if (!obj.Try(additive_expression.Match())) return null; // fail goto
                {
                    // start of node additive_expression
                    if (!obj.Try(_shift_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _shift_expression →
         | LEFT_OP additive_expression _shift_expression 
         | RIGHT_OP additive_expression _shift_expression 
         | E 

    */
    public class _shift_expression : Node
    {
        public static bool Check() => CheckToken(LEFT_OP) || CheckToken(RIGHT_OP) || true;

        public static _shift_expression Match()
        {
            var obj = new _shift_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LEFT_OP)))) goto production0_2; // normal goto & last go
                {
                    // start of node LEFT_OP
                    if (!obj.Try(additive_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node additive_expression
                        if (!obj.Try(_shift_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(RIGHT_OP)))) goto production0_3; // normal goto & last go
                {
                    // start of node RIGHT_OP
                    if (!obj.Try(additive_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node additive_expression
                        if (!obj.Try(_shift_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    relational_expression →
         | shift_expression _relational_expression 

    */
    public class relational_expression : Node
    {
        public static bool Check() => shift_expression.Check();

        public static relational_expression Match()
        {
            var obj = new relational_expression();
            {
                // start of node 
                if (!obj.Try(shift_expression.Match())) return null; // fail goto
                {
                    // start of node shift_expression
                    if (!obj.Try(_relational_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _relational_expression →
         | LEFT_ANGLE shift_expression _relational_expression 
         | RIGHT_ANGLE shift_expression _relational_expression 
         | LE_OP shift_expression _relational_expression 
         | GE_OP shift_expression _relational_expression 
         | E 

    */
    public class _relational_expression : Node
    {
        public static bool Check() => CheckToken(LEFT_ANGLE) || CheckToken(RIGHT_ANGLE) || CheckToken(LE_OP) ||
                                      CheckToken(GE_OP) || true;

        public static _relational_expression Match()
        {
            var obj = new _relational_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LEFT_ANGLE)))) goto production0_2; // normal goto & last go
                {
                    // start of node LEFT_ANGLE
                    if (!obj.Try(shift_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node shift_expression
                        if (!obj.Try(_relational_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(RIGHT_ANGLE)))) goto production0_3; // normal goto & last go
                {
                    // start of node RIGHT_ANGLE
                    if (!obj.Try(shift_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node shift_expression
                        if (!obj.Try(_relational_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LE_OP)))) goto production0_4; // normal goto & last go
                {
                    // start of node LE_OP
                    if (!obj.Try(shift_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node shift_expression
                        if (!obj.Try(_relational_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(GE_OP)))) goto production0_5; // normal goto & last go
                {
                    // start of node GE_OP
                    if (!obj.Try(shift_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node shift_expression
                        if (!obj.Try(_relational_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_5: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    equality_expression →
         | relational_expression _equality_expression 

    */
    public class equality_expression : Node
    {
        public static bool Check() => relational_expression.Check();

        public static equality_expression Match()
        {
            var obj = new equality_expression();
            {
                // start of node 
                if (!obj.Try(relational_expression.Match())) return null; // fail goto
                {
                    // start of node relational_expression
                    if (!obj.Try(_equality_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _equality_expression →
         | EQ_OP relational_expression _equality_expression 
         | NE_OP relational_expression _equality_expression 
         | E 

    */
    public class _equality_expression : Node
    {
        public static bool Check() => CheckToken(EQ_OP) || CheckToken(NE_OP) || true;

        public static _equality_expression Match()
        {
            var obj = new _equality_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(EQ_OP)))) goto production0_2; // normal goto & last go
                {
                    // start of node EQ_OP
                    if (!obj.Try(relational_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node relational_expression
                        if (!obj.Try(_equality_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(NE_OP)))) goto production0_3; // normal goto & last go
                {
                    // start of node NE_OP
                    if (!obj.Try(relational_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node relational_expression
                        if (!obj.Try(_equality_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    and_expression →
         | equality_expression _and_expression 

    */
    public class and_expression : Node
    {
        public static bool Check() => equality_expression.Check();

        public static and_expression Match()
        {
            var obj = new and_expression();
            {
                // start of node 
                if (!obj.Try(equality_expression.Match())) return null; // fail goto
                {
                    // start of node equality_expression
                    if (!obj.Try(_and_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _and_expression →
         | AMPERSAND equality_expression _and_expression 
         | E 

    */
    public class _and_expression : Node
    {
        public static bool Check() => CheckToken(AMPERSAND) || true;

        public static _and_expression Match()
        {
            var obj = new _and_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(AMPERSAND)))) goto production0_2; // normal goto & last go
                {
                    // start of node AMPERSAND
                    if (!obj.Try(equality_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node equality_expression
                        if (!obj.Try(_and_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    exclusive_or_expression →
         | and_expression _exclusive_or_expression 

    */
    public class exclusive_or_expression : Node
    {
        public static bool Check() => and_expression.Check();

        public static exclusive_or_expression Match()
        {
            var obj = new exclusive_or_expression();
            {
                // start of node 
                if (!obj.Try(and_expression.Match())) return null; // fail goto
                {
                    // start of node and_expression
                    if (!obj.Try(_exclusive_or_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _exclusive_or_expression →
         | CARET and_expression _exclusive_or_expression 
         | E 

    */
    public class _exclusive_or_expression : Node
    {
        public static bool Check() => CheckToken(CARET) || true;

        public static _exclusive_or_expression Match()
        {
            var obj = new _exclusive_or_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(CARET)))) goto production0_2; // normal goto & last go
                {
                    // start of node CARET
                    if (!obj.Try(and_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node and_expression
                        if (!obj.Try(_exclusive_or_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    inclusive_or_expression →
         | exclusive_or_expression _inclusive_or_expression 

    */
    public class inclusive_or_expression : Node
    {
        public static bool Check() => exclusive_or_expression.Check();

        public static inclusive_or_expression Match()
        {
            var obj = new inclusive_or_expression();
            {
                // start of node 
                if (!obj.Try(exclusive_or_expression.Match())) return null; // fail goto
                {
                    // start of node exclusive_or_expression
                    if (!obj.Try(_inclusive_or_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _inclusive_or_expression →
         | VERTICAL_BAR exclusive_or_expression _inclusive_or_expression 
         | E 

    */
    public class _inclusive_or_expression : Node
    {
        public static bool Check() => CheckToken(VERTICAL_BAR) || true;

        public static _inclusive_or_expression Match()
        {
            var obj = new _inclusive_or_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(VERTICAL_BAR)))) goto production0_2; // normal goto & last go
                {
                    // start of node VERTICAL_BAR
                    if (!obj.Try(exclusive_or_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node exclusive_or_expression
                        if (!obj.Try(_inclusive_or_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    logical_and_expression →
         | inclusive_or_expression _logical_and_expression 

    */
    public class logical_and_expression : Node
    {
        public static bool Check() => inclusive_or_expression.Check();

        public static logical_and_expression Match()
        {
            var obj = new logical_and_expression();
            {
                // start of node 
                if (!obj.Try(inclusive_or_expression.Match())) return null; // fail goto
                {
                    // start of node inclusive_or_expression
                    if (!obj.Try(_logical_and_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _logical_and_expression →
         | AND_OP inclusive_or_expression _logical_and_expression 
         | E 

    */
    public class _logical_and_expression : Node
    {
        public static bool Check() => CheckToken(AND_OP) || true;

        public static _logical_and_expression Match()
        {
            var obj = new _logical_and_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(AND_OP)))) goto production0_2; // normal goto & last go
                {
                    // start of node AND_OP
                    if (!obj.Try(inclusive_or_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node inclusive_or_expression
                        if (!obj.Try(_logical_and_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    logical_xor_expression →
         | logical_and_expression _logical_xor_expression 

    */
    public class logical_xor_expression : Node
    {
        public static bool Check() => logical_and_expression.Check();

        public static logical_xor_expression Match()
        {
            var obj = new logical_xor_expression();
            {
                // start of node 
                if (!obj.Try(logical_and_expression.Match())) return null; // fail goto
                {
                    // start of node logical_and_expression
                    if (!obj.Try(_logical_xor_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _logical_xor_expression →
         | XOR_OP logical_and_expression _logical_xor_expression 
         | E 

    */
    public class _logical_xor_expression : Node
    {
        public static bool Check() => CheckToken(XOR_OP) || true;

        public static _logical_xor_expression Match()
        {
            var obj = new _logical_xor_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(XOR_OP)))) goto production0_2; // normal goto & last go
                {
                    // start of node XOR_OP
                    if (!obj.Try(logical_and_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node logical_and_expression
                        if (!obj.Try(_logical_xor_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    logical_or_expression →
         | logical_xor_expression _logical_or_expression 

    */
    public class logical_or_expression : Node
    {
        public static bool Check() => logical_xor_expression.Check();

        public static logical_or_expression Match()
        {
            var obj = new logical_or_expression();
            {
                // start of node 
                if (!obj.Try(logical_xor_expression.Match())) return null; // fail goto
                {
                    // start of node logical_xor_expression
                    if (!obj.Try(_logical_or_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _logical_or_expression →
         | OR_OP logical_xor_expression _logical_or_expression 
         | E 

    */
    public class _logical_or_expression : Node
    {
        public static bool Check() => CheckToken(OR_OP) || true;

        public static _logical_or_expression Match()
        {
            var obj = new _logical_or_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(OR_OP)))) goto production0_2; // normal goto & last go
                {
                    // start of node OR_OP
                    if (!obj.Try(logical_xor_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node logical_xor_expression
                        if (!obj.Try(_logical_or_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    conditional_expression →
         | logical_or_expression 
         | logical_or_expression QUESTION expression COLON assignment_expression 

    */
    public class conditional_expression : Node
    {
        public static bool Check() => logical_or_expression.Check();

        public static conditional_expression Match()
        {
            var obj = new conditional_expression();
            {
                // start of node 
                if (!obj.Try(logical_or_expression.Match())) return null; // fail goto
                {
                    // start of node logical_or_expression
                    if (!obj.Try(new TokenNode(MatchToken(QUESTION)))) goto production1_END; // normal goto & last go
                    {
                        // start of node QUESTION
                        if (!obj.Try(expression.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node expression
                            if (!obj.Try(new TokenNode(MatchToken(COLON))))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node COLON
                                if (!obj.Try(assignment_expression.Match()))
                                    goto production4_END; // normal goto & last go
                                return obj;
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    assignment_expression →
         | conditional_expression 
         | unary_expression assignment_operator assignment_expression 

    */
    public class assignment_expression : Node
    {
        public static bool Check() => conditional_expression.Check() || unary_expression.Check();

        public static assignment_expression Match()
        {
            var obj = new assignment_expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(conditional_expression.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(unary_expression.Match())) return null; // fail goto
                {
                    // start of node unary_expression
                    if (!obj.Try(assignment_operator.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node assignment_operator
                        if (!obj.Try(assignment_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    assignment_operator →
         | EQUAL 
         | MUL_ASSIGN 
         | DIV_ASSIGN 
         | MOD_ASSIGN 
         | ADD_ASSIGN 
         | SUB_ASSIGN 
         | LEFT_ASSIGN 
         | RIGHT_ASSIGN 
         | AND_ASSIGN 
         | XOR_ASSIGN 
         | OR_ASSIGN 

    */
    public class assignment_operator : Node
    {
        public static bool Check() => CheckToken(EQUAL) || CheckToken(MUL_ASSIGN) || CheckToken(DIV_ASSIGN) ||
                                      CheckToken(MOD_ASSIGN) || CheckToken(ADD_ASSIGN) || CheckToken(SUB_ASSIGN) ||
                                      CheckToken(LEFT_ASSIGN) || CheckToken(RIGHT_ASSIGN) || CheckToken(AND_ASSIGN) ||
                                      CheckToken(XOR_ASSIGN) || CheckToken(OR_ASSIGN);

        public static assignment_operator Match()
        {
            var obj = new assignment_operator();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(EQUAL)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MUL_ASSIGN)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DIV_ASSIGN)))) goto production0_4; // normal goto & last go
                return obj;
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MOD_ASSIGN)))) goto production0_5; // normal goto & last go
                return obj;
                production0_5: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ADD_ASSIGN)))) goto production0_6; // normal goto & last go
                return obj;
                production0_6: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SUB_ASSIGN)))) goto production0_7; // normal goto & last go
                return obj;
                production0_7: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LEFT_ASSIGN)))) goto production0_8; // normal goto & last go
                return obj;
                production0_8: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(RIGHT_ASSIGN)))) goto production0_9; // normal goto & last go
                return obj;
                production0_9: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(AND_ASSIGN)))) goto production0_10; // normal goto & last go
                return obj;
                production0_10: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(XOR_ASSIGN)))) goto production0_11; // normal goto & last go
                return obj;
                production0_11: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(OR_ASSIGN)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    expression →
         | assignment_expression _expression 

    */
    public class expression : Node
    {
        public static bool Check() => assignment_expression.Check();

        public static expression Match()
        {
            var obj = new expression();
            {
                // start of node 
                if (!obj.Try(assignment_expression.Match())) return null; // fail goto
                {
                    // start of node assignment_expression
                    if (!obj.Try(_expression.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _expression →
         | COMMA assignment_expression _expression 
         | E 

    */
    public class _expression : Node
    {
        public static bool Check() => CheckToken(COMMA) || true;

        public static _expression Match()
        {
            var obj = new _expression();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(COMMA)))) goto production0_2; // normal goto & last go
                {
                    // start of node COMMA
                    if (!obj.Try(assignment_expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node assignment_expression
                        if (!obj.Try(_expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    constant_expression →
         | conditional_expression 

    */
    public class constant_expression : Node
    {
        public static bool Check() => conditional_expression.Check();

        public static constant_expression Match()
        {
            var obj = new constant_expression();
            {
                // start of node 
                if (!obj.Try(conditional_expression.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    declaration →
         | function_prototype SEMICOLON 
         | init_declarator_list SEMICOLON 
         | PRECISION precision_qualifier type_specifier_no_prec SEMICOLON 
         | type_qualifier IDENTIFIER LEFT_BRACE struct_declaration_list RIGHT_BRACE SEMICOLON 
         | type_qualifier IDENTIFIER LEFT_BRACE struct_declaration_list RIGHT_BRACE IDENTIFIER SEMICOLON 
         | type_qualifier IDENTIFIER LEFT_BRACE struct_declaration_list RIGHT_BRACE IDENTIFIER LEFT_BRACKET RIGHT_BRACKET SEMICOLON 
         | type_qualifier IDENTIFIER LEFT_BRACE struct_declaration_list RIGHT_BRACE IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET SEMICOLON 
         | type_qualifier SEMICOLON 

    */
    public class declaration : Node
    {
        public static bool Check() => function_prototype.Check() || init_declarator_list.Check() ||
                                      CheckToken(PRECISION) || type_qualifier.Check();

        public static declaration Match()
        {
            var obj = new declaration();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_prototype.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node function_prototype
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(init_declarator_list.Match())) goto production0_3; // normal goto & last go
                {
                    // start of node init_declarator_list
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(PRECISION)))) goto production0_4; // normal goto & last go
                {
                    // start of node PRECISION
                    if (!obj.Try(precision_qualifier.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node precision_qualifier
                        if (!obj.Try(type_specifier_no_prec.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node type_specifier_no_prec
                            if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_qualifier.Match())) return null; // fail goto
                {
                    // start of node type_qualifier
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_2; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACE))))
                            goto production2_END; // normal goto & last go
                        {
                            // start of node LEFT_BRACE
                            if (!obj.Try(struct_declaration_list.Match()))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node struct_declaration_list
                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node RIGHT_BRACE
                                    var p5 = pointer; //节点开始保存状态
                                    production5_0: //节点开始 标签 
                                    production5_1: // Sub(n)
                                    obj.Backtracking(p5);
                                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                        goto production5_2; // normal goto & last go
                                    return obj;
                                    production5_2: // Sub(n)
                                    obj.Backtracking(p5);
                                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER))))
                                        goto production5_END; // normal goto & last go
                                    {
                                        // start of node IDENTIFIER
                                        var p6 = pointer; //节点开始保存状态
                                        production6_0: //节点开始 标签 
                                        production6_1: // Sub(n)
                                        obj.Backtracking(p6);
                                        if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                            goto production6_2; // normal goto & last go
                                        return obj;
                                        production6_2: // Sub(n)
                                        obj.Backtracking(p6);
                                        if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET))))
                                            goto production6_END; // normal goto & last go
                                        {
                                            // start of node LEFT_BRACKET
                                            var p7 = pointer; //节点开始保存状态
                                            production7_0: //节点开始 标签 
                                            production7_1: // Sub(n)
                                            obj.Backtracking(p7);
                                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                                goto production7_2; // normal goto & last go
                                            {
                                                // start of node RIGHT_BRACKET
                                                if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                                    goto production8_END; // normal goto & last go
                                                return obj;
                                                production8_END: // Sub(1)
                                                ;
                                            } //depth: 8 
                                            production7_2: // Sub(n)
                                            obj.Backtracking(p7);
                                            if (!obj.Try(constant_expression.Match()))
                                                goto production7_END; // normal goto & last go
                                            {
                                                // start of node constant_expression
                                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                                    goto production8_END; // normal goto & last go
                                                {
                                                    // start of node RIGHT_BRACKET
                                                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                                        goto production9_END; // normal goto & last go
                                                    return obj;
                                                    production9_END: // Sub(1)
                                                    ;
                                                } //depth: 9 
                                                production8_END: // Sub(1)
                                                ;
                                            } //depth: 8 
                                            production7_END: // Sub(n)
                                            ;
                                        } // depth: 7 
                                        production6_END: // Sub(n)
                                        ;
                                    } // depth: 6 
                                    production5_END: // Sub(n)
                                    ;
                                } // depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    function_prototype →
         | function_declarator RIGHT_PAREN 

    */
    public class function_prototype : Node
    {
        public static bool Check() => function_declarator.Check();

        public static function_prototype Match()
        {
            var obj = new function_prototype();
            {
                // start of node 
                if (!obj.Try(function_declarator.Match())) return null; // fail goto
                {
                    // start of node function_declarator
                    if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    function_declarator →
         | function_header 
         | function_header_with_parameters 

    */
    public class function_declarator : Node
    {
        public static bool Check() => function_header.Check() || function_header_with_parameters.Check();

        public static function_declarator Match()
        {
            var obj = new function_declarator();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_header.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_header_with_parameters.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    function_header_with_parameters →
         | function_header parameter_declaration _function_header_with_parameters 

    */
    public class function_header_with_parameters : Node
    {
        public static bool Check() => function_header.Check();

        public static function_header_with_parameters Match()
        {
            var obj = new function_header_with_parameters();
            {
                // start of node 
                if (!obj.Try(function_header.Match())) return null; // fail goto
                {
                    // start of node function_header
                    if (!obj.Try(parameter_declaration.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node parameter_declaration
                        if (!obj.Try(_function_header_with_parameters.Match()))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _function_header_with_parameters →
         | COMMA parameter_declaration _function_header_with_parameters 
         | E 

    */
    public class _function_header_with_parameters : Node
    {
        public static bool Check() => CheckToken(COMMA) || true;

        public static _function_header_with_parameters Match()
        {
            var obj = new _function_header_with_parameters();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(COMMA)))) goto production0_2; // normal goto & last go
                {
                    // start of node COMMA
                    if (!obj.Try(parameter_declaration.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node parameter_declaration
                        if (!obj.Try(_function_header_with_parameters.Match()))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    function_header →
         | fully_specified_type IDENTIFIER LEFT_PAREN 

    */
    public class function_header : Node
    {
        public static bool Check() => fully_specified_type.Check();

        public static function_header Match()
        {
            var obj = new function_header();
            {
                // start of node 
                if (!obj.Try(fully_specified_type.Match())) return null; // fail goto
                {
                    // start of node fully_specified_type
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_END; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    parameter_declarator →
         | type_specifier IDENTIFIER 
         | type_specifier IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET 

    */
    public class parameter_declarator : Node
    {
        public static bool Check() => type_specifier.Check();

        public static parameter_declarator Match()
        {
            var obj = new parameter_declarator();
            {
                // start of node 
                if (!obj.Try(type_specifier.Match())) return null; // fail goto
                {
                    // start of node type_specifier
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_END; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET))))
                            goto production2_END; // normal goto & last go
                        {
                            // start of node LEFT_BRACKET
                            if (!obj.Try(constant_expression.Match())) goto production3_END; // normal goto & last go
                            {
                                // start of node constant_expression
                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                    goto production4_END; // normal goto & last go
                                return obj;
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    parameter_declaration →
         | parameter_type_qualifier parameter_qualifier parameter_declarator 
         | parameter_qualifier parameter_declarator 
         | parameter_type_qualifier parameter_qualifier parameter_type_specifier 
         | parameter_qualifier parameter_type_specifier 

    */
    public class parameter_declaration : Node
    {
        public static bool Check() => parameter_type_qualifier.Check() || parameter_qualifier.Check();

        public static parameter_declaration Match()
        {
            var obj = new parameter_declaration();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(parameter_type_qualifier.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node parameter_type_qualifier
                    if (!obj.Try(parameter_qualifier.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node parameter_qualifier
                        var p2 = pointer; //节点开始保存状态
                        production2_0: //节点开始 标签 
                        production2_1: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(parameter_declarator.Match())) goto production2_2; // normal goto & last go
                        return obj;
                        production2_2: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(parameter_type_specifier.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(n)
                        ;
                    } // depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(parameter_qualifier.Match())) return null; // fail goto
                {
                    // start of node parameter_qualifier
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(parameter_declarator.Match())) goto production1_2; // normal goto & last go
                    return obj;
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(parameter_type_specifier.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    parameter_qualifier →
         | IN 
         | OUT 
         | INOUT 

    */
    public class parameter_qualifier : Node
    {
        public static bool Check() => CheckToken(IN) || CheckToken(OUT) || CheckToken(INOUT);

        public static parameter_qualifier Match()
        {
            var obj = new parameter_qualifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IN)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(OUT)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(INOUT)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    parameter_type_specifier →
         | type_specifier 

    */
    public class parameter_type_specifier : Node
    {
        public static bool Check() => type_specifier.Check();

        public static parameter_type_specifier Match()
        {
            var obj = new parameter_type_specifier();
            {
                // start of node 
                if (!obj.Try(type_specifier.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    init_declarator_list →
         | single_declaration _init_declarator_list 

    */
    public class init_declarator_list : Node
    {
        public static bool Check() => single_declaration.Check();

        public static init_declarator_list Match()
        {
            var obj = new init_declarator_list();
            {
                // start of node 
                if (!obj.Try(single_declaration.Match())) return null; // fail goto
                {
                    // start of node single_declaration
                    if (!obj.Try(_init_declarator_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _init_declarator_list →
         | COMMA IDENTIFIER _init_declarator_list 
         | COMMA IDENTIFIER LEFT_BRACKET RIGHT_BRACKET _init_declarator_list 
         | COMMA IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET _init_declarator_list 
         | COMMA IDENTIFIER LEFT_BRACKET RIGHT_BRACKET EQUAL initializer _init_declarator_list 
         | COMMA IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET EQUAL initializer _init_declarator_list 
         | COMMA IDENTIFIER EQUAL initializer _init_declarator_list 
         | E 

    */
    public class _init_declarator_list : Node
    {
        public static bool Check() => CheckToken(COMMA) || true;

        public static _init_declarator_list Match()
        {
            var obj = new _init_declarator_list();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(COMMA)))) goto production0_2; // normal goto & last go
                {
                    // start of node COMMA
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_END; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        var p2 = pointer; //节点开始保存状态
                        production2_0: //节点开始 标签 
                        production2_1: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(_init_declarator_list.Match())) goto production2_2; // normal goto & last go
                        return obj;
                        production2_2: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET))))
                            goto production2_3; // normal goto & last go
                        {
                            // start of node LEFT_BRACKET
                            var p3 = pointer; //节点开始保存状态
                            production3_0: //节点开始 标签 
                            production3_1: // Sub(n)
                            obj.Backtracking(p3);
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                goto production3_2; // normal goto & last go
                            {
                                // start of node RIGHT_BRACKET
                                var p4 = pointer; //节点开始保存状态
                                production4_0: //节点开始 标签 
                                production4_1: // Sub(n)
                                obj.Backtracking(p4);
                                if (!obj.Try(_init_declarator_list.Match()))
                                    goto production4_2; // normal goto & last go
                                return obj;
                                production4_2: // Sub(n)
                                obj.Backtracking(p4);
                                if (!obj.Try(new TokenNode(MatchToken(EQUAL))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node EQUAL
                                    if (!obj.Try(initializer.Match())) goto production5_END; // normal goto & last go
                                    {
                                        // start of node initializer
                                        if (!obj.Try(_init_declarator_list.Match()))
                                            goto production6_END; // normal goto & last go
                                        return obj;
                                        production6_END: // Sub(1)
                                        ;
                                    } //depth: 6 
                                    production5_END: // Sub(1)
                                    ;
                                } //depth: 5 
                                production4_END: // Sub(n)
                                ;
                            } // depth: 4 
                            production3_2: // Sub(n)
                            obj.Backtracking(p3);
                            if (!obj.Try(constant_expression.Match())) goto production3_END; // normal goto & last go
                            {
                                // start of node constant_expression
                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node RIGHT_BRACKET
                                    var p5 = pointer; //节点开始保存状态
                                    production5_0: //节点开始 标签 
                                    production5_1: // Sub(n)
                                    obj.Backtracking(p5);
                                    if (!obj.Try(_init_declarator_list.Match()))
                                        goto production5_2; // normal goto & last go
                                    return obj;
                                    production5_2: // Sub(n)
                                    obj.Backtracking(p5);
                                    if (!obj.Try(new TokenNode(MatchToken(EQUAL))))
                                        goto production5_END; // normal goto & last go
                                    {
                                        // start of node EQUAL
                                        if (!obj.Try(initializer.Match()))
                                            goto production6_END; // normal goto & last go
                                        {
                                            // start of node initializer
                                            if (!obj.Try(_init_declarator_list.Match()))
                                                goto production7_END; // normal goto & last go
                                            return obj;
                                            production7_END: // Sub(1)
                                            ;
                                        } //depth: 7 
                                        production6_END: // Sub(1)
                                        ;
                                    } //depth: 6 
                                    production5_END: // Sub(n)
                                    ;
                                } // depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(n)
                            ;
                        } // depth: 3 
                        production2_3: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(new TokenNode(MatchToken(EQUAL)))) goto production2_END; // normal goto & last go
                        {
                            // start of node EQUAL
                            if (!obj.Try(initializer.Match())) goto production3_END; // normal goto & last go
                            {
                                // start of node initializer
                                if (!obj.Try(_init_declarator_list.Match()))
                                    goto production4_END; // normal goto & last go
                                return obj;
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(n)
                        ;
                    } // depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    single_declaration →
         | fully_specified_type 
         | fully_specified_type IDENTIFIER 
         | fully_specified_type IDENTIFIER LEFT_BRACKET RIGHT_BRACKET 
         | fully_specified_type IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET 
         | fully_specified_type IDENTIFIER LEFT_BRACKET RIGHT_BRACKET EQUAL initializer 
         | fully_specified_type IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET EQUAL initializer 
         | fully_specified_type IDENTIFIER EQUAL initializer 
         | IDENTIFIER 

    */
    public class single_declaration : Node
    {
        public static bool Check() => fully_specified_type.Check() || CheckToken(IDENTIFIER);

        public static single_declaration Match()
        {
            var obj = new single_declaration();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(fully_specified_type.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node fully_specified_type
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_END; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        var p2 = pointer; //节点开始保存状态
                        production2_0: //节点开始 标签 
                        production2_1: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET))))
                            goto production2_2; // normal goto & last go
                        {
                            // start of node LEFT_BRACKET
                            var p3 = pointer; //节点开始保存状态
                            production3_0: //节点开始 标签 
                            production3_1: // Sub(n)
                            obj.Backtracking(p3);
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                goto production3_2; // normal goto & last go
                            {
                                // start of node RIGHT_BRACKET
                                if (!obj.Try(new TokenNode(MatchToken(EQUAL))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node EQUAL
                                    if (!obj.Try(initializer.Match())) goto production5_END; // normal goto & last go
                                    return obj;
                                    production5_END: // Sub(1)
                                    ;
                                } //depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_2: // Sub(n)
                            obj.Backtracking(p3);
                            if (!obj.Try(constant_expression.Match())) goto production3_END; // normal goto & last go
                            {
                                // start of node constant_expression
                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node RIGHT_BRACKET
                                    if (!obj.Try(new TokenNode(MatchToken(EQUAL))))
                                        goto production5_END; // normal goto & last go
                                    {
                                        // start of node EQUAL
                                        if (!obj.Try(initializer.Match()))
                                            goto production6_END; // normal goto & last go
                                        return obj;
                                        production6_END: // Sub(1)
                                        ;
                                    } //depth: 6 
                                    production5_END: // Sub(1)
                                    ;
                                } //depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(n)
                            ;
                        } // depth: 3 
                        production2_2: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(new TokenNode(MatchToken(EQUAL)))) goto production2_END; // normal goto & last go
                        {
                            // start of node EQUAL
                            if (!obj.Try(initializer.Match())) goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(n)
                        ;
                    } // depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    fully_specified_type →
         | type_specifier 
         | type_qualifier type_specifier 

    */
    public class fully_specified_type : Node
    {
        public static bool Check() => type_specifier.Check() || type_qualifier.Check();

        public static fully_specified_type Match()
        {
            var obj = new fully_specified_type();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_specifier.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_qualifier.Match())) return null; // fail goto
                {
                    // start of node type_qualifier
                    if (!obj.Try(type_specifier.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    invariant_qualifier →
         | INVARIANT 

    */
    public class invariant_qualifier : Node
    {
        public static bool Check() => CheckToken(INVARIANT);

        public static invariant_qualifier Match()
        {
            var obj = new invariant_qualifier();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(INVARIANT)))) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    interpolation_qualifier →
         | SMOOTH 
         | FLAT 
         | NOPERSPECTIVE 

    */
    public class interpolation_qualifier : Node
    {
        public static bool Check() => CheckToken(SMOOTH) || CheckToken(FLAT) || CheckToken(NOPERSPECTIVE);

        public static interpolation_qualifier Match()
        {
            var obj = new interpolation_qualifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SMOOTH)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(FLAT)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(NOPERSPECTIVE)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    layout_qualifier →
         | LAYOUT LEFT_PAREN layout_qualifier_id_list RIGHT_PAREN 

    */
    public class layout_qualifier : Node
    {
        public static bool Check() => CheckToken(LAYOUT);

        public static layout_qualifier Match()
        {
            var obj = new layout_qualifier();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(LAYOUT)))) return null; // fail goto
                {
                    // start of node LAYOUT
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_PAREN
                        if (!obj.Try(layout_qualifier_id_list.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node layout_qualifier_id_list
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                                goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    layout_qualifier_id_list →
         | layout_qualifier_id _layout_qualifier_id_list 

    */
    public class layout_qualifier_id_list : Node
    {
        public static bool Check() => layout_qualifier_id.Check();

        public static layout_qualifier_id_list Match()
        {
            var obj = new layout_qualifier_id_list();
            {
                // start of node 
                if (!obj.Try(layout_qualifier_id.Match())) return null; // fail goto
                {
                    // start of node layout_qualifier_id
                    if (!obj.Try(_layout_qualifier_id_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _layout_qualifier_id_list →
         | COMMA layout_qualifier_id _layout_qualifier_id_list 
         | E 

    */
    public class _layout_qualifier_id_list : Node
    {
        public static bool Check() => CheckToken(COMMA) || true;

        public static _layout_qualifier_id_list Match()
        {
            var obj = new _layout_qualifier_id_list();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(COMMA)))) goto production0_2; // normal goto & last go
                {
                    // start of node COMMA
                    if (!obj.Try(layout_qualifier_id.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node layout_qualifier_id
                        if (!obj.Try(_layout_qualifier_id_list.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    layout_qualifier_id →
         | IDENTIFIER 
         | IDENTIFIER EQUAL INTCONSTANT 

    */
    public class layout_qualifier_id : Node
    {
        public static bool Check() => CheckToken(IDENTIFIER);

        public static layout_qualifier_id Match()
        {
            var obj = new layout_qualifier_id();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) return null; // fail goto
                {
                    // start of node IDENTIFIER
                    if (!obj.Try(new TokenNode(MatchToken(EQUAL)))) goto production1_END; // normal goto & last go
                    {
                        // start of node EQUAL
                        if (!obj.Try(new TokenNode(MatchToken(INTCONSTANT))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    parameter_type_qualifier →
         | CONST 

    */
    public class parameter_type_qualifier : Node
    {
        public static bool Check() => CheckToken(CONST);

        public static parameter_type_qualifier Match()
        {
            var obj = new parameter_type_qualifier();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(CONST)))) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    type_qualifier →
         | storage_qualifier 
         | layout_qualifier 
         | layout_qualifier storage_qualifier 
         | interpolation_qualifier storage_qualifier 
         | interpolation_qualifier 
         | invariant_qualifier storage_qualifier 
         | invariant_qualifier interpolation_qualifier storage_qualifier 
         | invariant_qualifier 

    */
    public class type_qualifier : Node
    {
        public static bool Check() => storage_qualifier.Check() || layout_qualifier.Check() ||
                                      interpolation_qualifier.Check() || invariant_qualifier.Check();

        public static type_qualifier Match()
        {
            var obj = new type_qualifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(storage_qualifier.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(layout_qualifier.Match())) goto production0_3; // normal goto & last go
                {
                    // start of node layout_qualifier
                    if (!obj.Try(storage_qualifier.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(interpolation_qualifier.Match())) goto production0_4; // normal goto & last go
                {
                    // start of node interpolation_qualifier
                    if (!obj.Try(storage_qualifier.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(invariant_qualifier.Match())) return null; // fail goto
                {
                    // start of node invariant_qualifier
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(storage_qualifier.Match())) goto production1_2; // normal goto & last go
                    return obj;
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(interpolation_qualifier.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node interpolation_qualifier
                        if (!obj.Try(storage_qualifier.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    storage_qualifier →
         | CONST 
         | ATTRIBUTE 
         | VARYING 
         | CENTROID VARYING 
         | IN 
         | OUT 
         | CENTROID IN 
         | CENTROID OUT 
         | UNIFORM 

    */
    public class storage_qualifier : Node
    {
        public static bool Check() => CheckToken(CONST) || CheckToken(ATTRIBUTE) || CheckToken(VARYING) ||
                                      CheckToken(CENTROID) || CheckToken(IN) || CheckToken(OUT) || CheckToken(UNIFORM);

        public static storage_qualifier Match()
        {
            var obj = new storage_qualifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(CONST)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ATTRIBUTE)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(VARYING)))) goto production0_4; // normal goto & last go
                return obj;
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(CENTROID)))) goto production0_5; // normal goto & last go
                {
                    // start of node CENTROID
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(VARYING)))) goto production1_2; // normal goto & last go
                    return obj;
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(IN)))) goto production1_3; // normal goto & last go
                    return obj;
                    production1_3: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(OUT)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_5: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IN)))) goto production0_6; // normal goto & last go
                return obj;
                production0_6: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(OUT)))) goto production0_7; // normal goto & last go
                return obj;
                production0_7: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(UNIFORM)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    type_specifier →
         | type_specifier_no_prec 
         | precision_qualifier type_specifier_no_prec 

    */
    public class type_specifier : Node
    {
        public static bool Check() => type_specifier_no_prec.Check() || precision_qualifier.Check();

        public static type_specifier Match()
        {
            var obj = new type_specifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_specifier_no_prec.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(precision_qualifier.Match())) return null; // fail goto
                {
                    // start of node precision_qualifier
                    if (!obj.Try(type_specifier_no_prec.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    type_specifier_no_prec →
         | type_specifier_nonarray 
         | type_specifier_nonarray LEFT_BRACKET RIGHT_BRACKET 
         | type_specifier_nonarray LEFT_BRACKET constant_expression RIGHT_BRACKET 

    */
    public class type_specifier_no_prec : Node
    {
        public static bool Check() => type_specifier_nonarray.Check();

        public static type_specifier_no_prec Match()
        {
            var obj = new type_specifier_no_prec();
            {
                // start of node 
                if (!obj.Try(type_specifier_nonarray.Match())) return null; // fail goto
                {
                    // start of node type_specifier_nonarray
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET))))
                        goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_BRACKET
                        var p2 = pointer; //节点开始保存状态
                        production2_0: //节点开始 标签 
                        production2_1: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                            goto production2_2; // normal goto & last go
                        return obj;
                        production2_2: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(constant_expression.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node constant_expression
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(n)
                        ;
                    } // depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    type_specifier_nonarray →
         | VOID 
         | FLOAT 
         | INT 
         | UINT 
         | BOOL 
         | VEC2 
         | VEC3 
         | VEC4 
         | BVEC2 
         | BVEC3 
         | BVEC4 
         | IVEC2 
         | IVEC3 
         | IVEC4 
         | UVEC2 
         | UVEC3 
         | UVEC4 
         | MAT2 
         | MAT3 
         | MAT4 
         | MAT2X2 
         | MAT2X3 
         | MAT2X4 
         | MAT3X2 
         | MAT3X3 
         | MAT3X4 
         | MAT4X2 
         | MAT4X3 
         | MAT4X4 
         | SAMPLER1D 
         | SAMPLER2D 
         | SAMPLER3D 
         | SAMPLERCUBE 
         | SAMPLER1DSHADOW 
         | SAMPLER2DSHADOW 
         | SAMPLERCUBESHADOW 
         | SAMPLER1DARRAY 
         | SAMPLER2DARRAY 
         | SAMPLER1DARRAYSHADOW 
         | SAMPLER2DARRAYSHADOW 
         | ISAMPLER1D 
         | ISAMPLER2D 
         | ISAMPLER3D 
         | ISAMPLERCUBE 
         | ISAMPLER1DARRAY 
         | ISAMPLER2DARRAY 
         | USAMPLER1D 
         | USAMPLER2D 
         | USAMPLER3D 
         | USAMPLERCUBE 
         | USAMPLER1DARRAY 
         | USAMPLER2DARRAY 
         | SAMPLER2DRECT 
         | SAMPLER2DRECTSHADOW 
         | ISAMPLER2DRECT 
         | USAMPLER2DRECT 
         | SAMPLERBUFFER 
         | ISAMPLERBUFFER 
         | USAMPLERBUFFER 
         | SAMPLER2DMS 
         | ISAMPLER2DMS 
         | USAMPLER2DMS 
         | SAMPLER2DMSARRAY 
         | ISAMPLER2DMSARRAY 
         | USAMPLER2DMSARRAY 
         | struct_specifier 
         | TYPE_NAME 

    */
    public class type_specifier_nonarray : Node
    {
        public static bool Check() => CheckToken(VOID) || CheckToken(FLOAT) || CheckToken(INT) || CheckToken(UINT) ||
                                      CheckToken(BOOL) || CheckToken(VEC2) || CheckToken(VEC3) || CheckToken(VEC4) ||
                                      CheckToken(BVEC2) || CheckToken(BVEC3) || CheckToken(BVEC4) ||
                                      CheckToken(IVEC2) || CheckToken(IVEC3) || CheckToken(IVEC4) ||
                                      CheckToken(UVEC2) || CheckToken(UVEC3) || CheckToken(UVEC4) || CheckToken(MAT2) ||
                                      CheckToken(MAT3) || CheckToken(MAT4) || CheckToken(MAT2X2) ||
                                      CheckToken(MAT2X3) || CheckToken(MAT2X4) || CheckToken(MAT3X2) ||
                                      CheckToken(MAT3X3) || CheckToken(MAT3X4) || CheckToken(MAT4X2) ||
                                      CheckToken(MAT4X3) || CheckToken(MAT4X4) || CheckToken(SAMPLER1D) ||
                                      CheckToken(SAMPLER2D) || CheckToken(SAMPLER3D) || CheckToken(SAMPLERCUBE) ||
                                      CheckToken(SAMPLER1DSHADOW) || CheckToken(SAMPLER2DSHADOW) ||
                                      CheckToken(SAMPLERCUBESHADOW) || CheckToken(SAMPLER1DARRAY) ||
                                      CheckToken(SAMPLER2DARRAY) || CheckToken(SAMPLER1DARRAYSHADOW) ||
                                      CheckToken(SAMPLER2DARRAYSHADOW) || CheckToken(ISAMPLER1D) ||
                                      CheckToken(ISAMPLER2D) || CheckToken(ISAMPLER3D) || CheckToken(ISAMPLERCUBE) ||
                                      CheckToken(ISAMPLER1DARRAY) || CheckToken(ISAMPLER2DARRAY) ||
                                      CheckToken(USAMPLER1D) || CheckToken(USAMPLER2D) || CheckToken(USAMPLER3D) ||
                                      CheckToken(USAMPLERCUBE) || CheckToken(USAMPLER1DARRAY) ||
                                      CheckToken(USAMPLER2DARRAY) || CheckToken(SAMPLER2DRECT) ||
                                      CheckToken(SAMPLER2DRECTSHADOW) || CheckToken(ISAMPLER2DRECT) ||
                                      CheckToken(USAMPLER2DRECT) || CheckToken(SAMPLERBUFFER) ||
                                      CheckToken(ISAMPLERBUFFER) || CheckToken(USAMPLERBUFFER) ||
                                      CheckToken(SAMPLER2DMS) || CheckToken(ISAMPLER2DMS) || CheckToken(USAMPLER2DMS) ||
                                      CheckToken(SAMPLER2DMSARRAY) || CheckToken(ISAMPLER2DMSARRAY) ||
                                      CheckToken(USAMPLER2DMSARRAY) || struct_specifier.Check() ||
                                      CheckToken(TYPE_NAME);

        public static type_specifier_nonarray Match()
        {
            var obj = new type_specifier_nonarray();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(VOID)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(FLOAT)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(INT)))) goto production0_4; // normal goto & last go
                return obj;
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(UINT)))) goto production0_5; // normal goto & last go
                return obj;
                production0_5: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BOOL)))) goto production0_6; // normal goto & last go
                return obj;
                production0_6: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(VEC2)))) goto production0_7; // normal goto & last go
                return obj;
                production0_7: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(VEC3)))) goto production0_8; // normal goto & last go
                return obj;
                production0_8: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(VEC4)))) goto production0_9; // normal goto & last go
                return obj;
                production0_9: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BVEC2)))) goto production0_10; // normal goto & last go
                return obj;
                production0_10: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BVEC3)))) goto production0_11; // normal goto & last go
                return obj;
                production0_11: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BVEC4)))) goto production0_12; // normal goto & last go
                return obj;
                production0_12: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IVEC2)))) goto production0_13; // normal goto & last go
                return obj;
                production0_13: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IVEC3)))) goto production0_14; // normal goto & last go
                return obj;
                production0_14: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(IVEC4)))) goto production0_15; // normal goto & last go
                return obj;
                production0_15: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(UVEC2)))) goto production0_16; // normal goto & last go
                return obj;
                production0_16: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(UVEC3)))) goto production0_17; // normal goto & last go
                return obj;
                production0_17: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(UVEC4)))) goto production0_18; // normal goto & last go
                return obj;
                production0_18: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT2)))) goto production0_19; // normal goto & last go
                return obj;
                production0_19: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT3)))) goto production0_20; // normal goto & last go
                return obj;
                production0_20: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT4)))) goto production0_21; // normal goto & last go
                return obj;
                production0_21: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT2X2)))) goto production0_22; // normal goto & last go
                return obj;
                production0_22: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT2X3)))) goto production0_23; // normal goto & last go
                return obj;
                production0_23: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT2X4)))) goto production0_24; // normal goto & last go
                return obj;
                production0_24: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT3X2)))) goto production0_25; // normal goto & last go
                return obj;
                production0_25: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT3X3)))) goto production0_26; // normal goto & last go
                return obj;
                production0_26: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT3X4)))) goto production0_27; // normal goto & last go
                return obj;
                production0_27: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT4X2)))) goto production0_28; // normal goto & last go
                return obj;
                production0_28: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT4X3)))) goto production0_29; // normal goto & last go
                return obj;
                production0_29: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MAT4X4)))) goto production0_30; // normal goto & last go
                return obj;
                production0_30: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER1D)))) goto production0_31; // normal goto & last go
                return obj;
                production0_31: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2D)))) goto production0_32; // normal goto & last go
                return obj;
                production0_32: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER3D)))) goto production0_33; // normal goto & last go
                return obj;
                production0_33: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLERCUBE)))) goto production0_34; // normal goto & last go
                return obj;
                production0_34: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER1DSHADOW)))) goto production0_35; // normal goto & last go
                return obj;
                production0_35: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DSHADOW)))) goto production0_36; // normal goto & last go
                return obj;
                production0_36: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLERCUBESHADOW))))
                    goto production0_37; // normal goto & last go
                return obj;
                production0_37: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER1DARRAY)))) goto production0_38; // normal goto & last go
                return obj;
                production0_38: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DARRAY)))) goto production0_39; // normal goto & last go
                return obj;
                production0_39: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER1DARRAYSHADOW))))
                    goto production0_40; // normal goto & last go
                return obj;
                production0_40: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DARRAYSHADOW))))
                    goto production0_41; // normal goto & last go
                return obj;
                production0_41: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER1D)))) goto production0_42; // normal goto & last go
                return obj;
                production0_42: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER2D)))) goto production0_43; // normal goto & last go
                return obj;
                production0_43: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER3D)))) goto production0_44; // normal goto & last go
                return obj;
                production0_44: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLERCUBE)))) goto production0_45; // normal goto & last go
                return obj;
                production0_45: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER1DARRAY)))) goto production0_46; // normal goto & last go
                return obj;
                production0_46: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER2DARRAY)))) goto production0_47; // normal goto & last go
                return obj;
                production0_47: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER1D)))) goto production0_48; // normal goto & last go
                return obj;
                production0_48: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER2D)))) goto production0_49; // normal goto & last go
                return obj;
                production0_49: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER3D)))) goto production0_50; // normal goto & last go
                return obj;
                production0_50: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLERCUBE)))) goto production0_51; // normal goto & last go
                return obj;
                production0_51: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER1DARRAY)))) goto production0_52; // normal goto & last go
                return obj;
                production0_52: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER2DARRAY)))) goto production0_53; // normal goto & last go
                return obj;
                production0_53: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DRECT)))) goto production0_54; // normal goto & last go
                return obj;
                production0_54: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DRECTSHADOW))))
                    goto production0_55; // normal goto & last go
                return obj;
                production0_55: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER2DRECT)))) goto production0_56; // normal goto & last go
                return obj;
                production0_56: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER2DRECT)))) goto production0_57; // normal goto & last go
                return obj;
                production0_57: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLERBUFFER)))) goto production0_58; // normal goto & last go
                return obj;
                production0_58: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLERBUFFER)))) goto production0_59; // normal goto & last go
                return obj;
                production0_59: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLERBUFFER)))) goto production0_60; // normal goto & last go
                return obj;
                production0_60: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DMS)))) goto production0_61; // normal goto & last go
                return obj;
                production0_61: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER2DMS)))) goto production0_62; // normal goto & last go
                return obj;
                production0_62: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER2DMS)))) goto production0_63; // normal goto & last go
                return obj;
                production0_63: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SAMPLER2DMSARRAY)))) goto production0_64; // normal goto & last go
                return obj;
                production0_64: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(ISAMPLER2DMSARRAY))))
                    goto production0_65; // normal goto & last go
                return obj;
                production0_65: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(USAMPLER2DMSARRAY))))
                    goto production0_66; // normal goto & last go
                return obj;
                production0_66: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(struct_specifier.Match())) goto production0_67; // normal goto & last go
                return obj;
                production0_67: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(TYPE_NAME)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    precision_qualifier →
         | HIGH_PRECISION 
         | MEDIUM_PRECISION 
         | LOW_PRECISION 

    */
    public class precision_qualifier : Node
    {
        public static bool Check() =>
            CheckToken(HIGH_PRECISION) || CheckToken(MEDIUM_PRECISION) || CheckToken(LOW_PRECISION);

        public static precision_qualifier Match()
        {
            var obj = new precision_qualifier();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(HIGH_PRECISION)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(MEDIUM_PRECISION)))) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(LOW_PRECISION)))) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    struct_specifier →
         | STRUCT IDENTIFIER LEFT_BRACE struct_declaration_list RIGHT_BRACE 
         | STRUCT LEFT_BRACE struct_declaration_list RIGHT_BRACE 

    */
    public class struct_specifier : Node
    {
        public static bool Check() => CheckToken(STRUCT);

        public static struct_specifier Match()
        {
            var obj = new struct_specifier();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(STRUCT)))) return null; // fail goto
                {
                    // start of node STRUCT
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_2; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACE))))
                            goto production2_END; // normal goto & last go
                        {
                            // start of node LEFT_BRACE
                            if (!obj.Try(struct_declaration_list.Match()))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node struct_declaration_list
                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE))))
                                    goto production4_END; // normal goto & last go
                                return obj;
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACE)))) goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_BRACE
                        if (!obj.Try(struct_declaration_list.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node struct_declaration_list
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE))))
                                goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    struct_declaration_list →
         | struct_declaration _struct_declaration_list 

    */
    public class struct_declaration_list : Node
    {
        public static bool Check() => struct_declaration.Check();

        public static struct_declaration_list Match()
        {
            var obj = new struct_declaration_list();
            {
                // start of node 
                if (!obj.Try(struct_declaration.Match())) return null; // fail goto
                {
                    // start of node struct_declaration
                    if (!obj.Try(_struct_declaration_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _struct_declaration_list →
         | struct_declaration _struct_declaration_list 
         | E 

    */
    public class _struct_declaration_list : Node
    {
        public static bool Check() => struct_declaration.Check() || true;

        public static _struct_declaration_list Match()
        {
            var obj = new _struct_declaration_list();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(struct_declaration.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node struct_declaration
                    if (!obj.Try(_struct_declaration_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    struct_declaration →
         | type_specifier struct_declarator_list SEMICOLON 
         | type_qualifier type_specifier struct_declarator_list SEMICOLON 

    */
    public class struct_declaration : Node
    {
        public static bool Check() => type_specifier.Check() || type_qualifier.Check();

        public static struct_declaration Match()
        {
            var obj = new struct_declaration();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_specifier.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node type_specifier
                    if (!obj.Try(struct_declarator_list.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node struct_declarator_list
                        if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(type_qualifier.Match())) return null; // fail goto
                {
                    // start of node type_qualifier
                    if (!obj.Try(type_specifier.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node type_specifier
                        if (!obj.Try(struct_declarator_list.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node struct_declarator_list
                            if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    struct_declarator_list →
         | struct_declarator _struct_declarator_list 

    */
    public class struct_declarator_list : Node
    {
        public static bool Check() => struct_declarator.Check();

        public static struct_declarator_list Match()
        {
            var obj = new struct_declarator_list();
            {
                // start of node 
                if (!obj.Try(struct_declarator.Match())) return null; // fail goto
                {
                    // start of node struct_declarator
                    if (!obj.Try(_struct_declarator_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _struct_declarator_list →
         | COMMA struct_declarator _struct_declarator_list 
         | E 

    */
    public class _struct_declarator_list : Node
    {
        public static bool Check() => CheckToken(COMMA) || true;

        public static _struct_declarator_list Match()
        {
            var obj = new _struct_declarator_list();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(COMMA)))) goto production0_2; // normal goto & last go
                {
                    // start of node COMMA
                    if (!obj.Try(struct_declarator.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node struct_declarator
                        if (!obj.Try(_struct_declarator_list.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    struct_declarator →
         | IDENTIFIER 
         | IDENTIFIER LEFT_BRACKET RIGHT_BRACKET 
         | IDENTIFIER LEFT_BRACKET constant_expression RIGHT_BRACKET 

    */
    public class struct_declarator : Node
    {
        public static bool Check() => CheckToken(IDENTIFIER);

        public static struct_declarator Match()
        {
            var obj = new struct_declarator();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) return null; // fail goto
                {
                    // start of node IDENTIFIER
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACKET))))
                        goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_BRACKET
                        var p2 = pointer; //节点开始保存状态
                        production2_0: //节点开始 标签 
                        production2_1: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                            goto production2_2; // normal goto & last go
                        return obj;
                        production2_2: // Sub(n)
                        obj.Backtracking(p2);
                        if (!obj.Try(constant_expression.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node constant_expression
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACKET))))
                                goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(n)
                        ;
                    } // depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    initializer →
         | assignment_expression 

    */
    public class initializer : Node
    {
        public static bool Check() => assignment_expression.Check();

        public static initializer Match()
        {
            var obj = new initializer();
            {
                // start of node 
                if (!obj.Try(assignment_expression.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    declaration_statement →
         | declaration 

    */
    public class declaration_statement : Node
    {
        public static bool Check() => declaration.Check();

        public static declaration_statement Match()
        {
            var obj = new declaration_statement();
            {
                // start of node 
                if (!obj.Try(declaration.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    statement →
         | compound_statement 
         | simple_statement 

    */
    public class statement : Node
    {
        public static bool Check() => compound_statement.Check() || simple_statement.Check();

        public static statement Match()
        {
            var obj = new statement();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(compound_statement.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(simple_statement.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    simple_statement →
         | declaration_statement 
         | expression_statement 
         | selection_statement 
         | switch_statement 
         | case_label 
         | iteration_statement 
         | jump_statement 

    */
    public class simple_statement : Node
    {
        public static bool Check() => declaration_statement.Check() || expression_statement.Check() ||
                                      selection_statement.Check() || switch_statement.Check() || case_label.Check() ||
                                      iteration_statement.Check() || jump_statement.Check();

        public static simple_statement Match()
        {
            var obj = new simple_statement();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(declaration_statement.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(expression_statement.Match())) goto production0_3; // normal goto & last go
                return obj;
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(selection_statement.Match())) goto production0_4; // normal goto & last go
                return obj;
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(switch_statement.Match())) goto production0_5; // normal goto & last go
                return obj;
                production0_5: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(case_label.Match())) goto production0_6; // normal goto & last go
                return obj;
                production0_6: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(iteration_statement.Match())) goto production0_7; // normal goto & last go
                return obj;
                production0_7: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(jump_statement.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    compound_statement →
         | LEFT_BRACE RIGHT_BRACE 
         | LEFT_BRACE statement_list RIGHT_BRACE 

    */
    public class compound_statement : Node
    {
        public static bool Check() => CheckToken(LEFT_BRACE);

        public static compound_statement Match()
        {
            var obj = new compound_statement();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACE)))) return null; // fail goto
                {
                    // start of node LEFT_BRACE
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE)))) goto production1_2; // normal goto & last go
                    return obj;
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(statement_list.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node statement_list
                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    statement_no_new_scope →
         | compound_statement_no_new_scope 
         | simple_statement 

    */
    public class statement_no_new_scope : Node
    {
        public static bool Check() => compound_statement_no_new_scope.Check() || simple_statement.Check();

        public static statement_no_new_scope Match()
        {
            var obj = new statement_no_new_scope();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(compound_statement_no_new_scope.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(simple_statement.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    compound_statement_no_new_scope →
         | LEFT_BRACE RIGHT_BRACE 
         | LEFT_BRACE statement_list RIGHT_BRACE 

    */
    public class compound_statement_no_new_scope : Node
    {
        public static bool Check() => CheckToken(LEFT_BRACE);

        public static compound_statement_no_new_scope Match()
        {
            var obj = new compound_statement_no_new_scope();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACE)))) return null; // fail goto
                {
                    // start of node LEFT_BRACE
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE)))) goto production1_2; // normal goto & last go
                    return obj;
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(statement_list.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node statement_list
                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    statement_list →
         | statement _statement_list 

    */
    public class statement_list : Node
    {
        public static bool Check() => statement.Check();

        public static statement_list Match()
        {
            var obj = new statement_list();
            {
                // start of node 
                if (!obj.Try(statement.Match())) return null; // fail goto
                {
                    // start of node statement
                    if (!obj.Try(_statement_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _statement_list →
         | statement _statement_list 
         | E 

    */
    public class _statement_list : Node
    {
        public static bool Check() => statement.Check() || true;

        public static _statement_list Match()
        {
            var obj = new _statement_list();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(statement.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node statement
                    if (!obj.Try(_statement_list.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    expression_statement →
         | SEMICOLON 
         | expression SEMICOLON 

    */
    public class expression_statement : Node
    {
        public static bool Check() => CheckToken(SEMICOLON) || expression.Check();

        public static expression_statement Match()
        {
            var obj = new expression_statement();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(expression.Match())) return null; // fail goto
                {
                    // start of node expression
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    selection_statement →
         | IF LEFT_PAREN expression RIGHT_PAREN selection_rest_statement 

    */
    public class selection_statement : Node
    {
        public static bool Check() => CheckToken(IF);

        public static selection_statement Match()
        {
            var obj = new selection_statement();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(IF)))) return null; // fail goto
                {
                    // start of node IF
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_PAREN
                        if (!obj.Try(expression.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node expression
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node RIGHT_PAREN
                                if (!obj.Try(selection_rest_statement.Match()))
                                    goto production4_END; // normal goto & last go
                                return obj;
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    selection_rest_statement →
         | statement ELSE statement 
         | statement 

    */
    public class selection_rest_statement : Node
    {
        public static bool Check() => statement.Check();

        public static selection_rest_statement Match()
        {
            var obj = new selection_rest_statement();
            {
                // start of node 
                if (!obj.Try(statement.Match())) return null; // fail goto
                {
                    // start of node statement
                    if (!obj.Try(new TokenNode(MatchToken(ELSE)))) goto production1_END; // normal goto & last go
                    {
                        // start of node ELSE
                        if (!obj.Try(statement.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    condition →
         | expression 
         | fully_specified_type IDENTIFIER EQUAL initializer 

    */
    public class condition : Node
    {
        public static bool Check() => expression.Check() || fully_specified_type.Check();

        public static condition Match()
        {
            var obj = new condition();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(expression.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(fully_specified_type.Match())) return null; // fail goto
                {
                    // start of node fully_specified_type
                    if (!obj.Try(new TokenNode(MatchToken(IDENTIFIER)))) goto production1_END; // normal goto & last go
                    {
                        // start of node IDENTIFIER
                        if (!obj.Try(new TokenNode(MatchToken(EQUAL)))) goto production2_END; // normal goto & last go
                        {
                            // start of node EQUAL
                            if (!obj.Try(initializer.Match())) goto production3_END; // normal goto & last go
                            return obj;
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    switch_statement →
         | SWITCH LEFT_PAREN expression RIGHT_PAREN LEFT_BRACE switch_statement_list RIGHT_BRACE 

    */
    public class switch_statement : Node
    {
        public static bool Check() => CheckToken(SWITCH);

        public static switch_statement Match()
        {
            var obj = new switch_statement();
            {
                // start of node 
                if (!obj.Try(new TokenNode(MatchToken(SWITCH)))) return null; // fail goto
                {
                    // start of node SWITCH
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_PAREN
                        if (!obj.Try(expression.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node expression
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node RIGHT_PAREN
                                if (!obj.Try(new TokenNode(MatchToken(LEFT_BRACE))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node LEFT_BRACE
                                    if (!obj.Try(switch_statement_list.Match()))
                                        goto production5_END; // normal goto & last go
                                    {
                                        // start of node switch_statement_list
                                        if (!obj.Try(new TokenNode(MatchToken(RIGHT_BRACE))))
                                            goto production6_END; // normal goto & last go
                                        return obj;
                                        production6_END: // Sub(1)
                                        ;
                                    } //depth: 6 
                                    production5_END: // Sub(1)
                                    ;
                                } //depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    switch_statement_list →
         | statement_list 

    */
    public class switch_statement_list : Node
    {
        public static bool Check() => statement_list.Check();

        public static switch_statement_list Match()
        {
            var obj = new switch_statement_list();
            {
                // start of node 
                if (!obj.Try(statement_list.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    case_label →
         | CASE expression COLON 
         | DEFAULT COLON 

    */
    public class case_label : Node
    {
        public static bool Check() => CheckToken(CASE) || CheckToken(DEFAULT);

        public static case_label Match()
        {
            var obj = new case_label();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(CASE)))) goto production0_2; // normal goto & last go
                {
                    // start of node CASE
                    if (!obj.Try(expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node expression
                        if (!obj.Try(new TokenNode(MatchToken(COLON)))) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DEFAULT)))) return null; // fail goto
                {
                    // start of node DEFAULT
                    if (!obj.Try(new TokenNode(MatchToken(COLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    iteration_statement →
         | WHILE LEFT_PAREN condition RIGHT_PAREN statement_no_new_scope 
         | DO statement WHILE LEFT_PAREN expression RIGHT_PAREN SEMICOLON 
         | FOR LEFT_PAREN for_init_statement for_rest_statement RIGHT_PAREN statement_no_new_scope 

    */
    public class iteration_statement : Node
    {
        public static bool Check() => CheckToken(WHILE) || CheckToken(DO) || CheckToken(FOR);

        public static iteration_statement Match()
        {
            var obj = new iteration_statement();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(WHILE)))) goto production0_2; // normal goto & last go
                {
                    // start of node WHILE
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_PAREN
                        if (!obj.Try(condition.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node condition
                            if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node RIGHT_PAREN
                                if (!obj.Try(statement_no_new_scope.Match()))
                                    goto production4_END; // normal goto & last go
                                return obj;
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DO)))) goto production0_3; // normal goto & last go
                {
                    // start of node DO
                    if (!obj.Try(statement.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node statement
                        if (!obj.Try(new TokenNode(MatchToken(WHILE)))) goto production2_END; // normal goto & last go
                        {
                            // start of node WHILE
                            if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN))))
                                goto production3_END; // normal goto & last go
                            {
                                // start of node LEFT_PAREN
                                if (!obj.Try(expression.Match())) goto production4_END; // normal goto & last go
                                {
                                    // start of node expression
                                    if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                                        goto production5_END; // normal goto & last go
                                    {
                                        // start of node RIGHT_PAREN
                                        if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                                            goto production6_END; // normal goto & last go
                                        return obj;
                                        production6_END: // Sub(1)
                                        ;
                                    } //depth: 6 
                                    production5_END: // Sub(1)
                                    ;
                                } //depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(FOR)))) return null; // fail goto
                {
                    // start of node FOR
                    if (!obj.Try(new TokenNode(MatchToken(LEFT_PAREN)))) goto production1_END; // normal goto & last go
                    {
                        // start of node LEFT_PAREN
                        if (!obj.Try(for_init_statement.Match())) goto production2_END; // normal goto & last go
                        {
                            // start of node for_init_statement
                            if (!obj.Try(for_rest_statement.Match())) goto production3_END; // normal goto & last go
                            {
                                // start of node for_rest_statement
                                if (!obj.Try(new TokenNode(MatchToken(RIGHT_PAREN))))
                                    goto production4_END; // normal goto & last go
                                {
                                    // start of node RIGHT_PAREN
                                    if (!obj.Try(statement_no_new_scope.Match()))
                                        goto production5_END; // normal goto & last go
                                    return obj;
                                    production5_END: // Sub(1)
                                    ;
                                } //depth: 5 
                                production4_END: // Sub(1)
                                ;
                            } //depth: 4 
                            production3_END: // Sub(1)
                            ;
                        } //depth: 3 
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    for_init_statement →
         | expression_statement 
         | declaration_statement 

    */
    public class for_init_statement : Node
    {
        public static bool Check() => expression_statement.Check() || declaration_statement.Check();

        public static for_init_statement Match()
        {
            var obj = new for_init_statement();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(expression_statement.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(declaration_statement.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    conditionopt →
         | condition 

    */
    public class conditionopt : Node
    {
        public static bool Check() => condition.Check();

        public static conditionopt Match()
        {
            var obj = new conditionopt();
            {
                // start of node 
                if (!obj.Try(condition.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    for_rest_statement →
         | conditionopt SEMICOLON 
         | conditionopt SEMICOLON expression 

    */
    public class for_rest_statement : Node
    {
        public static bool Check() => conditionopt.Check();

        public static for_rest_statement Match()
        {
            var obj = new for_rest_statement();
            {
                // start of node 
                if (!obj.Try(conditionopt.Match())) return null; // fail goto
                {
                    // start of node conditionopt
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    {
                        // start of node SEMICOLON
                        if (!obj.Try(expression.Match())) goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    jump_statement →
         | CONTINUE SEMICOLON 
         | BREAK SEMICOLON 
         | RETURN SEMICOLON 
         | RETURN expression SEMICOLON 
         | DISCARD SEMICOLON 

    */
    public class jump_statement : Node
    {
        public static bool Check() =>
            CheckToken(CONTINUE) || CheckToken(BREAK) || CheckToken(RETURN) || CheckToken(DISCARD);

        public static jump_statement Match()
        {
            var obj = new jump_statement();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(CONTINUE)))) goto production0_2; // normal goto & last go
                {
                    // start of node CONTINUE
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(BREAK)))) goto production0_3; // normal goto & last go
                {
                    // start of node BREAK
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_3: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(RETURN)))) goto production0_4; // normal goto & last go
                {
                    // start of node RETURN
                    var p1 = pointer; //节点开始保存状态
                    production1_0: //节点开始 标签 
                    production1_1: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_2; // normal goto & last go
                    return obj;
                    production1_2: // Sub(n)
                    obj.Backtracking(p1);
                    if (!obj.Try(expression.Match())) goto production1_END; // normal goto & last go
                    {
                        // start of node expression
                        if (!obj.Try(new TokenNode(MatchToken(SEMICOLON))))
                            goto production2_END; // normal goto & last go
                        return obj;
                        production2_END: // Sub(1)
                        ;
                    } //depth: 2 
                    production1_END: // Sub(n)
                    ;
                } // depth: 1 
                production0_4: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(new TokenNode(MatchToken(DISCARD)))) return null; // fail goto
                {
                    // start of node DISCARD
                    if (!obj.Try(new TokenNode(MatchToken(SEMICOLON)))) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    translation_unit →
         | external_declaration _translation_unit 

    */
    public class translation_unit : Node
    {
        public static bool Check() => external_declaration.Check();

        public static translation_unit Match()
        {
            var obj = new translation_unit();
            {
                // start of node 
                if (!obj.Try(external_declaration.Match())) return null; // fail goto
                {
                    // start of node external_declaration
                    if (!obj.Try(_translation_unit.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }

    /*
    _translation_unit →
         | external_declaration _translation_unit 
         | E 

    */
    public class _translation_unit : Node
    {
        public static bool Check() => external_declaration.Check() || true;

        public static _translation_unit Match()
        {
            var obj = new _translation_unit();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(external_declaration.Match())) goto production0_2; // normal goto & last go
                {
                    // start of node external_declaration
                    if (!obj.Try(_translation_unit.Match())) goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_2: // Sub(n)
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return obj;
        }
    }

    /*
    external_declaration →
         | function_definition 
         | declaration 

    */
    public class external_declaration : Node
    {
        public static bool Check() => function_definition.Check() || declaration.Check();

        public static external_declaration Match()
        {
            var obj = new external_declaration();
            {
                // start of node 
                var p0 = pointer; //节点开始保存状态
                production0_0: //节点开始 标签 
                production0_1: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(function_definition.Match())) goto production0_2; // normal goto & last go
                return obj;
                production0_2: // Sub(n)
                obj.Backtracking(p0);
                if (!obj.Try(declaration.Match())) return null; // fail goto
                return obj;
                production0_END: // Sub(n)
                ;
            } // depth: 0 
            return null;
        }
    }

    /*
    function_definition →
         | function_prototype compound_statement_no_new_scope 

    */
    public class function_definition : Node
    {
        public static bool Check() => function_prototype.Check();

        public static function_definition Match()
        {
            var obj = new function_definition();
            {
                // start of node 
                if (!obj.Try(function_prototype.Match())) return null; // fail goto
                {
                    // start of node function_prototype
                    if (!obj.Try(compound_statement_no_new_scope.Match()))
                        goto production1_END; // normal goto & last go
                    return obj;
                    production1_END: // Sub(1)
                    ;
                } //depth: 1 
                production0_END: // Sub(1)
                ;
            } //depth: 0 
            return null;
        }
    }
}