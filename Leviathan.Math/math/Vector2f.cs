using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2f
    {
        public float X;
        public float Y;

        public Vector2f(float _x, float _y)
        {
            X = _x;
            Y = _y;
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

        public static readonly Vector2f UnitX = new Vector2f(1.0f, 0.0f);
        public static readonly Vector2f UnitY = new Vector2f(0.0f, 1.0f);
        public static readonly Vector2f One =   new Vector2f(1.0f, 1.0f);
        public static readonly Vector2f Zero =  new Vector2f(0.0f, 0.0f);


        public void Set(float _x, float _y)
        {
            X = _x;
            Y = _y;
        }
        public float Length()
        {
            return MathF.Sqrt((X * X) + (Y * Y));
        }

        public float LengthSqr()
        {
            return (X * X) + (Y * Y);
        }

        public void Normalize()
        {
            var scale = 1.0f / Length();
            X *= scale;
            Y *= scale;
        }

        #region Static_Funcs
        public static Vector2f Normalize(Vector2f v)
        {
            float scale = 1.0f / v.Length();
            float _x = scale * v.X;
            float _y = scale * v.Y;
            return new Vector2f(_x, _y);
        }

        public static float Dot(Vector2f v1, Vector2f v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }


        public static Vector2f Lerp(Vector2f v1, Vector2f v2, float t)
        {
            return v1 + (v2 - v1) * t;
        }
        #endregion

        #region Operators
        [Pure]
        public static Vector2f operator +(Vector2f left, Vector2f right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        [Pure]
        public static Vector2f operator -(Vector2f vec)
        {
            return new Vector2f(-vec.X, -vec.Y);
        }

        [Pure]
        public static Vector2f operator -(Vector2f left, Vector2f right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        [Pure]
        public static Vector2f operator *(Vector2f left, Vector2f right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            return left;
        }

        [Pure]
        public static Vector2f operator *(float scalar, Vector2f right)
        {
            right.X *= scalar;
            right.Y *= scalar;
            return right;
        }

        [Pure]
        public static Vector2f operator *(Vector2f right, float scalar)
        {
            right.X *= scalar;
            right.Y *= scalar;
            return right;
        }

        [Pure]
        public static Vector2f operator /(Vector2f left, Vector2f right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            return left;
        }

        [Pure]
        public static Vector2f operator /(float scalar, Vector2f right)
        {
            right.X /= scalar;
            right.Y /= scalar;
            return right;
        }

        [Pure]
        public static Vector2f operator /(Vector2f right, float scalar)
        {
            right.X /= scalar;
            right.Y /= scalar;
            return right;
        }

        [Pure]
        public static bool operator ==(Vector2f left, Vector2f right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector2f left, Vector2f right)
        {
            return !(left.Equals(right));
        }

        #endregion

        #region Conversion

        [Pure]
        public static explicit operator Vector2f(Vector2i vec)
        {
            return new Vector2f(vec.X, vec.Y);
        }

        [Pure]
        public static explicit operator Vector2f(Vector2d vec)
        {
            return new Vector2f((float)vec.X, (float)vec.Y);
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is Vector2f && Equals((Vector2f)obj);
        }

        public bool Equals(Vector2f obj)
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
