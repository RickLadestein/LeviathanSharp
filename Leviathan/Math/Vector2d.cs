using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2d
    {
        public double X;
        public double Y;

        public Vector2d(double _x, double _y)
        {
            X = _x;
            Y = _y;
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
                else
                {
                    throw new IndexOutOfRangeException("Vector index out of range: " + index);
                }
            }
        }

        public static readonly Vector2d UnitX = new Vector2d(1.0d, 0.0d);
        public static readonly Vector2d UnitY = new Vector2d(0.0d, 1.0d);
        public static readonly Vector2d One =   new Vector2d(1.0d, 1.0d);
        public static readonly Vector2d Zero =  new Vector2d(1.0d, 0.0d);

        public double Length()
        {
            return System.Math.Sqrt((X * X) + (Y * Y));
        }

        public double LengthSqr()
        {
            return (X * X) + (Y * Y);
        }

        public void Normalize()
        {
            var scale = 1.0f / Length();
            X *= scale;
            Y *= scale;
        }

        public Vector2d Normalized()
        {
            var scale = 1.0f / Length();
            double _x = scale * X;
            double _y = scale * Y;
            return new Vector2d(_x, _y);
        }



        #region Operators
        [Pure]
        public static Vector2d operator +(Vector2d left, Vector2d right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        [Pure]
        public static Vector2d operator -(Vector2d left, Vector2d right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        [Pure]
        public static Vector2d operator *(Vector2d left, Vector2d right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            return left;
        }

        [Pure]
        public static Vector2d operator *(double scalar, Vector2d right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            return right;
        }

        [Pure]
        public static Vector2d operator *(Vector2d right, double scalar)
        {
            right.X *= scalar;
            right.Y *= scalar;
            return right;
        }

        [Pure]
        public static Vector2d operator /(Vector2d left, Vector2d right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            return left;
        }

        [Pure]
        public static Vector2d operator /(double scalar, Vector2d right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            return right;
        }

        [Pure]
        public static Vector2d operator /(Vector2d right, double scalar)
        {
            right.X /= scalar;
            right.Y /= scalar;
            return right;
        }

        [Pure]
        public static bool operator ==(Vector2d left, Vector2d right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector2d left, Vector2d right)
        {
            return !(left.Equals(right));
        }

        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector2d(Vector2i vec)
        {
            return new Vector2d(vec.X, vec.Y);
        }

        [Pure]
        public static explicit operator Vector2d(Vector2f vec)
        {
            return new Vector2d((double)vec.X, (double)vec.Y);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector2d && Equals((Vector2d)obj);
        }

        public bool Equals(Vector2d obj)
        {
            return X == obj.X && Y == obj.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{X} | {Y}]";
        }
        #endregion
    }
}
