using System;
using System.Collections.Generic;
using System.IO;
using ALight.Render.Components;
using ALight.Render.Instances;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using ALight.Render.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using  static ALight.Render.Resource.ResourceManager;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render
{
    public struct LightSource
    {
        public static List<LightSource> lights=new List<LightSource>();
        public Hitable obj;
        public Vector3 point;

        public LightSource(Hitable h, Vector3 p)
        {
            obj = h;
            point = p;
            lights.Add(this);
        }

        public static LightSource GetRandom() => lights[(int) (Random.Get() * (lights.Count - 0.1f))];
        public static Ray GetRandomRay()
        {
            var light = GetRandom();
            return new Ray(light.point,light.obj.Random(light.point));
        }

    }
    public class Scene
    {
        public Skybox sky;
        public readonly HitableList world = new HitableList(), Important = new HitableList();
        public bool Skybox = true;
        public static Scene main;
        public Camera camera;
        public int width => Configuration.width;
        public int height => Configuration.height;

        public Scene(string path)
        {
            main = this;
            //"D:\\Codes\\Projects\\Academic\\ComputerGraphic\\ALight\\Resources\\Scenes\\CubeMaterial\\Davis.json"
            LoadScene(path);
        }

        public void LoadScene(string path)
        {
            Console.WriteLine("Loading Scene");
            var root=Path.GetDirectoryName(path);
            var json =(JObject)JsonConvert.DeserializeObject(File.ReadAllText(path));

            //Camera
            var camera_data = (JObject)JsonConvert.DeserializeObject(json["Camera"].ToString());
            camera = new Camera(
                Vector3.FromList(JsonConvert.DeserializeObject<List<float>>(camera_data["lookFrom"].ToString())),
                Vector3.FromList(JsonConvert.DeserializeObject<List<float>>(camera_data["lookAt"].ToString())),
                new Vector3(0, 1, 0),
                JsonToF(float.Parse(camera_data["FOV"].ToString())),
                width / (float)height,
                JsonToF(float.Parse(camera_data["radius"].ToString())),
                JsonToF(float.Parse(camera_data["focus"].ToString())),
                JsonToF(float.Parse(camera_data["shutter"].ToString())));
            //Sky
            Skybox = json.ContainsKey("Sky");
            if (Skybox)
            {
                var skydata = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(root + "/" +json["Sky"]));
                var datas = JsonConvert.DeserializeObject<List<string>>(skydata["data"].ToString());
                var r = Path.GetDirectoryName(root + "/" + json["Sky"])+"/";
                sky = new CubeMap(r + datas[0], r + datas[1], r + datas[2], r + datas[3], r + datas[4], r + datas[5]);
            }
            //Material
            var shaders=new Dictionary<string, Shader>();

            var material_datas = (JObject)JsonConvert.DeserializeObject(json["Materials"].ToString());
            foreach (var pair in material_datas)
            {
                Shader shader = null;
                var materjson = (JObject) JsonConvert.DeserializeObject(File.ReadAllText(root+"/"+pair.Value));
                var datas = JsonConvert.DeserializeObject<List<string>>(materjson["data"].ToString());
                var material_root = Path.GetDirectoryName(root + "/" + pair.Value);
                switch (materjson["type"].ToString())
                {
                    case "standard":
                        Texture reflect = null;
                        if (float.TryParse(datas[2],out var r))
                            reflect = new GrayTexture(r);
                        else reflect = new ImageTexture(material_root + "/" + datas[2]);
                        shader = new StardardShader(
                            new ImageTexture(material_root + "/" + datas[0]),
                            new ImageTexture(material_root + "/" + datas[1]),
                            reflect,datas[3].ToString()==""?null:new ImageTexture(material_root + "/" + datas[3])
                        );
                        break;
                    case "metal":
                        shader = new Metal(
                            new ImageTexture(material_root + "/" + datas[0]),
                            float.Parse(datas[1]));
                        break;
                    case "dielectirc":
                        shader = datas.Count==1 
                            ? new Dielectirc(float.Parse(datas[0])) 
                            : new Dielectirc(float.Parse(datas[0]),new Color32(int.Parse(datas[1]) / 255f, int.Parse(datas[2]) / 255f, int.Parse(datas[3]) / 255f));
                        break;
                    case "Lambertian":
                        shader=new Lambertian(new ImageTexture(material_root + "/" + datas[0]));
                        break;
                    case "light":
                        shader = datas.Count == 2
                            ? new DiffuseLight(new ImageTexture(material_root + "/" + datas[1]), float.Parse(datas[0]))
                            :new DiffuseLight(new ConstantTexture(new Color32(int.Parse(datas[1]) / 255f, int.Parse(datas[2]) / 255f, int.Parse(datas[3]) / 255f)), float.Parse(datas[0]));
                        break;
                }
                shaders.Add(pair.Key.ToString(),shader);
            }

            Shader[] GetModelShaderList(List<string> name)
            {
                var result = new Shader[name.Count];
                for (var i = 0; i < name.Count; i++) result[i] = shaders[name[i]];
                return result;
            }
            //Objects
            var obj_datas = (JObject)JsonConvert.DeserializeObject(json["Objects"].ToString());
            var objlist = new List<Hitable>();
            foreach (var pair in obj_datas)
            {
                var obj = (JObject)JsonConvert.DeserializeObject(pair.Value.ToString());
                Hitable a = null;
               
                switch (obj["type"].ToString())
                {
                    case "model":
                        var scale = JsonToV3(obj["scale"]);
                        var pos = JsonToV3(obj["position"]);
                        var rot = JsonToV3(obj["rotation"]);
                        var m = new Translate(new RotateY(ByteModel.Load(root + "/" + obj["file"],
                            GetModelShaderList(JsonConvert.DeserializeObject<List<string>>(obj["material"].ToString())),
                            new Vector3(scale.x, scale.y, scale.z)),rot.y),pos);
                        objlist.Add(m);
                        if (bool.Parse(obj["important"].ToString())) Important.list.Add(a);
                        break;
                    case "Oldmodel":
                        var scale_0 = JsonToV3(obj["scale"]);
                        var pos_0= JsonToV3(obj["position"]);
                        var rot_0= JsonToV3(obj["rotation"]);
                        var _m = new Translate(new RotateY(ByteModel.LoadOld(root + "/" + obj["file"],
                            GetModelShaderList(JsonConvert.DeserializeObject<List<string>>(obj["material"].ToString()))[0],
                            new Vector3(scale_0.x, scale_0.y, scale_0.z)),rot_0.y),pos_0);
                        objlist.Add(_m);
                        if (bool.Parse(obj["important"].ToString())) Important.list.Add(a);
                        break;
                    case "cube":
                        a = new Translate(
                            new Mesh(Instances.Model.Cube, 
                                shaders[
                                    JsonConvert.DeserializeObject<List<string>>(obj["material"].ToString())[0]
                                ]).Create(),
                            JsonToV3(obj["position"]));
                        objlist.Add(a);
                        if(bool.Parse(obj["important"].ToString()))Important.list.Add(a);
                        break;
                    case "sphere":
                        a = new Sphere(JsonToV3(obj["position"]), float.Parse(obj["radius"].ToString()),
                            shaders[
                                JsonConvert.DeserializeObject<List<string>>(obj["material"].ToString())[0]
                            ]);
                        objlist.Add(a);
                        if (bool.Parse(obj["important"].ToString())) Important.list.Add(a);
                        break;
                }
            }
            world.list.Add(new RotateY(new BVHNode(objlist.ToArray(),objlist.Count, 0, 1), 0));
            PreviewWindow.main.Loading.Visible = false;
        }
    }
}
