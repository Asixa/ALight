using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Tokenizer;
using static ACGLSL.Parse.Parser;
using static ACGLSL.Debug.Debugger;

namespace ACGLSL.Parse.Structure
{
//    public class Preprocessor : Stmt
//    {
//        //Page 8
//        //TODO 
//        public enum PreprocessCommand
//        {
//            Define,
//            Undef,
//            If,
//            Ifdef,
//            Ifndef,
//            Else,
//            Elif,
//            Endif,
//            Error,
//            Pragma,
//            Extension,
//            Version,
//            Line
//        }
//
//        public PreprocessCommand command;
//
//        public static Preprocessor Match()
//        {
//            var p = new Preprocessor();
//
//            MatchToken(TT.Preprocessor);
//            if (!Check(TT.Identifer)) Error(2);
//            switch (token.content)
//            {
//                case "define":
//                    p.command = PreprocessCommand.Define;
//                    break;
//                case "undef":
//                    p.command = PreprocessCommand.Undef;
//                    break;
//                case "if":
//                    p.command = PreprocessCommand.If;
//                    break;
//                case "ifdef":
//                    p.command = PreprocessCommand.Ifdef;
//                    break;
//                case "ifndef":
//                    p.command = PreprocessCommand.Ifndef;
//                    break;
//                case "else":
//                    p.command = PreprocessCommand.Else;
//                    break;
//                case "elif":
//                    p.command = PreprocessCommand.Elif;
//                    break;
//                case "endif":
//                    p.command = PreprocessCommand.Endif;
//                    break;
//                case "error":
//                    p.command = PreprocessCommand.Error;
//                    break;
//                case "pragma":
//                    p.command = PreprocessCommand.Pragma;
//                    break;
//                case "extension":
//                    p.command = PreprocessCommand.Extension;
//                    break;
//                case "version":
//                    p.command = PreprocessCommand.Version;
//                    break;
//                case "line":
//                    p.command = PreprocessCommand.Line;
//                    break;
//                default:
//                    Error(2);
//                    break;
//
//            }
//
//            return p;
//        }
//    }
}
