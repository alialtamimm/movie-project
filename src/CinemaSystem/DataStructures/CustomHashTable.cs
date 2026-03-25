using System;
using CinemaSystem.Models;

namespace CinemaSystem.DataStructures
{
    // Custom hash table using separate chaining for collisions
    public class CustomHashTable<TKey, TValue>
    {
        private HashNode<TKey, TValue>[] buckets;
        private int size;
        private int count;

        public int Count { get { return count; } }

        public CustomHashTable(int tableSize)
        {
            size = tableSize;
            buckets = new HashNode<TKey, TValue>[size];
            count = 0;
        }

        // Hash function - gets the index for a key
        private int GetHashIndex(TKey key)
        {
            int hash = Math.Abs(key.GetHashCode());
            return hash % size;
        }

        // Insert a key value pair into the table
        public void Insert(TKey key, TValue value)
        {
            int index = GetHashIndex(key);

            // Check if key already exists, if so update it
            HashNode<TKey, TValue> current = buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    current.Value = value;
                    return;
                }
                current = current.Next;
            }

            // Key doesnt exist so add new node at the front of the chain
            HashNode<TKey, TValue> newNode = new HashNode<TKey, TValue>(key, value);
            newNode.Next = buckets[index];
            buckets[index] = newNode;
            count++;
        }

        // Search for a value by key, returns default if not found
        public TValue Search(TKey key)
        {
            int index = GetHashIndex(key);

            HashNode<TKey, TValue> current = buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    return current.Value;
                }
                current = current.Next;
            }

            // Not found
            return default(TValue);
        }

        // Check if a key exists in the table
        public bool ContainsKey(TKey key)
        {
            int index = GetHashIndex(key);

            HashNode<TKey, TValue> current = buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        // Delete a key value pair
        public bool Delete(TKey key)
        {
            int index = GetHashIndex(key);

            HashNode<TKey, TValue> current = buckets[index];
            HashNode<TKey, TValue> previous = null;

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (previous == null)
                    {
                        // Its the first node in the chain
                        buckets[index] = current.Next;
                    }
                    else
                    {
                        // Skip over the node to remove it
                        previous.Next = current.Next;
                    }
                    count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }

            return false;
        }

        // Get all values in the table as an array
        public TValue[] GetAllValues()
        {
            TValue[] values = new TValue[count];
            int i = 0;

            for (int b = 0; b < size; b++)
            {
                HashNode<TKey, TValue> current = buckets[b];
                while (current != null)
                {
                    values[i] = current.Value;
                    i++;
                    current = current.Next;
                }
            }

            return values;
        }

        // Print everything in the table
        public void Display()
        {
            for (int i = 0; i < size; i++)
            {
                HashNode<TKey, TValue> current = buckets[i];
                if (current != null)
                {
                    Console.Write($"Bucket {i}: ");
                    while (current != null)
                    {
                        Console.Write($"[{current.Key}: {current.Value}] -> ");
                        current = current.Next;
                    }
                    Console.WriteLine("null");
                }
            }
        }
    }
}