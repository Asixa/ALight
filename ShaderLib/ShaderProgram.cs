using System;
using System.Collections.Generic;
using System.Text;
using ShaderLib.Base;

namespace ShaderLib
{
    public class ShaderProgram
    {
        // ReSharper disable InconsistentNaming
        public VertexShader VS;
        public FragShader FS;
        public ShaderProgram(VertexShader vs, FragShader fs)
        {
            VS = vs;
            FS = fs;
        }

        public void SetUniform()
        {

        }
    }
}
