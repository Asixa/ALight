using AcDx;
using ALight.Render.Mathematics;

namespace ALight.Render
{
    public class Preview : DxWindow
    {
        public override void Update()
        {

            for (var i = 0; i < Buff.Length; i++)
            {
                Buff[i] = (byte) Mathf.Range(Renderer.main.buff[i] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);
            }
        }
    }
}
