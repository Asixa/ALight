using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using AcDx;
using ALight.Render.Components;
using ALight.Render.Denoise;
using ALight.Render.Instances;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using ALight.Render.Primitives;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render
{
    public enum Mode
    {
        NormalMap,
        Diffusing
    }
    public class Renderer
    {
        public static Renderer main;
        private const Mode mode = Mode.Diffusing;
        private bool SkyColor=true;
        public readonly HitableList world = new HitableList(), Important = new HitableList();
        public int Samples = 50000,MAX_SCATTER_TIME = 4;
        public int width =512, height =512;
        private Preview preview = new Preview();
        public float[] buff;
        public int[] Changes;

        private Camera camera;
        private float recip_width, recip_height;
        public int now_sample = 0;

        public void InitPreview()
        {
            preview = new Preview();
            preview.Run(new DxConfiguration("Preview", width, height));
        }

        public void Init()
        {
            main = this;
            buff = new float[width * height * 4];
            Changes = new int[width * height];
            InitScene();
            Start();
            InitPreview();
        }

        private void MC()
        {
            var lens_radius = 0;
            var forcus_dis = 1;

            var sun = new Sphere(new Vector3(-10, 10, 10), 4f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 0.9f, 0.7f, 1)), 500));
            world.list.Add(sun);//sun
            Important.list.Add(sun);
            camera = new Camera(new Vector3(-3, 1, -1), new Vector3(-5, 2,8), new Vector3(0, 1, 0), 60, width / (float)height, lens_radius, forcus_dis, 0, 1);
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
                new Lambertian(new ImageTexture("MC/hay_block_top.png")),
                new Lambertian(new ImageTexture("MC/hay_block_side.png")),
                new Lambertian(new ImageTexture("MC/hay_block_side.png",1,1)));
            var crafttable = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/crafting_table_top.png")), new Lambertian(new ImageTexture("MC/crafting_table_front.png")), new Lambertian(new ImageTexture("MC/crafting_table_front.png",1,1)));
            var book = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/planks_oak.png")), new Lambertian(new ImageTexture("MC/bookshelf.png")),new Lambertian(new ImageTexture("MC/bookshelf.png",1,1)));
            var demond = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/diamond_block.png")));
            var huosai = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/piston_side.png")));
            var noteblock = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/noteblock.png")));

            var TNT = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/tnt_top.png")), new Lambertian(new ImageTexture("MC/tnt_side.png")));
            var command = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/command_block_back.png")), new Lambertian(new ImageTexture("MC/command_block_side.png")),
                new Lambertian(new ImageTexture("MC/command_block_side.png",1,1)));

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
            var yj = new Translate(lava, new Vector3(-6, -0.2f, 8));
            //Important.list.Add(yj);
            list.Add(yj);
            list.Add(new Translate(stonebrick, new Vector3(-6, -0.5f, 7)));
            list.Add(new Translate(crafttable, new Vector3(-6, 0f, 6)));
            list.Add(new Translate(command, new Vector3(-6, 0f, 5)));
            list.Add(new Translate(new RotateY( TNT,90), new Vector3(-6, 0f, 3)));
            list.Add(new Translate(new RotateY( TNT,90), new Vector3(-6, 0f, 2)));
            list.Add(new Translate(new RotateY( TNT,90), new Vector3(-6, 1f, 2)));
            list.Add(new Translate(new RotateY(book, 90), new Vector3(-6, 3f, 3)));
            list.Add(new Translate(new RotateY(book, 90), new Vector3(-6, 3f, 2)));
            list.Add(new Translate(new RotateY(book, 90), new Vector3(-6, 3f, 4)));

            list.Add(new Translate(new RotateY(book, 90), new Vector3(-1, 3f, 3)));
            list.Add(new Translate(new RotateY(book, 90), new Vector3(-1, 3f, 2)));
            list.Add(new Translate(new RotateY(book, 90), new Vector3(-1, 3f, 4)));
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

            var ys = new Translate(lamestone, new Vector3(-1, 3, 8));
            list.Add(ys);
            //Important.list.Add(ys);
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
            recip_width = 1f / width;
            recip_height = 1f / height;

            //Demo();
            CornellBox();
            //MC();
            //Model();
        }

        public void Model()
        {
            var lens_radius = 0;
            var forcus_dis = 1;
            camera = new Camera(new Vector3(1f, 1f, 0f), new Vector3(1, 0, 0), new Vector3(0, 1, 0), 90, (float)width / (float)height, lens_radius, forcus_dis, 0, 1);
            var sun = new Sphere(new Vector3(-2, 10, 2), 4f,new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 2));
            world.list.Add(sun);//sun
            Important.list.Add(sun);
            world.list.Add(new Tri( new Vector3(0, 0, 0), new Vector3(1,0,0), new Vector3(1, 1, 0), new Lambertian(new ImageTexture("MC/glowstone.png"))   ));
            //world.list.Add(new Triangle(new Vector3(0,1,1),new Vector3(0,1,0),new Vector3(0,0,0),new Lambertian(new ConstantTexture(Color32.White))   ));
            //world.list.Add(new Triangle(new Vector3(0,1,1),new Vector3(0,0,0),new Vector3(0,1,0),new Lambertian(new ConstantTexture(Color32.White))   ));
            world.list.Add(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.2f)));
        }

        private void Demo()
        {
            var lens_radius = 0;
            var forcus_dis = 1;
            camera = new Camera(new Vector3(0f, 1f, 3f), new Vector3(0, 0, 0), new Vector3(0, 1, 0), 90, (float)width / (float)height, lens_radius, forcus_dis, 0, 1);
            var sun = new Sphere(new Vector3(-2, 10, 2), 4f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 2));
            world.list.Add(sun);//sun
            Important.list.Add(sun);
            world.list.Add(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.2f)));
            world.list.Add(new Sphere(new Vector3(-2, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0f)));
            world.list.Add(new Sphere(new Vector3(-1, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.25f)));
            world.list.Add(new Sphere(new Vector3(0, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.5f)));
            world.list.Add(new Sphere(new Vector3(1, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.75f)));
            world.list.Add(new Sphere(new Vector3(2, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 1)));

        }

        private void CornellBox()
        {
            SkyColor = false;
            camera = new Camera(new Vector3(278, 278, -800), new Vector3(278, 278, 0), new Vector3(0, 1, 0), 40, (float)width / (float)height);
            var list = new List<Hitable>();
            list.Add(new FilpNormals(new PlaneYZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.Green)))));
            list.Add(new PlaneYZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.Red))));
            //var lt = new FilpNormals(new PlaneXZ(213, 343, 227, 332, 354,new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 15)));
            var lt = new PlaneXZ(213, 343, 227, 332, 554,new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 15));
            list.Add(lt);
            Important.list.Add(lt);
            list.Add(new FilpNormals(new PlaneXZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));
            list.Add(new PlaneXZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.White))));
            list.Add(new FilpNormals(new PlaneXY(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));

            var material = new Lambertian(new ConstantTexture(Color32.White));

            world.list.Add(new BVHNode(list.ToArray(), list.Count, 0, 1));
            var cube1 = new Translate(new RotateY(
                    new Cube(
                        new Vector3(0, 0, 0),
                        new Vector3(165, 330, 165),
                        material), 15)
                , new Vector3(265, 0, 295));
            var cube2 = new Translate(new RotateY(
                    new Cube(
                        new Vector3(0, 0, 0),
                        new Vector3(165, 165, 165),
                        material), -18),
                new Vector3(130, 0, 65));
            world.list.Add(cube1);
            world.list.Add(cube2);

            //var sphere = new Sphere(new Vector3(190, 90, 190), 90, new Dielectirc(1.5f));
            //Important.list.Add(sphere);
            //world.list.Add(sphere);
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
            //await Task.Factory.StartNew(delegate { LinearScanner(new ScannerConfig(height, width)); });
            for (var i = 0; i < Samples; i++)
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
                            (j + Random.Get()) * recip_height), world,Important,0).DeNaN()
                        : NormalMap(camera.CreateRay(
                            (i + Random.Get()) * recip_width,
                            (j + Random.Get()) * recip_height), world);
                    SetPixel(config.w-i-1, config.h - j-1,color);
                }
            now_sample++;
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
            var t = 0.5f * ray.normal_direction.y + 1f;
            return (1 - t) * new Color32(1, 1, 1) + t * new Color32(0.5f, 0.7f, 1);
        }

        private Color32 Diffusing(Ray r, HitableList hitableList, Hitable importance, int depth)
        {
            var hrec = new HitRecord();
            if (hitableList.Hit(r, 0.0001f, float.MaxValue, ref hrec))
            {
                var srec = new ScatterRecord();
                var emitted = hrec.material.emitted(r,hrec,hrec.u, hrec.v, hrec.p);
                if (depth < MAX_SCATTER_TIME && hrec.material.scatter(r, ref hrec, ref srec))
                {
                    if (srec.is_specular)
                    {
                        return srec.attenuation * Diffusing(srec.specular_ray, world, importance, depth + 1);
                    }
                    else
                    {

                        var plight = new HitablePdf(importance, hrec.p);
                        var p = new MixturePdf(plight, srec.pdf);
                        var scattered = new Ray(hrec.p, p.Generate(), r.time);
                        var pdf = p.Value(scattered.direction);
                        return emitted + srec.attenuation * hrec.material.scattering_pdf(r, hrec, scattered) *
                               Diffusing(scattered, world, importance, depth + 1) / pdf;
                    }
                }
                else return emitted;

            }
            var t = 0.5f * r.normal_direction.y + 1f;
            return SkyColor ? (1 - t) * new Color32(2, 2, 2) + t * new Color32(0.5f, 0.7f, 1) : Color32.Black;
        }


        public void Save(string path="a.png")
        {
            int Get(int i)=> (byte)Mathf.Range(main.buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255f);
            var pic = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (var i = 0; i < main.buff.Length; i+=4)
            {
                var c = Color.FromArgb(Get(i+3), Get(i), Get(i + 1), Get(i + 2));
                pic.SetPixel(i % (width*4)/4, i / (width*4), c);
            }
            pic.Save(path);
        }


        #region Denoise
        public void Denoise()
        {
            //this.progressBar1.Value = 0;
            //this.lock_Buttons();
            //Denoiser.input_image = this.textBox1.Text;
            //Denoiser.output_image = this.textBox2.Text;
            //Denoiser.blend = (float)this.trackBar1.Value / 100f;
            Denoiser.Denoise(ImageCallBack, ProgressCallBack, DenoiseFinishedCallBack);
        }

        public int DenoiseFinishedCallBack()
        {
            //this.unlock_Buttons();
            return 0;
        }
        public int ProgressCallBack(float progress)
        {
            //this.progressBar1.Value = (int)(progress * 100f);
            //this.pictureBox1.Image = this.pBuffer;
            return 0;
        }
        public int ImageCallBack(IntPtr data, int w, int h, int size)
        {
            //this.pBuffer = new Bitmap(w, h, size / h, PixelFormat.Format32bppArgb, data);
            //this.pBuffer.RotateFlip(RotateFlipType.Rotate180FlipX);
            //this.DoEvents();
            return 0;
        }
        #endregion
    }
}
