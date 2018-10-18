using System.Collections.Generic;
using ALight.Render.Components;
using ALight.Render.Instances;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using ALight.Render.Primitives;

namespace ALight.Render
{
    public struct LightSource
    {
        public static List<LightSource> lights=new List<LightSource>();
        public Hitable obj;
        public Vector3 point;

        public LightSource(Hitable h, Vector3 p)
        {
            obj = h;
            point = p;
            lights.Add(this);
        }

        public static LightSource GetRandom() => lights[(int) (Random.Get() * (lights.Count - 0.1f))];
        public static Ray GetRandomRay()
        {
            var light = GetRandom();
            return new Ray(light.point,light.obj.Random(light.point));
        }

    }
    public class Scene
    {
        public readonly HitableList world = new HitableList(), Important = new HitableList();
        public bool SkyColor = true;
        public static Scene main=new Scene();
        public Camera camera;
        public int width => Configuration.width;
        public int height => Configuration.height;

        public void LoadScene()
        {

        }

        public void Init()
        {
            //Sky();
            //Demo();
            //MC();
            //CornellBox();
            //ByteModels();
            //Bunny();
             //Dragon();
            //BDCornellBox();

            CornellBox18();
            //高考();
            //GlassTest();
            //Model();
            LoadSky();

            
        }

        public void LoadSky()
        {
            if (!SkyColor) return;
            string e = ".png";
            string skyname = "Epic/A" + "/";
            sky = new CubeMap(ResourceManager.SkyboxPath + skyname + "Front" + e,
                ResourceManager.SkyboxPath + skyname + "Up" + e,
                ResourceManager.SkyboxPath + skyname + "Left" + e,
                ResourceManager.SkyboxPath + skyname + "Back" + e, ResourceManager.SkyboxPath + skyname + "Down" + e,
                ResourceManager.SkyboxPath + skyname + "Right" + e);
        }

        public void GlassTest()
        {
            camera = new Camera(new Vector3(0f, 2f, -2f), new Vector3(0f, 0, 0), new Vector3(0, 1, 0), 60, (float)width / (float)height, 0, 1, 0, 1);
            var list = new List<Hitable>();
            list.Add(new Translate(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new ImageTexture(ResourceManager.TexturePath + "TexturesCom_WoodPlanksBare0467_2_seamless_S.jpg", 1), 0.08f)), new Vector3(0, 0, 0)));
            //var g = new Dielectirc(1.5f, new Color32(0, 1f, 0, 1f));
            var g=new Lambertian(new ImageTexture("MC/command_block_back.png"));
            var r = new Metal(new ConstantTexture(new Color32(0, 1, 0)), 0);
            var cube2 = new Translate(new RotateY(
                    new Cube(
                        -new Vector3(0.5f, 0.5f, 0.5f),
                        new Vector3(0.5f, 0.5f, 0.5f),
                        Shader.Glass), 0),
                new Vector3(0, 0, 0));
            var cube = new Translate(new Mesh(Instances.Model.Cube,Shader.Glass).Create(), new Vector3(0, 0, 0));
        

            var sphere = new Sphere(new Vector3(0, 0,0) / 2, 0.5f, new Dielectirc(1.5f));

            list.Add(cube);
            var sun = new Sphere(new Vector3(-2, 1, -4), 0.5f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 20));
            list.Add(sun);
            Important.list.Add(sun);
            world.list.Add(new RotateY(new BVHNode(list.ToArray(), list.Count, 0, 1), 0));
        }
        public void 高考()
        {
            camera = new Camera(new Vector3(-4f, 3f, -4f), new Vector3(0f,2f, 0), new Vector3(0, 1, 0), 80, (float)width / (float)height, 0, 1, 0, 1);
            var list = new List<Hitable>();
            //new ImageTexture(ResourceManager.TexturePath + "TexturesCom_WoodPlanksBare0467_2_seamless_S.jpg", 1)
            list.Add(new Translate(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new ConstantTexture(Color32.White), 0.08f)), new Vector3(0, 0, -5)));
            var g = new Dielectirc(1.5f, new Color32(0, 1f, 0, 1f));
            var g2 = new Dielectirc(1.5f, new Color32(1, 0f, 0, 1f));
            var r = new Metal(new ConstantTexture(new Color32(0, 1, 0)), 0);
            var model = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/高考.ACM",g), new Vector3(-0f, 4f, 0));//new Metal(new ConstantTexture(new Color32(0.6f,1,0)),0 )
            var model2 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/加油.ACM", g2), new Vector3(-0f, 1f, 0));//new Metal(new ConstantTexture(new Color32(0.6f,1,0)),0 )
            var cube = new Translate(new Mesh(Instances.Model.Cube, Shader.Glass).Create(), new Vector3(0, 0, 0));
            //var model3 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/Dragon.ACM", g), new Vector3(-0f, -0.5f, 0));
            list.Add(model);
            list.Add(model2);
             //list.Add(model3);
            
            Important.list.Add(cube);
            world.list.Add(new RotateY(new BVHNode(list.ToArray(), list.Count, 0, 1), 45));
        }
        public void Dragon()
        {
            camera = new Camera(new Vector3(2f, 2.5f, 4f), new Vector3(0f, 1, 0), new Vector3(0, 1, 0), 60, (float)width / (float)height, 0, 1, 0, 1);
            var list = new List<Hitable>();
            list.Add(new Translate(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new ImageTexture(ResourceManager.TexturePath + "TexturesCom_WoodPlanksBare0467_2_seamless_S.jpg", 1), 0.08f)), new Vector3(0, 0, 0)));
            var g = new Dielectirc(1.5f, new Color32(1, 1f, 1,1f));
            var r=new Metal(new ConstantTexture(new Color32(0.5f,1,0)), 0);
            var model = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/Dragon.ACM",
                new Subsurface(1.5f, Color32.Green, 0.05f)
                ), new Vector3(-0f, -0.5f, 0));//new Metal(new ConstantTexture(new Color32(0.6f,1,0)),0 )
            var cube = new Translate(new Mesh(Instances.Model.Cube,g).Create(), new Vector3(10, 0, 0));
            list.Add(model);
            var sun = new Sphere(new Vector3(-2, 1,-4), 0.5f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 20));
            //list.Add(sun);
            Important.list.Add(model);
            world.list.Add(new RotateY(new BVHNode(list.ToArray(), list.Count, 0, 1), 0));
        }
        public void Bunny()
        {
            camera = new Camera(new Vector3(0f, 2.5f, 4f), new Vector3(0f, 1, 0), new Vector3(0, 1, 0), 80, (float)width / (float)height, 0, 1, 0, 1);
            var list = new List<Hitable>();
            //list.Add(new Translate(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new ImageTexture(ResourceManager.TexturePath + "TexturesCom_WoodPlanksBare0467_2_seamless_S.jpg", 1), 0.08f)), new Vector3(1, 0, 0)));
            var g = new Dielectirc(1.5f, new Color32(0, 1f, 0, 1f));
            var inside = new Metal(new ConstantTexture(new Color32(0f, 0.6f, 01f)), 0);
            //var model =new Translate(new RotateY(ShaderBall.Create(new Subsurface(1.5f,Color32.White, 0.0f), inside, inside),+20),new Vector3(0,-0.5f,0));

            var model = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/Bunny.ACM",inside),new Vector3(-0f,-1.2f,0));
            //var model = new RotateY(new Translate(car(), new Vector3(-0f, -0.5f, 0)), 90);
            var cube = new Translate(new Mesh(Instances.Model.Cube, new DiffuseLight(new ConstantTexture(new Color32(1,1,1)),10)).Create(), new Vector3(0, 0, -4));
            

            list.Add(model);
            //world.list.Add(cube);
            Important.list.Add(cube);
            world.list.Add(new RotateY(new BVHNode(list.ToArray(), list.Count, 0, 1), 0));
        }
        public void ByteModels()
        {
            SkyColor = false;
            camera = new Camera(new Vector3(0f, 0f, -2f), new Vector3(0f, 0f, 0), new Vector3(0, 1, 0), 60, (float)width / (float)height, 0, 1, 0, 1);
            //world.list.Add(new Translate(new PlaneXZ(-5,5, -5, 5, -0.5f, new Metal(new ImageTexture(ResourceManager.TexturePath + "TexturesCom_MetalGalvanized0005_S.jpg", 1), 0.08f)), new Vector3(1, 0, 0)));
            world.list.Add(new FilpNormals(new Translate(new PlaneXY(-5,5, -5, 5, 0.5f, Shader.WhiteLambertion), new Vector3(1, 0, 0))));
            var inside =new Metal(new ConstantTexture(new Color32(0,0.6f,1)),0);
            //var model =new Translate(new RotateY(ShaderBall.Create(Shader.Glass, inside, inside),+20),new Vector3(0,-0.5f,0));

            //var model = new RotateY(new Translate(car(), new Vector3(-0f, -0.5f, 0)),90);
            var cube=new Translate(new Mesh(Instances.Model.Cube, Shader.Glass).Create(),new Vector3(10,0,0));
            //var R=new Metal(new ConstantTexture(new Color32(1,0.5f,0.5f)), 0.7f);
            //var G = new Metal(new ConstantTexture(new Color32(0.5f, 1f, 0.5f)), 0.7f);
            //var B = new Metal(new ConstantTexture(new Color32(0.5f, 0.5f, 1f)), 0.7f);


            var R = new DiffuseLight(new ConstantTexture(new Color32(1, 0.5f, 0.5f)),2);
            var G = new DiffuseLight(new ConstantTexture(new Color32(0.5f, 1f, 0.5f)),2);
            var B = new DiffuseLight(new ConstantTexture(new Color32(0.5f, 0.5f, 1f)), 2);


            var list = new List<Hitable>();

            var a = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/F1.模型", R),
                new Vector3(-0f, 0f, 0));
            var a2 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/F2.模型", R),
                new Vector3(-0f, 0f, 0));
            var a3 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/F3.模型", R),
                new Vector3(-0f, 0f, 0));
            var a4 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/X1.模型", G),
                new Vector3(-0f, 0f, 0));
            var a5 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/M1.模型", B),
                new Vector3(-0f, 0f, 0));
            var a6 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/M2.模型", B),
                new Vector3(-0f, 0f, 0));
            var a7 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/M3.模型", B),
                new Vector3(-0f, 0f, 0));
            var a8 = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/M4.模型", B),
                new Vector3(-0f, 0f, 0));
            list.Add(a);
            list.Add(a2);
            list.Add(a3);
            list.Add(a4);
            list.Add(a5);
            list.Add(a6);
            list.Add(a7);
            list.Add(a8);
            
           
           
            //Important.list.Add(cube);
            Important.list.Add(a);
            Important.list.Add(a2);
            Important.list.Add(a3);
            Important.list.Add(a4);
            Important.list.Add(a5);
            Important.list.Add(a6);
            Important.list.Add(a7);
            Important.list.Add(a8);
            world.list.Add(new RotateY(new BVHNode(list.ToArray(), list.Count, 0, 1), 0));
        }


        public Hitable car()
        {
            var p = ResourceManager.ModelPath + "ByteModel/Cars/";
            
            float f = 1.22f, b = -1.5f;
            var carm=new Metal(new ConstantTexture(new Color32(0.9f,0.1f,0)),0f );
            var carm2 = new Metal(new ConstantTexture(new Color32(0.3f,0.3f,0.3f)), 0.8f);
            var wheelm=new Lambertian(new ImageTexture(p+"wheel.png"));
            var bm = new Lambertian(new ImageTexture(p + "body.png"));
            var wheel = ByteModel.Load(p + "Wheel_Back_L.ACM", wheelm);
            var l=new DiffuseLight(new ConstantTexture(Color32.White),1 );
            var list = new List<Hitable>
            {
                new Translate(wheel,new Vector3(1,0.3f,b)),
                new Translate(wheel,new Vector3(-1,0.3f,b)),

                new Translate(wheel,new Vector3(1,0.24f,f )),
                new Translate(wheel,new Vector3(-1,0.24f,f )),
                ByteModel.Load(p + "Body_43.ACM", carm),

                //ByteModel.Load(p + "break_L.ACM", Shader.Sliver),
                //ByteModel.Load(p + "break_R.ACM", Shader.Sliver),

                ByteModel.Load(p + "Glass_43.ACM", Shader.Glass),
                
                new Translate(ByteModel.Load(p + "Glass_ight.ACM", Shader.Glass),new Vector3(0,0.4f,2.1f )),
                ByteModel.Load(p + "Matte_43.ACM", bm),
                //ByteModel.Load(p + "Taillights_43.ACM", Shader.Sliver),


            };
            return new BVHNode(list.ToArray(), list.Count, 0, 1);
        }
        public void Model()
        {
            var lens_radius = 0f;
            var forcus_dis = 1;
            camera = new Camera(new Vector3(-5,10f, -10), new Vector3(0f, 0, 0), new Vector3(0, 1, 0), 80, (float)width / (float)height, lens_radius, forcus_dis, 0, 1);
            var tea = ObjModelLoader.ObjLoader.load(ResourceManager.ModelPath+"tea.obj");
            var list=new List<Hitable>();
            list.Add(new Translate(
                new PlaneXZ(-20, 20, -20, 20, -0.5f,
                    new Metal(
                        new ImageTexture(
                            ResourceManager.TexturePath + "TexturesCom_WoodPlanksBare0467_2_seamless_S.jpg", 2), 0.2f)),
                new Vector3(3, 0, 0)));
            list.Add(new Translate(new RotateY(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/tea.ACM", 
                new Subsurface(1.5f,Color32.White, 0f)), 30),new Vector3(0,0,0)));//x -4
            var sphere = new Translate(new Sphere(new Vector3(0, 0, 0), 4, Shader.Glass), new Vector3(8, 3, 0));
            Important.list.Add(sphere);
            world.list.Add(new RotateY(new BVHNode(list.ToArray(),list.Count,0,1),0 ));
        }



        public void Sky()
        {

            var lens_radius = 0;
            var forcus_dis = 1;
            camera = new Camera(new Vector3(-1f, 0.1f,-2f), new Vector3(0f,0f, 0f), new Vector3(0, 1, 0), 90,
                width / (float) height, lens_radius, forcus_dis, 0, 1);
            var sun = new Sphere(new Vector3(-200, 10, 2), 4f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 2));
            world.list.Add(sun); //sun
            Important.list.Add(sun);
            var m4 = new Dielectirc(1.5f);
            var m2=new Metal(new ConstantTexture(Color32.White),0f );
            var m = new Metal(
                new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)),
                    new ConstantTexture(Color32.White)), 0.05f);
            //world.list.Add(new Translate(new Sphere(new Vector3(0, 0, 0), 0.5f,
            //    m2), new Vector3(0, 0, 0)));



            world.list.Add(new PlaneXZ(-5, 5, -5, 5, -0.5f,new Metal(new ImageTexture("CalibrationFloorDiffuse.png"), 0.2f)));
            world.list.Add(new Sphere(new Vector3(-2, 0, 0), 0.5f,
                new Metal(
                    new ConstantTexture(Color32.White), 0f)));
            world.list.Add(new Sphere(new Vector3(-1, 0, 0), 0.5f,
                new Metal(
                    new ConstantTexture(Color32.White), 0.25f)));
            world.list.Add(new Sphere(new Vector3(0, 0, 0), 0.5f,
                new Metal(
                    new ConstantTexture(Color32.White), 0.5f)));
            world.list.Add(new Sphere(new Vector3(1, 0, 0), 0.5f,
                new Metal(
                    new ConstantTexture(Color32.White), 0.75f)));
            world.list.Add(new Sphere(new Vector3(2, 0, 0), 0.5f,
                new Metal(
                    new ConstantTexture(Color32.White), 1)));

            //world.list.Add(new Translate(new Mesh(ObjModelLoader.ObjLoader.load("tea.obj"), m2).Create(),
            //    new Vector3(0, 0, 0)));
        }
        public Skybox sky;
        private void Demo()
        {
            var lens_radius = 0;
            var forcus_dis = 1;
            camera = new Camera(new Vector3(0f, 1f, 3f), new Vector3(0, 0, 0), new Vector3(0, 1, 0), 90, (float)width / (float)height, lens_radius, forcus_dis, 0, 1);
            var sun = new Sphere(new Vector3(-2, 10, 2), 1f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 1, 1, 1)), 2));
            world.list.Add(sun);//sun
            Important.list.Add(sun);
            world.list.Add(new PlaneXZ(-5, 5, -5, 5, -0.5f, new Metal(new ImageTexture("TexturesCom_MetalBare0181_16_seamless_S.jpg"), 0.2f)));
            world.list.Add(new Sphere(new Vector3(-2, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0f)));
            world.list.Add(new Sphere(new Vector3(-1, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.25f)));
            world.list.Add(new Sphere(new Vector3(0, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.5f)));
            world.list.Add(new Sphere(new Vector3(1, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 0.75f)));
            world.list.Add(new Sphere(new Vector3(2, 0, 0), 0.5f, new Metal(new CheckerTexture(new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)), new ConstantTexture(Color32.White)), 1)));
           // world.list.Add(new Translate(new Mesh(ObjModelLoader.ObjLoader.load("Model/barrel.obj"), new Metal(new ImageTexture("TexturesCom_MetalGalvanized0005_S.jpg"), 0.2f)).Create(), new Vector3(0, 0, -5)));
        }

        private void CornellBox()
        {
            SkyColor = false;
            camera = new Camera(new Vector3(278, 278, -800), new Vector3(278, 278, 0), new Vector3(0, 1, 0), 40, (float)width / (float)height);
            var list = new List<Hitable>();
            list.Add(new FilpNormals(new PlaneYZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.Blue)))));//green
            list.Add(new PlaneYZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.Red))));//red
            var lt = new PlaneXZ(213, 343, 227, 332, 554, new DiffuseLight(new ConstantTexture(new Color32(1f, 1,1f, 1)), 35));
            list.Add(lt);
            Important.list.Add(lt);
            list.Add(new FilpNormals(new PlaneXZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));
            list.Add(new PlaneXZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.White))));
            list.Add(new FilpNormals(new PlaneXY(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));

            var material = new Lambertian(new ConstantTexture(Color32.White));
            var glass=new Dielectirc(1.5f,new Color32(0,1,0));
            var glass2 = new Dielectirc(1.5f, new Color32(1, 0, 0));
            var glass3 = new Dielectirc(1.5f, new Color32(0, 0, 1));
            var cube1 = new Translate(new RotateY(
                    new Cube(
                        new Vector3(0, 0, 0),
                        new Vector3(165, 330, 165),
                        glass), 15)
                , new Vector3(265, 0, 295));
            var cube2 = new Translate(new RotateY(
                    new Cube(
                        new Vector3(0, 0, 0),
                        new Vector3(165, 165, 165),
                        material), -18),
                new Vector3(130, 0, 65));
            //list.Add(cube1);
            //list.Add(cube2);
            var model = new RotateY(new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/Bunny.ACM",
               new Subsurface(1.5f,Color32.White, 0.05f)
               
                , new Vector3(100)),new Vector3(-307.5f, 30, -277.5f)),180);
            list.Add(model);
            world.list.Add(new BVHNode(list.ToArray(), list.Count, 0, 1));

        }
        private void BDCornellBox()
        {
            SkyColor = false;
            camera = new Camera(new Vector3(278, 278, -800), new Vector3(278, 278, 0), new Vector3(0, 1, 0), 40, (float)width / (float)height);
            var list = new List<Hitable>();
            list.Add(new FilpNormals(new PlaneYZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.Blue)))));//green
            list.Add(new PlaneYZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.Red))));//red
            var lt = new PlaneXZ(213, 343, 227, 332, 554, new DiffuseLight(new ConstantTexture(new Color32(1f, 1, 1f, 1)), 35));
            list.Add(new PlaneXZ(213, 343, 227, 332, 540, new Lambertian(new ConstantTexture(new Color32(0f, 0, 0f)))));
            list.Add(lt);
            Important.list.Add(lt);
            list.Add(new FilpNormals(new PlaneXZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));
            list.Add(new PlaneXZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.White))));
            list.Add(new FilpNormals(new PlaneXY(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));

            var material = new Lambertian(new ConstantTexture(Color32.White));
            var glass = new Dielectirc(1.5f, new Color32(0, 1, 0));
            var glass2 = new Dielectirc(1.5f, new Color32(1, 0, 0));
            var glass3 = new Dielectirc(1.5f, new Color32(0, 0, 1));
            var cube1 = new Translate(new RotateY(
                    new Cube(
                        new Vector3(0, 0, 0),
                        new Vector3(165, 330, 165),
                        Shader.WhiteLambertion), 15)
                , new Vector3(265, 0, 295));
            var cube2 = new Translate(new RotateY(
                    new Cube(
                        new Vector3(0, 0, 0),
                        new Vector3(165, 165, 165),
                        Shader.WhiteLambertion), -18),
                new Vector3(130, 0, 65));
            list.Add(cube1);
            list.Add(cube2);//Important.list.Add(lt);
            //var model = new RotateY(new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/Bunny.ACM", Shader.Glass, new Vector3(100)), new Vector3(-307.5f, 30, -277.5f)), 180);
           // list.Add(model);
            world.list.Add(new BVHNode(list.ToArray(), list.Count, 0, 1));

        }
        private void CornellBox18()
        {
            SkyColor = false;
            var light = new DiffuseLight(new ConstantTexture(new Color32(1f, 1, 1f, 1)), 35);
            camera = new Camera(new Vector3(278, 278, -800), new Vector3(278, 278, 0), new Vector3(0, 1, 0), 40, (float)width / (float)height);
            var list = new List<Hitable>();
            list.Add(new FilpNormals(new PlaneYZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.Blue)))));//green
            list.Add(new PlaneYZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.Red))));//red
            var lt = new PlaneXZ(213, 343, 227, 332, 554,  light);
            list.Add(lt);
            Important.list.Add(lt);
            list.Add(new FilpNormals(new PlaneXZ(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));
            list.Add(new PlaneXZ(0, 555, 0, 555, 0, new Lambertian(new ConstantTexture(Color32.White))));
            list.Add(new FilpNormals(new PlaneXY(0, 555, 0, 555, 555, new Lambertian(new ConstantTexture(Color32.White)))));

            var material = new Lambertian(new ConstantTexture(Color32.White));
            var glass = new Dielectirc(1.5f, new Color32(0, 1, 0));
            var glass2 = new Dielectirc(1.5f, new Color32(1, 0, 0));
            var glass3 = new Dielectirc(1.5f, new Color32(0, 0, 1));
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
            //list.Add(cube1);
            //list.Add(cube2);
            //var a = new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/18.模型", glass),
            //   new Vector3(-0f, 0f, 0));
            var model = new RotateY(new Translate(ByteModel.Load(ResourceManager.ModelPath + "/ByteModel/FXM/18.模型",
                glass, new Vector3(-500,500,500)), new Vector3(-287.5f, 130, -277.5f)), 180);
            list.Add(model);
            world.list.Add(new BVHNode(list.ToArray(), list.Count, 0, 1));

        }
        private void MC()
        {
            var lens_radius = 0;
            var forcus_dis = 1;

            var sun = new Sphere(new Vector3(-10, 10, 10), 4f,
                new DiffuseLight(new ConstantTexture(new Color32(1, 0.9f, 0.7f, 1)), 500));
            world.list.Add(sun);//sun
            Important.list.Add(sun);
            camera = new Camera(new Vector3(-3, 1, -1), new Vector3(-5, 2, 8), new Vector3(0, 1, 0), 60, width / (float)height, lens_radius, forcus_dis, 0, 1);
            var lava = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/lava.png"), 4));
            var lamestone = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/glowstone.png"), 3));
            var grass = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Metal(new ImageTexture("MC/mycelium_top.png"),0.2f));
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
                new Lambertian(new ImageTexture("MC/hay_block_side.png", 1, 1)));
            var crafttable = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/crafting_table_top.png")), new Lambertian(new ImageTexture("MC/crafting_table_front.png")), new Lambertian(new ImageTexture("MC/crafting_table_front.png", 1, 1)));
            var book = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Lambertian(new ImageTexture("MC/planks_oak.png")), new Lambertian(new ImageTexture("MC/bookshelf.png")), new Lambertian(new ImageTexture("MC/bookshelf.png", 1, 1)));
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
                new Lambertian(new ImageTexture("MC/command_block_side.png", 1, 1)));

            var red = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/redstone_block.png"), 1));

            var lamp = new Cube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new DiffuseLight(new ImageTexture("MC/redstone_lamp_on.png"), 2));
            var list = new List<Hitable>();
            for (int i = 0; i > -7; i--)
                for (int j = 0; j < 10; j++)
                    list.Add(new Translate(new RotateY(grass, 0), new Vector3(i, -1, j)));

            //for (int i = 0; i > -7; i--)
            //for (int j = 0; j < 5; j++)
            //    list.Add(new Translate(new RotateY(cobblestone_mossy, 0), new Vector3(i, j, 0)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 1)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 2)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 3)));
            list.Add(new Translate(new RotateY(stonebrick_mossy, 90), new Vector3(0, 0, 4)));
            list.Add(new Translate(new RotateY(stonebrick_mossy, 90), new Vector3(0, 0, 5)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 6)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 7)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 8)));
            list.Add(new Translate(new RotateY(stonebrick, 90), new Vector3(0, 0, 9)));

            list.Add(new Translate(stonebrick, new Vector3(-1, 0, 9)));
            list.Add(new Translate(stonebrick, new Vector3(-2, 0, 9)));
            list.Add(new Translate(planks_oak, new Vector3(-3, 0, 9)));
            list.Add(new Translate(stonebrick_mossy, new Vector3(-4, 0, 9)));
            list.Add(new Translate(stonebrick, new Vector3(-5, 0, 9)));
            list.Add(new Translate(stonebrick, new Vector3(-6, 0, 9)));

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
            list.Add(new Translate(new RotateY(TNT, 90), new Vector3(-6, 0f, 3)));
            list.Add(new Translate(new RotateY(TNT, 90), new Vector3(-6, 0f, 2)));
            list.Add(new Translate(new RotateY(TNT, 90), new Vector3(-6, 1f, 2)));
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

            list.Add(new Translate(lamp, new Vector3(-5, -0.98f, 5)));

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
            list.Add(new Translate(planks_oak, new Vector3(-4, 1, 9)));
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
    }
}
