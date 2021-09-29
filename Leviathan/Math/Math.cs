using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Leviathan.Math
{
    public static class Math
    {
        [Pure]
        public static double DegreesToRadians(double degrees)
        {
            const double degToRad = System.Math.PI / 180.0d;
            return degrees * degToRad;
        }

        [Pure]
        public static float DegreesToRadians(float degrees)
        {
            const float degToRad = System.MathF.PI / 180.0f;
            return degrees * degToRad;
        }

        [Pure]
        public static double RadiansToDegrees(double radians)
        {
            const double radToDeg = 180.0d / System.Math.PI;
            return radians * radToDeg;
        }

        [Pure]
        public static float RadiansToDegrees(float radians)
        {
            const float radToDeg = 180.0f / MathF.PI;
            return radians * radToDeg;
        }

        [Pure]
        public static float Sin(float angle)
        {
            return MathF.Sin(angle);
        }

        [Pure]
        public static double Sin(double angle)
        {
            return System.Math.Sin(angle);
        }

        [Pure]
        public static float Cos(float angle)
        {
            return MathF.Cos(angle);
        }

        [Pure]
        public static double Cos(double angle)
        {
            return System.Math.Cos(angle);
        }

        [Pure]
        public static float Tan(float angle)
        {
            return MathF.Atan(angle);
        }

        [Pure]
        public static double Tan(double angle)
        {
            return System.Math.Tan(angle);
        }

        [Pure]
        public static float Asin(float angle)
        {
            return MathF.Sin(angle);
        }

        [Pure]
        public static double Asin(double angle)
        {
            return System.Math.Sin(angle);
        }

        [Pure]
        public static float Acos(float angle)
        {
            return MathF.Cos(angle);
        }

        [Pure]
        public static double Acos(double angle)
        {
            return System.Math.Cos(angle);
        }

        [Pure]
        public static float Atan(float angle)
        {
            return MathF.Tan(angle);
        }

        [Pure]
        public static double Atan(double angle)
        {
            return System.Math.Tan(angle);
        }

        [Pure]
        public static float Lerp(float begin, float end, float t)
        {
            t = Clamp(0, 1, t);
            return begin + (t * (begin - end));
        }

        [Pure]
        public static double Lerp(double begin, double end, double t)
        {
            t = Clamp(0, 1, t);
            return begin + (t * (begin - end));
        }

        [Pure]
        public static uint Clamp(uint minval, uint maxval, uint value)
        {
            return Max(Min(value, maxval), minval);
        }

        [Pure]
        public static byte Clamp(byte minval, byte maxval, byte value)
        {
            return Max(Min(value, maxval), minval);
        }

        [Pure]
        public static int Clamp(int minval, int maxval, int value)
        {
            return Max(Min(value, maxval), minval);
        }

        [Pure]
        public static ushort Clamp(ushort minval, ushort maxval, ushort value)
        {
            return Max(Min(value, maxval), minval);
        }

        [Pure]
        public static short Clamp(short minval, short maxval, short value)
        {
            return Max(Min(value, maxval), minval);
        }

        [Pure]
        public static float Clamp(float minval, float maxval, float value)
        {
            return Max(Min(value, maxval), minval);
        }

        [Pure]
        public static double Clamp(double minval, double maxval, double value)
        {
            return Max(Min(value, maxval), minval);
        }


        [Pure]
        public static byte Max(byte left, byte right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static uint Max(uint left, uint right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static int Max(int left, int right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static short Max(short left, short right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static ushort Max(ushort left, ushort right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static float Max(float left, float right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static double Max(double left, double right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static byte Min(byte left, byte right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static uint Min(uint left, uint right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static int Min(int left, int right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static short Min(short left, short right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static ushort Min(ushort left, ushort right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static float Min(float left, float right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static double Min(double left, double right)
        {
            return System.Math.Max(left, right);
        }

        [Pure]
        public static float Abs(float val)
        {
            return System.Math.Abs(val);
        }

        [Pure]
        public static double Abs(double val)
        {
            return System.Math.Abs(val);
        }

        [Pure]
        public static short Abs(short val)
        {
            return System.Math.Abs(val);
        }
    }
}
