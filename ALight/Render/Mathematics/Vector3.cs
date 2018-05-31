using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ObjModelLoader;

namespace ALight.Render.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        private readonly float[] data;
        public float x {get => data[0];set => data[0] = value;}
        public float y {get => data[1];set => data[1] = value;}
        public float z {get => data[2];set => data[2] = value;}
       
        public float this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }
        public Vector3(float x, float y, float z)=>data = new[] {x, y, z};
        public Vector3(int a)=>data = new[] { 0f, 0f, 0f };
        public Vector3(Vector3 copy)=> data = new[] { copy.x, copy.y, copy.z };
        
        public float length() => Mathf.Sqrt(x * x + y * y + z * z);

        public override string ToString()
        {
            return "<" + x + "," + y + "," + z + ">";
        }

        public float Magnitude() => Mathf.Sqrt(x * x + y * y + z * z);

        public Vector3 Normalized()
        {
            var magnitude = Magnitude();
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
            return this;
            //return new Vector3(x / magnitude, y / magnitude, z / magnitude);
        }

        public static Vector3 Normalize(Vector3 v)
        {
            var magnitude = v.Magnitude();
            return new Vector3(v.x / magnitude, v.y / magnitude, v.z / magnitude);
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a - b).Magnitude();
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.x + rhs.x,lhs.y + rhs.y,lhs.z + rhs.z);

        public static Vector3 operator *(Vector3 lhs, float v) => new Vector3(lhs.x * v,lhs.y * v,lhs.z * v);

        public static Vector3 operator *(float v, Vector3 rhs) => new Vector3(rhs.x * v, rhs.y * v,rhs.z * v);

        public static Vector3 operator /(Vector3 lhs, float v) => new Vector3(lhs.x / v,lhs.y / v,lhs.z / v);

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.x - rhs.x,lhs.y - rhs.y,lhs.z - rhs.z );

        public float SqrtMagnitude => x * x + y * y + z * z;

        public static Vector3 operator -(Vector3 a) => new Vector3(-a.x, -a.y, -a.z);

        public static bool operator ==(Vector3 lhs, Vector3 rhs) => SqrMagnitude(lhs - rhs) < 9.99999944E-11f;

        public static bool operator !=(Vector3 lhs, Vector3 rhs) => !(lhs == rhs);

        public static float SqrMagnitude(Vector3 vector) =>
            vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;

        public static float Dot(Vector3 lhs, Vector3 rhs) => lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;

        public static Vector3 Cross(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);

        public static Vector3 zero = new Vector3(0, 0, 0);
        public static Vector3 one = new Vector3(1, 1, 1);

        public static Vector3 FromObj(ObjVector3 v) =>  v==null?new Vector3(0): new Vector3(v.x, v.y, v.z);

        public static List<Vector3> FromObj(ObjVector3[] v) => v.Select(FromObj).ToList();
    }
}
