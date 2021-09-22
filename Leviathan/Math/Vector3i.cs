using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

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
