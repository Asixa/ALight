using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Parse.Structure;
using static ACGLSL.Parse.Parser;
using static ACGLSL.Tokenizer.TT;
using static ACGLSL.Debug.Debugger;

namespace ACGLSL.Parse.Expression
{

    public class PrimaryExpression : Node
    {
        /*  primary_expression:
                variable_identifier 
                INTCONSTANT
                UINTCONSTANT 
                FLOATCONSTANT 
                BOOLCONSTANT 
                LEFT_PAREN expression RIGHT_PAREN 
        */
        public static bool Check() =>
            CheckToken(INTCONSTANT, UINTCONSTANT, FLOATCONSTANT, LEFT_PAREN); //TODO variable_identifier 

        public static PrimaryExpression Match()
        {
            return null;
        }
    }

    public class PostfixExpression : Node
    {
        public static bool Check() => PrimaryExpression.Check(); //|| FunctionCall.Check();

        public static PostfixExpression Match()
        {
            return null;
        }
    }




}
