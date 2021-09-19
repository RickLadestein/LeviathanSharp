using NUnit.Framework;
using Leviathan.Math;
using System;

namespace LeviathanUnitTest
{
    class Vector2fTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConstructorTest()
        {
            Vector2f vector = new Vector2f(1.0f, 1.0f);
            Assert.IsTrue(vector.X == 1.0f);
            Assert.IsTrue(vector.X == 1.0f);
        }

        [Test]
        public void LengthTest()
        {
            Vector2f vector = new Vector2f(1.0f, 1.0f);
            Assert.IsTrue(vector.Length() == MathF.Sqrt(2));
        }

        [Test]
        public void LengthSqrTest()
        {
            Vector2f vector = new Vector2f(1.0f, 1.0f);
            Assert.IsTrue(vector.LengthSqr() == 2);
        }

        [Test]
        public void NormalizeTest()
        {
            Vector2f vector = new Vector2f(2.0f, 2.0f);
            vector.Normalize();
            float truth = MathF.Sqrt(2) / 2.0f;
            Assert.IsTrue(vector.X == truth && vector.Y == truth);
        }

        [Test]
        public void NormalizedTest()
        {
            Vector2f vector = new Vector2f(2.0f, 2.0f);
            Vector2f norm = vector.Normalized();
            float truth = MathF.Sqrt(2) / 2.0f;
            Assert.IsTrue(norm.X == truth && norm.Y == truth);
        }

        [Test]
        public void OperatorPlusTest()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            Vector2f v2 = new Vector2f(2.0f, 2.0f);
            Vector2f v3 = v1 + v2;
            Assert.IsTrue(v3.X == 4.0f && v3.Y == 4.0f);
        }

        [Test]
        public void OperatorMinusTest()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            Vector2f v2 = new Vector2f(2.0f, 2.0f);
            Vector2f v3 = v1 - v2;
            Assert.IsTrue(v3.X == 0.0f && v3.Y == 0.0f);
        }

        [Test]
        public void OperatorMultiply1Test()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            Vector2f v2 = new Vector2f(2.0f, 2.0f);
            Vector2f v3 = v1 * v2;
            Assert.IsTrue(v3.X == 4.0f && v3.Y == 4.0f);
        }

        [Test]
        public void OperatorMultiply2Test()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            float scalar = 2;
            Vector2f v3 = v1 * scalar;
            Assert.IsTrue(v3.X == 4.0f && v3.Y == 4.0f);
        }

        [Test]
        public void OperatorMultiply3Test()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            float scalar = 2;
            Vector2f v3 = scalar * v1;
            Assert.IsTrue(v3.X == 4.0f && v3.Y == 4.0f);
        }

        [Test]
        public void OperatorDevide1Test()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            Vector2f v2 = new Vector2f(2.0f, 2.0f);
            Vector2f v3 = v1 / v2;
            Assert.IsTrue(v3.X == 1.0f && v3.Y == 1.0f);
        }

        [Test]
        public void OperatorDevide2Test()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            float scalar = 2;
            Vector2f v3 = v1 / scalar;
            Assert.IsTrue(v3.X == 1.0f && v3.Y == 1.0f);
        }

        [Test]
        public void OperatorDevide3Test()
        {
            Vector2f v1 = new Vector2f(2.0f, 2.0f);
            float scalar = 2;
            Vector2f v3 = scalar / v1;
            Assert.IsTrue(v3.X == 1.0f && v3.Y == 1.0f);
        }
    }
}
