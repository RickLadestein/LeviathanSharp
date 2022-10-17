using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Leviathan.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3f
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3f(float _x, float _y, float _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }

        public Vector3f(Vector2f vec, float _z)
        {
            X = vec.X;
            Y = vec.Y;
            Z = _z;
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

        public static readonly Vector3f UnitX       = new Vector3f(1.0f, 0.0f, 0.0f);
        public static readonly Vector3f UnitY       = new Vector3f(0.0f, 1.0f, 0.0f);
        public static readonly Vector3f UnitZ       = new Vector3f(0.0f, 0.0f, 1.0f);
        public static readonly Vector3f Up          = new Vector3f(0.0f, 1.0f, 0.0f);
        public static readonly Vector3f One         = new Vector3f(1.0f, 1.0f, 1.0f);
        public static readonly Vector3f Zero        = new Vector3f(0.0f, 0.0f, 0.0f);
        public static readonly Vector3f Forward     = new Vector3f(0.0f, 0.0f, -1.0f);
        public static readonly Vector3f Backward    = new Vector3f(0.0f, 0.0f, 1.0f);
        public static readonly Vector3f Right       = new Vector3f(1.0f, 0.0f, 0.0f);
        public static readonly Vector3f Left        = new Vector3f(-1.0f, 0.0f, 0.0f);

        public void Set(float _x, float _y, float _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }
        public float Length()
        {
            return System.MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public float LengthSqr()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        public void Normalize()
        {
            var scale = 1.0f / Length();
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        public static Vector3f Normalize(Vector3f input)
        {
            input.Normalize();
            return input;
        }

        public Vector3f Normalized()
        {
            var scale = 1.0f / Length();
            float _x = scale * X;
            float _y = scale * Y;
            float _z = scale * Z;
            return new Vector3f(_x, _y, _z);
        }

        public static float Dot(Vector3f v1, Vector3f v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static Vector3f Cross(Vector3f left, Vector3f right)
        {
            Vector3f result = Vector3f.Zero;
            result.X = (left.Y * right.Z) - (left.Z * right.Y);
            result.Y = (left.Z * right.X) - (left.X * right.Z);
            result.Z = (left.X * right.Y) - (left.Y * right.X);
            return result;
        }

        public static Vector3f Parse(string[] tokens, int begin, NumberFormatInfo format = null)
        {
            if (tokens.Length < 3)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector3f: Expected >= 3 and got {tokens.Length}");
            }

            if (tokens.Length - begin < 0)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector3f: Expected >= 3 parseable tokens and got {tokens.Length - begin} accounting for start index");
            }

            float x, y, z;
            if (format != null)
            {
                x = float.Parse(tokens[begin], format);
                y = float.Parse(tokens[begin + 1], format);
                z = float.Parse(tokens[begin + 2], format);
            }
            else
            {
                x = float.Parse(tokens[begin]);
                y = float.Parse(tokens[begin + 1]);
                z = float.Parse(tokens[begin + 2]);
            }
            return new Vector3f(x, y, z);

        }

        public float[] ToArray()
        {
            return new float[3] { X, Y, Z };
        }


        #region Operators

        [Pure]
        public static Vector3f operator +(Vector3f left, Vector3f right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        [Pure]
        public static Vector3f operator -(Vector3f vec)
        {
            return new Vector3f(-vec.X, -vec.Y, -vec.Z);
        }

        [Pure]
        public static Vector3f operator -(Vector3f left, Vector3f right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        [Pure]
        public static Vector3f operator *(Vector3f left, Vector3f right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            return left;
        }

        [Pure]
        public static Vector3f operator *(float scalar, Vector3f right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            right.Z *= scalar;
            return right;
        }

        [Pure]
        public static Vector3f operator *(Vector3f left, float scalar)
        {
            left.X *= scalar;
            left.Y *= scalar;
            left.Z *= scalar;
            return left;
        }

        [Pure]
        public static Vector3f operator /(Vector3f left, Vector3f right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            return left;
        }

        [Pure]
        public static Vector3f operator /(float scalar, Vector3f right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            right.Z /= scalar;
            return right;
        }

        [Pure]
        public static Vector3f operator /(Vector3f left, float scalar)
        {
            left.X /= scalar;
            left.Y /= scalar;
            left.Z /= scalar;
            return left;
        }

        [Pure]
        public static bool operator ==(Vector3f left, Vector3f right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector3f left, Vector3f right)
        {
            return !(left.Equals(right));
        }
        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector3f(Vector3i vec)
        {
            return new Vector3f(vec.X, vec.Y, vec.Z);
        }

        [Pure]
        public static explicit operator Vector3f(Vector3d vec)
        {
            return new Vector3f((float)vec.X, (float)vec.Y, (float)vec.Z);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector3f && Equals((Vector3f)obj);
        }

        public bool Equals(Vector3f obj)
        {
            return X == obj.X && Y == obj.Y && Z == obj.Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{X} | {Y} | {Z}]";
        }
        #endregion
    }
}
