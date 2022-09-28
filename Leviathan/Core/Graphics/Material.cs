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
        public Vector3f ColorAmbient;

        /// <summary>
        /// Specifies the specular diffuse reflectivity using RGB values
        /// </summary>
        [JsonProperty("Kd")]
        public Vector3f ColorDiffuse;

        /// <summary>
        /// Specifies the specular reflectivity using RGB values.
        /// </summary>
        [JsonProperty("Ks")]
        public Vector3f ColorSpecular;

        /// <summary>
        /// Specifies the specular exponent for the current material.  This defines 
        /// the focus of the specular highlight (Roughness, Glossyness).
        /// </summary>
        [JsonProperty("Ns")]
        public float SpecularExponent;

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

        [JsonProperty("map_Ka")]
        public string map_ambient_id;
        
        [JsonProperty("map_Kd")]
        public string map_diffuse_id;
        
        [JsonProperty("map_Ks")]
        public string map_specular_id;

        [JsonProperty("map_Disp")]
        public string map_displacement_id;

        [JsonProperty("map_Ns")]
        public string map_specular_exp_id;

        [JsonProperty("map_d")]
        public string map_transperency_id;


        public static readonly Material Default = new Material();
        
        public Material()
        {
            Name = "Default";
            ColorAmbient = Vector3f.Zero;
            ColorDiffuse = Vector3f.Zero;
            ColorSpecular = Vector3f.Zero;
            SpecularExponent = 1.0f;
            RefractionIndex = 1.0f;
            Transperency = 0.0f;
            Sharpness = 1.0f;
            map_ambient_id = "";
            map_diffuse_id = "";
            map_specular_id = "";
            map_displacement_id = "";
            map_specular_exp_id = "";
            map_transperency_id = "";
        }

    }

    public class MaterialLoader
    {
        public List<Material> materials;
        public string material_path;

        private StreamReader reader;

        public MaterialLoader()
        {
            this.materials = new List<Material>();
        }

        public void PushToRenderContext()
        {
            foreach(Material material in materials)
            {
                Context.MaterialManager.AddResource(material.Name, material);
            }
        }

        public void ParseMaterialFromFile(string file_path)
        {
            this.material_path = file_path;
            if(!file_path.EndsWith(".mtl"))
            {
                throw new ArgumentException("File extension not supported: only mtl files are supported");
            }

            if(!File.Exists(file_path))
            {
                throw new FileLoadException($"Could not locate file at path: {file_path}");
            }

            Material current = new Material();
            reader = new StreamReader(file_path);
            string line = string.Empty;
            bool first = false;
            do
            {
                line = reader.ReadLine();
                if(line.Length == 0)
                {
                    continue;
                } else
                {
                    string[] tokens = line.Split(" ");
                    switch (tokens[0])
                    {
                        case "newmtl":
                            if(first)
                            {
                                materials.Add(current);
                                current = new Material();
                            } else
                            {
                                first = true;
                            }
                            ParseName(tokens, current);
                            break;
                        case "Ka":
                            ParseColorAmbient(tokens, current);
                            break;
                        case "Kd":
                            ParseColorDiffuse(tokens, current);
                            break;
                        case "Ks":
                            ParseColorSpecular(tokens, current);
                            break;
                        case "Ns":
                            ParseSpecularExponent(tokens, current);
                            break;
                        case "Ni":
                            ParseRefractionIndex(tokens, current);
                            break;
                        case "d":
                            ParseTransperency(tokens, current);
                            break;
                        case "sharpness":
                            ParseSharpness(tokens, current);
                            break;
                        case "illum":
                            ParseIllumMode(tokens, current);
                            break;
                        case "map_Ka":
                            ParseAmbientMap(tokens, current);
                            break;
                        case "map_Kd":
                            ParseDiffuseMap(tokens, current);
                            break;
                        case "map_Ks":
                            ParseSpecularMap(tokens, current);
                            break;
                        case "map_Disp":
                            ParseDisplacementMap(tokens, current);
                            break;
                        case "map_Ns":
                            ParseSpecularExpMap(tokens, current);
                            break;
                        case "map_d":
                            ParseTransperencyMap(tokens, current);
                            break;
                        case "#": // comments
                            break;
                        default:
                            throw new Exception($"Unknown identifier {tokens[0]}");

                    }
                }
                
            } while (!reader.EndOfStream);
        }

        private void ParseName(string[] tokens, Material mat)
        {
            if(tokens.Length > 1)
            {
                mat.Name = tokens[1];
            } else
            {
                throw new Exception("Error parsing material name: not enough tokens found to parse a name");
            }
        }

        private void ParseColorAmbient(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                if (tokens[1] == "xyz" || tokens[1] == "spectral")
                {
                    throw new Exception("Spectral or XYZ coordinates are not supported for Ambient Color Yet");
                } else
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture.NumberFormat);
                    mat.ColorAmbient = new Vector3f(x, y, z);
                }
            }
            else
            {
                throw new Exception("Error parsing material Ambient Color: not enough tokens found to parse a Ambient Color");
            }
        }

        private void ParseColorDiffuse(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                if (tokens[1] == "xyz" || tokens[1] == "spectral")
                {
                    throw new Exception("Spectral or XYZ coordinates are not supported for Diffuse Color Yet");
                }
                else
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture.NumberFormat);
                    mat.ColorDiffuse = new Vector3f(x, y, z);
                }
            }
            else
            {
                throw new Exception("Error parsing material Diffuse Color: not enough tokens found to parse a Diffuse Color");
            }
        }

        private void ParseColorSpecular(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                if (tokens[1] == "xyz" || tokens[1] == "spectral")
                {
                    throw new Exception("Spectral or XYZ coordinates are not supported for Specular Color Yet");
                }
                else
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture.NumberFormat);
                    mat.ColorSpecular = new Vector3f(x, y, z);
                }
            }
            else
            {
                throw new Exception("Error parsing material Specular Color: not enough tokens found to parse a Specular Color");
            }
        }

        private void ParseSpecularExponent(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.SpecularExponent = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                throw new Exception("Error parsing material Specular Exponent: not enough tokens found to parse a Specular Exponent");
            }
        }

        private void ParseRefractionIndex(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.RefractionIndex = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                throw new Exception("Error parsing material Refraction Index: not enough tokens found to parse a Refraction Index");
            }
        }

        private void ParseTransperency(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.Transperency = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                throw new Exception("Error parsing material Transperency: not enough tokens found to parse a Transperency");
            }
        }

        private void ParseSharpness(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.Sharpness = float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                throw new Exception("Error parsing material Sharpness: not enough tokens found to parse a Sharpness");
            }
        }

        private void ParseIllumMode(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.illumination_mode = uint.Parse(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material Illum mode: not enough tokens found to parse a Illum mode");
            }
        }

        private void ParseAmbientMap(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.map_ambient_id = Path.GetFileNameWithoutExtension(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material Ambient Map: not enough tokens found to parse a Ambient Map");
            }
        }

        private void ParseDiffuseMap(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.map_diffuse_id = Path.GetFileNameWithoutExtension(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material Diffuse Map: not enough tokens found to parse a Diffuse Map");
            }
        }

        private void ParseSpecularMap(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.map_specular_id = Path.GetFileNameWithoutExtension(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material Specular Map: not enough tokens found to parse a Specular Map");
            }
        }

        private void ParseDisplacementMap(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.map_displacement_id = Path.GetFileNameWithoutExtension(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material Displacement Map: not enough tokens found to parse a Displacement Map");
            }
        }

        private void ParseSpecularExpMap(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.map_specular_exp_id = Path.GetFileNameWithoutExtension(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material SpecularExp Map: not enough tokens found to parse a SpecularExp Map");
            }
        }

        private void ParseTransperencyMap(string[] tokens, Material mat)
        {
            if (tokens.Length > 1)
            {
                mat.map_transperency_id = Path.GetFileNameWithoutExtension(tokens[1]);
            }
            else
            {
                throw new Exception("Error parsing material Transperency Map: not enough tokens found to parse a Transperency Map");
            }
        }


    }
}
