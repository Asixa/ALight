using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACGLSL.Tokenizer;
using static ACGLSL.Debug.Debugger;
namespace ACGLSL.Parse
{
    public enum ShaderType
    {

    }
    public class Parser
    {
        public static Token token=> tokens[pointer];
        public static  int pointer;
        public static  List<Token> tokens=new List<Token>();
        public static void SetPointer(int i) => pointer = i;
        public Parser()
        {
        }

        public static void Init()
        {
            while (!Lexer.stream_reader.EndOfStream)
            {
                tokens.Add(Lexer.Scan());
            }
        }
        public static bool CheckToken(TT type) => token.type == type;
        public static bool CheckToken(params TT[]types) => types.Contains(token.type);
        public static bool CheckToken(string content) => token.content == content;

        public static void Move() => pointer++;
        public static Token MatchToken(TT type)
        {
            if (CheckToken(type))
            {
                var tok = token;
                Move();
                return tok;
            }
            else
            {
                Error(6,type,token);
                return null;
            }
        }
        public static void MatchT(string content)
        {
            if (CheckToken(content)) Move();
            else Error(6, content, token);
        }
    }
}
