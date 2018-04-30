using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Math;

namespace ALight.Render.Components
{
    class Camera
    {
        public Vector3 position, lowLeftCorner, horizontal, vertical;

        public Camera(Vector3 lookFrom, Vector3 lookat, Vector3 vup, float vfov, float aspect)
        {
            float unitAngle = Mathf.PI / 180f * vfov;
            float halfHeight = Mathf.Tan(unitAngle * 0.5f);
            float halfWidth = aspect * halfHeight;
            position = lookFrom;
            Vector3 w = (lookat - lookFrom).Normalized();
            Vector3 u = Vector3.Cross(vup, w).Normalized();
            Vector3 v = Vector3.Cross(w, u).Normalized();
            lowLeftCorner = lookFrom + w - halfWidth * u - halfHeight * v;
            horizontal = 2 * halfWidth * u;
            vertical = 2 * halfHeight * v;
        }

        public Ray CreateRay(float u, float v) =>
            new Ray(position, lowLeftCorner + u * horizontal + v * vertical - position);
    }
}
