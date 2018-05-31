using System.Runtime.InteropServices;

namespace ALight.Render.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Onb
    {
        private readonly Vector3[] data;
        private Vector3 u { get => data[0]; set => data[0] = value; }
        private Vector3 v { get => data[1]; set => data[1] = value; }
        public Vector3 w { get => data[2]; set => data[2] = value; }

        public Vector3 this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public Onb(Vector3 n)
        {
            data= new Vector3[3];
            w = n.Normalized();
            var a = (Mathf.Abs(w.x) > 0.9f) ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0);
            v = Vector3.Cross(w, a).Normalized();
            u = Vector3.Cross(w, v);
        }

        public Vector3 Local(float a, float b, float c)=>a * u + b * v + c * w;
        public Vector3 Local(Vector3 a)=>a.x * u +a.y * v +a.z * w;
    }
}
