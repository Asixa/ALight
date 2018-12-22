using System;
using System.Numerics;
using ALightRaster.Render.Mathematics;

namespace ALightRaster.Render.Components
{
    public class Camera:Component
    {
        public static Camera main;
        public  float FOV, Aspect, Near, Far, Size;

        public static Matrix4x4 V, P;
        public static void Caculate()
        {
            V = Matrix4x4.Transpose(MathRaster.GetRotationMatrix(main.transform.rotation) * Matrix4x4.CreateTranslation(main.transform.position));
            P = GetProjection(main.FOV, main.Aspect, main.Near, main.Far);
        }

        private static Matrix4x4 PerspectiveFieldOfView(float fov, float aspect, float near, float far) =>
            new Matrix4x4(MathF.Atan(fov / 2) / aspect, 0, 0, 0, 0, MathF.Atan(fov / 2), 0, 0,
                0, 0, -(far + near) / (far - near), -(2 * near * far) / (far - near), 0, 0, -1, 0);

        public static Matrix4x4 GetProjection(float fov, float aspect, float zn, float zf)
        {
            Matrix4x4 p = new Matrix4x4();
            p.M11 = (float)(1 / (Math.Tan(fov * 0.5f) * aspect));
            p.M22 = (float)(1 / Math.Tan(fov * 0.5f));
            p.M33 = zf / (zf - zn);
            p.M34 = 1f;
            p.M43 = (zn * zf) / (zn - zf);
            return p;
        }
        public Camera(float fov, float aspect, float near, float far)
        {
            main = this;
            this.FOV = fov;
            this.Aspect = aspect;
            this.Near = near;
            this.Far = far;
        }
    }
}
