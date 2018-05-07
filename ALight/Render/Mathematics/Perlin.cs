namespace ALight.Render.Mathematics
{
    public static class Perlin
    {
        static float TrillinearInterp(Vector3[,,] c, float u, float v, float w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);
            float accum = 0;
            for (var i = 0; i < 2; i++)
            for (var j = 0; j < 2; j++)
            for (var k = 0; k < 2; k++)
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
            var u = p.x - Mathf.Floor(p.x);
            var v = p.y - Mathf.Floor(p.y);
            var w = p.z - Mathf.Floor(p.z);
            var i = Mathf.Floor2Int(p.x);
            var j = Mathf.Floor2Int(p.y);
            var k = Mathf.Floor2Int(p.z);
            var c = new Vector3[2, 2, 2];
            for (var di = 0; di < 2; di++)
            for (var dj = 0; dj < 2; dj++)
            for (var dk = 0; dk < 2; dk++)
                c[di, dj, dk] = ranvec[perm_x[(i + di) & 255] ^ perm_y[(j + dj) & 255] ^ perm_z[(k + dk) & 255]];
            return TrillinearInterp(c, u, v, w);
        }
        static readonly int[] perm_x = PerlinGeneratePerm();
        static readonly int[] perm_y = PerlinGeneratePerm();
        static readonly int[] perm_z = PerlinGeneratePerm();

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

        static readonly Vector3[] ranvec=PerlinGenerate();
        private static Vector3[] PerlinGenerate()
        {
            var p = new Vector3[256];
            for (var i = 0; i < 256; ++i)p[i] = new Vector3(-1 + 2 * Random.Get(), -1 + 2 * Random.Get(), -1 + 2 * Random.Get()).Normalized();
            return p;
        }

        private static void Permute(int[] p, int n)
        {
            for (var i = n - 1; i > 0; i--)
            {
                var target = (int)(Random.Get() * (i + 1));
                var tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }

        private static int[] PerlinGeneratePerm()
        {
            var p = new int[256];
            for (var i = 0; i < 256; i++)p[i] = i;
            Permute(p, 256);
            return p;
        }
    }
}
