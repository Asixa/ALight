using System;
using ALight.Render.Mathematics;

namespace ALight.Render.Materials
{

    public abstract class Skybox
    {
        public abstract Color32 Value(Vector3 dir);
    }
    public class CubeMap:Skybox
    {
        public const int FACE_FRONT = 0, FACE_BACK = 1, FACE_LEFT = 2, FACE_RIGHT = 3, FACE_UP = 4, FACE_DOWN = 5;
        private readonly ImageTexture[] buffer=new ImageTexture[6];

        public CubeMap(params string[] files)
        {
            if (files.Length == 1) CreateFromPanorama(files[0]);
            else for (var i = 0; i < 6; i++) buffer[i] = new ImageTexture(files[i]);
        }

        public void CreateFromPanorama(string file)
        {
            int texSize = 1024;
            var src = new ImageTexture(file);
            ImageTexture Create(int faceIndex)
            {
                var data = new Byte[texSize * texSize*3];

                void SetPixel(int u, int v, Color32 color)
                {
                    var p = texSize * 3 * v + u * 3;
                    data[p] = (byte) (color.r * 255 + 0.5f);
                    data[p+ 1] = (byte) (color.g * 255 + 0.5f);
                    data[p + 2] = (byte) (color.b * 255 + 0.5f);
                }

                Vector3[] vDirA = new Vector3[4];
                if (faceIndex == FACE_FRONT)
                {
                    vDirA[0] = new Vector3(-1.0f, -1.0f, -1.0f);
                    vDirA[1] = new Vector3(1.0f, -1.0f, -1.0f);
                    vDirA[2] = new Vector3(-1.0f, 1.0f, -1.0f);
                    vDirA[3] = new Vector3(1.0f, 1.0f, -1.0f);
                }
                else if (faceIndex == FACE_BACK)
                {
                    vDirA[0] = new Vector3(1.0f, -1.0f, 1.0f);
                    vDirA[1] = new Vector3(-1.0f, -1.0f, 1.0f);
                    vDirA[2] = new Vector3(1.0f, 1.0f, 1.0f);
                    vDirA[3] = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else if (faceIndex == FACE_LEFT)
                {
                    vDirA[0] = new Vector3(1.0f, -1.0f, -1.0f);
                    vDirA[1] = new Vector3(1.0f, -1.0f, 1.0f);
                    vDirA[2] = new Vector3(1.0f, 1.0f, -1.0f);
                    vDirA[3] = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (faceIndex == FACE_RIGHT)
                {
                    vDirA[0] = new Vector3(-1.0f, -1.0f, 1.0f);
                    vDirA[1] = new Vector3(-1.0f, -1.0f, -1.0f);
                    vDirA[2] = new Vector3(-1.0f, 1.0f, 1.0f);
                    vDirA[3] = new Vector3(-1.0f, 1.0f, -1.0f);
                }
                else if (faceIndex == FACE_UP)
                {
                    vDirA[0] = new Vector3(-1.0f, 1.0f, -1.0f);
                    vDirA[1] = new Vector3(1.0f, 1.0f, -1.0f);
                    vDirA[2] = new Vector3(-1.0f, 1.0f, 1.0f);
                    vDirA[3] = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (faceIndex == FACE_DOWN)
                {
                    vDirA[0] = new Vector3(-1.0f, -1.0f, 1.0f);
                    vDirA[1] = new Vector3(1.0f, -1.0f, 1.0f);
                    vDirA[2] = new Vector3(-1.0f, -1.0f, -1.0f);
                    vDirA[3] = new Vector3(1.0f, -1.0f, -1.0f);
                }

                Vector3 rotDX1 = (vDirA[1] - vDirA[0]) / (float) texSize;
                Vector3 rotDX2 = (vDirA[3] - vDirA[2]) / (float) texSize;

                float dy = 1.0f / (float) texSize;
                float fy = 0.0f;


                for (int y = 0; y < texSize; y++)
                {
                    Vector3 xv1 = vDirA[0];
                    Vector3 xv2 = vDirA[2];
                    for (int x = 0; x < texSize; x++)
                    {
                        Vector3 v = ((xv2 - xv1) * fy) + xv1;
                        v = v.Normalized();
                        SetPixel(x, y, Spherical(v));
                        xv1 += rotDX1;
                        xv2 += rotDX2;
                    }
                    fy += dy;
                }
                return new ImageTexture(data, texSize, texSize);
            }
            Color32 Spherical(Vector3 vDir)
            {

                float theta = Mathf.Atan2(vDir.z, vDir.x);
                float phi = Mathf.Acos(vDir.y);
                var m_direction = 0;
                theta += m_direction * Mathf.PI / 180.0f;
                while (theta < -Mathf.PI) theta += Mathf.PI + Mathf.PI;
                while (theta > Mathf.PI) theta -= Mathf.PI + Mathf.PI;

                float dx = theta / Mathf.PI;
                float dy = phi / Mathf.PI;

                dx = dx * 0.5f + 0.5f;
                int px = (int) (dx * src.w);
                if (px < 0) px = 0;
                if (px >= src.w) px = src.w - 1;
                int py = (int) (dy * src.w);
                if (py < 0) py = 0;
                if (py >= src.w) py = src.w - 1;
               //Console.WriteLine(px+"-"+ (src.w - py - 1)+"--"+src.w+"-"+py);
                Color32 col = src.GetPixel(px, src.w - py - 1);
                return col;
            }
            buffer[0] = Create(FACE_FRONT);
            buffer[1] = Create(FACE_UP);
            buffer[2] = Create(FACE_LEFT);
            buffer[3] = Create(FACE_BACK);
            buffer[4] = Create(FACE_DOWN);
            buffer[5] = Create(FACE_RIGHT);

            for (int i = 0; i < 6; i++) buffer[i].Save("sky"+i);
        }

        public override Color32 Value(Vector3 dir)
        {
            var index = GetFace(dir);
            var uv = GetUV(index, dir);
            return buffer[index].Value(uv.x, uv.y, Vector3.zero);
        }

        private Vector2 GetUV(int index, Vector3 dir)
        {
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

    public class Panorama : Skybox
    {
        public ImageTexture src;
        public Panorama(string file)
        {
            src=new ImageTexture(file);
        }

        public override Color32 Value(Vector3 dir)
        {
            float theta = Mathf.Atan2(dir.z, dir.x);
            float phi = Mathf.Acos(dir.y);
            var m_direction = 0;
            theta += m_direction * Mathf.PI / 180.0f;
            while (theta < -Mathf.PI) theta += Mathf.PI + Mathf.PI;
            while (theta > Mathf.PI) theta -= Mathf.PI + Mathf.PI;

            float dx = theta / Mathf.PI;
            float dy = phi / Mathf.PI;

            dx = dx * 0.5f + 0.5f;
            int px = (int) (dx * src.w);
            if (px < 0) px = 0;
            if (px >= src.w) px = src.w - 1;
            int py = (int) (dy * src.w);
            if (py < 0) py = 0;
            if (py >= src.w) py = src.w - 1;

            Color32 col = src.Value(px, src.w - py - 1, Vector3.zero);
            return col;
        }
    }
}
