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
    public struct Vector3i
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3i(int _x, int _y, int _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
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

        public static readonly Vector3i UnitX = new Vector3i(1, 0, 0);
        public static readonly Vector3i UnitY = new Vector3i(0, 1, 0);
        public static readonly Vector3i UnitZ = new Vector3i(0, 0, 1);
        public static readonly Vector3i Up =    new Vector3i(0, 1, 0);
        public static readonly Vector3i One =   new Vector3i(1, 1, 1);
        public static readonly Vector3i Zero =  new Vector3i(0, 0, 0);

        public void Set(int _x, int _y, int _z)
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
            X = (int)(scale * X);
            Y = (int)(scale * Y);
            Z = (int)(scale * Z);
        }

        public Vector3i Normalized()
        {
            var scale = 1.0f / Length();
            int _x = (int)(scale * X);
            int _y = (int)(scale * Y);
            int _z = (int)(scale * Z);
            return new Vector3i(_x, _y, _z);
        }

        public static int Dot(Vector3i v1, Vector3i v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static Vector3i Cross(Vector3i left, Vector3i right)
        {
            Vector3i result = Vector3i.Zero;
            result.X = (left.Y * right.Z) - (left.Z * right.Y);
            result.Y = (left.Z * right.X) - (left.X * right.Z);
            result.Z = (left.X * right.Y) - (left.Y * right.X);
            return result;
        }

        public static Vector3i Parse(string[] tokens, int begin, NumberFormatInfo format = null)
        {
            if (tokens.Length < 3)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector3i: Expected >= 3 and got {tokens.Length}");
            }

            if (tokens.Length - begin < 0)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector3i: Expected >= 3 parseable tokens and got {tokens.Length - begin} accounting for start index");
            }

            int x, y, z;
            if (format != null)
            {
                x = int.Parse(tokens[begin], format);
                y = int.Parse(tokens[begin + 1], format);
                z = int.Parse(tokens[begin + 2], format);
            }
            else
            {
                x = int.Parse(tokens[begin]);
                y = int.Parse(tokens[begin + 1]);
                z = int.Parse(tokens[begin + 2]);
            }
            return new Vector3i(x, y, z);
        }

        public int[] ToArray()
        {
            return new int[3] { X, Y, Z };
        }

        #region Operators
        [Pure]
        public static Vector3i operator +(Vector3i left, Vector3i right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        [Pure]
        public static Vector3i operator -(Vector3i vec)
        {
            return new Vector3i(-vec.X, -vec.Y, -vec.Z);
        }

        [Pure]
        public static Vector3i operator -(Vector3i left, Vector3i right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        [Pure]
        public static Vector3i operator *(Vector3i left, Vector3i right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            return left;
        }

        [Pure]
        public static Vector3i operator *(int scalar, Vector3i right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            right.Z *= scalar;
            return right;
        }

        [Pure]
        public static Vector3i operator *(Vector3i left, int scalar)
        {
            left.X *= scalar;
            left.Y *= scalar;
            left.Z *= scalar;
            return left;
        }

        [Pure]
        public static Vector3i operator /(Vector3i left, Vector3i right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            return left;
        }

        [Pure]
        public static Vector3i operator /(int scalar, Vector3i right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            right.Z /= scalar;
            return right;
        }

        [Pure]
        public static Vector3i operator /(Vector3i left, int scalar)
        {
            left.X /= scalar;
            left.Y /= scalar;
            left.Z /= scalar;
            return left;
        }

        [Pure]
        public static bool operator ==(Vector3i left, Vector3i right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector3i left, Vector3i right)
        {
            return !(left.Equals(right));
        }
        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector3i(Vector3f vec)
        {
            return new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
        }

        [Pure]
        public static explicit operator Vector3i(Vector3d vec)
        {
            return new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector3i && Equals((Vector3i)obj);
        }

        public bool Equals(Vector3i obj)
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
