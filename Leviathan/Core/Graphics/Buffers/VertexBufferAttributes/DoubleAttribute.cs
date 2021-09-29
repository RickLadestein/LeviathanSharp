using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public class DoubleAttribute : Attribute
    {
        public DoubleAttribute(DataCollectionType col_type, uint reserve = 0) : base(AttributeDataType.DOUBLE, col_type, reserve) {
            if(coll_type == DataCollectionType.MAT2 || coll_type == DataCollectionType.MAT3 || coll_type == DataCollectionType.MAT4)
            {
                throw new NotImplementedException("Matrix functionality not supported for this data type");
            }
        }

        public void AddData(double datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector2d datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector3d datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector4d datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(double[] datapoints)
        {
            AddDataColl<double>(datapoints);
        }

        public void AddData(Vector2d[] datapoints)
        {
            AddDataColl<Vector2d>(datapoints);
        }

        public void AddData(Vector3d[] datapoints)
        {
            AddDataColl<Vector3d>(datapoints);
        }

        public void AddData(Vector4d[] datapoints)
        {
            AddDataColl<Vector4d>(datapoints);
        }


    }
}
