namespace ALightRaster.Render.Components
{
    public class Component
    {
        public GameObject gameObject;
        public Transform transform=>gameObject.transform;
        public virtual void Update() { }
        public virtual void Start() { }

        public Component(bool tf)
        {
            gameObject=new GameObject();
            gameObject.components.Add(this);
        }
        public Component() { }

        public void Link(GameObject game_object)
        {
            gameObject = game_object;
            game_object.components.Add(this);
        }
    }
}
