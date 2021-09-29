using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3d
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3d(double _x, double _y, double _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }

        public double this[int index]
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

                if(index == 2)
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
                } else if (index == 2)
                {
                    Z = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Vector index out of range: " + index);
                }
            }
        }

        public static readonly Vector3d UnitX = new Vector3d(1.0d, 0.0d, 0.0d);
        public static readonly Vector3d UnitY = new Vector3d(0.0d, 1.0d, 0.0d);
        public static readonly Vector3d UnitZ = new Vector3d(0.0d, 0.0d, 1.0d);
        public static readonly Vector3d Up =    new Vector3d(0.0d, 1.0d, 0.0d);
        public static readonly Vector3d One =   new Vector3d(1.0d, 1.0d, 1.0d);
        public static readonly Vector3d Zero =  new Vector3d(0.0d, 0.0d, 0.0d);

        public void Set(double _x, double _y, double _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }
        public double Length()
        {
            return System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public double LengthSqr()
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

        public Vector3d Normalized()
        {
            var scale = 1.0f / Length();
            double _x = scale * X;
            double _y = scale * Y;
            double _z = scale * Z;
            return new Vector3d(_x, _y, _z);
        }

        public static double Dot(Vector3d v1, Vector3d v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static Vector3d Cross(Vector3d left, Vector3d right)
        {
            Vector3d result = Vector3d.Zero;
            result.X = (left.Y * right.Z) - (left.Z * right.Y);
            result.Y = (left.Z * right.X) - (left.X * right.Z);
            result.Z = (left.X * right.Y) - (left.Y * right.X);
            return result;
        }

        #region Operators
        [Pure]
        public static Vector3d operator +(Vector3d left, Vector3d right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        [Pure]
        public static Vector3d operator -(Vector3d vec)
        {
            return new Vector3d(-vec.X, -vec.Y, -vec.Z);
        }

        [Pure]
        public static Vector3d operator -(Vector3d left, Vector3d right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        [Pure]
        public static Vector3d operator *(Vector3d left, Vector3d right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            return left;
        }

        [Pure]
        public static Vector3d operator *(double scalar, Vector3d right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            right.Z *= scalar;
            return right;
        }

        [Pure]
        public static Vector3d operator *(Vector3d left, double scalar)
        {
            left.X *= scalar;
            left.Y *= scalar;
            left.Z *= scalar;
            return left;
        }

        [Pure]
        public static Vector3d operator /(Vector3d left, Vector3d right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            return left;
        }

        [Pure]
        public static Vector3d operator /(double scalar, Vector3d right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            right.Z /= scalar;
            return right;
        }

        [Pure]
        public static Vector3d operator /(Vector3d left, double scalar)
        {
            left.X /= scalar;
            left.Y /= scalar;
            left.Z /= scalar;
            return left;
        }

        [Pure]
        public static bool operator ==(Vector3d left, Vector3d right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector3d left, Vector3d right)
        {
            return !(left.Equals(right));
        }
        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector3d(Vector3i vec)
        {
            return new Vector3d(vec.X, vec.Y, vec.Z);
        }

        [Pure]
        public static explicit operator Vector3d(Vector3f vec)
        {
            return new Vector3d((double)vec.X, (double)vec.Y, (double)vec.Z);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector3d && Equals((Vector3d)obj);
        }

        public bool Equals(Vector3d obj)
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
