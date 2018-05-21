using ALight.Render.Mathematics;

namespace ALight.Render.Materials
{
    public class CubeMap
    {
        private readonly ImageTexture[] buffer=new ImageTexture[6];

        public CubeMap(params string[] files)
        {
           for (var i = 0; i < 6; i++) buffer[i] = new ImageTexture(files[i]);
        }

        public Color32 Value(Vector3 dir)
        {
            
            var index = GetFace(dir);
            var uv = GetUV(index, dir);
            return buffer[index].Value(uv.x, uv.y, Vector3.zero);
        }

        private Vector2 GetUV(int index, Vector3 dir)
        {

            //var t = index - (index > 2 ? 3 : 0);
            float u=0, v=0,factor;
            switch (index)
            {
                case 0: //前
                    factor = 1/ dir[0];
                    u = 1 + dir[2] * factor;
                    v = 1 + dir[1] * factor;
                    break;
                case 1://上
                    factor =1/ dir[1];
                    u = 1 + dir[2] * factor;
                    //u = 2 - u;
                    v = 1 + dir[0] * factor;
                    v = 2 - v;
                    break;
                case 2://右
                    factor = 1/ dir[2];
                    u = 1 + dir[0] * factor;
                    u = 2 - u;
                    v = 1 + dir[1] * factor;
                    break;

                case 3: //后
                    factor = 1f / dir[0];
                    u = 1 + dir[2] * factor;
                    v = 1 + dir[1] * factor;
                    v = 2 - v;
                    break;
                case 4: //底
                    factor = 1/ dir[1];
                    u = 1 + dir[2] * factor;
                    u = 2 - u;
                    v = 1 + dir[0] * factor;
                    v = 2 - v;
                    break;
                case 5: //左
                    factor = 1f / dir[2];
                    u = 1 + dir[0] * factor;
                    u = 2 - u;
                    v = 1 + dir[1] * factor;
                    v = 2 - v;
                    break;
            }
            return new Vector2(u/2,v/2);
        }

        private static int GetFace(Vector3 dir)
        {
            var MAX = 0;
            for (var i = 1; i < 3; i++)if (Mathf.Abs(dir[i]) > Mathf.Abs(dir[MAX])) MAX = i;
            return MAX+(dir[MAX] < 0 ? 3 : 0);
        }

    }
}
