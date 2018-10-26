using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render.Mathematics
{
    public class Matrix3x3
    {
        private float[,] m = new float[3, 3];

        public Matrix3x3(float[,] _m)
        {
             m=_m;
        }
        public float this[int i, int j]
        {
            get => m[i, j];
            set => m[i, j] = value;
        }


        public static Vector3 operator *(Matrix3x3 lhs, Vector3 v3)
        {
            return new Vector3(
                v3.x * lhs[0, 0] + v3.y * lhs[0, 1] + v3.z * lhs[0, 2],
                v3.x * lhs[1, 0] + v3.y * lhs[1, 1] + v3.z * lhs[1, 2],
                v3.x * lhs[2, 0] + v3.y * lhs[2, 1] + v3.z * lhs[2, 2]);
        }
    }
}
