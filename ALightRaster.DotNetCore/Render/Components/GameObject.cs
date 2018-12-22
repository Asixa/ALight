using System.Collections.Generic;
using System.Numerics;
using ALightRealtime.Render;

namespace ALightRaster.Render.Components
{
    public class GameObject
    {
        public MeshRenderer renderer;
        public Transform transform;
        public string tag;
        public List<Component> components=new List<Component>();
        public GameObject() => Scene.current.gameObjects.Add(this);
        public void AddComponent(Component component) => component.Link(this);

        public static GameObject Create(Vector3 pos, Vector3 rot,MeshRenderer renderer=null)
        {
            return new GameObject
            {
                transform = new Transform(pos,rot),
                renderer = renderer
            };
        }
    }
}
