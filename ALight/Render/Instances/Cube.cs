using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;

namespace ALight.Render.Primitives
{
    class Cube:Hitable
    {
        public Vector3 pmin, pmax;
        public Hitable list;
        public Cube(Vector3 p0, Vector3 p1, Material mat,Material m2=null,Material m3=null)
        {
            pmax = p1;
            pmin = p0;
            if (m2 == null) m2 = mat;
            if (m3 == null) m3 = m2;
            var l = new List<Hitable>
            {
                new PlaneXY(p0.x, p1.x, p0.y, p1.y, p1.z, m2),//前
                new FilpNormals(new PlaneXY(p0.x, p1.x, p0.y, p1.y, p0.z, m2)),//后
                new PlaneXZ(p0.x, p1.x, p0.z, p1.z, p1.y, mat),//上
                new FilpNormals(new PlaneXZ(p0.x, p1.x, p0.z, p1.z, p0.y, mat)),//下
                new PlaneYZ(p0.y, p1.y, p0.z, p1.z, p1.x, m3),
                new PlaneYZ(p0.y, p1.y, p0.z, p1.z, p0.x, m3)
            };
            list=new BVHNode(l.ToArray(),l.Count,0,1);
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            box=new AABB(pmin,pmax);
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            return list.Hit(ray, t_min, t_max, ref rec);
        }
    }
}
