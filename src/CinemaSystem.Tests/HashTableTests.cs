using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaSystem.DataStructures;
using CinemaSystem.Models;

namespace CinemaSystem.Tests
{
    [TestClass]
    public class HashTableTests
    {
        [TestMethod]
        public void TestInsert()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "test value");

            Assert.AreEqual(1, table.Count);
        }

        [TestMethod]
        public void TestSearch()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "hello");

            string result = table.Search(1);
            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public void TestSearchNotFound()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "hello");

            string result = table.Search(99);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestDelete()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "hello");

            bool deleted = table.Delete(1);
            Assert.IsTrue(deleted);
            Assert.AreEqual(0, table.Count);
        }

        [TestMethod]
        public void TestDeleteNotFound()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "hello");

            bool deleted = table.Delete(99);
            Assert.IsFalse(deleted);
            Assert.AreEqual(1, table.Count);
        }

        [TestMethod]
        public void TestContainsKey()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(5, "exists");

            Assert.IsTrue(table.ContainsKey(5));
            Assert.IsFalse(table.ContainsKey(10));
        }

        [TestMethod]
        public void TestUpdateExistingKey()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "old value");
            table.Insert(1, "new value");

            string result = table.Search(1);
            Assert.AreEqual("new value", result);
            Assert.AreEqual(1, table.Count); // should still be 1 not 2
        }

        [TestMethod]
        public void TestMultipleInserts()
        {
            CustomHashTable<int, Film> table = new CustomHashTable<int, Film>(10);
            Film f1 = new Film(1, "Inception", "Sci-Fi", 148, "12A", "18:30", 12.99m, 50);
            Film f2 = new Film(2, "Batman", "Action", 152, "12A", "20:00", 14.99m, 50);

            table.Insert(f1.FilmId, f1);
            table.Insert(f2.FilmId, f2);

            Assert.AreEqual(2, table.Count);
            Assert.AreEqual("Inception", table.Search(1).Title);
            Assert.AreEqual("Batman", table.Search(2).Title);
        }

        [TestMethod]
        public void TestGetAllValues()
        {
            CustomHashTable<int, string> table = new CustomHashTable<int, string>(10);
            table.Insert(1, "one");
            table.Insert(2, "two");
            table.Insert(3, "three");

            string[] values = table.GetAllValues();
            Assert.AreEqual(3, values.Length);
        }
    }
}