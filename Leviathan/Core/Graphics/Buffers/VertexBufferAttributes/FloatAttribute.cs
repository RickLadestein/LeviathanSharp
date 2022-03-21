using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Attribute class for storing float values
    /// </summary>
    public class FloatAttribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of FloatAttribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of float valuetype spaces that are reserved</param>
        public FloatAttribute(uint reserve = 0) : base(AttributeDataType.FLOAT, DataCollectionType.SINGULAR, reserve) { }

        /// <summary>
        /// Adds a single float value datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(float datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of float datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(float[] datapoints)
        {
            AddDataColl<float>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }

    /// <summary>
    /// Attribute class for storing Vector2f values
    /// </summary>
    public class Float2Attribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of Float2Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector2f spaces that are reserved</param>
        public Float2Attribute(uint reserve = 0) : base(AttributeDataType.FLOAT, DataCollectionType.VEC2, reserve) { }

        /// <summary>
        /// Adds a single Vector2f datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector2f datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of Vector2f datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector2f[] datapoints)
        {
            AddDataColl<Vector2f>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }

    /// <summary>
    /// Attribute class for storing Vector3f values
    /// </summary>
    public class Float3Attribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of Float3Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector3f spaces that are reserved</param>
        public Float3Attribute(uint reserve = 0) : base(AttributeDataType.FLOAT, DataCollectionType.VEC3, reserve) { }

        /// <summary>
        /// Adds a single Vector3f datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector3f datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of Vector3f datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector3f[] datapoints)
        {
            AddDataColl<Vector3f>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }

    /// <summary>
    /// Attribute class for storing Vector4f values
    /// </summary>
    public class Float4Attribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of Float4Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector4f spaces that are reserved</param>
        public Float4Attribute(uint reserve = 0) : base(AttributeDataType.FLOAT, DataCollectionType.VEC4, reserve) { }

        /// <summary>
        /// Adds a single Vector4f datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector4f datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of Vector4f datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector4f[] datapoints)
        {
            AddDataColl<Vector4f>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }
}
