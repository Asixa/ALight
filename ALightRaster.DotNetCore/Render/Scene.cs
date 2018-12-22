using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ALightRaster.Render;
using ALightRaster.Render.Components;
using ALightRaster.Render.Structure;
using ALightRaster.Scripts;
using ALightRealtime.Render.Structure;

namespace ALightRealtime.Render
{
    public class Scene
    {
        public static Scene current;
        public List<GameObject> gameObjects=new List<GameObject>();

        public static void Init()
        {
            current=new Scene();
            current.TestScene();
        }

        public void TestScene()
        {
            var camera = GameObject.Create(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            camera.AddComponent(new Camera((float)Math.PI / 4, Canvas.Width / (float)Canvas.Height,  1f,500f));
            var game_object2 = new GameObject
            {
                renderer = new MeshRenderer
                {
                    mesh_filter = new MeshFilter
                    {
                        mesh = new Mesh(Model.CubeBig)
                    }
                },
                transform = new Transform(new Vector3(-0.5f, 0, 5), new Vector3(0, 0, 0)),
            };


            var game_object = new GameObject
            {
                renderer = new MeshRenderer
                {
                    mesh_filter = new MeshFilter
                    {
                        mesh = new Mesh(Model.CubeBig)
                    }
                },
                transform = new Transform(position:new Vector3(0.5f, 0,4.9f),rotation:new Vector3(0,0,0)),
            };

            game_object.AddComponent(new AutoRotate(2));
            game_object2.AddComponent(new AutoRotate(-2));
        }

        public void UpdateScripts()
        {
            foreach (var obj in gameObjects)foreach (var com in obj.components)com.Update();
        }
        public void StartScripts()
        {
            foreach (var obj in gameObjects) foreach (var com in obj.components) com.Update();
        }
    }
}
