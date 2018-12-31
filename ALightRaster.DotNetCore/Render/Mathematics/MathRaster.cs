using System.Numerics;

namespace ALightRaster.DotNetCore.Render.Mathematics
{
    public static class MathRaster
    {
        public static Matrix4x4 GetRotationMatrix(Vector3 v) =>
            Matrix4x4.CreateRotationX(v.X) * Matrix4x4.CreateRotationY(v.Y) * Matrix4x4.CreateRotationZ(v.Z);

        public static Vector3 V3(this Vector4 v) => new Vector3(v.X, v.Y, v.Z);
        public static Vector4 V4(this Vector3 v) => new Vector4(v.X, v.Y, v.Z,1);
        public static string ToString(this Matrix4x4 m) => "[" +
                                                      m.M11 + "," + m.M12 + "," + m.M13 + "," + m.M14 + "," + "\n" +
                                                      //
                                                      m.M21 + "," + m.M22 + "," + m.M23 + "," + m.M24 + "," + "\n" +
                                                      //
                                                      m.M31 + "," + m.M32 + "," + m.M33 + "," + m.M34 + "," + "\n" +
                                                      //
                                                      m.M41 + "," + m.M42 + "," + m.M43 + "," + m.M44 + "," + "]\n";

        public static Vector4 Times(this Vector4 v, Matrix4x4 m) => new Vector4(
            v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + v.W * m.M41,
            v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + v.W * m.M42,
            v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + v.W * m.M43,
            v.X * m.M14 + v.Y * m.M24 + v.Z * m.M34 + v.W * m.M44);

        public static float Range(float v, float min, float max) => (v <= min) ? min :
            v >= max ? max : v;

        public static int Range(int v, int min, int max) => (v <= min) ? min :
            v >= max ? max : v;
        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0) return a;
            if (t >= 1) return b;
            return b * t + (1 - t) * a;
        }
        public static int Lerp(int a, int b, float t)
        {
            if (t <= 0) return a;
            if (t >= 1) return b;
            return (int)(b * t + (1 - t) * a + 0.5f);
        }






        //Garbage
        public static Matrix4x4 GetTranslate(Vector3 v)
        {
            return new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                v.X, v.Y, v.Z, 1);
        }

        public static Matrix4x4 GetRotateY(float r)
        {
            Matrix4x4 rm = Matrix4x4.Identity;
            rm.M11 = (float) (System.Math.Cos(r));

            rm.M13 = (float) (-System.Math.Sin(r));
            //
            rm.M31 = (float) (System.Math.Sin(r));
            rm.M33 = (float) (System.Math.Cos(r));
            return rm;
        }

        public static Matrix4x4 GetRotateX(float r)
        {
            Matrix4x4 rm = Matrix4x4.Identity;

            rm.M22 = (float) (System.Math.Cos(r));
            rm.M23 = (float) (System.Math.Sin(r));
            //

            rm.M32 = (float) (-System.Math.Sin(r));
            rm.M33 = (float) (System.Math.Cos(r));
            return rm;
        }

        public static Matrix4x4 GetRotateZ(float r)
        {
            Matrix4x4 rm = Matrix4x4.Identity;
            rm.M11 = (float) (System.Math.Cos(r));
            rm.M12 = (float) (System.Math.Sin(r));
            //
            rm.M21 = (float) (-System.Math.Sin(r));
            rm.M22 = (float) (System.Math.Cos(r));
            return rm;
        }
    }
}