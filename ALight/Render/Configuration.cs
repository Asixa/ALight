
namespace ALight.Render
{
    public static class Configuration
    {
        public static Mode mode = Mode.Shaded;
        public static int scale =1;
        //public static int width=1024/scale, height= 1024/ scale;
        public static int width=1920/scale, height= 1080/ scale;
        public static int SPP=128, MAX_SCATTER_TIME=8,MAX_SCATTER_TIME_LIGHT=4;

        public static int divide_w,divide_h = 2;
        public static int block_size = 64;
    }
}
