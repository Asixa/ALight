using System;
using System.Collections.Generic;
using System.Text;
using static ALMicro;
// ReSharper disable InconsistentNaming
public enum ALMicro
{
    AL_ARRAY_BUFFER,

}
namespace ALightRaster.DotNetCore.Render
{
    public class AL
    {


        public float[] VAO,VBO;


        public void AlBufferData(ALMicro type, int size,float[] buffer,ALMicro DataChange)
        {
            switch (type)
            {
                case AL_ARRAY_BUFFER: VBO = buffer;return;
            }
        }

        public void AlVertexAttribPointer(int pointer, int strip, int total_strip)
        {

        }
    }
}
