using System.Numerics;
using ALightRaster.Render.Mathematics;

namespace ALightRaster.Render.Components
{
    public class Transform:Component
    {
        public Vector3 position, rotation;
        public Matrix4x4 M;

        public Transform(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;

        }

        public Matrix4x4 CaculateMatrix()
        {
            M =MathRaster.GetRotationMatrix(rotation) *Matrix4x4.CreateTranslation(position);
            return M;
        }
        public Transform()
        {
            rotation=position=Vector3.Zero;
        }
    }
}
