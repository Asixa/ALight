using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using ObjModelLoader;
using Random = System.Random;

namespace ALight.Render.Instances
{
    public class Mesh
    {
        public Vertex[] vertices;
        public Shader shader;
        public Mesh(Model model,Shader s)
        {
            shader = s;
            vertices = new Vertex[model.indexs.Count];
            for (var i = 0; i < model.indexs.Count; i++)
            {
                var index = model.indexs[i];
                var point = model.points[index];
                vertices[i] = new Vertex(point, model.norlmas[i], model.uvs[i].x, model.uvs[i].y);
            }
        }

        public Hitable Create() => Create(vertices,shader);

        public static Hitable Create(Vertex[] vertices,Shader shader)
        {
            //Console.WriteLine("______________________");
            var list = new List<Hitable>();
            for (var i = 0; i < vertices.Length / 3; i++)
            {
                //Console.WriteLine("-----");
                //Console.WriteLine("顶点 " + vertices[3 * i].point + " " + vertices[3 * i].normal + " " + vertices[3 * i].uv);
                //Console.WriteLine("顶点 " + vertices[3 * i+1].point + " " + vertices[3 * i+1].normal + " " + vertices[3 * i+1].uv);
                //Console.WriteLine("顶点 " + vertices[3 * i+2].point + " " + vertices[3 * i+2].normal + " " + vertices[3 * i+2].uv);
                list.Add(new Triangle(vertices[3 * i], vertices[3 * i + 1], vertices[3 * i + 2], shader));
            }
            return new BVHNode(list.ToArray(), list.Count, 0, 1);
        }

        public Mesh(ObjMesh model, Shader s)
        {
            shader = s;
            vertices = new Vertex[model.TriangleArray.Length];
            for (var i = 0; i < model.TriangleArray.Length; i++)
            {
                var index = model.TriangleArray[i];
                var point = model.VertexArray[index];
                vertices[i] = new Vertex(Vector3.FromObj(point),
                    Vector3.FromObj(model.NormalArray[index]),
                    model.UVArray[index].x,
                    model.UVArray[index].y);
            }
        }
    }

    public class Vertex
    {
        public Vector3 point;
        public Vector2 uv;
        public Vector3 normal;

        public Vertex(Vector3 point, Vector3 normal, float u, float v)
        {
            this.point = point;
            this.normal = normal;
            uv=new Vector2(u,v);
        }

        public Vertex()
        {
            point = new Vector3();
            uv=new Vector2();
            normal = new Vector3();
        }
        public Vertex(Vector3 v3)
        {
            point = v3;
            uv = new Vector2();
            normal = new Vector3();
        }

        public Vertex(Vertex v)
        {
            point = v.point;
            normal = v.normal;
            uv = v.uv;
        }
    }

    public class Model
    {
        //顶点坐标
        public List<Vector3> points = new List<Vector3>();

        //三角形顶点索引 12个面
        public List<int> indexs = new List<int>();

        //uv坐标
        public List<Vector2> uvs = new List<Vector2>();



        //法线
        public List<Vector3> norlmas = new List<Vector3>();
        //材质
        //public static Material mat = new Material(new Color(0, 0, 0.1f), 0.1f, new Color(0.3f, 0.3f, 0.3f), new Color(1, 1, 1), 99);

        public Model(List<Vector3> p, List<int> i, List<Vector2> uv, List<Vector3> vertC, List<Vector3> n)
        {
            points = p;
            indexs = i;
            uvs = uv;
            norlmas = n;
        }
        public Model() { }

        public static Model
            Cube = new Model
            {
                points =
                {
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),

                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f)
                },
                indexs =
                {
                    0,
                    1,
                    2,
                    0,
                    2,
                    3,
                    //
                    7,
                    6,
                    5,
                    7,
                    5,
                    4,
                    //
                    0,
                    4,
                    5,
                    0,
                    5,
                    1,
                    //
                    1,
                    5,
                    6,
                    1,
                    6,
                    2,
                    //
                    2,
                    6,
                    7,
                    2,
                    7,
                    3,
                    //
                    3,
                    7,
                    4,
                    3,
                    4,
                    0
                },
                uvs =
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    ///
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
                },
               
                norlmas =
                {
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    //
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    //
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    //
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    //
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    //
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0)
                }
            },
            Plane = new Model
            {
                points = new List<Vector3>
                {
                    new Vector3(-2, 0, -2),
                    new Vector3(-1, 0, -2),
                    new Vector3(-0, 0, -2),
                    new Vector3(1, 0, -2),
                    new Vector3(2, 0, -2),

                    new Vector3(-2, 0, -1),
                    new Vector3(-1, 0, -1),
                    new Vector3(-0, 0, -1),
                    new Vector3(1, 0, -1),
                    new Vector3(2, 0, -1),

                    new Vector3(-2, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-0, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(2, 0, 0),

                    new Vector3(-2, 0, 1),
                    new Vector3(-1, 0, 1),
                    new Vector3(-0, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(2, 0, 1),

                    new Vector3(-2, 0, 2),
                    new Vector3(-1, 0, 2),
                    new Vector3(-0, 0, 2),
                    new Vector3(1, 0, 2),
                    new Vector3(2, 0, 2),
                },
                indexs = new List<int>
                {
                    0,
                    5,
                    6,
                    1,
                    0,
                    6,
                    1,
                    6,
                    7,
                    2,
                    1,
                    7,
                    2,
                    7,
                    8,
                    3,
                    2,
                    8,
                    3,
                    8,
                    9,
                    4,
                    3,
                    9,
                    5,
                    10,
                    11,
                    6,
                    5,
                    11,
                    6,
                    11,
                    12,
                    7,
                    6,
                    12,
                    7,
                    12,
                    13,
                    8,
                    7,
                    13,
                    8,
                    13,
                    14,
                    9,
                    8,
                    14,
                    10,
                    15,
                    16,
                    11,
                    10,
                    16,
                    11,
                    16,
                    17,
                    12,
                    11,
                    17,
                    12,
                    17,
                    18,
                    13,
                    12,
                    18,
                    13,
                    18,
                    19,
                    14,
                    13,
                    19,
                    15,
                    20,
                    21,
                    16,
                    15,
                    21,
                    16,
                    21,
                    22,
                    17,
                    16,
                    22,
                    17,
                    22,
                    23,
                    18,
                    17,
                    23,
                    18,
                    23,
                    24,
                    19,
                    18,
                    24
                },
                uvs = new List<Vector2>
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),

                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                },
                
                norlmas = new List<Vector3>
                {
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),

                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                }
            },
            Skybox = new Model
            {
                points =
                {
                    new Vector3(-5f, 5f, -5f),
                    new Vector3(-5f, -5f, -5f),
                    new Vector3(5f, -5f, -5f),
                    new Vector3(5f, 5f, -5f),

                    new Vector3(-5f, 5f, 5f),
                    new Vector3(-5f, -5f, 5f),
                    new Vector3(5f, -5f, 5f),
                    new Vector3(5f, 5f, 5f)
                },
                indexs =
                {
                    0,
                    1,
                    2,
                    0,
                    2,
                    3,
                    //
                    7,
                    6,
                    5,
                    7,
                    5,
                    4,
                    //
                    0,
                    4,
                    5,
                    0,
                    5,
                    1,
                    //
                    1,
                    5,
                    6,
                    1,
                    6,
                    2,
                    //
                    2,
                    6,
                    7,
                    2,
                    7,
                    3,
                    //
                    3,
                    7,
                    4,
                    3,
                    4,
                    0
                },
                uvs =
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    ///
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
                },
              
                //vertColors =
                //{
                //    new Vector3(0, 1, 0),
                //    new Vector3(0, 0, 1),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 1, 0),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 0, 1),
                //    //
                //    new Vector3(0, 1, 0),
                //    new Vector3(0, 0, 1),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 1, 0),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 0, 1),
                //    //
                //    new Vector3(0, 1, 0),
                //    new Vector3(0, 0, 1),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 1, 0),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 0, 1),
                //    //
                //    new Vector3(0, 1, 0),
                //    new Vector3(0, 0, 1),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 1, 0),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 0, 1),
                //    //
                //    new Vector3(0, 1, 0),
                //    new Vector3(0, 0, 1),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 1, 0),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 0, 1),
                //    //
                //    new Vector3(0, 1, 0),
                //    new Vector3(0, 0, 1),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 1, 0),
                //    new Vector3(1, 0, 0),
                //    new Vector3(0, 0, 1)
                //},
                norlmas =
                {
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, 1),
                    //
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    //
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 0),
                    //
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    //
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(-1, 0, 0),
                    //
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, -1, 0)
                }
            },

            Cube2 = new Model
            {
                points =
                {
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),

                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f)
                },
                indexs =
                {
                    0,
                    1,
                    2,
                    0,
                    2,
                    3,
                    //
                    //7,6,5,
                    //7,5,4,
                    ////
                    //0,4,5,
                    //0,5,1,
                    ////
                    //1,5,6,
                    //1,6,2,
                    ////
                    //2,6,7,
                    //2,7,3,
                    ////
                    //3,7,4,
                    //3,4,0
                },
                uvs =
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    //
                    //new Vector2(0, 0),
                    //new Vector2(0, 1),
                    //new Vector2(1, 1),
                    //new Vector2(0, 0),
                    //new Vector2(1, 1),
                    //new Vector2(1, 0),
                    ////
                    //new Vector2(0, 0),
                    //new Vector2(0, 1),
                    //new Vector2(1, 1),
                    //new Vector2(0, 0),
                    //new Vector2(1, 1),
                    //new Vector2(1, 0),
                    ////
                    //new Vector2(0, 0),
                    //new Vector2(0, 1),
                    //new Vector2(1, 1),
                    //new Vector2(0, 0),
                    //new Vector2(1, 1),
                    //new Vector2(1, 0),
                    ////
                    //new Vector2(0, 0),
                    //new Vector2(0, 1),
                    //new Vector2(1, 1),
                    //new Vector2(0, 0),
                    //new Vector2(1, 1),
                    //new Vector2(1, 0),
                    /////
                    //new Vector2(0, 0),
                    //new Vector2(0, 1),
                    //new Vector2(1, 1),
                    //new Vector2(0, 0),
                    //new Vector2(1, 1),
                    //new Vector2(1, 0)
                },
                norlmas =
                {
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 0, -1),
                    //
                    //new Vector3(0, 0, 1),
                    //new Vector3(0, 0, 1),
                    //new Vector3(0, 0, 1),
                    //new Vector3(0, 0, 1),
                    //new Vector3(0, 0, 1),
                    //new Vector3(0, 0, 1),
                    ////
                    //new Vector3(-1, 0, 0),
                    //new Vector3(-1, 0, 0),
                    //new Vector3(-1, 0, 0),
                    //new Vector3(-1, 0, 0),
                    //new Vector3(-1, 0, 0),
                    //new Vector3(-1, 0, 0),
                    ////
                    //new Vector3(0, -1, 0),
                    //new Vector3(0, -1, 0),
                    //new Vector3(0, -1, 0),
                    //new Vector3(0, -1, 0),
                    //new Vector3(0, -1, 0),
                    //new Vector3(0, -1, 0),
                    ////
                    //new Vector3(1, 0, 0),
                    //new Vector3(1, 0, 0),
                    //new Vector3(1, 0, 0),
                    //new Vector3(1, 0, 0),
                    //new Vector3(1, 0, 0),
                    //new Vector3(1, 0, 0),
                    ////
                    //new Vector3(0, 1, 0),
                    //new Vector3(0, 1, 0),
                    //new Vector3(0, 1, 0),
                    //new Vector3(0, 1, 0),
                    //new Vector3(0, 1, 0),
                    //new Vector3(0, 1, 0)
                }
            };
    }

    //public class ObjModel
    //{
    //    public static Hitable Load(string path,params Shader[] shaders)
    //    {
    //        var result = new ObjLoaderFactory().Create().Load(new FileStream(path, FileMode.Open));
    //        List<Hitable> list=new List<Hitable>();

    //        Vertex Get(FaceVertex face)
    //        {
    //            var v = result.Vertices[face.VertexIndex];
    //            var n = result.Normals[face.NormalIndex];
    //            var uv = result.Textures[face.TextureIndex];
    //            return new Vertex(
    //                new Vector3(v.X, v.Y, v.Z),
    //                new Vector3(n.X, n.Y, n.Z),
    //                uv.X, uv.Y
    //            );
    //        }

    //        int ig = 0;
    //        for (var index = 0; index < result.Groups.Count; index++)
    //        {
    //            if (result.Groups[index].Faces.Count == 0)
    //            {
    //                ig++;continue;}
    //            Console.WriteLine("载入物体 "+index);
    //            var triangles = new List<Triangle>();

    //            int i = 0;
              
    //            foreach (var f in result.Groups[index].Faces)
    //            {

    //                Console.WriteLine("载入物体 " + index + "面"+ ++i+"/"+result.Groups[index].Faces.Count);
    //                if (f.Count == 4)
    //                {
    //                    var v0 = Get(f[0]);
    //                    var v1 = Get(f[1]);
    //                    var v2 = Get(f[2]);
    //                    triangles.Add(new Triangle(v0,v1 , v2,shaders[index-ig]));

    //                    triangles.Add(new Triangle(Get(f[1]), Get(f[2]), Get(f[3]), shaders[index-ig]));
    //                }
    //            }

    //            //list.Add(new BVHNode(triangles.ToArray(),triangles.Count,0,1));
    //        }


    //        return new BVHNode(list.ToArray(), list.Count, 0, 1);
    //    }
    //}


    public class ByteModel
    {
   
        public static Hitable Load(string path, Shader shader) => Load(path, shader, Vector3.one);
        public static Hitable Load(string path,Shader shader,Vector3 scale)
        {
            var binary_reader = new BinaryReader(new FileStream(path, FileMode.Open));
            var count = binary_reader.ReadInt32();

            var vertices=new List<Vertex>();
            for (var i = 0; i < count; i++)
            {
                var p=new Vector3(binary_reader.ReadSingle(), binary_reader.ReadSingle(), binary_reader.ReadSingle())*scale;
                var n = new Vector3(binary_reader.ReadSingle(), binary_reader.ReadSingle(), binary_reader.ReadSingle());
                var uv = new Vector2(binary_reader.ReadSingle(), binary_reader.ReadSingle());
                vertices.Add(new Vertex(p,n,uv.x,uv.y));
            }
            binary_reader.Close();
            

            //
            var a = Mesh.Create(vertices.ToArray(), shader);
            shader.special = a;
            return a;
            //return Mesh.Create(vertices.ToArray(), shader);
        }
    }
}
