using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public abstract class Attribute<ValueType>
    {
        public VertexAttributeDescriptor descriptor;

        public List<ValueType> attribute_data;

        public uint segment_count { get; private set; }


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
