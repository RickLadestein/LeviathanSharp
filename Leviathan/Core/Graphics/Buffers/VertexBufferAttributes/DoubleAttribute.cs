using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Attribute class for storing double values
    /// </summary>
    public class DoubleAttribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of DoubleAttribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of double valuetype spaces that are reserved</param>
        public DoubleAttribute(uint reserve = 0) : base(AttributeDataType.DOUBLE, DataCollectionType.SINGULAR, reserve) {}

        /// <summary>
        /// Adds a single double value datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(double datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of double datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(double[] datapoints)
        {
            AddDataColl<double>(datapoints);
            this.SegmentCount += 1;
        }
    }

    /// <summary>
    /// Attribute class for storing Vector2d values
    /// </summary>
    public class Double2Attribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of Double2Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector2d spaces that are reserved</param>
        public Double2Attribute(uint reserve = 0) : base(AttributeDataType.DOUBLE, DataCollectionType.VEC2, reserve) { }

        /// <summary>
        /// Adds a single Vector2d datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector2d datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of Vector2d datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector2d[] datapoints)
        {
            AddDataColl<Vector2d>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }

    /// <summary>
    /// Attribute class for storing Vector3d values
    /// </summary>
    public class Double3Attribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of Double3Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector3d valuetype spaces that are reserved</param>
        public Double3Attribute(uint reserve = 0) : base(AttributeDataType.DOUBLE, DataCollectionType.VEC3, reserve) { }

        /// <summary>
        /// Adds a single Vector3d datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector3d datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of Vector3d datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector3d[] datapoints)
        {
            AddDataColl<Vector3d>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }

    /// <summary>
    /// Attribute class for storing Vector4d values
    /// </summary>
    public class Double4Attribute : Attribute
    {
        /// <summary>
        /// Instantiates a new instance of Double4Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector4d valuetype spaces that are reserved</param>
        public Double4Attribute(uint reserve = 0) : base(AttributeDataType.DOUBLE, DataCollectionType.VEC4, reserve) { }

        /// <summary>
        /// Adds a single Vector4d datapoint to the attribute storage
        /// </summary>
        /// <param name="datapoint">Datapoint to be added to attribute storage</param>
        public void AddData(Vector4d datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
            this.SegmentCount += 1;
        }

        /// <summary>
        /// Adds a collection of Vector4d datapoints to the attribute storage
        /// </summary>
        /// <param name="datapoints">Datapoints to be added to attribute storage</param>
        public void AddData(Vector4d[] datapoints)
        {
            AddDataColl<Vector4d>(datapoints);
            this.SegmentCount += datapoints.Length;
        }
    }
}
