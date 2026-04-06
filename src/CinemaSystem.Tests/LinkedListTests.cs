using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Tests
{
    [TestClass]
    public class LinkedListTests
    {
        [TestMethod]
        public void TestInsertAtHead()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtHead(10);
            list.InsertAtHead(20);

            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void TestInsertAtTail()
        {
            CustomLinkedList<string> list = new CustomLinkedList<string>();
            list.InsertAtTail("first");
            list.InsertAtTail("second");

            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void TestSearch()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtTail(10);
            list.InsertAtTail(20);
            list.InsertAtTail(30);

            int result = list.Search(x => x == 20);
            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void TestSearchNotFound()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtTail(10);
            list.InsertAtTail(20);

            int result = list.Search(x => x == 99);
            Assert.AreEqual(0, result); // default int is 0
        }

        [TestMethod]
        public void TestDeleteHead()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtTail(10);
            list.InsertAtTail(20);

            bool deleted = list.Delete(x => x == 10);
            Assert.IsTrue(deleted);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void TestDeleteMiddle()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtTail(10);
            list.InsertAtTail(20);
            list.InsertAtTail(30);

            bool deleted = list.Delete(x => x == 20);
            Assert.IsTrue(deleted);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void TestDeleteNotFound()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtTail(10);

            bool deleted = list.Delete(x => x == 99);
            Assert.IsFalse(deleted);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void TestToArray()
        {
            CustomLinkedList<string> list = new CustomLinkedList<string>();
            list.InsertAtTail("a");
            list.InsertAtTail("b");
            list.InsertAtTail("c");

            string[] arr = list.ToArray();
            Assert.AreEqual(3, arr.Length);
            Assert.AreEqual("a", arr[0]);
            Assert.AreEqual("c", arr[2]);
        }

        [TestMethod]
        public void TestSearchAll()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.InsertAtTail(5);
            list.InsertAtTail(10);
            list.InsertAtTail(15);
            list.InsertAtTail(20);

            int[] results = list.SearchAll(x => x > 10);
            Assert.AreEqual(2, results.Length);
        }

        [TestMethod]
        public void TestEmptyList()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();

            Assert.AreEqual(0, list.Count);
            bool deleted = list.Delete(x => x == 1);
            Assert.IsFalse(deleted);
        }
    }
}