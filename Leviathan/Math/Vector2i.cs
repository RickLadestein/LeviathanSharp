using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i
    {
        public int X;
        public int Y;

        public Vector2i(int _x, int _y)
        {
            X = _x;
            Y = _y;
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

        public static readonly Vector2i UnitX = new Vector2i(1, 0);
        public static readonly Vector2i UnitY = new Vector2i(0, 1);
        public static readonly Vector2i One =   new Vector2i(1, 1);
        public static readonly Vector2i Zero =  new Vector2i(0, 0);

        public void Set(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        public float Length()
        {
            return MathF.Sqrt((X * X) + (Y * Y));
        }

        public int LengthSqr()
        {
            return (X * X) + (Y * Y);
        }

        public void Normalize()
        {
            var scale = 1.0f / Length();
            X = (int)(scale * X);
            Y = (int)(scale * Y);
        }

        public Vector2i Normalized()
        {
            var scale = 1.0f / Length();
            int _x = (int) (scale * X);
            int _y = (int) (scale * Y);
            return new Vector2i(_x, _y);
        }



        #region Operators
        [Pure]
        public static Vector2i operator +(Vector2i left, Vector2i right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        [Pure]
        public static Vector2i operator -(Vector2i left, Vector2i right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        [Pure]
        public static Vector2i operator *(Vector2i left, Vector2i right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            return left;
        }

        [Pure]
        public static Vector2i operator *(int scalar, Vector2i right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            return right;
        }

        [Pure]
        public static Vector2i operator *(Vector2i right, int scalar)
        {
            right.X *= scalar;
            right.Y *= scalar;
            return right;
        }

        [Pure]
        public static Vector2i operator /(Vector2i left, Vector2i right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            return left;
        }

        [Pure]
        public static Vector2i operator /(int scalar, Vector2i right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            return right;
        }

        [Pure]
        public static Vector2i operator /(Vector2i right, int scalar)
        {
            right.X /= scalar;
            right.Y /= scalar;
            return right;
        }

        [Pure]
        public static bool operator ==(Vector2i left, Vector2i right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector2i left, Vector2i right)
        {
            return !(left.Equals(right));
        }

        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector2i(Vector2f vec)
        {
            return new Vector2i((int)vec.X, (int)vec.Y);
        }

        [Pure]
        public static explicit operator Vector2i(Vector2d vec)
        {
            return new Vector2i((int)vec.X, (int)vec.Y);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector2i && Equals((Vector2i)obj);
        }

        public bool Equals(Vector2i obj)
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
