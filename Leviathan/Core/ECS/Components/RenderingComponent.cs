using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components
{
    public class RenderingComponent : Component
    {

        public RenderingComponent(String prog_id, String mesh_id)
        {
            this.Shader = ShaderProgram.GetShaderPorgramFromStorage(prog_id);
            this.Mesh = Mesh.GetMeshFromLoadedMeshes(mesh_id);
        }
        public ShaderProgram Shader { get; set; }

        private Mesh _mesh;
        public Mesh Mesh { 
            get { 
                return _mesh; 
            } 
            set { 
                _mesh = value;
                Buffer?.Destroy();
                Buffer = new VertexBuffer(); 
                if(_mesh == null) { return; }
                Buffer.BufferData(_mesh.Object_data); 
            }
        }

        public VertexBuffer Buffer { get; private set; }
    }
}
