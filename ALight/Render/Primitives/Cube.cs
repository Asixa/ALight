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
        public HitableList list;
        public Cube(Vector3 p0, Vector3 p1, Material mat,Material m2=null)
        {
            pmax = p1;
            pmin = p0;
            if (m2 == null) m2 = mat;
            list=new HitableList();
            list.list.Add(new PlaneXY(p0.x,p1.x,p0.y,p1.y,p1.z,m2));
            list.list.Add(new FilpNormals(new PlaneXY(p0.x, p1.x, p0.y, p1.y, p0.z, m2))); 
            list.list.Add(new PlaneXZ(p0.x,p1.x,p0.z,p1.z,p1.y,mat));
            list.list.Add(new FilpNormals(new PlaneXZ(p0.x,p1.x,p0.z,p1.z,p0.y,mat)));
            list.list.Add(new PlaneYZ(p0.y,p1.y,p0.z,p1.z,p1.x,m2));
            list.list.Add(new FilpNormals(new PlaneYZ(p0.y,p1.y,p0.z,p1.z,p0.x,m2)));
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
