using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderLib.Attributes
{
    public class InputAttribute:Attribute
    {
    }
    public class OutputAttribute : Attribute
    {
    }

    public class UniformAttribute : Attribute
    {
    }

    public class LayoutAttribute : Attribute
    {
        public int pointer;
        public LayoutAttribute(int p) => pointer = p;
    }
}
