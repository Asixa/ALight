using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Mathematics;

namespace ALight.Render.Instances
{
    public class Transform : Hitable
    {
        private readonly Hitable Object;
        public Vector3 position, rotation, scale;

        public Transform(Hitable obj, Vector3 p,Vector3 r,Vector3 s)
        {
            position = p;
            Object = obj;
            rotation = r;
            scale = s;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            if (!Object.BoundingBox(t0, t1, ref box)) return false;
            box = new AABB(box.min + position, box.max + position);
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var moved = new Ray(ray.origin - position, ray.direction, ray.time);
            if (!Object.Hit(moved, t_min, t_max, ref rec)) return false;
            rec.p += position;
            return true;
        }
    }
}
