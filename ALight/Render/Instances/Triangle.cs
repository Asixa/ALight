using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;

namespace ALight.Render.Instances
{

    public class Tri:Hitable
    {
        public Vector3 v0, v1, v2;
        public Vector2 uv0, uv1, uv2;
        public Vector3 normal;
        public Shader shader;

        ///                a           b           c
        public Tri(Vertex a, Vertex b, Vertex c, Shader shader)
        {
            //Console.WriteLine("添加三角形 " + a.point + b.point + c.point);
            v0 = a.point;//a
            v1 = b.point;//b
            v2 =c.point; //c
            uv0 = new Vector2(a.u,a.v);
            uv1 = new Vector2(b.u, b.v);
            uv2 = new Vector2(c.u, c.v);
            normal = a.normal;
            //normal = Vector3.Cross(v1 - v0, v2 - v0);
            //Console.WriteLine("Normal "+normal);

            this.shader = shader;
        }

        Vector2 __a(Vector3 p)
        {
            var f1 = v0 - p;
            var f2 = v1 - p;
            var f3 = v2 - p;
            // calculate the areas and factors (order of parameters doesn't matter):
            var a = Vector3.Cross(v0 - v1, v0 - v2).Magnitude(); // main triangle area a
            var a1= Vector3.Cross(f2, f3).Magnitude() / a; // p1's triangle area / a
            var a2= Vector3.Cross(f3, f1).Magnitude() / a; // p2's triangle area / a 
            var a3= Vector3.Cross(f1, f2).Magnitude() / a; // p3's triangle area / a
            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            var uv  = uv0 * a1 + uv1 * a2 + uv2 * a3;
            return uv;
        }

        public override bool Hit(Ray r, float t_min, float t_max, ref HitRecord rec)
        {
            #region OLD




            //var dir = r.direction.Normalized();
            //var v0v1 = v1 - v0;
            //var v0v2 = v2 - v0;
            //var ao = r.origin - v0;
            //float D = v0v1[0] * v0v2[1] * (-dir[2]) + v0v1[1] * v0v2[2] * (-dir[0]) + v0v1[2] * v0v2[0] * (-dir[1]) -
            //          (-dir[0] * v0v2[1] * v0v1[2] - dir[1] * v0v2[2] * v0v1[0] - dir[2] * v0v2[0] * v0v1[1]);
            //float D1 = ao[0] * v0v2[1] * (-dir[2]) + ao[1] * v0v2[2] * (-dir[0]) + ao[2] * v0v2[0] * (-dir[1]) -
            //           (-dir[0] * v0v2[1] * ao[2] - dir[1] * v0v2[2] * ao[0] - dir[2] * v0v2[0] * ao[1]);

            //float D2 = v0v1[0] * ao[1] * (-dir[2]) + v0v1[1] * ao[2] * (-dir[0]) + v0v1[2] * ao[0] * (-dir[1]) -
            //           (-dir[0] * ao[1] * v0v1[2] - dir[1] * ao[2] * v0v1[0] - dir[2] * ao[0] * v0v1[1]);

            //float D3 = v0v1[0] * v0v2[1] * ao[2] + v0v1[1] * v0v2[2] * ao[0] + v0v1[2] * v0v2[0] * ao[1] -
            //           (ao[0] * v0v2[1] * v0v1[2] + ao[1] * v0v2[2] * v0v1[0] + ao[2] * v0v2[0] * v0v1[1]);

            //float a = D1 / D;
            //float b = D2 / D;
            //float temp = D3 / D;

            //if (temp < 0)
            //{
            //    return false;
            //}

            //if (a >= 0 && b >= 0 && a + b < 1)
            //{
            //    if (temp == 0)
            //    {
            //        Console.WriteLine(D3 + " " + D);
            //        Console.ReadLine();
            //    }
            //    rec.t = temp;
            //    rec.p = r.GetPoint(rec.t);
            //    rec.normal = normal;
            //    rec.shader = shader;
            //    var uvw = __a(rec.p); //Console.WriteLine(temp);
            //    rec.u = uvw.x;
            //    rec.v = uvw.y;
            //    return true;
            //}
            //return false;
            #endregion
            if(Vector3.Dot(normal,r.direction)>=0)return false;
            if (!RayIntersectsTriangle(r.origin, r.direction.Normalized(), out Vector3 p)) return false;
            rec.t = Vector3.Distance(r.origin,p);
            rec.p = p;
            rec.normal = normal;
            rec.shader = shader;
            var uvw = __a(rec.p); //Console.WriteLine(temp);
            rec.u = uvw.x;
            rec.v = uvw.y;
            return true;

        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            var bl = new Vector3(Mathf.Min(Mathf.Min(v0[0], v1[0]), v2[0]), Mathf.Min(Mathf.Min(v0[1], v1[1]), v2[1]), Mathf.Min(Mathf.Min(v0[2], v1[2]), v2[2]));
            var tr = new Vector3(Mathf.Max(Mathf.Max(v0[0], v1[0]), v2[0]), Mathf.Max(Mathf.Max(v0[1], v1[1]), v2[1]), Mathf.Max(Mathf.Max(v0[2], v1[2]), v2[2]));
            box = new AABB(bl-new Vector3(0.1f, 0.1f, 0.1f), tr+new Vector3(0.1f, 0.1f, 0.1f));
            return true;
        }


        bool RayIntersectsTriangle(Vector3 rayOrigin,Vector3 rayVector,out Vector3 outIntersectionPoint)
        {
            outIntersectionPoint=new Vector3();
            const float EPSILON = 0.0000001f;
            Vector3 vertex0 = v0;
            Vector3 vertex1 = v1;
            Vector3 vertex2 = v2;
            Vector3 edge1, edge2, h, s, q;
            float a, f, u, v;
            edge1 = vertex1 - vertex0;
            edge2 = vertex2 - vertex0;
            h =Vector3.Cross(rayVector,edge2); 
            a = Vector3.Dot(edge1,h);
            if (a > -EPSILON && a < EPSILON)
                return false;
            f = 1 / a;
            s = rayOrigin - vertex0;
            u = f * (Vector3.Dot(s,h));
            if (u < 0.0 || u > 1.0)
                return false;
            q = Vector3.Cross(s,edge1);
            v = f * Vector3.Dot(rayVector,q);
            if (v < 0.0 || u + v > 1.0)
                return false;
            // At this stage we can compute t to find out where the intersection point is on the line.
            var t = f * Vector3.Dot(edge2,q);
            if (t > EPSILON) // ray intersection
            {
                outIntersectionPoint = rayOrigin + rayVector * t;
                return true;
            }
            else  return false;
        }
    }

    

    public class Matrices
    {
        private double[,] matrix;
        public double[,] Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
        private int _row;

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        private int _col;

        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        private double[,] _invers;

        public double[,] Invers
        {
            get { return _invers; }
            set { _invers = value; }
        }

        private bool isInvertable;

        public Matrices(string command, double x, double y, double z)
        {
            if (command.Equals("translate"))
            {
                this.matrix = new double[4, 4]
                    { {1.0 ,0   ,0   ,x},
                      {0   ,1.0 ,0   ,y},
                      {0   ,0   ,1.0 ,z},
                      {0   ,0   ,0   ,1.0} };
            }
            else if (command.Equals("scale"))
            {
                Console.WriteLine("dalam scale");
                this.matrix = new double[4, 4]
                    { {x ,0 ,0 ,0  },
                      {0 ,y ,0 ,0  },
                      {0 ,0 ,z ,0  },
                      {0 ,0 ,0 ,1.0} };
            }
            this.Row = this.Col = 4;
            this.isInvertable = true;
        }

        public Matrices(int row, int col)
        {
            this.Row = row;
            this.Col = col;
            this.matrix = new double[row, col];
            if (row == col)
            {
                for (int i = 0; i < row; i++)
                {
                    this.Matrix[i, i] = 1.0;
                }
            }
            if (col == row) this.isInvertable = true;
            else this.isInvertable = false;
        }

        public Matrices(int x, int y, int z, float angle)
        {
            double cos = Math.Cos(Mathf.DegreeToRadian(angle));
            double sin = Math.Sin(Mathf.DegreeToRadian(angle));
            if (x == 1)
            {
                this.matrix = new double[4, 4]
                    { {1.0  ,0    ,0    ,0  },
                      {0    ,cos  ,-sin ,0  },
                      {0    ,sin  ,cos  ,0  },
                      {0    ,0    ,0    ,1.0} };
                this.Row = this.Col = 4;
            }
            else if (y == 1)
            {
                this.matrix = new double[4, 4]
                    { {cos  ,0    ,sin  ,0  },
                      {0    ,1.0  ,0    ,0  },
                      {-sin ,0    ,cos  ,0  },
                      {0    ,0    ,0    ,1.0} };
                this.Row = this.Col = 4;
            }
            else if (z == 1)
            {
                this.matrix = new double[4, 4]
                    { {cos  ,-sin ,0    ,0  },
                      {sin  ,cos  ,0    ,0  },
                      {0    ,0    ,1.0  ,0  },
                      {0    ,0    ,0    ,1.0} };
                this.Row = this.Col = 4;
            }
            this.isInvertable = true;
        }

        public Matrices(double a, double b, double c, double d)
        {
            this.matrix = new double[4, 1]
                { {a},
                  {b},
                  {c},
                  {d}};
            this.Row = 4;
            this.Col = 1;
            this.isInvertable = false;
        }

        public static double[,] InversCalculate(double[,] matrixTemp)
        {
            int n = matrixTemp.GetLength(0);
            double[,] invers = new double[n, n];

            double scale;
            for (int i = 0; i < n; i++)
                invers[i, i] = 1.0;

            for (int r0 = 0; r0 < n; r0++)
            {
                if (Math.Abs(matrixTemp[r0, r0]) <= 2.0 * double.Epsilon)
                {
                    for (int r1 = r0 + 1; r1 < n; r1++)
                    {
                        if (Math.Abs(matrixTemp[r1, r0]) <= 2.0 * double.Epsilon)
                        {
                            RowSwap(matrixTemp, n, r0, r1);
                            RowSwap(invers, n, r0, r1);
                            break;
                        }
                    }
                }
                scale = 1.0 / matrixTemp[r0, r0];
                RowScale(matrixTemp, n, scale, r0);
                RowScale(invers, n, scale, r0);

                for (int r1 = 0; r1 < n; r1++)
                {
                    if (r1 != r0)
                    {
                        scale = -matrixTemp[r1, r0];
                        RowScaleAdd(matrixTemp, n, scale, r0, r1);
                        RowScaleAdd(invers, n, scale, r0, r1);
                    }
                }
            }
            return invers;
        }

        private static void RowScale(double[,] matrixTemp, int n, double a, int r)
        {
            for (int i = 0; i < n; ++i)
            {
                matrixTemp[r, i] *= a;
            }
        }

        private static void RowScaleAdd(double[,] matrixTemp, int n, double a, int r0, int r1)
        {
            for (int i = 0; i < n; ++i)
            {
                matrixTemp[r1, i] += a * matrixTemp[r0, i];
            }
        }

        private static void RowSwap(double[,] matrixTemp, int n, int r0, int r1)
        {
            double tmp;
            for (int a = 0; a < n; a++)
            {
                tmp = matrixTemp[r0, a];
                matrixTemp[r0, a] = matrixTemp[r1, a];
                matrixTemp[r1, a] = tmp;
            }
        }

        public static Matrices operator *(double[,] matrix1, Matrices matrix2)
        {
            Matrices hasil = new Matrices(matrix1.GetLength(0), matrix2.Col);
            for (int row = 0; row < hasil.Row; row++)
            {
                for (int col = 0; col < hasil.Col; col++)
                {
                    double temp = 0.0;
                    for (int count = 0; count < matrix2.Row; count++)
                    {
                        temp += matrix1[row, count] * matrix2.Matrix[count, col];
                    }
                    hasil.Matrix[row, col] = temp;
                }
            }

            return hasil;
        }

        public static Matrices operator *(Matrices matrix1, Matrices matrix2)
        {
            Matrices hasil = new Matrices(matrix1.Row, matrix2.Col);

            for (int row = 0; row < hasil.Row; row++)
            {
                for (int col = 0; col < hasil.Col; col++)
                {
                    hasil.Matrix[row, col] = 0;
                    for (int count = 0; count < matrix1.Col; count++)
                    {
                        hasil.Matrix[row, col] += matrix1.Matrix[row, count] * matrix2.Matrix[count, col];
                    }
                }
            }
            return hasil;
        }

        public static Matrices operator /(Matrices matrix1, double n)
        {
            Matrices hasil = new Matrices(matrix1.Row, matrix1.Col);

            for (int row = 0; row < hasil.Row; row++)
            {
                for (int col = 0; col < hasil.Col; col++)
                {
                    hasil.Matrix[row, col] = matrix1.Matrix[row, col] / n;
                }
            }

            return hasil;
        }

        public static Matrices operator *(Matrices matrix1, double n)
        {
            Matrices hasil = new Matrices(matrix1.Row, matrix1.Col);

            for (int row = 0; row < hasil.Row; row++)
            {
                for (int col = 0; col < hasil.Col; col++)
                {
                    hasil.Matrix[row, col] = matrix1.Matrix[row, col] * n;
                }
            }

            return hasil;
        }

        public static Matrices operator +(Matrices matrix1, Matrices matrix2)
        {
            int n = matrix1.Row;
            Matrices hasil = new Matrices(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    hasil.Matrix[i, j] = matrix1.Matrix[i, j] + matrix2.Matrix[i, j];
                }
            }
            return hasil;
        }

        public static Matrices operator -(Matrices matrix1, Matrices matrix2)
        {
            int n = matrix1.Row;
            Matrices hasil = new Matrices(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    hasil.Matrix[i, j] = matrix1.Matrix[i, j] - matrix2.Matrix[i, j];
                }
            }
            return hasil;
        }
    }
}
