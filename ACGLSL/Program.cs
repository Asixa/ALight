using System;
using System.IO;
using ACC.Generated;
using ACGLSL.Parse;
using ACGLSL.Tokenizer;

namespace ACGLSL
{
    class Program
    {
        static void Main(string[] args)
        {
            //Lexer.InitPath("Tests/Sample1.shader");
            Lexer.InitCode("int a =1; ");
            //Console.WriteLine(Lexer.stream_reader.ReadToEnd());
            Parser.Init();
//            while (!Lexer.stream_reader.EndOfStream)
//            {
//                Console.WriteLine(Lexer.Scan());
//            }
            var a = simple_statement.Match();
            a.PrintPretty("",true);
            Console.WriteLine("END");
            Console.ReadKey();
        }
    }
}
