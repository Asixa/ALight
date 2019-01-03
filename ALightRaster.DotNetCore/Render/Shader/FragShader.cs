using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ShaderLib.Base
{
    public abstract class FragShader
    {
        public abstract Vector4 Main(Intermediate data);
    }
}
