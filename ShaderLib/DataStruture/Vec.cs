using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderLib.DataStruture
{
    public class Vec<T>
    {
        public T[] datas;
        public Vec(int length,params T[] init)=>datas = init;

        public T X => datas[0];
        public T Y => datas[1];
        public T Z => datas[2];
        public T W => datas[3];

        public T R => datas[0];
        public T G => datas[1];
        public T B => datas[2];
        public T A => datas[3];
    }

    public class Vec2<T> : Vec<T>
    {
        public Vec2(params T[] init) : base(2,init) { }
    }
    public class Vec3<T> : Vec<T>
    {
        public Vec3(params T[] init) : base(3, init) { }
    }
    public class Vec4<T> : Vec<T>
    {
        public Vec4(params T[] init) : base(4, init) { }
    }

    public class Mat
    {
        public Mat()
        {

        }
    }


    public class Test
    {
        public Test()
        {

        }
        public void Tests()
        {
            var a =new Vec<byte>(1,2,3,4,5);
            
            
        }
    }

}
