using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render.Mathematics
{
    public struct Point
    {
        public int x, y;

        public Point(int a, int b)
        {
            x = a;
            y = b;
        }

        public override string ToString()=> "<" + x + "," + y + ">";
        

        public static Point operator /(Point a, int b) => new Point(a.x / b, a.y / b);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public float length => Mathf.Sqrt(x * x + y * y);
    }
}
