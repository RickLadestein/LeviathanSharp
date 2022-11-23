using Leviathan.Util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Util.UnitTest
{
    public class LinkedListTest
    {
        [Test]
        public void AddTest()
        {
            Leviathan.Util.Collections.LinkedList<int>  list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            for (int i = 0; i < testdata.Length; i++)
            {
                list.Add(testdata[i]);
                Assert.That(list[i], Is.EqualTo(testdata[i]));
            }
        }

        [Test]
        public void AddRangeTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);
            for(int i = 0; i < testdata.Length; i++)
            {
                Assert.That(list[i], Is.EqualTo(testdata[i]));
            }
        }

        [Test]
        public void ContainsTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);

            bool shouldcontain = list.Contains(5);
            bool shouldnotcontain = list.Contains(11);

            Assert.IsTrue(shouldcontain);
            Assert.IsFalse(shouldnotcontain);
        }

        [Test]
        public void ClearTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);

            list.Clear();
            Assert.IsTrue(list.Count == 0);
        }

        [Test]
        public void GetIndexTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);

            Assert.IsTrue(list.GetIndex(0) == testdata[0]);
            Assert.IsTrue(list.GetIndex(2) == testdata[2]);
            Assert.IsTrue(list.GetIndex(4) == testdata[4]);
            Assert.IsTrue(list.GetIndex(5) == testdata[5]);
            Assert.IsTrue(list.GetIndex(6) == testdata[6]);
        }

        [Test]
        public void SetIndexTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);
            
            list.SetIndex(0, 10);
            Assert.IsTrue(list.GetIndex(0) == 10);

            list.SetIndex(5, 50);
            Assert.IsTrue(list.GetIndex(5) == 50);
        }

        [Test]
        public void RemoveTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);

            list.Remove(5);
            list.Remove(1);

            Assert.IsTrue(list.Count == 8);
            Assert.IsFalse(list.Contains(5));
            Assert.IsFalse(list.Contains(1));
        }

        [Test]
        public void RemoveIndexTest()
        {
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);

            list.RemoveIndex(5);
            list.RemoveIndex(1);
            list.RemoveIndex(0);

            Assert.IsTrue(list.Count == 7);
            Assert.IsFalse(list.Contains(testdata[5]));
            Assert.IsFalse(list.Contains(testdata[1]));
            Assert.IsFalse(list.Contains(testdata[0]));
        }

        [Test]
        public void RemoveAllTest()
        {
            return;
            Leviathan.Util.Collections.LinkedList<int> list = new Leviathan.Util.Collections.LinkedList<int>();
            int[] testdata = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            list.AddRange(testdata);

            list.RemoveAll(new Predicate<int>((e) => { return (e % 2) == 0; }));
            Assert.IsTrue(list.Count == 5);
            Assert.IsTrue(list.Contains(1));
            Assert.IsFalse(list.Contains(2));
            Assert.IsTrue(list.Contains(3));
            Assert.IsFalse(list.Contains(4));
            Assert.IsTrue(list.Contains(5));
            Assert.IsFalse(list.Contains(6));
            Assert.IsTrue(list.Contains(7));
            Assert.IsFalse(list.Contains(8));
            Assert.IsTrue(list.Contains(9));
            Assert.IsFalse(list.Contains(10));

        }
    }
}
