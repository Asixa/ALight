using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Tokenizer;
using static ACGLSL.Parse.Parser;
using static ACGLSL.Debug.Debugger;
namespace ACGLSL.Parse.Statements
{
//    public class Struct
//    {
//        public string name;
//        public List<Declaration> declarations=new List<Declaration>();
//        public static Struct Match()
//        {
//            MatchT("struct");
//            if (!Check(TT.Identifer)) Error(5, -1, -1, token.content);
//            var structure = new Struct {name = token.content};
//            Move();
//            MatchT("{");
//            while (Declaration.IsThis(1))
//            {
//                structure.declarations.Add(Declaration.Match(1));
//            }
//            MatchT("}");
//            if (Check(TT.Identifer))
//            {
//
//            }
//            MatchT(";");
//            return structure;
//        }
//    }
}
