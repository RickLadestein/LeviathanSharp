using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public abstract class Attribute<ValueType> where ValueType : unmanaged
    {
        public VertexAttributeDescriptor descriptor;

        public List<ValueType> attribute_data;

        public uint SegmentCount { get; private set; }

        protected Attribute(LAttributeDataType value_type, LDataCollectionType collection_type)
        {
            descriptor = new VertexAttributeDescriptor
            {
                value_type = value_type,
                collection_type = collection_type
            };

            switch (value_type)
            {
                case LAttributeDataType.INT:
                case LAttributeDataType.UINT:
                case LAttributeDataType.FLOAT:
                    descriptor.value_byte_size = 4;
                    break;
                case LAttributeDataType.DOUBLE:
                    descriptor.value_byte_size = 8;
                    break;
                default:
                    throw new NotSupportedException($"AttributeDataType: {value_type} not yet supported!");
            }
            descriptor.segment_byte_size = ((int)collection_type) * descriptor.value_byte_size;
        }


        /// <summary>
        /// Adds a single value datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(ValueType datapoint)
        {
            attribute_data.Add(datapoint);
            //descriptor.segment_count += 1;
        }

        /// <summary>
        /// Adds a collection of datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(ValueType[] datapoints)
        {
            attribute_data.AddRange(datapoints);
            //descriptor.segment_count += datapoints.Length;
        }

        /// <summary>
        /// Adds a collection of datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(IEnumerable<ValueType> datapoints)
        {
            attribute_data.AddRange(datapoints);
            //descriptor.segment_count += datapoints.Count();
        }

        public abstract VertexAttribute CompileToVertexAttribute();
    }
}
