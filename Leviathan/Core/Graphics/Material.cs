using Leviathan.Math;
using Silk.NET.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Assimp;
using System.Runtime.CompilerServices;
using System.IO;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Leviathan.Util.util;
using Leviathan.ECS.Systems;

namespace Leviathan.Core.Graphics
{
    public class Material
    {
        [JsonProperty("newmtl")]
        public string Name;

        /// <summary>
        /// Specifies the specular reflectivity using RGB values
        /// </summary>
        [JsonProperty("Ka")]
        public Vector4f ColorAmbient;

        /// <summary>
        /// Specifies the specular diffuse reflectivity using RGB values
        /// </summary>
        [JsonProperty("Kd")]
        public Vector4f ColorDiffuse;

        /// <summary>
        /// Specifies the specular reflectivity using RGB values.
        /// </summary>
        [JsonProperty("Ks")]
        public Vector4f ColorSpecular;

        [JsonProperty("Ke")]
        public Vector4f ColorEmissive;

        /// <summary>
        /// Specifies the specular exponent for the current material.  This defines 
        /// the focus of the specular highlight (Roughness, Glossyness).
        /// </summary>
        [JsonProperty("Ns")]
        public float Shininess;

        /// <summary>
        /// Specifies the optical density for the surface.  This is also known as index of refraction.
        /// </summary>
        [JsonProperty("Ni")]
        public float RefractionIndex;

        /// <summary>
        /// Specifies the transperency of the object that is observed
        /// </summary>
        [JsonProperty("d")]
        public float Transperency;

        [JsonProperty("illum")]
        public uint illumination_mode;

        /// <summary>
        /// Specifies the sharpness of the reflections from the local reflection map.
        /// </summary>
        [JsonProperty("sharpness")]
        public float Sharpness;

        [JsonProperty("maps")]
        public List<Tuple<Assimp.TextureType, TextureSlot>> maps;


        public static Material Default
        {
            get
            {
                return new Material();
            }
        }
        
        public Material()
        {
            Name = "Default";
            ColorAmbient = Vector4f.Zero;
            ColorDiffuse = Vector4f.Zero;
            ColorSpecular = Vector4f.Zero;
            ColorEmissive = Vector4f.Zero;
            Shininess = 1.0f;
            RefractionIndex = 1.0f;
            Transperency = 0.0f;
            Sharpness = 1.0f;
            maps = new List<Tuple<Assimp.TextureType, TextureSlot>>();
        }

        public static Material FromAssimpMaterial(Assimp.Material a_material)
        {
            Material output = new()
            {
                Name = a_material.Name,
                ColorAmbient = FromAssimpColor(a_material.ColorAmbient),
                ColorDiffuse = FromAssimpColor(a_material.ColorDiffuse),
                ColorSpecular = FromAssimpColor(a_material.ColorSpecular),
                ColorEmissive = FromAssimpColor(a_material.ColorEmissive),
                Shininess = a_material.ShininessStrength,
                Sharpness = a_material.Reflectivity,
                Transperency = a_material.Opacity
            };

            if (a_material.HasTextureAmbient)
            {
                output.maps.Add(new Tuple<Assimp.TextureType, TextureSlot>(Assimp.TextureType.Ambient, a_material.TextureAmbient));
            } else
            {
                output.maps.Add(null);
            }

            if (a_material.HasTextureDiffuse)
            {
                output.maps.Add(new Tuple<Assimp.TextureType, TextureSlot>(Assimp.TextureType.Diffuse, a_material.TextureDiffuse));
            }
            else
            {
                output.maps.Add(null);
            }

            if (a_material.HasTextureSpecular)
            {
                output.maps.Add(new Tuple<Assimp.TextureType, TextureSlot>(Assimp.TextureType.Specular, a_material.TextureSpecular));
            }
            else
            {
                output.maps.Add(null);
            }

            if (a_material.HasTextureDisplacement)
            {
                output.maps.Add(new Tuple<Assimp.TextureType, TextureSlot>(Assimp.TextureType.Displacement, a_material.TextureDisplacement));
            }
            else
            {
                output.maps.Add(null);
            }

            if (a_material.HasTextureOpacity)
            {
                output.maps.Add(new Tuple<Assimp.TextureType, TextureSlot>(Assimp.TextureType.Opacity, a_material.TextureOpacity));
            }
            else
            {
                output.maps.Add(null);
            }

            if (a_material.HasTextureReflection)
            {
                output.maps.Add(new Tuple<Assimp.TextureType, TextureSlot>(Assimp.TextureType.Reflection, a_material.TextureReflection));
            }
            else
            {
                output.maps.Add(null);
            }
            return output;
        }

        public static void FromAssimpMaterials(List<Assimp.Material> a_materials, out List<Material> materials)
        {
            materials = new List<Material>();
            foreach (Assimp.Material mat in a_materials)
            {
                materials.Add(Material.FromAssimpMaterial(mat));
            }
        }

        public static void ImportTexturesSynchronous(Material mat)
        {
            foreach (var tuple in mat.maps)
            {
                if (tuple == null)
                    continue;
                TextureType type = tuple.Item1;
                TextureSlot unit = tuple.Item2;
                string filename = Path.GetFileName(unit.FilePath);

                if (!Context.TextureManager.HasResource(filename))
                {
                    Texture2D tex = Texture2D.ImportTexture($"./assets/{unit.FilePath}");
                    if (tex == null)
                    {
                        Console.WriteLine($"Could not find texture resource at: ./assets/{unit.FilePath}");
                    }
                    else
                    {
                        Context.TextureManager.AddResource(filename, tex);
                    }
                }
                else
                {
                    Console.WriteLine($"Texture loading collision {filename}");
                }

            }
        }
        public static void ImportTexturesAsync(Material mat)
        {
            foreach (var tuple in mat.maps)
            {
                if (tuple == null)
                    continue;
                TextureType type = tuple.Item1;
                TextureSlot unit = tuple.Item2;
                string filename = Path.GetFileName(unit.FilePath);

                if (!Context.TextureManager.HasResource(filename))
                {
                    Job job = new Job(new Action<JobResultHandler>(
                        (e) =>
                        {
                            Texture2D tex = Texture2D.ImportTexture($"./assets/{unit.FilePath}");
                            if (tex == null)
                            {
                                e.JobFailed.Invoke($"Could not import file: ./assets/{unit.FilePath}");
                            }
                            else
                            {
                                Context.TextureManager.AddResource(filename, tex);
                            }
                        }),
                        new JobResultHandler(
                            new Action(() => { }),
                            new Action<string>((e) => { Console.WriteLine(e); }))
                    );
                    DelayedExecutionSystem system = (DelayedExecutionSystem) World.Current.GetSystem<DelayedExecutionSystem>();
                    system.AddJob(job);
                    
                }
                else
                {
                    Console.WriteLine($"Texture loading collision {filename}");
                }

            }
            
        }
        private static Vector4f FromAssimpColor(Assimp.Color4D color)
        {
            return new Vector4f(color.R, color.G, color.B, color.A);
        }
    }

    public class MaterialLoader
    {
        public List<Material> materials;

        public MaterialLoader()
        {
            this.materials = new List<Material>();
        }

        public void ImportMaterialsFromAssimpMaterial(List<Assimp.Material> _materials)
        {
            foreach(Assimp.Material m in _materials)
            {
                Material mat = Material.FromAssimpMaterial(m);
                this.materials.Add(mat);
            }
        }

        public void PushToRenderContext()
        {
            foreach(Material material in materials)
            {
                Context.MaterialManager.AddResource(material.Name, material);
            }
        }
        
    }
}
