using System;

namespace CinemaSystem.DataStructures
{
    // node for the linked list
    public class ListNode<T>
    {
        public T Data { get; set; }
        public ListNode<T> Next { get; set; }

        public ListNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    // custom linked list for storing bookings, customers etc
    public class CustomLinkedList<T>
    {
        private ListNode<T> head;
        private int count;

        public int Count { get { return count; } }

        public CustomLinkedList()
        {
            head = null;
            count = 0;
        }

        // add to the front
        public void InsertAtHead(T data)
        {
            ListNode<T> newNode = new ListNode<T>(data);
            newNode.Next = head;
            head = newNode;
            count++;
        }

        // add to the end
        public void InsertAtTail(T data)
        {
            ListNode<T> newNode = new ListNode<T>(data);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                ListNode<T> current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            count++;
        }

        // search using a condition, returns first match or default
        public T Search(Func<T, bool> condition)
        {
            ListNode<T> current = head;
            while (current != null)
            {
                if (condition(current.Data))
                {
                    return current.Data;
                }
                current = current.Next;
            }
            return default(T);
        }

        // get all items that match a condition
        public T[] SearchAll(Func<T, bool> condition)
        {
            // first count how many match
            int matchCount = 0;
            ListNode<T> current = head;
            while (current != null)
            {
                if (condition(current.Data))
                {
                    matchCount++;
                }
                current = current.Next;
            }

            // now fill array
            T[] results = new T[matchCount];
            current = head;
            int i = 0;
            while (current != null)
            {
                if (condition(current.Data))
                {
                    results[i] = current.Data;
                    i++;
                }
                current = current.Next;
            }

            return results;
        }

        // delete first item that matches condition
        public bool Delete(Func<T, bool> condition)
        {
            if (head == null) return false;

            // check if its the head
            if (condition(head.Data))
            {
                head = head.Next;
                count--;
                return true;
            }

            ListNode<T> current = head;
            while (current.Next != null)
            {
                if (condition(current.Next.Data))
                {
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        // get all items as an array
        public T[] ToArray()
        {
            T[] arr = new T[count];
            ListNode<T> current = head;
            int i = 0;
            while (current != null)
            {
                arr[i] = current.Data;
                i++;
                current = current.Next;
            }
            return arr;
        }
    }
}