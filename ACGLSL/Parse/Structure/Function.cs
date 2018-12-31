using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Parse.Element;
using ACGLSL.Tokenizer;
using Type=ACGLSL.Parse.Element.Type;
using static ACGLSL.Parse.Parser;
using static ACGLSL.Debug.Debugger;
namespace ACGLSL.Parse.Structure
{
//    public class Function
//    {
//        public struct Param
//        {
//            public Type type;
//            public Identifier identifier;
//        }
//
//        public bool defined = false;
//        public Type ReturnType;
//        public string name;
//        public Stmt stmts;
//        public List<Param> param_list=new List<Param>();
//        public static Function Match()
//        {
//            var function = new Function {ReturnType = Type.Match()};
//            if(!Check(TT.Identifer))Error(5,-1,-1,token.content);
//            function.name = token.content;
//            Move();
//            MatchT("(");
//            while (Type.IsThis())
//            {
//                function.param_list.Add(new Param {type = Type.Match(), identifier = Identifier.Match()});
//                MatchT(",");
//            }
//            MatchT(")");
//            if (CheckToken(";")) return function;
//            function.defined = true;
//            MatchT("{");
//            function.stmts = Stmts.Match();
//            MatchT("}");
//            return function;
//        }
//}
}
