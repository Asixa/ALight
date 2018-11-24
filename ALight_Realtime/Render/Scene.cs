using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALightRealtime.Render.Components;

namespace ALightRealtime.Render
{
    public class Scene
    {
        public Camera MainCamera;
        public static Scene current;
        public List<GameObject> gameObjects=new List<GameObject>();


    }
}
