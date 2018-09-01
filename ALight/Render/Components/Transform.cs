using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Mathematics;

namespace ALight.Render.Components
{
    public class Transform:Component
    {
        public Vector3 position, scale, rotation;
        public GameObject gameobject;
    }
}
