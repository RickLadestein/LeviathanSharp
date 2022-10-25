using Leviathan.Core.Graphics;
using Leviathan.ECS;
using Leviathan.ECS.BasicScripts;
using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Leviathan.Core
{
    public class Scene
    {
        public List<Entity> Entities { get; private set; }
        public Scene()
        {
            Entities = new List<Entity>();
        }

        public void SaveScene()
        {

        }

        public static Scene LoadSceneFromObj(string path)
        {
            Scene output = new Scene();
            
            MeshLoader loader = new MeshLoader();
            loader.Import(path, LPrimitiveType.TRIANGLES);
            loader.PushToRenderContext();

            foreach(Tuple<Mesh, Material> m in loader.materialbindings)
            {
                Entity entity = new Entity();
                entity.Name = m.Item1.Name;

                MaterialComponent matcomp = new MaterialComponent();
                matcomp.SetShader("default");
                matcomp.SetMaterial(m.Item2.Name);
                entity.AddComponent(matcomp);

                MeshComponent mcomp = new MeshComponent();
                mcomp.SetMesh(m.Item1.Name);
                entity.AddComponent(mcomp);
                
                RenderComponent rcomp = new RenderComponent();
                entity.AddComponent(rcomp);
                
                output.Entities.Add(entity);
            }

            return output;
        }

        public static Scene DefaultScene
        {
            get
            {
                Scene s = new Scene();

                Entity camera = new Entity("camera");
                
                CameraComponent ccomp = new CameraComponent();
                ccomp.Primary = true;
                camera.AddComponent(ccomp);
                //camera.GetComponent<CameraComponent>().Primary = true;
                camera.Transform.LocalPosition = Vector3f.Zero;

                Entity body = new Entity("body");
                body.Transform.LocalPosition = new Vector3f(0, 1, 0);
                BasicPlayerScript bps = new BasicPlayerScript();
                bps.camera = camera;
                body.AddScript(bps);
                body.AddChild(camera);
                s.Entities.Add(body);


                Entity ground = new Entity("groundplane");
                ground.Transform.LocalPosition = Vector3f.Zero;
                ground.Transform.LocalScale = new Vector3f(10, 1, 10);
                MeshComponent mcomp = new MeshComponent();
                mcomp.SetMesh("Cube");
                RenderComponent rcomp = new RenderComponent();
                MaterialComponent matcomp = new MaterialComponent();
                matcomp.SetShader("default");
                ground.AddComponent(mcomp);
                ground.AddComponent(rcomp);
                ground.GetComponent<MaterialComponent>().SetShader("default");
                s.Entities.Add(ground);
                return s;
            }
        }
    }
}
