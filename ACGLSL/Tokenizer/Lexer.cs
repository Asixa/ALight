using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using static ACGLSL.Tokenizer.Token;
using static ACGLSL.Debug.Debugger;
namespace ACGLSL.Tokenizer
{
    public static class Lexer
    {
        public static int line=0, ch;
        public static string[] line_of_codes;

        private static readonly Dictionary<string, TT> Keywords = new Dictionary<string, TT>
        {

            {"attribute", TT.ATTRIBUTE}, {"const", TT.CONST}, {"uniform", TT.UNIFORM}, {"varying", TT.VARYING},
            {"layout", TT.LAYOUT}, {"centroid", TT.CENTROID}, {"flat", TT.FLAT}, {"smooth", TT.SMOOTH},
            {"noperspective", TT.NOPERSPECTIVE}, {"break", TT.BREAK}, {"continue", TT.CONTINUE}, {"do", TT.DO}, {"for", TT.FOR},
            {"while", TT.WHILE}, {"switch", TT.SWITCH}, {"case", TT.CASE}, {"default", TT.DEFAULT}, {"if", TT.IF},
            {"else", TT.ELSE}, {"in", TT.IN}, {"out", TT.OUT}, {"inout", TT.INOUT}, {"float", TT.FLOAT},
            {"int", TT.INT}, {"void", TT.VOID}, {"bool", TT.BOOL}, {"true", TT.BOOLCONSTANT}, {"false", TT.BOOLCONSTANT}, {"invariant", TT.INVARIANT},
            {"discard", TT.DISCARD}, {"return", TT.RETURN}, {"mat2", TT.MAT2},{"mat3", TT.MAT3},{"mat4", TT.MAT4}, {"mat2x2", TT.MAT2X2},
            {"mat2x3", TT.MAT2X3}, {"mat2x4", TT.MAT2X4}, {"mat3x2", TT.MAT3X2}, {"mat3x3", TT.MAT3X3}, {"mat3x4", TT.MAT3X4},
            {"mat4x2", TT.MAT4X2}, {"mat4x3", TT.MAT4X3}, {"mat4x4", TT.MAT4X4}, {"vec2", TT.VEC2}, {"vec3", TT.VEC3},{"vec4", TT.VEC4},
            {"ivec2", TT.IVEC2}, {"ivec3", TT.IVEC3}, {"ivec4", TT.IVEC4}, {"bvec2", TT.BVEC2}, {"bvec3", TT.BVEC3},
            {"bvec4", TT.BVEC4}, {"uint", TT.INT}, {"uvec2", TT.UVEC2}, {"uvec3", TT.UVEC3}, {"uvec4", TT.UVEC4},
            {"lowp", TT.LOW_PRECISION}, {"mediump", TT.MEDIUM_PRECISION}, {"highp", TT.HIGH_PRECISION}, {"precision", TT.PRECISION},
            {"sampler1D", TT.SAMPLER1D}, {"sampler2D", TT.SAMPLER2D}, {"sampler3D", TT.SAMPLER3D}, {"samplerCube", TT.SAMPLERCUBE},

            {"sampler1DShadow", TT.SAMPLER1DSHADOW}, {"sampler2DShadow", TT.SAMPLER2DSHADOW}, {"samplerCubeShadow", TT.SAMPLERCUBESHADOW},
            {"sampler1DArray", TT.SAMPLER1DARRAY}, {"sampler2DArray", TT.SAMPLER2DARRAY}, {"sampler1DArrayShadow", TT.SAMPLER1DARRAYSHADOW},
            {"sampler2DArrayShadow", TT.SAMPLER2DARRAYSHADOW}, {"isampler1D", TT.ISAMPLER1D}, {"isampler2D", TT.ISAMPLER2D},
            {"isampler3D", TT.ISAMPLER3D}, {"isamplerCube", TT.ISAMPLERCUBE}, {"isampler1DArray", TT.ISAMPLER1DARRAY},
            {"isampler2DArray", TT.ISAMPLER2DARRAY},
            {"usampler1D", TT.USAMPLER1D}, {"usampler2D", TT.USAMPLER2D}, {"usampler3D", TT.USAMPLER3D}, {"usamplerCube", TT.USAMPLERCUBE},
            {"usampler1DArray", TT.USAMPLER1DARRAY}, {"usampler2DArray", TT.USAMPLER2DARRAY}, {"sampler2DRect", TT.SAMPLER2DRECT},
            {"sampler2DRectShadow", TT.SAMPLER2DRECTSHADOW}, {"isampler2DRect", TT.ISAMPLER2DRECT}, {"usampler2DRect", TT.USAMPLER2DRECT},
            {"samplerBuffer", TT.SAMPLERBUFFER}, {"isamplerBuffer", TT.ISAMPLERBUFFER}, {"usamplerBuffer", TT.USAMPLERBUFFER},
            {"sampler2DMS", TT.SAMPLER2DMS}, {"isampler2DMS", TT.ISAMPLER2DMS}, {"usampler2DMS", TT.USAMPLER2DMS},
            {"sampler2DMSArray", TT.SAMPLER2DMSARRAY}, {"isampler2DMSArray", TT.ISAMPLER2DMSARRAY}, {"usampler2DMSArray", TT.USAMPLER2DMSARRAY},
            {"struct", TT.STRUCT},

            // reserved for future use
            {"common", TT.RESERVED}, {"partition", TT.RESERVED}, {"active", TT.RESERVED}, {"asm", TT.RESERVED}, {"class", TT.RESERVED},
            {"union", TT.RESERVED}, {"enum", TT.RESERVED}, {"typedef", TT.RESERVED}, {"template", TT.RESERVED}, {"this", TT.RESERVED},
            {"packed", TT.RESERVED},  {"goto", TT.RESERVED},  {"inline", TT.RESERVED},
            {"noinline", TT.RESERVED}, {"volatile", TT.RESERVED}, {"public", TT.RESERVED}, {"static", TT.RESERVED},
            {"extern", TT.RESERVED}, {"external", TT.RESERVED}, {"interface", TT.RESERVED}, {"long", TT.RESERVED},
            {"short", TT.RESERVED}, {"double", TT.RESERVED}, {"half", TT.RESERVED}, {"fixed", TT.RESERVED}, {"unsigned", TT.RESERVED},
            {"superp", TT.RESERVED}, {"input", TT.RESERVED}, {"output", TT.RESERVED}, {"hvec2", TT.RESERVED}, {"hvec3", TT.RESERVED},
            {"hvec4", TT.RESERVED}, {"dvec2", TT.RESERVED}, {"dvec3", TT.RESERVED}, {"dvec4", TT.RESERVED}, {"fvec2", TT.RESERVED},
            {"fvec3", TT.RESERVED}, {"fvec4", TT.RESERVED}, {"sampler3DRect", TT.RESERVED},  
                  
            {"filter", TT.RESERVED},  {"image1D", TT.RESERVED}, {"image2D", TT.RESERVED},
            {"image3D", TT.RESERVED}, {"imageCube", TT.RESERVED},  {"iimage1D", TT.RESERVED}, {"iimage2D", TT.RESERVED},
            {"iimage3D", TT.RESERVED}, {"iimageCube", TT.RESERVED},  {"uimage1D", TT.RESERVED},
            {"uimage2D", TT.RESERVED}, {"uimage3D", TT.RESERVED}, {"uimageCube", TT.RESERVED}, 
            {"image1DArray", TT.RESERVED}, {"image2DArray", TT.RESERVED},   
            {"iimage1DArray", TT.RESERVED}, {"iimage2DArray", TT.RESERVED}, {"uimage1DArray", TT.RESERVED},
            {"uimage2DArray", TT.RESERVED}, {"image1DShadow", TT.RESERVED}, {"image2DShadow", TT.RESERVED},
            {"image1DArrayShadow", TT.RESERVED}, {"image2DArrayShadow", TT.RESERVED}, {"imageBuffer", TT.RESERVED},
            {"iimageBuffer", TT.RESERVED}, {"uimageBuffer", TT.RESERVED}, {"sizeof", TT.RESERVED}, {"cast", TT.RESERVED},
            {"namespace", TT.RESERVED}, {"using", TT.RESERVED}, {"row_major", TT.RESERVED}
        };

        public static StreamReader stream_reader;
        private static char peek;

        private static void ReadChar()
        {
            peek = (char) stream_reader.Read();
            ch++;
        }

        private static bool ReadChar(char ch)
        {
            if (stream_reader.EndOfStream) return false;
            ReadChar();
            if (peek != ch) return false;
            peek = ' ';
            return true;
        }

        public static void InitPath(string path)
        {
            line_of_codes = File.ReadAllLines(path);
            stream_reader = new StreamReader(path);
            ReadChar();
        }

        public static void InitCode(string code)
        {
            line_of_codes = code.Split('\n');
            stream_reader = new StreamReader(code.ToStream()); ReadChar();
        }

   

        public static Token Scan()
        {
            //remove spaces
            for (; !stream_reader.EndOfStream; ReadChar())
            {
                if (peek == ' ' || peek == '\t')
                {
                }
                else if (peek == '\r')
                {
                    ReadChar();
                    ++line;
                    ch = 0;
                }
                else break;
            }

            if (stream_reader.EndOfStream) return null;

            //remove comments
            if (peek == '/')
            {
                ReadChar();
                switch (peek)
                {
                    case '/':
                        for (;; ReadChar())
                            if (peek == '\r' || peek == '\uffff')
                                return Scan();
                    case '*':
                        for (ReadChar();; ReadChar())
                        {
                            switch (peek)
                            {
                                case '\r':
                                    line++;
                                    ch = 0;
                                    ReadChar();
                                    break;
                                case '*':
                                    ReadChar();
                                    if (peek == '/')
                                    {
                                        ReadChar();
                                        return Scan();
                                    }

                                    break;
                                case '\uffff':
                                    return Scan();
                            }
                        }
                }

                return new Token(TT.SLASH, "/");
            }

            //Operators
            switch (peek)
            {
                case '+': return ReadChar('=') ? PlusAssign : Plus;
                case '-': return ReadChar('=') ? MinusAssign : Minus;
                case '*': return ReadChar('=') ? TimeAssign : Time;
                case '/': return ReadChar('=') ? DevideAssign : Devide;
                case '&': return ReadChar('&') ? And : BitAnd;
                case '|': return ReadChar('|') ? Or : BitInOr;
                case '=': return ReadChar('=') ? Equal : Assign;
                case '!': return ReadChar('=') ? NotEqual : Not;
                case '<': return ReadChar('=') ? LessEqual : Less;
                case '>': return ReadChar('=') ? GreaterEqual : Greater;
                case '%': return ReadChar('=') ? MoldingAssign : Molding;
                case ':': return ReadChar(':') ?null: Colon;
                case ';':
                    ReadChar(); return Semicolon;
                case ',': ReadChar(); return Comma;
                case '(': ReadChar(); return LeftParentheses;
                case ')': ReadChar(); return RightParentheses;
                case '{': ReadChar(); return LeftBraces;
                case '}': ReadChar(); return RightBraces;
                case '[': ReadChar(); return LeftSquareBracket;
                case ']': ReadChar(); return RightSquareBracket;
                case '.': ReadChar(); return Dot;
                case '#': ReadChar(); return Preprocessor;
                case '^': ReadChar(); return BitExOr;
                case '~': ReadChar(); return Tilde;
            }

            //Digits
            if (char.IsDigit(peek))
            {
                var val = 0;
                do
                {
                    val = 10 * val + (peek - '0');
                    ReadChar();
                } while (char.IsDigit(peek));

                if (peek != '.') return new Token(TT.INTCONSTANT, val.ToString());

                float float_val = val;
                for (float d = 10;; d *= 10)
                {
                    ReadChar();
                    if (!char.IsDigit(peek)) break;
                    float_val += (peek - 48) / d;
                }

                return new Token(TT.FLOATCONSTANT, float_val.ToString(CultureInfo.InvariantCulture));
            }

            //Identifiers
            
            if (char.IsLetter(peek) || peek == '_')
            {
                var s = "";
                do
                {
                    s += peek;
                    ReadChar();
                } while (char.IsLetterOrDigit(peek)||peek=='_');

                //GLSL identifiers starts with "gl_" or contains "__" is not allowed
                if (s.Length >= 3)
                {
                    if (s.Substring(0, 3).ToLower() == "gl_") Error(4);
                    if (s.Contains("__")) Error(3);
                }
                return new Token(Keywords.ContainsKey(s) ? Keywords[s] : TT.IDENTIFIER, s);
            }

           return new Token(TT.UNEXPECTED,peek.ToString());
        }

        public static void Reset()
        {

        }
    }
}
