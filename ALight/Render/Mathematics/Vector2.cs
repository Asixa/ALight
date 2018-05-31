using System.Runtime.InteropServices;

namespace ALight.Render.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public class Vector2
    {
        private readonly float[] data = new float[2];
        public float x { get => data[0]; set => data[0] = value; }
        public float y { get => data[1]; set => data[1] = value; }

        public float this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }
        public Vector2() { }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs) => new Vector2
        {
            x = lhs.x + rhs.x,
            y = lhs.y + rhs.y,
        };

        public static Vector2 operator *(Vector2 lhs, float v) => new Vector2
        {
            x = lhs.x * v,
            y = lhs.y * v,
        };

        public static Vector2 operator *(float v, Vector2 rhs) => new Vector2
        {
            x = rhs.x * v,
            y = rhs.y * v,
        };

        public static Vector2 operator /(Vector2 lhs, float v) => new Vector2
        {
            x = lhs.x / v,
            y = lhs.y / v,
        };

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs) => new Vector2
        {
            x = lhs.x - rhs.x,
            y = lhs.y - rhs.y,
        };

        public override string ToString()
        {
            return "<" + x + "," + y + ">";
        }

        public float SqrtMagnitude => x * x + y * y;

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.x, -a.y);

        //public static bool operator ==(Vector2 lhs, Vector2 rhs) => Vector2.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;

        //public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);
    }
}
