using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ALightRealtime.Render.Structure;

namespace ShaderLib.Base
{
    public abstract class VertexShader
    {
        public Matrix4x4 M,tiM;
        public abstract Intermediate Main(Vertex vertex, out Vector4 position);
    }
}
