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
    public struct Vector4i
    {
        public int X;
        public int Y;
        public int Z;
        public int W;

        public Vector3i Xyz
        {
            get
            {
                return new Vector3i(X, Y, Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4i(int _x, int _y, int _z, int _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
        }

        public int this[int index]
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
                else if (index == 3)
                {
                    return W;
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
                else if (index == 3)
                {
                    W = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Vector index out of range: " + index);
                }
            }
        }

        public static readonly Vector4i UnitX = new Vector4i(1, 0, 0, 0);
        public static readonly Vector4i UnitY = new Vector4i(0, 1, 0, 0);
        public static readonly Vector4i UnitZ = new Vector4i(0, 0, 1, 0);
        public static readonly Vector4i UnitW = new Vector4i(0, 0, 1, 0);
        public static readonly Vector4i Up =    new Vector4i(0, 1, 0, 0);
        public static readonly Vector4i One =   new Vector4i(1, 1, 1, 1);
        public static readonly Vector4i Zero =  new Vector4i(0, 0, 0, 0);

        public void Set(int _x, int _y, int _z, int _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
        }
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
            X = (int)(scale * X);
            Y = (int)(scale * Y);
            Z = (int)(scale * Z);
            W = (int)(scale * W);
        }

        public Vector4i Normalized()
        {
            var scale = 1.0f / Length();
            int _x = (int)(scale * X);
            int _y = (int)(scale * Y);
            int _z = (int)(scale * Z);
            int _w = (int)(scale * W);
            return new Vector4i(_x, _y, _z, _w);
        }

        public static int Dot(Vector4i v1, Vector4i v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) + (v1.W * v2.W);
        }

        public static Vector4i Parse(string[] tokens, int begin, NumberFormatInfo format = null)
        {
            if (tokens.Length < 4)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector4i: Expected >= 4 and got {tokens.Length}");
            }

            if (tokens.Length - begin < 0)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector4i: Expected >= 4 parseable tokens and got {tokens.Length - begin} accounting for start index");
            }

            int x, y, z, w;
            if (format != null)
            {
                x = int.Parse(tokens[begin], format);
                y = int.Parse(tokens[begin + 1], format);
                z = int.Parse(tokens[begin + 2], format);
                w = int.Parse(tokens[begin + 4], format);

            }
            else
            {
                x = int.Parse(tokens[begin]);
                y = int.Parse(tokens[begin + 1]);
                z = int.Parse(tokens[begin + 2]);
                w = int.Parse(tokens[begin + 4]);
            }
            return new Vector4i(x, y, z, w);
        }

        public int[] ToArray()
        {
            return new int[4] { X, Y, Z, W };
        }

        #region Operators
        [Pure]
        public static Vector4i operator +(Vector4i left, Vector4i right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        [Pure]
        public static Vector4i operator -(Vector4i vec)
        {
            return new Vector4i(-vec.X, -vec.Y, -vec.Z, -vec.W);
        }

        [Pure]
        public static Vector4i operator -(Vector4i left, Vector4i right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        [Pure]
        public static Vector4i operator *(Vector4i left, Vector4i right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
            return left;
        }

        [Pure]
        public static Vector4i operator *(int scalar, Vector4i right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            right.Z *= scalar;
            right.W *= scalar;
            return right;
        }

        [Pure]
        public static Vector4i operator *(Vector4i left, int scalar)
        {
            left.X *= scalar;
            left.Y *= scalar;
            left.Z *= scalar;
            left.W *= scalar;
            return left;
        }

        [Pure]
        public static Vector4i operator /(Vector4i left, Vector4i right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            left.W /= right.W;
            return left;
        }

        [Pure]
        public static Vector4i operator /(int scalar, Vector4i right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            right.Z /= scalar;
            right.Z /= scalar;
            return right;
        }

        [Pure]
        public static Vector4i operator /(Vector4i left, int scalar)
        {
            left.X /= scalar;
            left.Y /= scalar;
            left.Z /= scalar;
            left.Z /= scalar;
            return left;
        }

        [Pure]
        public static bool operator ==(Vector4i left, Vector4i right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector4i left, Vector4i right)
        {
            return !(left.Equals(right));
        }
        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector4i(Vector4f vec)
        {
            return new Vector4i((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
        }

        [Pure]
        public static explicit operator Vector4i(Vector4d vec)
        {
            return new Vector4i((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector4i && Equals((Vector4i)obj);
        }

        public bool Equals(Vector4i obj)
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
