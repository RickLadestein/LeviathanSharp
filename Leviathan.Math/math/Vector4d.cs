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
    public struct Vector4d
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public Vector3d Xyz
        {
            get
            {
                return new Vector3d(X, Y, Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4d(double _x, double _y, double _z, double _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
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

        public static readonly Vector4d UnitX = new Vector4d(1.0d, 0.0d, 0.0d, 0.0d);
        public static readonly Vector4d UnitY = new Vector4d(0.0d, 1.0d, 0.0d, 0.0d);
        public static readonly Vector4d UnitZ = new Vector4d(0.0d, 0.0d, 1.0d, 0.0d);
        public static readonly Vector4d UnitW = new Vector4d(0.0d, 0.0d, 1.0d, 0.0d);
        public static readonly Vector4d Up =    new Vector4d(0.0d, 1.0d, 0.0d, 0.0d);
        public static readonly Vector4d One =   new Vector4d(1.0d, 1.0d, 1.0d, 1.0d);
        public static readonly Vector4d Zero =  new Vector4d(0.0d, 0.0d, 0.0d, 0.0d);

        public void Set(double _x, double _y, double _z, double _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
        }
        public double Length()
        {
            return System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        public double LengthSqr()
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

        public Vector4d Normalized()
        {
            var scale = 1.0f / Length();
            double _x = scale * X;
            double _y = scale * Y;
            double _z = scale * Z;
            double _w = scale * W;
            return new Vector4d(_x, _y, _z, _w);
        }

        public static double Dot(Vector4d v1, Vector4d v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) + (v1.W * v2.W);
        }

        public static Vector4d Parse(string[] tokens, int begin, NumberFormatInfo format = null)
        {
            if (tokens.Length < 4)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector4d: Expected >= 4 and got {tokens.Length}");
            }

            if (tokens.Length - begin < 0)
            {
                throw new ArgumentException($"Too few arguments given to parse Vector4d: Expected >= 4 parseable tokens and got {tokens.Length - begin} accounting for start index");
            }

            double x, y, z, w;
            if (format != null)
            {
                x = double.Parse(tokens[begin], format);
                y = double.Parse(tokens[begin + 1], format);
                z = double.Parse(tokens[begin + 2], format);
                w = double.Parse(tokens[begin + 4], format);
            }
            else
            {
                x = double.Parse(tokens[begin]);
                y = double.Parse(tokens[begin + 1]);
                z = double.Parse(tokens[begin + 2]);
                w = double.Parse(tokens[begin + 4]);
            }
            return new Vector4d(x, y, z, w);
        }

        public double[] ToArray()
        {
            return new double[4] { X, Y, Z, W };
        }

        #region Operators
        [Pure]
        public static Vector4d operator +(Vector4d left, Vector4d right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        [Pure]
        public static Vector4d operator -(Vector4d vec)
        {
            return new Vector4d(-vec.X, -vec.Y, -vec.Z, -vec.W);
        }

        [Pure]
        public static Vector4d operator -(Vector4d left, Vector4d right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        [Pure]
        public static Vector4d operator *(Vector4d left, Vector4d right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
            return left;
        }

        [Pure]
        public static Vector4d operator *(double scalar, Vector4d right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            right.Z *= scalar;
            right.W *= scalar;
            return right;
        }

        [Pure]
        public static Vector4d operator *(Vector4d left, double scalar)
        {
            left.X *= scalar;
            left.Y *= scalar;
            left.Z *= scalar;
            left.W *= scalar;
            return left;
        }

        [Pure]
        public static Vector4d operator /(Vector4d left, Vector4d right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            left.W /= right.W;
            return left;
        }

        [Pure]
        public static Vector4d operator /(double scalar, Vector4d right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            right.Z /= scalar;
            right.Z /= scalar;
            return right;
        }

        [Pure]
        public static Vector4d operator /(Vector4d left, double scalar)
        {
            left.X /= scalar;
            left.Y /= scalar;
            left.Z /= scalar;
            left.Z /= scalar;
            return left;
        }

        [Pure]
        public static bool operator ==(Vector4d left, Vector4d right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector4d left, Vector4d right)
        {
            return !(left.Equals(right));
        }
        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector4d(Vector4i vec)
        {
            return new Vector4d(vec.X, vec.Y, vec.Z, vec.W);
        }

        [Pure]
        public static explicit operator Vector4d(Vector4f vec)
        {
            return new Vector4d((double)vec.X, (double)vec.Y, (double)vec.Z, (double)vec.W);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector4d && Equals((Vector4d)obj);
        }

        public bool Equals(Vector4d obj)
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
