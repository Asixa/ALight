using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render.Mathematics
{
    public class Perlin
    {
        static float TrillinearInterp(Vector3[,,] c, float u, float v, float w)
        {
            float uu = u * u * (3 - 2 * u);
            float vv = v * v * (3 - 2 * v);
            float ww = w * w * (3 - 2 * w);
            float accum = 0;
            for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
            for (int k = 0; k < 2; k++)
            {
                accum += (i * uu + (1 - i) * (1 - uu)) *
                         (j * vv + (1 - j) * (1 - vv)) *
                         (k * ww + (1 - k) * (1 - ww)) *
                         Vector3.Dot(c[i,j,k], new Vector3(u - i, v - j, w - k));
            }
            return accum;
        }

        public static float Noise(Vector3 p)
        {
            float u = p.x - Mathf.Floor(p.x);
            float v = p.y - Mathf.Floor(p.y);
            float w = p.z - Mathf.Floor(p.z);
            int i = Mathf.Floor2int(p.x);
            int j = Mathf.Floor2int(p.y);
            int k = Mathf.Floor2int(p.z);
            var c = new Vector3[2, 2, 2];
            for (int di = 0; di < 2; di++)
            for (int dj = 0; dj < 2; dj++)
            for (int dk = 0; dk < 2; dk++)
                c[di, dj, dk] = ranvec[perm_x[(i + di) & 255] ^ perm_y[(j + dj) & 255] ^ perm_z[(k + dk) & 255]];
            return TrillinearInterp(c, u, v, w);
        }
        static readonly int[] perm_x = perlin_generate_perm();
        static readonly int[] perm_y = perlin_generate_perm();
        static readonly int[] perm_z = perlin_generate_perm();

        public static float Turb(Vector3 p, int depth = 7)
        {
            float accum = 0;
            var temp_p = p;
            var weight = 1.0f;
            for (var i = 0; i<depth; i++) {
                accum += weight* Noise(temp_p);
                weight *= 0.5f;
                temp_p *= 2;
            }
            return Mathf.Abs(accum);
        }

static readonly Vector3[] ranvec=perlin_generate();
        //private static float[] ranfloats= perlin_generate();
        private static Vector3[] perlin_generate()
        {
            var p = new Vector3[256];
            for (var i = 0; i < 256; ++i)p[i] = new Vector3(-1 + 2 * Random.Get(), -1 + 2 * Random.Get(), -1 + 2 * Random.Get()).Normalized();
            return p;
        }

        static void permute(int[] p, int n)
        {
            for (var i = n - 1; i > 0; i--)
            {
                var target = (int)(Random.Get() * (i + 1));
                var tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }

        private static int[] perlin_generate_perm()
        {
            var p = new int[256];
            for (var i = 0; i < 256; i++)p[i] = i;
            permute(p, 256);
            return p;
        }
    }
}
