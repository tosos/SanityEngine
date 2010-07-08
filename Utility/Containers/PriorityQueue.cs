using System;
using System.Collections.Generic;

namespace SanityEngine.Utility.Containers
{
    /// <summary>
    /// A simple heap based priority queue. Specially tailored for use
    /// in best first search.
    /// </summary>
    /// <typeparam name="T">The type contained in the queue.</typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> data = new List<T>();

        /// <summary>
        /// Add an item into the queue.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void Enqueue(T item)
        {
            int index = data.Count;
            data.Add(item);
            Promote(index);
        }

        /// <summary>
        /// Remove the highest priority item from the queue.
        /// </summary>
        /// <returns>The highest priority item.</returns>
        public T Dequeue()
        {
            if (Empty)
            {
                throw new IndexOutOfRangeException();
            }

            T item = data[0];
            int last = data.Count - 1;
            data[0] = data[last];
            data.RemoveAt(last);

            Demote(0);

            return item;
        }

        /// <summary>
        /// Get the first element without removing.
        /// </summary>
        /// <returns>The first element.</returns>
        public T Peek()
        {
            if (Empty)
            {
                throw new IndexOutOfRangeException();
            }

            return data[0];
        }

        /// <summary>
        /// Set to <code>true</code> if this queue is empty.
        /// </summary>
        public bool Empty
        {
            get { return data.Count <= 0; }
        }

        /// <summary>
        /// Clear the queue's contents.
        /// </summary>
        public void Clear()
        {
            data.Clear();
        }

        /// <summary>
        /// Checks to see if the queue contains the given item.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns><code>true</code> if the item is in the queue.</returns>
        public bool Contains(T item)
        {
            return data.Contains(item);
        }

        /// <summary>
        /// The number of items in the queue.
        /// </summary>
        public int Count
        {
            get { return data.Count; }
        }

        /// <summary>
        /// Corrects the item's position in the queue if the value has changed.
        /// </summary>
        /// <param name="item">The item to fix.</param>
        public void Fix(T item)
        {
            int index = data.IndexOf(item);
            if (index < 0)
            {
                return;
            }

            FixAt(index);
        }

        /// <summary>
        /// Remove the item from the priority queue.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void Remove(T item)
        {
            int index = data.IndexOf(item);
            if (index < 0)
            {
                return;
            }
            int last = data.Count - 1;
            if (index == last)
            {
                data.RemoveAt(last);
            }
            else
            {
                Swap(index, last);
                data.RemoveAt(last);
                FixAt(index);
            }
        }

        private void FixAt(int index)
        {
            int parent = Parent(index);
            if (data[parent].CompareTo(data[index]) > 0)
            {
                Promote(index);
            }
            else
            {
                Demote(index);
            }
        }

        private void Promote(int index)
        {
            int parent = Parent(index);
            if (data[parent].CompareTo(data[index]) > 0)
            {
                Swap(parent, index);
                Promote(parent);
            }
        }

        private void Demote(int index)
        {
            int left = Left(index);
            int right = Right(index);
            if (left >= data.Count)
            {
                // We've reached the end
                return;
            }

            if (right >= data.Count)
            {
                // Check the left side only
                if (left < data.Count && data[index].CompareTo(data[left]) > 0)
                {
                    Swap(index, left);
                    Demote(left);
                }
                return;
            }

            // Otherwise, compare to the smallest
            if (data[left].CompareTo(data[right]) < 0)
            {
                if (data[index].CompareTo(data[left]) > 0)
                {
                    Swap(index, left);
                    Demote(left);
                }
            }
            else
            {
                if (data[index].CompareTo(data[right]) > 0)
                {
                    Swap(index, right);
                    Demote(right);
                }
            }
        }

        private void Swap(int i1, int i2)
        {
            T tmp = data[i1];
            data[i1] = data[i2];
            data[i2] = tmp;
        }

        private static int Parent(int index)
        {
            return (index - 1) / 2;
        }

        private static int Left(int index)
        {
            return (index * 2) + 1;
        }

        private static int Right(int index)
        {
            return (index * 2) + 2;
        }
    }
}