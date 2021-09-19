using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Mat2
    {
        public Vector2f Row0;
        public Vector2f Row1;

        public Mat2(Vector2f _row0, Vector2f _row1)
        {
            Row0 = _row0;
            Row1 = _row1;
        }

        public Mat2(float _m00, float _m01, float _m10, float _m11)
        {
            Row0 = new Vector2f(_m00, _m01);
            Row1 = new Vector2f(_m10, _m11);
        }

        public static readonly Mat2 Identity    = new Mat2(Vector2f.UnitX, Vector2f.UnitY);
        public static readonly Mat2 Zero        = new Mat2(Vector2f.Zero, Vector2f.Zero);

        public float Determinant
        {
            get
            {
                float m11 = Row0.X;
                float m12 = Row0.Y;
                float m21 = Row1.X;
                float m22 = Row1.Y;

                return (m11 * m22) - (m12 * m21);
            }
        }

        public Vector2f Column0
        {
            get => new Vector2f(Row0.X, Row1.X);
            set
            {
                Row0.X = value.X;
                Row1.X = value.Y;
            }
        }

        public Vector2f Column1
        {
            get => new Vector2f(Row0.Y, Row1.Y);
            set
            {
                Row0.Y = value.X;
                Row1.Y = value.Y;
            }
        }

        public float M11
        {
            get => Row0.X;
            set => Row0.X = value;
        }

        public float M12
        {
            get => Row0.Y;
            set => Row0.Y = value;
        }

        public float M21
        {
            get => Row1.X;
            set => Row1.X = value;
        }

        public float M22
        {
            get => Row1.Y;
            set => Row1.Y = value;
        }

    }
}
