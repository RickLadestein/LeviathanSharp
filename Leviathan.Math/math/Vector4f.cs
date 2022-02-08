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

        public Vector3f Xyz
        {
            get
            {
                return new Vector3f(X, Y, Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4f(float _x, float _y, float _z, float _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
        }
        public Vector4f(Vector3f vec, float _w)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
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

        public static readonly Vector4f UnitX       = new Vector4f(1.0f, 0.0f, 0.0f, 0.0f);
        public static readonly Vector4f UnitY       = new Vector4f(0.0f, 1.0f, 0.0f, 0.0f);
        public static readonly Vector4f UnitZ       = new Vector4f(0.0f, 0.0f, 1.0f, 0.0f);
        public static readonly Vector4f UnitW       = new Vector4f(0.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Vector4f One         = new Vector4f(1.0f, 1.0f, 1.0f, 1.0f);
        public static readonly Vector4f Zero        = new Vector4f(0.0f, 0.0f, 0.0f, 0.0f);

        public void Set(float _x, float _y, float _z, float _w)
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

        public static float Dot(Vector4f v1, Vector4f v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) + (v1.W * v2.W);
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4f TransformColumn(Mat4 mat, Vector4f vec)
        {
            TransformColumn(in mat, in vec, out Vector4f result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4f TransformRow(Vector4f vec, Mat4 mat)
        {
            TransformRow(in vec, in mat, out Vector4f result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformRow(in Vector4f vec, in Mat4 mat, out Vector4f result)
        {
            result = new Vector4f(
                (vec.X * mat.Row0.X) + (vec.Y * mat.Row1.X) + (vec.Z * mat.Row2.X) + (vec.W * mat.Row3.X),
                (vec.X * mat.Row0.Y) + (vec.Y * mat.Row1.Y) + (vec.Z * mat.Row2.Y) + (vec.W * mat.Row3.Y),
                (vec.X * mat.Row0.Z) + (vec.Y * mat.Row1.Z) + (vec.Z * mat.Row2.Z) + (vec.W * mat.Row3.Z),
                (vec.X * mat.Row0.W) + (vec.Y * mat.Row1.W) + (vec.Z * mat.Row2.W) + (vec.W * mat.Row3.W));
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformColumn(in Mat4 mat, in Vector4f vec, out Vector4f result)
        {
            result = new Vector4f(
                (mat.Row0.X * vec.X) + (mat.Row0.Y * vec.Y) + (mat.Row0.Z * vec.Z) + (mat.Row0.W * vec.W),
                (mat.Row1.X * vec.X) + (mat.Row1.Y * vec.Y) + (mat.Row1.Z * vec.Z) + (mat.Row1.W * vec.W),
                (mat.Row2.X * vec.X) + (mat.Row2.Y * vec.Y) + (mat.Row2.Z * vec.Z) + (mat.Row2.W * vec.W),
                (mat.Row3.X * vec.X) + (mat.Row3.Y * vec.Y) + (mat.Row3.Z * vec.Z) + (mat.Row3.W * vec.W));
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
        public static Vector4f operator -(Vector4f vec)
        {
            return new Vector4f(-vec.X, -vec.Y, -vec.Z, -vec.W);
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

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4f operator *(Mat4 mat, Vector4f vec)
        {
            TransformRow(in vec, in mat, out Vector4f result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4f operator *(Vector4f vec, Mat4 mat)
        {
            TransformColumn(in mat, in vec, out Vector4f result);
            return result;
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
