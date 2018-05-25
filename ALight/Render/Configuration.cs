namespace ALight.Render
{
    public class Configuration
    {
        public const Mode mode = Mode.Diffusing;
        public static Configuration main;
        public static int width=1920, height=1080;
        public static int SPP=64, MAX_SCATTER_TIME=16;
        public static int divide = 8;
        public static  int SamplePerThread = 8;
    }
}
