using ALight.Render.Mathematics;

namespace ALight.Render.Components
{
    public class AABB
    {
        public Vector3 min, max;
        public AABB()
        {
            min = new Vector3();
            max =new Vector3();
        }
        public AABB(Vector3 a, Vector3 b)
        {
            min = a;
            max=b;
        }

        public bool Hit(Ray r, float tmin, float tmax)
        {
            for (var a = 0; a < 3; a++)
            {
                var t0 = Mathf.Min((min[a] - r.original[a]) / r.direction[a],
                    (max[a] - r.original[a]) / r.direction[a]);
                var t1 = Mathf.Max((min[a] - r.original[a]) / r.direction[a],
                    (max[a] - r.original[a]) / r.direction[a]);
                tmin = Mathf.Max(t0, tmin);
                tmax = Mathf.Min(t1, tmax);
                if (tmax <= tmin)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
