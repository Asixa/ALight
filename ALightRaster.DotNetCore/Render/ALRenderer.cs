using System;
using System.Collections.Generic;
using System.Text;
using ALightRaster.DotNetCore.Render;

namespace ALightRaster.DotNetCore
{
    public class ALRenderer:AL
    {

        public float[] Vertices = new[]
        {
            // 位置              // 颜色
            0.5f, -0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   // 右下
            -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // 左下
            0.0f,  0.5f, 0.0f,   0.0f, 0.0f, 1.0f    // 顶部
        };

        public void Start()
        {

        }
    }
}
