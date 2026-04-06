using System;

namespace CinemaSystem.DataStructures
{
    // custom hash table using separate chaining for collisions
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

        // hash function - gets the index for a key
        private int GetHashIndex(TKey key)
        {
            int hash = Math.Abs(key.GetHashCode());
            return hash % size;
        }

        // insert a key value pair into the table
        public void Insert(TKey key, TValue value)
        {
            int index = GetHashIndex(key);

            // check if key already exists, if so update it
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

            // key doesnt exist so add new node at the front of the chain
            HashNode<TKey, TValue> newNode = new HashNode<TKey, TValue>(key, value);
            newNode.Next = buckets[index];
            buckets[index] = newNode;
            count++;
        }

        // search for a value by key, returns default if not found
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

            return default(TValue);
        }

        // check if a key exists in the table
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

        // delete a key value pair
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
                        // its the first node in the chain
                        buckets[index] = current.Next;
                    }
                    else
                    {
                        // skip over the node to remove it
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

        // get all values in the table as an array
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
    }
}