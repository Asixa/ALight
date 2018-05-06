using System;
using System.Linq;
using ALight.Render.Components;

namespace ALight.Render.Instances
{
    public class BVHNode : Hitable
    {
        public Hitable left, right;
        public AABB box;

        public BVHNode()
        {
        }
        public BVHNode(Hitable[] p, int n, float time0, float time1)
        {
            int Compare(Hitable a, Hitable b, int i)
            {
                AABB l = new AABB(), r = new AABB();
                if (!a.BoundingBox(0, 0, ref l) || !b.BoundingBox(0, 0, ref r)) throw new Exception("NULL");
                return l.min[i] - r.min[i] < 0 ? -1 : 1;
            }

            Hitable[] SplitArray(Hitable[] Source, int StartIndex, int EndIndex)
            {
                var result = new Hitable[EndIndex - StartIndex + 1];
                for (var i = 0; i <= EndIndex - StartIndex; i++) result[i] = Source[i + StartIndex];
                return result;
            }
            var pl = p.ToList();
            var method = (int)(3 * Mathematics.Random.Get());
            pl.Sort((a, b) => Compare(a, b, method));
            p = pl.ToArray();
            switch (n)
            {
                case 1:
                    left = right = p[0];
                    break;
                case 2:
                    left = p[0];
                    right = p[1];
                    break;
                default:
                    left = new BVHNode(SplitArray(p, 0, n / 2 - 1), n / 2, time0, time1);
                    right = new BVHNode(SplitArray(p, n / 2, n - 1), n - n / 2, time0, time1);
                    break;
            }

            AABB box_left = new AABB(), box_right = new AABB();
            if (!left.BoundingBox(time0, time1, ref box_left) || !right.BoundingBox(time0, time1, ref box_right))
                throw new Exception("no bounding box in bvh_node constructor");
            box = GetBox(box_left, box_right);
        }

        public override bool BoundingBox(float t0, float t1, ref AABB b)
        {
            b = box;
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            if (box.Hit(ray, t_min, t_max))
            {
                HitRecord left_rec = new HitRecord(), right_rec = new HitRecord();
                var hit_left = left.Hit(ray, t_min, t_max, ref left_rec);
                var hit_right = right.Hit(ray, t_min, t_max, ref right_rec);
                if (hit_left && hit_right)
                {
                    rec = left_rec.t < right_rec.t ? left_rec : right_rec;
                    return true;
                }

                if (hit_left)
                {
                    rec = left_rec;
                    return true;
                }

                if (hit_right)
                {
                    rec = right_rec;
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
