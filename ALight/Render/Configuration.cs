
namespace ALight.Render
{
    public static class Configuration
    {
        public static Mode mode = Mode.Diffusing;
        public static int scale =1;
        public static int width=1024/scale, height= 1024/ scale;
        public static int SPP=8, MAX_SCATTER_TIME=16;
        public static int divide_w,divide_h = 8;
        public static int block_size = 64;
    }
}
