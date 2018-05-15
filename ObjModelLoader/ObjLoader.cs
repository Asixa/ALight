using System.IO;
using System.Text;

namespace ObjModelLoader
{
    public class ObjLoader
    {
        public static ObjMesh load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();
            if (!path.EndsWith(".obj"))
                throw new FileNotFoundException();

            var reader = new StreamReader(path, Encoding.Default);
            var content = reader.ReadToEnd();
            reader.Close();

            return new ObjMesh().LoadFromObj(content); ;
        }
    }
}
