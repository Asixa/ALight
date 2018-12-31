using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ALightRaster.DotNetCore.Render.Materials;
using ALightRaster.DotNetCore.Render.Materials.Shaders;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.Render;
using ALightRaster.Render.Components;
using ALightRaster.Render.Structure;
using ALightRaster.Scripts;

namespace ALightRealtime.Render
{
    public class Scene
    {
        public static Scene current;
        public static Light Sun;
        public List<GameObject> gameObjects = new List<GameObject>();

        public static void Init()
        {
            current = new Scene();
            //current.TestScene();
            current.TestScene();
        }

        public void Cube()
        {
            var texture = new ImageTexture(AppContext.BaseDirectory + "/Resources/Images/tex.jpg");
            var camera = GameObject.Create(new Vector3(0, 0, 0), new Vector3(30, 0, 0));
            camera.AddComponent(new Camera((float) Math.PI / 4, Canvas.Width / (float) Canvas.Height, 1f, 500f));
            var game_object2 = new GameObject
            {
                renderer = new MeshRenderer
                {
                    mesh_filter = new MeshFilter
                    {
                        mesh = new Mesh(Model.Cube)
                    },
                    material = new Material(new Unlit.RawTexture(), texture)
                    //new Material(new Phong(), Color32.Blue)
                },
                transform = new Transform(new Vector3(0f, -1.5f, 2.5f), new Vector3(0, 0, 0)),
            };
            game_object2.AddComponent(new AutoRotate(30));

        }

        public void TestScene()
        {
            var camera = GameObject.Create(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            camera.AddComponent(new Camera((float) Math.PI / 4, Canvas.Width / (float) Canvas.Height, 1f, 500f));

            var texture = new ImageTexture(AppContext.BaseDirectory + "/Resources/Images/tex.jpg");
//            var game_object2 = new GameObject
//            {
//                renderer = new MeshRenderer
//                {
//                    mesh_filter = new MeshFilter
//                    {
//                        mesh = new Mesh(Model.Cube)
//                    },material = new Material(new Gouraud_Image(),texture)
//                        //new Material(new Phong(), Color32.Blue)
//                },
//                transform = new Transform(new Vector3(0f, -0.5f, 5), new Vector3(0, 45, 0)),
//            };
            //game_object2.AddComponent(new AutoRotate(30));

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var plane = new GameObject
                    {
                        renderer = new MeshRenderer
                        {
                            mesh_filter = new MeshFilter
                            {
                                mesh = new Mesh(Model.Plane)
                            },
                            material = new Material(new Unlit.RawTexture(), texture)

                        },
                        transform = new Transform(new Vector3(-4 + 4f * i, -1f, 5 + 4f * j), new Vector3(180, 0, 0)),
                    };
                }
            }

            //game_object.AddComponent(new AutoRotate(30));

            var _sun = GameObject.Create(new Vector3(0, 0, 0), new Vector3(-1, -1, -1));
            Sun = (Light) _sun.AddComponent(new Light
            {
                color = Color32.White, intensity = 2, type = LightType.Directional
            });
            //_sun.AddComponent(new AutoRotate(3));
        }

        public void TestScene2()
        {
            var camera = GameObject.Create(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            camera.AddComponent(new Camera((float) Math.PI / 4, Canvas.Width / (float) Canvas.Height, 1f, 500f));

            var texture = new ImageTexture(AppContext.BaseDirectory + "/Resources/Images/pink512.jpg");
            var game_object2 = new GameObject
            {
                renderer = new MeshRenderer
                {
                    mesh_filter = new MeshFilter
                    {
                        mesh = new Mesh(Model.Cube)
                    },
                    material = new Material(new Unlit.RawTexture(), texture)
                    //new Material(new Phong(), Color32.Blue)
                },
                transform = new Transform(new Vector3(0f, -0.5f, 5), new Vector3(0, 45, 0)),
            };
            //game_object2.AddComponent(new AutoRotate(30));


            var plane = new GameObject
            {
                renderer = new MeshRenderer
                {
                    mesh_filter = new MeshFilter
                    {
                        mesh = new Mesh(Model.Plane)
                    },
                    material = new Material(new Unlit.RawTexture(), texture)

                },
                transform = new Transform(new Vector3(0, -1f, 5), new Vector3(180, 45, 0)),
            };


            //plane.AddComponent(new AutoRotate(30));

            var _sun = GameObject.Create(new Vector3(0, 0, 0), new Vector3(-1, -1, -1));
            Sun = (Light) _sun.AddComponent(new Light
            {
                color = Color32.White,
                intensity = 2,
                type = LightType.Directional
            });
            //_sun.AddComponent(new AutoRotate(3));
        }

        public void UpdateScripts()
        {
            foreach (var obj in gameObjects)
            foreach (var com in obj.components)
                com.Update();
        }

        public void StartScripts()
        {
            foreach (var obj in gameObjects)
            foreach (var com in obj.components)
                com.Update();
        }
    }
}
