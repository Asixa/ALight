using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;

namespace ALight.Render.Mathematics
{
    public abstract class Pdf
    {
        public abstract float value(Vector3 direction);
        public abstract Vector3 generate();
    }

    public class CosinePdf : Pdf
    {
        public Onb uvw;
        public CosinePdf(Vector3 w) => uvw = new Onb(w);
        public override float value(Vector3 direction)
        {
            float cos = Vector3.Dot(direction.Normalized(), uvw.w);
            return cos > 0 ? cos / Mathf.PI : 0;
        }

        public override Vector3 generate()
        {
            return uvw.Local(Mathf.RandomCosineDirection());
        }
    }

    public class HitablePdf : Pdf
    {
        private Vector3 o;
        Hitable p;
        public HitablePdf(Hitable p, Vector3 origin)
        {
            this.p = p;
            o = origin;
        }

        public override float value(Vector3 direction)
        {
            return p.PdfValue(o, direction);
        }

        public override Vector3 generate()
        {
            return p.random(o);
        }
    }

    public class MixturePdf : Pdf
    {
        Pdf[] p=new Pdf[2];
        public MixturePdf(Pdf p0, Pdf p1)
        {
            p[0] = p0;
            p[1] = p1;
        }

        public override float value(Vector3 direction)
        {
            return 0.5f * p[0].value(direction) + 0.5f * p[1].value(direction);
        }

        public override Vector3 generate()
        {
            return Random.Get() < 0.5f ? p[0].generate() : p[1].generate();
        }
    }
}
