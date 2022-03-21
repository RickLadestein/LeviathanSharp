using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Core.Graphics.Buffers
{
    public class UniformBuffer : BufferBase
    {
        public static List<ValueTuple<int, ValueTuple<string, uint>>> bindings = new();

        public uint byte_size;
        
        public String UniformName;
        public UniformLayout BufferLayout { get; private set; }
        public UniformBuffer(String uniform_name, UniformLayout buff_layout)
        {
            if(buff_layout is null)
            {
                throw new ArgumentNullException(nameof(buff_layout));
            }
            UniformName = uniform_name;
            BufferLayout = buff_layout;
            DetermineBufferSize();
            CreateBuffer();
            Build();
        }

        private void Build()
        {
        }

        private void DetermineBufferSize()
        {
            foreach (UniformDescriptor desc in BufferLayout.layout)
            {
                int base_type_size = 0;
                switch(desc.base_type)
                {
                    case UniformType.BOOL:
                    case UniformType.INT:
                    case UniformType.UINT:
                        base_type_size = sizeof(bool);
                        break;
                    case UniformType.FLOAT:
                        base_type_size = sizeof(float);
                        break;
                    case UniformType.DOUBLE:
                        base_type_size = sizeof(double);
                        break;
                }
                byte_size += (uint)(base_type_size * desc.count);
            }
        }

        private void BindUniformBuffer()
        {

        }

        public override void CreateBuffer()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            if(this.Handle != 0)
            {
                Context.GLContext.DeleteBuffer(this.Handle);
            }
        }
    }

    public class UniformBindingBuffer
    {
        

        private static UniformBindingBuffer instance;
        public static UniformBindingBuffer Instance { 
            get 
            {
                if(instance is null)
                {
                    instance = new UniformBindingBuffer();
                }
                return instance;
            } 
        }

        private Queue<uint> priority_q;
        private uint[] UniformBindings;

        private UniformBindingBuffer()
        {
            priority_q = new Queue<uint>();
            UniformBindings = new uint[512];
        }

        public void BindUniformBufferToGlobalIndex()
        {

        }
    }

    public class UniformLayout
    {
        public List<UniformDescriptor> layout;

        public UniformLayout()
        {
            layout = new List<UniformDescriptor>();
        }

        public UniformLayout(IEnumerable<UniformDescriptor> coll)
        {
            if(coll is null)
            {
                throw new ArgumentNullException(nameof(coll));
            }
            layout = new List<UniformDescriptor>(coll);
        }
    }

    public class UniformLayoutBuilder
    {

        private List<UniformDescriptor> buffer_layout;

        public UniformLayoutBuilder()
        {
            buffer_layout = new List<UniformDescriptor>();
        }

        public UniformLayout Build()
        {
            return new UniformLayout(buffer_layout);
        }

        public void AddSingle(string _name, UniformType utype)
        {
            buffer_layout.Add(
                new UniformDescriptor()
                {
                    name = _name,
                    base_type = utype,
                    storage_type = UniformStorageType.SINGLE,
                    count = 1
                });
        }

        public void AddArray(string _name, UniformType utype, uint _count)
        {
            buffer_layout.Add(
                new UniformDescriptor()
                {
                    name = _name,
                    base_type = utype,
                    storage_type = UniformStorageType.ARRAY,
                    count = _count
                });
        }

        public void AddArray(string _name, UniformType utype, uint _width, uint _height)
        {
            buffer_layout.Add(
                new UniformDescriptor()
                {
                    name = _name,
                    base_type = utype,
                    storage_type = UniformStorageType.ARRAY,
                    count = (_width * _height)
                }); ;
        }

        public void AddVector(string _name, UniformType utype, uint _length)
        {
            buffer_layout.Add(
                new UniformDescriptor()
                {
                    name = _name,
                    base_type = utype,
                    storage_type = UniformStorageType.VECTOR,
                    count = _length
                });
        }

        public void AddMatrix(string _name, uint rows, uint colls)
        {
            buffer_layout.Add(
                new UniformDescriptor()
                {
                    name = _name,
                    base_type = UniformType.FLOAT,
                    storage_type = UniformStorageType.MATRIX,
                    count = (rows * colls)
                });
        }
    }

    public struct UniformDescriptor
    {
        public string name;
        public UniformType base_type;
        public UniformStorageType storage_type;
        public uint count;
    }

    public enum UniformType
    {
        BOOL, 
        INT, 
        UINT, 
        FLOAT, 
        DOUBLE
    }

    public enum UniformStorageType
    {
        SINGLE,
        ARRAY,
        VECTOR,
        MATRIX
    }
}
