using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Attribute class for storing int values
    /// </summary>
    public class IntAttribute : VertexAttribute
    {
        /// <summary>
        /// Instantiates a new instance of IntAttribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of int valuetype spaces that are reserved</param>
        public IntAttribute(uint reserve = 0) : base(AttributeDataType.INT, DataCollectionType.SINGULAR, reserve) {}

        /// <summary>
        /// Adds a single int value datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(int datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        /// <summary>
        /// Adds a collection of int datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(int[] datapoints)
        {
            AddDataColl<int>(datapoints);
        }
    }

    /// <summary>
    /// Attribute class for storing Vector2i values
    /// </summary>
    public class Int2Attribute : VertexAttribute
    {
        /// <summary>
        /// Instantiates a new instance of Int2Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector2i spaces that are reserved</param>
        public Int2Attribute(uint reserve = 0) : base(AttributeDataType.INT, DataCollectionType.VEC2, reserve) { }

        /// <summary>
        /// Adds a single Vector2i datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector2i datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        /// <summary>
        /// Adds a collection of Vector2i datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector2i[] datapoints)
        {
            AddDataColl<Vector2i>(datapoints);
        }
    }

    /// <summary>
    /// Attribute class for storing Vector3i values
    /// </summary>
    public class Int3Attribute : VertexAttribute
    {
        /// <summary>
        /// Instantiates a new instance of Int3Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector3i spaces that are reserved</param>
        public Int3Attribute(uint reserve = 0) : base(AttributeDataType.INT, DataCollectionType.VEC3, reserve) { }

        /// <summary>
        /// Adds a single Vector3i datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector3i datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        /// <summary>
        /// Adds a collection of Vector3i datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector3i[] datapoints)
        {
            AddDataColl<Vector3i>(datapoints);
        }
    }

    /// <summary>
    /// Attribute class for storing Vector4i values
    /// </summary>
    public class Int4Attribute : VertexAttribute
    {
        /// <summary>
        /// Instantiates a new instance of Int4Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector4i spaces that are reserved</param>
        public Int4Attribute(uint reserve = 0) : base(AttributeDataType.INT, DataCollectionType.VEC4, reserve) { }

        /// <summary>
        /// Adds a single Vector4i datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector4i datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        /// <summary>
        /// Adds a collection of Vector4i datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector4i[] datapoints)
        {
            AddDataColl<Vector4i>(datapoints);
        }
    }
}
