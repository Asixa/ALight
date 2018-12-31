using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Parse;
using ACGLSL.Tokenizer;

namespace ACGLSL.Debug
{
    public static class Debugger
    {
        private static readonly string[] Errors = new[]
        {
            "",
            "Syntax error",
            "Invalid preprocessor directive",
            "Identifiers containing two consecutive underscores (__) are reserved as possible future keyword",
            "Identifiers starting with “gl_” are reserved for use by OpenGL",
            "Invalid token '{0}' in class, struct, or interface member declaration",
            "Syntax error at {1},{0} expected,"
        };
        private static readonly string[] Warning = new[]
        {
            "",
        };
        public static void Error(int id, params object[] param)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Errors[id],param);
            var line = Parser.tokens[Parser.pointer].line;
            var ch = Parser.tokens[Parser.pointer].ch;
            var header = "[" + line + "," +ch + "]";
            Console.WriteLine(header+Lexer.line_of_codes[line]);
            Console.WriteLine("↑".PadLeft(ch+header.Length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Warn(int id, int line = -1, int ch = -1, params object[] param)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Warning[id], param);
            if (line != -1) Console.WriteLine(Lexer.line_of_codes[line]);
            if (ch != -1) Console.WriteLine("^".PadRight(ch));
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
