using System;
using ALight.Render.Mathematics;
using Random=ALight.Render.Mathematics.Random;
namespace ALight.Render.Components
{
    public class Camera
    {
        private readonly Vector3 position,low_left_corner, horizontal, vertical;
        public Vector3 u, v, w;
        private readonly float radius;
        private readonly float time0,time1;
        public Vector3 direction;
        public Camera(Vector3 lookFrom, Vector3 lookat, Vector3 vup, float vfov, float aspect, 
            float r = 0,float focus_dist = 1,float t0=0,float t1=0)
        {
            direction = (lookat - lookFrom).Normalized();
            time0 = t0;
            time1 = t1;
            radius = r * 0.5f;
            var unit_angle = Mathf.PI / 180f * vfov;
            var half_height = Mathf.Tan(unit_angle * 0.5f);
            var half_width = aspect * half_height;
            position = lookFrom;
             w = (lookat - lookFrom).Normalized();
             u = Vector3.Cross(vup, w).Normalized();
             v = Vector3.Cross(w, u).Normalized();
            low_left_corner = lookFrom + w - half_width * u - half_height * v;
            horizontal = 2 * half_width * u;
            vertical = 2 * half_height * v;
            
        }

        private static Vector3 GetRandomPointInUnitDisk()
        {
            var p = 2f * new Vector3(Random.Get(), Random.Get(), 0) - new Vector3(1, 1, 0);
            return p.Normalized() * Random.Get();
        }

        public Ray CreateRay(float x, float y)
        {
            if (radius == 0f)return new Ray(position, low_left_corner + x * horizontal + y * vertical - position,time0 + Random.Get() * (time1 - time0));
            var rd = radius * GetRandomPointInUnitDisk();
            var offset = rd.x * u + rd.y * v;
           
            return new Ray(position + offset, low_left_corner + x * horizontal + y * vertical - position - offset, time0 + Random.Get() * (time1 - time0));
        }
    }
}
