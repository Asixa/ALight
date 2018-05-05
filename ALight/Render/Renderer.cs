using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using AcDx;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using ALight.Render.Primitives;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render
{
    public class Preview : DxWindow
    {
        public override void Update()
        {
            for (var i = 0; i < Buff.Length; i++)
                Buff[i] = (byte) Mathf.Range(Renderer.main.buff[i] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0,255f);
        }
    }
    public class Renderer
    {
        public static Renderer main;
        private readonly Mode mode = Mode.Diffusing;

        private readonly HitableList world = new HitableList();
        public int Samples = 10240,MAX_SCATTER_TIME = 16;
        private int width = 192*2*5, height =108*2*5;
        private readonly Preview preview = new Preview();
        public float[] buff;
        public int[] Changes;

        public Camera camera;
        private float recip_width, recip_height;
        private int NowSample = 0;

        public void Init()
        {
            main = this;
            buff = new float[width * height * 4];
            Changes = new int[width * height];
            InitScene();


            Start();
            preview.Run(new DxConfiguration("Preview", width, height));
        }
        public Hitable CreateCube(Vector3 p0, Vector3 p1, Material mat, Material m2 = null)
        {
            var list=new List<Hitable>();
            if (m2 == null) m2 = mat;
            list.Add(new PlaneXY(p0.x, p1.x, p0.y, p1.y, p1.z, m2));//前
            list.Add(new FilpNormals(new PlaneXY(p0.x, p1.x, p0.y, p1.y, p0.z, m2)));//后
            list.Add(new PlaneXZ(p0.x, p1.x, p0.z, p1.z, p1.y, mat));//顶
            list.Add(new FilpNormals(new PlaneXZ(p0.x, p1.x, p0.z, p1.z, p0.y, mat)));//底
            list.Add(new PlaneYZ(p0.y, p1.y, p0.z, p1.z, p1.x, m2));//左
            list.Add(new FilpNormals(new PlaneYZ(p0.y, p1.y, p0.z, p1.z, p0.x, m2)));//右面

            return new BVHNode(list.ToArray(),list.Count,0,1);
        }

        private void MC()
        {
            var lens_radius = 0;
            var forcus_dis = 1;
            world.list.Add(new Sphere(new Vector3(-10, 10, 10), 4f, new DiffuseLight(new ConstantTexture(new Color32(1, 1, 0, 1)), 50)));//sun
            camera = new Camera(new Vector3(-3, 1, -1), new Vector3(-5, 2,8), new Vector3(0, 1, 0), 60, (float)width / (float)height, lens_radius, forcus_dis, 0, 1);
            var lava = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/lava.png"), 4));
            var lamestone = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/glowstone.png"), 3));
            var grass = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/mycelium_top.png")));
            var stone = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/stone.png")));
            var cobblestone = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/cobblestone.png")));
            var stonebrick = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/stonebrick.png")));
            var cobblestone_mossy = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/cobblestone_mossy.png")));
            var planks_oak = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/planks_oak.png")));
            var stonebrick_mossy = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/stonebrick_mossy.png")));
            var hay = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/hay_block_top.png")), new Lambertian(new ImageTexture("MC/hay_block_side.png")));
            var crafttable = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/crafting_table_top.png")), new Lambertian(new ImageTexture("MC/crafting_table_front.png")));
            var demond = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/diamond_block.png")));
            var huosai = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/piston_side.png")));
            var noteblock = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/noteblock.png")));

            var TNT = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/tnt_top.png")), new Lambertian(new ImageTexture("MC/tnt_side.png")));
            var command = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/command_block_back.png")), new Lambertian(new ImageTexture("MC/command_block_side.png")));

            var red = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/redstone_block.png"),1));

            var lamp = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/redstone_lamp_on.png"),2));
            var list = new List<Hitable>();
            for (int i = 0; i > -7; i--)
            for (int j = 0; j < 10; j++)
                list.Add(new Translate(new RotateY(grass, 0), new Vector3(i, -1, j)));

            //for (int i = 0; i > -7; i--)
            //for (int j = 0; j < 5; j++)
            //    list.Add(new Translate(new RotateY(cobblestone_mossy, 0), new Vector3(i, j, 0)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,1)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,2)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,3)));
            list.Add(new Translate(new RotateY(stonebrick_mossy, 90),new Vector3(0,0,4)));
            list.Add(new Translate(new RotateY(stonebrick_mossy, 90),new Vector3(0,0,5)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,6)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,7)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,8)));
            list.Add(new Translate(new RotateY(stonebrick,90),new Vector3(0,0,9)));

            list.Add(new Translate(stonebrick,new Vector3(-1,0,9)));
            list.Add(new Translate(stonebrick,new Vector3(-2,0,9)));
            list.Add(new Translate(planks_oak, new Vector3(-3,0,9)));
            list.Add(new Translate(stonebrick_mossy,new Vector3(-4,0,9)));
            list.Add(new Translate(stonebrick,new Vector3(-5,0,9)));
            list.Add(new Translate(stonebrick,new Vector3(-6,0,9)));

            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 1)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 2)));
            list.Add(new Translate(new RotateY(stonebrick_mossy, 90), new Vector3(-7, 0, 3)));
            list.Add(new Translate(new RotateY(stonebrick_mossy, 90), new Vector3(-7, 0, 4)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 5)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 6)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 7)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 8)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(-7, 0, 9)));

            list.Add(new Translate(hay, new Vector3(-1, 0, 8)));

            list.Add(new Translate(stonebrick, new Vector3(-5, -0.5f, 8)));
            list.Add(new Translate(lava, new Vector3(-6, -0.2f, 8)));
            list.Add(new Translate(stonebrick, new Vector3(-6, -0.5f, 7)));
            list.Add(new Translate(crafttable, new Vector3(-6, 0f, 6)));
            list.Add(new Translate(command, new Vector3(-6, 0f, 5)));
            list.Add(new Translate(new RotateY( TNT,90), new Vector3(-6, 0f, 3)));
            list.Add(new Translate(new RotateY( TNT,90), new Vector3(-6, 0f, 2)));
            list.Add(new Translate(new RotateY( TNT,90), new Vector3(-6, 1f, 2)));
            list.Add(new Translate(new RotateY(huosai, 90), new Vector3(-6, 0f, 1)));
            list.Add(new Translate(new RotateY(noteblock, 90), new Vector3(-6, 1f, 1)));
            list.Add(new Translate(hay, new Vector3(-2, 0, 8)));
            list.Add(new Translate(hay, new Vector3(-1, 0, 7)));
            list.Add(new Translate(hay, new Vector3(-1, 0, 6)));

            list.Add(new Translate(lamp,new Vector3(-5,-0.98f,5)));

            //2 
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 1)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 2)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 3)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(0, 1, 4)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(0, 1, 5)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 6)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 7)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 8)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 1, 9)));

            list.Add(new Translate(cobblestone, new Vector3(-1, 1, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-2, 1, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-3, 1, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-4,1, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-5, 1, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-6, 1, 9)));

            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 1)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 2)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-7, 1, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-7, 1, 4)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 5)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 6)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 7)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 8)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 1, 9)));

            list.Add(new Translate(hay, new Vector3(-1, 1, 8)));
            list.Add(new Translate(hay, new Vector3(-1, 1, 7)));
            list.Add(new Translate(demond, new Vector3(-4, 0, 8)));

            //3
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 1)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 2)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(0, 2, 4)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(0, 2, 5)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 6)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 7)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 8)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 2, 9)));

            list.Add(new Translate(cobblestone, new Vector3(-1, 2, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-2, 2, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-3, 2, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-4, 2, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-5, 2, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-6, 2, 9)));

            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 1)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 2)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-7, 2, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-7, 2, 4)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 5)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 6)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 7)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 8)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 2, 9)));
            //4
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 1)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 2)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(0, 3, 4)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(0, 3, 5)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 6)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 7)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 8)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(0, 3, 9)));

            list.Add(new Translate(cobblestone, new Vector3(-1, 3, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-2, 3, 9)));
            list.Add(new Translate(cobblestone_mossy, new Vector3(-3, 3, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-4, 3, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-5, 3, 9)));
            list.Add(new Translate(cobblestone, new Vector3(-6, 3, 9)));

            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 1)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 2)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-7, 3, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-7, 3, 4)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 5)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 6)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 7)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 8)));
            list.Add(new Translate(new RotateY(cobblestone, 90), new Vector3(-7, 3, 9)));
            list.Add(new Translate(lamestone, new Vector3(-1, 3, 8)));
            //list.Add(new Translate(lamestone, new Vector3(-1, 3, 4)));
            //list.Add(new Translate(lamestone, new Vector3(-5, 4, 5)));


            //5
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 1)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 2)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 3)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 4)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 5)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 7)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 8)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 4, 9)));

   
            list.Add(new Translate(planks_oak, new Vector3(-2, 4, 9)));
            list.Add(new Translate(cobblestone_mossy, new Vector3(-3, 4, 9)));
            list.Add(new Translate(cobblestone_mossy, new Vector3(-4, 4, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-5, 4, 9)));
      

            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 1)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 2)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-6, 4, 4)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 5)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 7)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 8)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-6, 4, 9)));


            //6
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 1)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 2)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 3)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 4)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 5)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-1, 5, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 7)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 8)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 5, 9)));

            list.Add(new Translate(cobblestone_mossy, new Vector3(-3, 5, 9)));
            //list.Add(new Translate(cobblestone_mossy, new Vector3(-4, 5, 9)));

            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 1)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 2)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-5, 5, 4)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 5)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 7)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 8)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-5, 5, 9)));


            //7
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 1)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 2)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 3)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 4)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 5)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-2, 6, 6)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 7)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 8)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-3, 6, 9)));


            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4,6, 1)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 2)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 3)));
            list.Add(new Translate(new RotateY(cobblestone_mossy, 90), new Vector3(-4, 6, 4)));
            //list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 5)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 6)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 7)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 8)));
            list.Add(new Translate(new RotateY(planks_oak, 90), new Vector3(-4, 6, 9)));

            world.list.Add(new BVHNode(list.ToArray(), list.Count, 0, 1));
        }
        private void InitScene()
        {
            var lens_radius = 0;
            var forcus_dis = 1;
            camera = new Camera(new Vector3(0, 1, 2), new Vector3(0, 0, 0), new Vector3(0, 1, 0),90, (float)width/(float)height, lens_radius, forcus_dis, 0,1);
            recip_width = 1f / width;
            recip_height = 1f / height;
            //world.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0, 0, 0)), new ConstantTexture(Color32.white)), 0.2f)));//地面
            //world.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0, 0, 0)), new ConstantTexture(Color32.white)), 0.2f)));//地面
            //world.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f, new Lambertian(new CheckerTexture(new ConstantTexture(new Color32(0, 0, 0)), new ConstantTexture(Color32.white)))));//地面
            //world.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f, new Metal(new ImageTexture("top.png",20), 0.2f)));//地面
            //world.list.Add(new PlaneXZ(-5,5,-5,5,-0.5f,new Lambertian(new ImageTexture("top.png", 10))));

            //world.list.Add(new PlaneXZ(-5,5,-5,5,-0.5f,new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.white)), 0.2f)));

            //world.list.Add(new Sphere(new Vector3(-20, 10, 20), 4f, new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 10)));//sun

            //world.list.Add(new ConstantMedium(CreateCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
            //           new Metal(new ConstantTexture(Color32.white), 1f)),5f,new ConstantTexture(new Color32(1,1,1))));

            MC();

            //DEBUG_BVH();

            //var list=new List<Hitable>();
            //world.list.Add(new Translate(new RotateY(
            //    new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
            //        new Lambertian(new ImageTexture("crafting_table_top.png")), new Lambertian(new ImageTexture("crafting_table_front.png"))), 15),new Vector3(1,0,-2)));

            //world.list.Add(new Translate(new RotateY(
            //    new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
            //        new Lambertian(new ImageTexture("command_block_back.png")), new Lambertian(new ImageTexture("command_block_side.png"))), 15), new Vector3(1.5f, 0, -2)));

            //world.list.Add(new Translate(new RotateY(
            //    new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
            //        new Lambertian(new ImageTexture("command_block_back.png")), new Lambertian(new ImageTexture("command_block_side.png"))), 15), new Vector3(2, 0, -2)));
            // world.list.Add(new BVHNode(list.ToArray(),list.Count,0,1));


            //world.list.Add(new BVHNode(l.ToArray(),l.Count,0,1));

            //world.list.Add(CornellBox());
            //world.list.Add(new Translate(new RotateY(
            //    new Cube(
            //        new Vector3(0, 0, 0),
            //        new Vector3(165, 330, 165),
            //        new Lambertian(new ConstantTexture(Color32.white))), 15)
            //    , new Vector3(265, 0, 295)));
            //world.list.Add(new Translate(new RotateY(
            //    new Cube(
            //        new Vector3(0, 0, 0),
            //        new Vector3(165, 165, 165),
            //        new Lambertian(new ConstantTexture(Color32.white))), -18),
            //    new Vector3(130, 0, 65)));


            //world.list.Add(new Sphere(new Vector3(0, 0f, 0), 0.5f, new Metal(new ImageTexture("texture.jpg", 10),0.9f)));
            //var cube=new Cube(new Vector3(-0.5f, -0.5f, -1.5f), new Vector3(0.5f, 0.5f, -0.5f),new Lambertian(new ConstantTexture(new Color32(1,1,1))));
            // world.list.Add(new ConstantMedium(cube, 2f,new ConstantTexture(Color32.white)));
            //world.list.Add(new Sphere(new Vector3(0, 0f, -1), 0.25f, new Lambertian(new ConstantTexture(Color32.red))));
            //world.list.Add(new Sphere(new Vector3(0f, 0f, -1),0.5f,new Lambertian(new ImageTexture.NoiseTexture())));




            /// world.list.Add(CornellBox());
            //world.list.Add(new Sphere(new Vector3(278, 100, 278), 100f, new Dielectirc(5)));
            //world.list.Add(new PlaneXZ(0, 555, 0, 555, 0, new Lambertian(
            //new CheckerTexture(new ConstantTexture(new Color32(0, 0, 0)), new ConstantTexture(Color32.white)))));
        }

        Hitable CornellBox()
        {
            camera = new Camera(new Vector3(278, 278, -800), new Vector3(278, 278, 0), new Vector3(0, 1, 0), 40, 1);
            var list=new List<Hitable>();
            list.Add(new PlaneYZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.green))));
            list.Add(new PlaneYZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.red))));
            list.Add(new PlaneXZ(213, 343, 227, 332, 554,new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)),15)));
            list.Add(new FilpNormals(new PlaneXZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.white)))));
            list.Add(new PlaneXZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.white))));
            list.Add(new FilpNormals(new PlaneXY(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.white)))));
            return new BVHNode(list.ToArray(),list.Count,0,1);
        }

        private class ScannerConfig
        {
            public readonly int w,h;

            public ScannerConfig( int h, int w)
            {
                this.h = h;
                this.w = w;
            }
        }

        private async void Start()
        {
            ThreadPool.SetMaxThreads(16, 16);
            await Task.Factory.StartNew(delegate { LinearScanner(new ScannerConfig(height, width)); });
            for (var i = 1; i < Samples; i++)
                ThreadPool.QueueUserWorkItem(LinearScanner, new ScannerConfig(height, width));
        }

        private void LinearScanner(object o)
        {
            var config = (ScannerConfig)o;
            for (var j = config.h - 1; j >= 0; j--)
                for (var i = 0; i < config.w; i++)
                {
                    var color = mode == Mode.Diffusing
                        ? Diffusing(camera.CreateRay(
                            (i + Random.Get()) * recip_width,
                            (j + Random.Get()) * recip_height), world, 0)
                        : NormalMap(camera.CreateRay(
                            (i + Random.Get()) * recip_width,
                            (j + Random.Get()) * recip_height), world);
                    SetPixel(config.w-i-1, config.h - j-1,color);
                }
            Form1.main.BeginInvoke(new Action(() => { Form1.main.SetSPP();}));
        }

        private void SetPixel(int x, int y, Color32 c32)
        { 
            var i = width * 4 * y + x * 4;
            Changes[width * y + x]++;
            buff[i] += c32.r;
            buff[i + 1] += c32.g;
            buff[i + 2] += c32.b;
            buff[i + 3] += c32.a;
        }

        private Color32 NormalMap(Ray ray, HitableList hitableList)
        {
            var record = new HitRecord();
            if (hitableList.Hit(ray, 0f, float.MaxValue, ref record))
                return 0.5f * new Color32(record.normal.x + 1, record.normal.y + 1, record.normal.z + 1, 2f);
            var t = 0.5f * ray.normalDirection.y + 1f;
            return (1 - t) * new Color32(1, 1, 1) + t * new Color32(0.5f, 0.7f, 1);
        }

        private Color32 Diffusing(Ray ray, HitableList hitableList, int depth)
        {
            var record = new HitRecord();
            if (hitableList.Hit(ray, 0.0001f, float.MaxValue, ref record))
            {
                var r = new Ray(Vector3.zero, Vector3.zero);
                var attenuation = Color32.black;
                var emitted = record.material.emitted(record.u, record.v, record.p);
                if (depth >= MAX_SCATTER_TIME || !record.material.scatter(ray, record, ref attenuation, ref r))
                    return emitted;
                var c = Diffusing(r, hitableList, depth + 1);
                return new Color32(c.r * attenuation.r, c.g * attenuation.g, c.b * attenuation.b)+emitted;
            }
            var t = 0.5f * ray.normalDirection.y + 1f;
            return (1 - t) * new Color32(2, 2, 2) + t * new Color32(0.5f, 0.7f, 1);
            return Color32.black;
        }

        private enum Mode
        {
            NormalMap,
            Diffusing
        };

        public void Save()
        {
            var pic_buff = preview.Buff;
            var pic = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (var i = 0; i < pic_buff.Length; i+=4)
            {
                var c = Color.FromArgb(pic_buff[i+3], pic_buff[i], pic_buff[i+1], pic_buff[i+2]);
                pic.SetPixel(i % (width*4)/4, i / (width*4), c);
            }
            pic.Save("a.png");
        }
    }
}
