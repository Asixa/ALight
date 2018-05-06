using ALight.Render.Components;
using ALight.Render.Mathematics;

namespace ALight.Render.Instances
{
    public class FilpNormals : Hitable
    {
        private readonly Hitable hitable;

        public FilpNormals(Hitable p)
        {
            hitable = p;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            return hitable.BoundingBox(t0, t1, ref box);
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            if (!hitable.Hit(ray, t_min, t_max, ref rec)) return false;
            rec.normal = -rec.normal;
            return true;
        }
    }

    public class Translate : Hitable
    {
        public Hitable Object;
        private Vector3 offset;

        public Translate(Hitable p, Vector3 displace)
        {
            offset = displace;
            Object = p;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            if (!Object.BoundingBox(t0, t1, ref box)) return false;
            box = new AABB(box.min + offset, box.max + offset);
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var moved = new Ray(ray.original - offset, ray.direction, ray.time);
            if (!Object.Hit(moved, t_min, t_max, ref rec)) return false;
            rec.p += offset;
            return true;
        }
    }

    public class RotateY : Hitable
    {
        public AABB bbox = new AABB();
        public bool hasbox;
        public Hitable Object;
        private float sin_theta, cos_theta;

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            box = bbox;
            return hasbox;
        }

        public RotateY(Hitable p, float angle)
        {
            Object = p;
            var radians = (Mathf.PI / 180f) * angle;
            sin_theta = Mathf.Sin(radians);
            cos_theta = Mathf.Cos(radians);
            hasbox = Object.BoundingBox(0, 1, ref bbox);
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue);
            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 2; j++)
                    for (var k = 0; k < 2; k++)
                    {
                        var x = i * bbox.max.x + (1 - i) * bbox.min.x;
                        var y = j * bbox.max.y + (1 - j) * bbox.min.y;
                        var z = k * bbox.max.z + (1 - k) * bbox.min.z;
                        var newx = cos_theta * x + sin_theta * z;
                        var newz = -sin_theta * x + cos_theta * z;
                        var tester = new Vector3(newx, y, newz);
                        for (int c = 0; c < 3; c++)
                        {
                            if (tester[c] > max[c]) max[c] = tester[c];
                            if (tester[c] < min[c]) min[c] = tester[c];
                        }
                    }
            bbox = new AABB(min, max);
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var origin = new Vector3(ray.original);
            var direction = new Vector3(ray.direction);
            origin[0] = cos_theta * ray.original[0] - sin_theta * ray.original[2];
            origin[2] = sin_theta * ray.original[0] + cos_theta * ray.original[2];
            direction[0] = cos_theta * ray.direction[0] - sin_theta * ray.direction[2];
            direction[2] = sin_theta * ray.direction[0] + cos_theta * ray.direction[2];
            var rotatedR = new Ray(origin, direction, ray.time);
            var r = new HitRecord(rec);
            if (Object.Hit(rotatedR, t_min, t_max, ref rec))
            {
                var p = new Vector3(rec.p);
                var normal = new Vector3(rec.normal);
                p[0] = cos_theta * rec.p[0] + sin_theta * rec.p[2];
                p[2] = -sin_theta * rec.p[0] + cos_theta * rec.p[2];
                normal[0] = cos_theta * rec.normal[0] + sin_theta * rec.normal[2];
                normal[2] = -sin_theta * rec.normal[0] + cos_theta * rec.normal[2];
                rec.p = p;
                rec.normal = normal;
                return true;
            }
            rec = r;
            return false;
        }
    }
}
