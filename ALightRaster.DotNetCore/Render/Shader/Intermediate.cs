using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderLib.Base
{
    public abstract class Intermediate
    {   public float z_buff;
        public abstract Intermediate Lerp(Intermediate rhs,float fater);
    }
}
