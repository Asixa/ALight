using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderLib.Base
{
    public abstract class Intermediate
    {
        public abstract Intermediate Lerp(Intermediate lhs,Intermediate rhs,float fater);
    }
}
