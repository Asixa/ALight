using System;


namespace ALight.Render.Math
{
    public static class Random
    {
        private static readonly System.Random random = new System.Random();

        public static float Range(float f, float t)
        {
            lock (random)
            {
                return random.Next((int) (f * 10000), (int) (t * 10000)) / 10000f;
            }
        }
        //public static float Range(float f, float t)=>new System.Random(Guid.NewGuid().GetHashCode()).Next((int) (f * 10000), (int) (t * 10000)) / 10000f;
        
    }
}
