using System;
using ALight.Render.Mathematics;
using Random=ALight.Render.Mathematics.Random;
namespace ALight.Render.Components
{
    public class Camera
    {
        public Vector3 position, lowLeftCorner, horizontal, vertical;
        public Vector3 u, v, w;
        public float radius;
        public float time0, time1;
        public Camera(Vector3 lookFrom, Vector3 lookat, Vector3 vup, float vfov, float aspect, 
            float r = 0,float focus_dist = 1,float t0=0,float t1=0)
        {
            time0 = t0;
            time1 = t1;
            radius = r * 0.5f;
            var unitAngle = Mathf.PI / 180f * vfov;
            var halfHeight = Mathf.Tan(unitAngle * 0.5f);
            var halfWidth = aspect * halfHeight;
            position = lookFrom;
             w = (lookat - lookFrom).Normalized();
             u = Vector3.Cross(vup, w).Normalized();
             v = Vector3.Cross(w, u).Normalized();
            lowLeftCorner = lookFrom + w - halfWidth * u - halfHeight * v;
            horizontal = 2 * halfWidth * u;
            vertical = 2 * halfHeight * v;
            
        }

        public static Vector3 GetRandomPointInUnitDisk()
        {
            var p = 2f * new Vector3(Random.Get(), Random.Get(), 0) - new Vector3(1, 1, 0);
            return p.Normalized() * Random.Get();
        }

        public Ray CreateRay(float x, float y)
        {
            if (radius == 0f)return new Ray(position, lowLeftCorner + x * horizontal + y * vertical - position,time0 + Random.Get() * (time1 - time0));
            var rd = radius * GetRandomPointInUnitDisk();
            var offset = rd.x * u + rd.y * v;
           
            return new Ray(position + offset, lowLeftCorner + x * horizontal + y * vertical - position - offset, time0 + Random.Get() * (time1 - time0));
        }
    }
}
