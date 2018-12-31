using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Tokenizer;
using static ACGLSL.Parse.Parser;
namespace ACGLSL.Parse.Structure
{
//    public class Stmt : Node
//    {
//        protected static readonly Stmt Null = new Stmt();
//        protected static Stmt Match()
//        {
//            switch (token.type)
//            {
//                case TT.Semicolon:
//                    Move();
//                    return Null;
//                case TT.LBraces:
//
//                default:
//                {
//                    return null;
//                    //return FuncCall.MatchT();
//                }
//            }
//        }
//
//       
//    }
//
//    public class Stmts : Stmt
//    {
//        private readonly Stmt stmt1, stmt2;
//
//        private Stmts(Stmt s1, Stmt s2)
//        {
//            stmt1 = s1;
//            stmt2 = s2;
//        }
//
//        public new static Stmt Match() => CheckToken("}") ? Null : new Stmts(Stmt.Match(), Match());
//    }
//
//
//    public class StmtZ : Stmt
//    {
//        private readonly Stmt stmt1, stmt2;
//
//        private Stmts(Stmt s1, Stmt s2)
//        {
//            stmt1 = s1;
//            stmt2 = s2;
//        }
//
//        public new static Stmt Match() => CheckToken("}") ? Null : new StmtZ(Stmt.Match(), Match());
//    }
}
