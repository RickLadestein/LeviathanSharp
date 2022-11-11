using Assimp.Configs;
using Assimp;
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

            ImportAssimpMeshes(path, out List<Assimp.Mesh> loadedMeshes, out List<Assimp.Material> loadedMaterials);

            MaterialLoader matloader = new MaterialLoader();
            matloader.ImportMaterialsFromAssimpMaterial(loadedMaterials);
            matloader.PushToRenderContext();

            foreach(Graphics.Material material in matloader.materials)
            {
                Graphics.Material.ImportTexturesSynchronous(material);
            }

            MeshLoader meshloader = new MeshLoader();
            meshloader.ImportMeshesFromScene(loadedMeshes);
            meshloader.PushToRenderContext();


            for(int i = 0; i < meshloader.Meshes.Count; i++)
            {
                Graphics.Mesh m = meshloader.Meshes[i];
                Entity entity = new Entity();
                entity.Name = m.Name;

                int mat_index = meshloader.material_bindings[i];
                MaterialComponent matcomp = new MaterialComponent();
                matcomp.SetShader("default");
                matcomp.SetMaterial(matloader.materials[mat_index].Name);
                entity.AddComponent(matcomp);

                MeshComponent mcomp = new MeshComponent();
                mcomp.SetMesh(m.Name);
                entity.AddComponent(mcomp);
                
                RenderComponent rcomp = new RenderComponent();
                entity.AddComponent(rcomp);
                
                output.Entities.Add(entity);
            }
            meshloader.Dispose();
            return output;
        }

        private static void ImportAssimpMeshes(string file_path, out List<Assimp.Mesh> meshes, out List<Assimp.Material> materials)
        {
            meshes = new List<Assimp.Mesh>();
            materials = new List<Assimp.Material>();

            Assimp.AssimpContext ac = new AssimpContext();
            ac.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            Assimp.Scene scene = ac.ImportFile(file_path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);
            meshes.AddRange(scene.Meshes);
            materials.AddRange(scene.Materials);
            ac.Dispose();
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
