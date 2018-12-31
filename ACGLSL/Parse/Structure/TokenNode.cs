using System;
using System.Collections.Generic;
using System.Text;
using ACGLSL.Tokenizer;

namespace ACGLSL.Parse.Structure
{
    public class TokenNode : Node
    {
        public Token t;

        public TokenNode(Token t)
        {
            this.t = t;
        }
    }
}
