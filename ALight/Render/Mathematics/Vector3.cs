namespace ALight.Render.Mathematics
{
    public class Vector3
    {
        public float[] data=new float[3];
        public float x {get => data[0];set => data[0] = value;}
        public float y {get => data[1];set => data[1] = value;}
        public float z {get => data[2];set => data[2] = value;}
       
        public float this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3()
        {
        }
        public Vector3(Vector3 copy)
        {
            x = copy.x;
            y = copy.y;
            z = copy.z;
        }
        public float length() => Mathf.Sqrt(x * x + y * y + z * z);

        public override string ToString()
        {
            return "<" + x + "," + y + "," + z + ">";
        }

        public float Magnitude() => Mathf.Sqrt(x * x + y * y + z * z);

        public Vector3 Normalized()
        {
            var magnitude = Magnitude();
            return new Vector3(x / magnitude, y / magnitude, z / magnitude);
        }

        public static Vector3 Normalize(Vector3 v)
        {
            var magnitude = v.Magnitude();
            return new Vector3(v.x / magnitude, v.y / magnitude, v.z / magnitude);
        }
     
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs) => new Vector3
        {
            x = lhs.x + rhs.x,
            y = lhs.y + rhs.y,
            z = lhs.z + rhs.z
        };

        public static Vector3 operator *(Vector3 lhs, float v) => new Vector3
        {
            x = lhs.x * v,
            y = lhs.y * v,
            z = lhs.z * v
        };

        public static Vector3 operator *(float v, Vector3 rhs) => new Vector3
        {
            x = rhs.x * v,
            y = rhs.y * v,
            z = rhs.z * v
        };

        public static Vector3 operator /(Vector3 lhs, float v) => new Vector3
        {
            x = lhs.x / v,
            y = lhs.y / v,
            z = lhs.z / v
        };

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs) => new Vector3
        {
            x = lhs.x - rhs.x,
            y = lhs.y - rhs.y,
            z = lhs.z - rhs.z
        };

        public static Vector3 operator -(Vector3 a) => new Vector3(-a.x, -a.y, -a.z);

        public static bool operator ==(Vector3 lhs, Vector3 rhs) => Vector3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;

        public static bool operator !=(Vector3 lhs, Vector3 rhs) => !(lhs == rhs);

        public static float SqrMagnitude(Vector3 vector) =>
            vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;

        public static float Dot(Vector3 lhs, Vector3 rhs) => lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;

        public static Vector3 Cross(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);

        public static Vector3 zero = new Vector3(0, 0, 0);
        public static Vector3 one = new Vector3(1, 1, 1);
    }
}
