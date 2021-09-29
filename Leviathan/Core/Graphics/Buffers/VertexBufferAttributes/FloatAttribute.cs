using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public class FloatAttribute : Attribute
    {
        public FloatAttribute(DataCollectionType col_type, uint reserve = 0) : base(AttributeDataType.FLOAT, col_type, reserve) { }

        public void AddData(float datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector2f datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector3f datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Vector4f datapoint)
        {
            byte[] data = this.ToByteArray(datapoint);
            this.AddByteData(data);
        }

        public void AddData(Mat2 datamatrix)
        {
            byte[] data = this.ToByteArray(datamatrix);
            this.AddByteData(data);
        }
        public void AddData(Mat3 datamatrix)
        {
            byte[] data = this.ToByteArray(datamatrix);
            this.AddByteData(data);
        }

        public void AddData(Mat4 datamatrix)
        {
            byte[] data = this.ToByteArray(datamatrix);
            this.AddByteData(data);
        }

        public void AddData(float[] datapoints)
        {
            AddDataColl<float>(datapoints);
        }

        public void AddData(Vector2f[] datapoints)
        {
            AddDataColl<Vector2f>(datapoints);
        }

        public void AddData(Vector3f[] datapoints)
        {
            AddDataColl<Vector3f>(datapoints);
        }

        public void AddData(Vector4f[] datapoints)
        {
            AddDataColl<Vector4f>(datapoints);
        }

        public void AddData(Mat2[] datamatrices)
        {
            AddDataColl<Mat2>(datamatrices);
        }

        public void AddData(Mat3[] datamatrices)
        {
            AddDataColl<Mat3>(datamatrices);
        }

        public void AddData(Mat4[] datamatrices)
        {
            AddDataColl<Mat4>(datamatrices);
        }
    }
}
