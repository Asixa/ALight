using ALight.Render.Components;

namespace ALight.Render.Mathematics
{
    public abstract class Pdf
    {
        public abstract float Value(Vector3 direction);
        public abstract Vector3 Generate();
    }

    public class CosinePdf : Pdf
    {
        private Onb uvw;
        public CosinePdf(Vector3 w) => uvw = new Onb(w);
        public override float Value(Vector3 direction)
        {
            var cos = Vector3.Dot(direction.Normalized(), uvw.w);
            return cos > 0 ? cos / Mathf.PI : 0;
        }
        public override Vector3 Generate()=>uvw.Local(Mathf.RandomCosineDirection());
    }

    public class HitablePdf : Pdf
    {
        private readonly Vector3 o;
        private readonly Hitable p;
        public HitablePdf(Hitable p, Vector3 origin)
        {
            this.p = p;
            o = origin;
        }

        public override float Value(Vector3 direction)=>p.PdfValue(o, direction);
        public override Vector3 Generate()=> p.Random(o);
    }

    public class MixturePdf : Pdf
    {
        private readonly Pdf[] p=new Pdf[2];
        public MixturePdf(Pdf p0, Pdf p1)
        {
            p[0] = p0;
            p[1] = p1;
        }

        public override float Value(Vector3 direction)=>0.5f * p[0].Value(direction) + 0.5f * p[1].Value(direction);
        public override Vector3 Generate()=>Random.Get() < 0.5f ? p[0].Generate() : p[1].Generate();
        
    }
}
