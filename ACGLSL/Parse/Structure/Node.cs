using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Tokenizer;

namespace ACGLSL.Parse.Structure
{
    public class Node
    {
        public readonly int line, ch;
        public List<Node>nodes=new List<Node>();
        protected Node()
        {
            line = Lexer.line;
            ch = Lexer.ch;
        }

        public bool Try(Node n)
        {
            if (n == null) return false;
            if (n.GetType() == typeof(TokenNode))
                if (((TokenNode) n).t == null)
                    return false;
            Console.WriteLine("ADD "+n);
            nodes.Add(n);
            return true;

        }

        public void Backtracking(int pointer)
        {
            var garbage_length=Parser.pointer - pointer;
            Parser.pointer = pointer;
            if(garbage_length==0||nodes.Count==0)return;
            nodes.RemoveRange(nodes.Count-garbage_length,garbage_length);
            
        }
        public void PrintPretty(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            if (this.GetType() == typeof(TokenNode))
            {
                var tn = (TokenNode) this;
                Console.WriteLine(tn.t.content);
            }
            else Console.WriteLine(GetType().ToString());

            for (int i = 0; i < nodes.Count; i++)
                nodes[i].PrintPretty(indent, i == nodes.Count - 1);
        }

    }
}
