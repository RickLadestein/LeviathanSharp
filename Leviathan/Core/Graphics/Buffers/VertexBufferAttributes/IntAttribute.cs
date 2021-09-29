using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public class IntAttribute : Attribute
    {
        public IntAttribute(DataCollectionType col_type, uint reserve = 0) : base(AttributeDataType.INT, col_type, reserve) {
            if (coll_type == DataCollectionType.MAT2 || coll_type == DataCollectionType.MAT3 || coll_type == DataCollectionType.MAT4)
            {
                throw new NotImplementedException("Matrix functionality not supported for this data type");
            }
        }

        public void AddData(int datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector2i datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector3i datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector4i datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(int[] datapoints)
        {
            AddDataColl<int>(datapoints);
        }

        public void AddData(Vector2i[] datapoints)
        {
            AddDataColl<Vector2i>(datapoints);
        }

        public void AddData(Vector3i[] datapoints)
        {
            AddDataColl<Vector3i>(datapoints);
        }

        public void AddData(Vector4i[] datapoints)
        {
            AddDataColl<Vector4i>(datapoints);
        }
    }
}
