using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using FileFormatWavefront;
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
                vertices[i] = new Vertex(point, model.norlmas[i], model.uvs[i].x, model.uvs[i].y, model.vertColors[index]);
            }
        }
        public Mesh(FileLoadResult<FileFormatWavefront.Model.Scene> model, Shader s)
        {
            
            //shader = s;

            //vertices = new Vertex[model.Model.UngroupedFaces.Count];
            //for (var i = 0; i < model.Model.UngroupedFaces.Count]; i++)
            //{
            //    var color = Mathematics.Random.Get();
            //    var index = model.Model.UngroupedFaces[i];
            //    var point = model.VertexArray[index];
            //    vertices[i] = new Vertex(Vector3.FromObj(point),
            //        Vector3.FromObj(model.NormalArray[index]),
            //        model.UVArray[index].x,
            //        model.UVArray[index].y, new Vector3(color, color, color));
            //}

            //vertices = new Vertex[model.indexs.Count];
            //for (var i = 0; i < model.indexs.Count; i++)
            //{
            //    var index = model.indexs[i];
            //    var point = model.points[index];
            //    vertices[i] = new Vertex(point, model.norlmas[i], model.uvs[i].x, model.uvs[i].y, model.vertColors[index]);
            //}
        }

        //public Hitable Create()
        //{
        //    var list = new HitableList();
        //    for (var i = 0; i < vertices.Length / 3; i++) list.list.Add(new Tri(vertices[3 * i], vertices[3 * i + 1], vertices[3 * i + 2], shader));
        //    return list;
        //}
        public Hitable Create()
        {
            var list = new List<Hitable>();
            for (var i = 0; i < vertices.Length / 3; i++) list.Add(new Tri(vertices[3 * i], vertices[3 * i + 1], vertices[3 * i + 2], shader));
            return new BVHNode(list.ToArray(), list.Count, 0, 1);
        }

        public Mesh(ObjMesh model, Shader s)
        {
            shader = s;
            vertices = new Vertex[model.TriangleArray.Length];
            for (var i = 0; i < model.TriangleArray.Length; i++)
            {
                var color = Mathematics.Random.Get();
                var index = model.TriangleArray[i];
                var point = model.VertexArray[index];
                vertices[i] = new Vertex(Vector3.FromObj(point),
                    Vector3.FromObj(model.NormalArray[index]),
                    model.UVArray[index].x,
                    model.UVArray[index].y, new Vector3(color, color, color));
            }
        }
    }

    public class Vertex
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public Vector3 point;
        /// <summary>
        /// 纹理坐标
        /// </summary>
        public float u;
        public float v;
        /// <summary>
        /// 顶点色
        /// </summary>
        public Color32 vcolor;
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 normal;
        /// <summary>
        /// 光照颜色
        /// </summary>
        public Color32 lightingColor;
        /// <summary>
        /// 1/z，用于顶点信息的透视校正
        /// </summary>
        public float onePerZ;

        public Vertex(Vector3 point, Vector3 normal, float u, float v, Vector3 color)
        {
            this.point = point;
            this.normal = normal;
            vcolor = new Color32(color.x, color.y, color.z);
            onePerZ = 1;
            this.u = u;
            this.v = v;
            lightingColor = new Color32(1, 1, 1);
        }

        public Vertex()
        {
            point = new Vector3();
            vcolor = new Color32(0, 0, 0);
            lightingColor = new Color32(0, 0, 0);
            u = v = 0;
            normal = new Vector3();
            onePerZ = 1;
        }
        public Vertex(Vector3 v3)
        {
            point = v3;
            vcolor = new Color32(0, 0, 0);
            lightingColor = new Color32(0, 0, 0);
            u = v = 0;
            normal = new Vector3();
            onePerZ = 1;
        }

        public Vertex(Vertex v)
        {
            point = v.point;
            normal = v.normal;
            this.vcolor = v.vcolor;
            onePerZ = 1;
            this.u = v.u;
            this.v = v.v;
            this.lightingColor = v.lightingColor;
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

        //顶点色
        public List<Vector3> vertColors = new List<Vector3>();

        //法线
        public List<Vector3> norlmas = new List<Vector3>();
        //材质
        //public static Material mat = new Material(new Color(0, 0, 0.1f), 0.1f, new Color(0.3f, 0.3f, 0.3f), new Color(1, 1, 1), 99);

        public Model(List<Vector3> p, List<int> i, List<Vector2> uv, List<Vector3> vertC, List<Vector3> n)
        {
            points = p;
            indexs = i;
            uvs = uv;
            vertColors = vertC;
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
                vertColors =
                {
                    //new Vector3(0, 0, 0),
                    //new Vector3(0.125f,0.125f, 0.125f),
                    //new Vector3(0.25f,0.25f, 0.25f),
                    //new Vector3(0.375f, 0.375f, 00.375f),
                    //new Vector3(0.5f, 0.5f, 0.5f),
                    //new Vector3(0.625f, 0.625f, 0.625f),
                    //new Vector3(0.75f, 0.75f, 0.75f),
                    //new Vector3(1, 1, 1),
                    new Vector3(0.5f, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 1, 0),
                    new Vector3(0, 1, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 1, 0.5f),

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
                vertColors = new List<Vector3>
                {
                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),

                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),
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
                vertColors =
                {
                    //new Vector3(0, 0, 0),
                    //new Vector3(0.125f,0.125f, 0.125f),
                    //new Vector3(0.25f,0.25f, 0.25f),
                    //new Vector3(0.375f, 0.375f, 00.375f),
                    //new Vector3(0.5f, 0.5f, 0.5f),
                    //new Vector3(0.625f, 0.625f, 0.625f),
                    //new Vector3(0.75f, 0.75f, 0.75f),
                    //new Vector3(1, 1, 1),
                    new Vector3(0.5f, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 1, 0),
                    new Vector3(0, 1, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 1, 0.5f),

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
                vertColors =
                {
                    //new Vector3(0, 0, 0),
                    //new Vector3(0.125f,0.125f, 0.125f),
                    //new Vector3(0.25f,0.25f, 0.25f),
                    //new Vector3(0.375f, 0.375f, 00.375f),
                    //new Vector3(0.5f, 0.5f, 0.5f),
                    //new Vector3(0.625f, 0.625f, 0.625f),
                    //new Vector3(0.75f, 0.75f, 0.75f),
                    //new Vector3(1, 1, 1),
                    new Vector3(0.5f, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(1, 1, 0),
                    new Vector3(0, 1, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 1, 0.5f),

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

    public class Loader
    {
        public static void Load(string file)
        {
            var result = FileFormatObj.Load("MyFile.obj", false);


        }
    }
}
