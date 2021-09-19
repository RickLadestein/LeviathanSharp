using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4f
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4f(float _x, float _y, float _z, float _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
        }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return X;
                }

                if (index == 1)
                {
                    return Y;
                }

                if (index == 2)
                {
                    return Z;
                }

                throw new IndexOutOfRangeException("Vector index out of range: " + index);
            }

            set
            {
                if (index == 0)
                {
                    X = value;
                }
                else if (index == 1)
                {
                    Y = value;
                }
                else if (index == 2)
                {
                    Z = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Vector index out of range: " + index);
                }
            }
        }

        public static readonly Vector4f UnitX = new Vector4f(1.0f, 0.0f, 0.0f, 0.0f);
        public static readonly Vector4f UnitY = new Vector4f(0.0f, 1.0f, 0.0f, 0.0f);
        public static readonly Vector4f UnitZ = new Vector4f(0.0f, 0.0f, 1.0f, 0.0f);
        public static readonly Vector4f UnitW = new Vector4f(0.0f, 0.0f, 1.0f, 0.0f);
        public static readonly Vector4f Up =    new Vector4f(0.0f, 1.0f, 0.0f, 0.0f);
        public static readonly Vector4f One =   new Vector4f(1.0f, 1.0f, 1.0f, 1.0f);
        public static readonly Vector4f Zero =  new Vector4f(0.0f, 0.0f, 0.0f, 0.0f);

        public float Length()
        {
            return System.MathF.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        public float LengthSqr()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        public void Normalize()
        {
            var scale = 1.0f / Length();
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        public Vector4f Normalized()
        {
            var scale = 1.0f / Length();
            float _x = scale * X;
            float _y = scale * Y;
            float _z = scale * Z;
            float _w = scale * W;
            return new Vector4f(_x, _y, _z, _w);
        }

        #region Operators
        [Pure]
        public static Vector4f operator +(Vector4f left, Vector4f right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        [Pure]
        public static Vector4f operator -(Vector4f left, Vector4f right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        [Pure]
        public static Vector4f operator *(Vector4f left, Vector4f right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
            return left;
        }

        [Pure]
        public static Vector4f operator *(float scalar, Vector4f right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            right.Z *= scalar;
            right.W *= scalar;
            return right;
        }

        [Pure]
        public static Vector4f operator *(Vector4f left, float scalar)
        {
            left.X *= scalar;
            left.Y *= scalar;
            left.Z *= scalar;
            left.W *= scalar;
            return left;
        }

        [Pure]
        public static Vector4f operator /(Vector4f left, Vector4f right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            left.W /= right.W;
            return left;
        }

        [Pure]
        public static Vector4f operator /(float scalar, Vector4f right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            right.Z /= scalar;
            right.Z /= scalar;
            return right;
        }

        [Pure]
        public static Vector4f operator /(Vector4f left, float scalar)
        {
            left.X /= scalar;
            left.Y /= scalar;
            left.Z /= scalar;
            left.Z /= scalar;
            return left;
        }

        [Pure]
        public static bool operator ==(Vector4f left, Vector4f right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector4f left, Vector4f right)
        {
            return !(left.Equals(right));
        }
        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector4f(Vector4i vec)
        {
            return new Vector4f(vec.X, vec.Y, vec.Z, vec.W);
        }

        [Pure]
        public static explicit operator Vector4f(Vector4d vec)
        {
            return new Vector4f((float)vec.X, (float)vec.Y, (float)vec.Z, (float)vec.W);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector4f && Equals((Vector4f)obj);
        }

        public bool Equals(Vector4f obj)
        {
            return X == obj.X && Y == obj.Y && Z == obj.Z && obj.W == W;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{X} | {Y} | {Z} | {W}]";
        }
        #endregion
    }
}
