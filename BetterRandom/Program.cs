using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterRandom
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 255; i++)
            {
                Console.WriteLine(Get());
            }

            Console.ReadLine();
        }

        static long seed = 1;
        static float Get()
        {
            seed = (0x5DEECE66DL * seed + 0xB16) & 0xFFFFFFFFFFFFL;
            var x = seed >> 16;
            return (x / (float) 0x100000000L);
        }
    }
}

   

