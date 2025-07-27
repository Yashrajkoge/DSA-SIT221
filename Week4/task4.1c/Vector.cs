using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace Vector
{
    public class Vector<T> : IEnumerable<T> where T : IComparable<T>
    {
        // This constant determines the default number of elements in a newly created vector.
        // It is also used to extended the capacity of the existing vector
        private const int DEFAULT_CAPACITY = 10;

        // This array represents the internal data structure wrapped by the vector class.
        // In fact, all the elements are to be stored in this private  array. 
        // You will just write extra functionality (methods) to make the work with the array more convenient for the user.
        private T[] data;

        // This property represents the number of elements in the vector
        public int Count { get; private set; } = 0;

        public int position { get;  set; } = 0;

        // This property represents the maximum number of elements (capacity) in the vector
        public int Capacity
        {
            get { return data.Length; }
        }

        // This is an overloaded constructor
        public Vector(int capacity)
        {
            data = new T[capacity];
        }

        // This is the implementation of the default constructor
        public Vector() : this(DEFAULT_CAPACITY) { }

        // An Indexer is a special type of property that allows a class or structure to be accessed the same way as array for its internal collection. 
        // For example, introducing the following indexer you may address an element of the vector as vector[i] or vector[0] or ...
        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();
                return data[index];
            }
            set
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();
                data[index] = value;
            }
        }

        // This private method allows extension of the existing capacity of the vector by another 'extraCapacity' elements.
        // The new capacity is equal to the existing one plus 'extraCapacity'.
        // It copies the elements of 'data' (the existing array) to 'newData' (the new array), and then makes data pointing to 'newData'.
        private void ExtendData(int extraCapacity)
        {
            T[] newData = new T[Capacity + extraCapacity];
            for (int i = 0; i < Count; i++) newData[i] = data[i];
            data = newData;
        }

        // This method adds a new element to the existing array.
        // If the internal array is out of capacity, its capacity is first extended to fit the new element.
        public void Add(T element)
        {
            if (Count == Capacity) ExtendData(DEFAULT_CAPACITY);
            data[Count++] = element;
        }

        // This method searches for the specified object and returns the zero‐based index of the first occurrence within the entire data structure.
        // This method performs a linear search; therefore, this method is an O(n) runtime complexity operation.
        // If occurrence is not found, then the method returns –1.
        // Note that Equals is the proper method to compare two objects for equality, you must not use operator '=' for this purpose.
        public int IndexOf(T element)
        {
            for (var i = 0; i < Count; i++)
            {
                if (data[i].Equals(element)) return i;
            }
            return -1;
        }

        public ISorter Sorter { set; get; } = new DefaultSorter();

        internal class DefaultSorter : ISorter
        {
            public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
            {
                if (comparer == null) comparer = Comparer<K>.Default;
                Array.Sort(sequence, comparer);
            }
        }

        public void Sort()
        {
            if (Sorter == null) Sorter = new DefaultSorter();
            Array.Resize(ref data, Count);
            Sorter.Sort(data, null);
        }

        public void Sort(IComparer<T> comparer)
        {
            if (Sorter == null) Sorter = new DefaultSorter();
            Array.Resize(ref data, Count);
            if (comparer == null) Sorter.Sort(data, null);
            else Sorter.Sort(data, comparer);
        }

        // TODO: Your task is to implement all the remaining methods.
        // Read the instruction carefully, study the code examples from above as they should help you to write the rest of the code.

        //public int BinarySearch(T element, IComparer<T> comparer)
        //{
        //    // throw new NotImplementedException();
        //    int left = 0;
        //    int right = Count - 1;

        //    while (left <= right)
        //    {
        //        int middle = left + (right - left) / 2;
        //        int comparisonResult = comparer.Compare(data[middle], element);
        //        if (comparisonResult == 0)
        //        {
        //            return middle; // Element found at index 'middle'
        //        }
        //        else if (comparisonResult < 0)
        //        {
        //            left = middle + 1; // Element may be on the right half
        //        }
        //        else
        //        {
        //            right = middle - 1; // Element may be on the left half
        //        }
        //    }
        //    return -1;
        //}

        public int BinarySearch(T element, IComparer<T> comparer)
        {
            return RecursiveBinarySearch(element, comparer, 0, Count - 1);
        }

        private int RecursiveBinarySearch(T element, IComparer<T> comparer, int left, int right)
        {
            if (left <= right)
            {
                int middle = left + (right - left) / 2;     //Calculating the middle value

                int comparisonResult = comparer.Compare(data[middle], element);  //Comparing the middle value with the targetelement 

                if (comparisonResult == 0)
                {
                    return middle; // Element found at index 'middle'
                }

                //Middle is smaller than the target value, the element is in right half of the given
                else if (comparisonResult < 0)  
                {
                    // Search in the right half
                    return RecursiveBinarySearch(element, comparer, middle + 1, right);
                }
                else
                {
                    // Search in the left half
                    return RecursiveBinarySearch(element, comparer, left, middle - 1);
                }
            }
            else
            {
                return -1; // Element not found
            }
        }

        // TODO: Add your methods for implementing the appropriate interface here
       
        public IEnumerator<T> GetEnumerator()
        {
            return new IteratorClass(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // TODO: Add an Iterator as an inner class here

        private class IteratorClass : IEnumerator<T>
        {
            private Vector<T> Vector;
            private int CurrentIndex;

            public IteratorClass(Vector<T> vector)
            {
                Vector = vector;
                CurrentIndex = -1;
            }
            public T Current
            {
                get
                {
                    // If the current index is invalid, return the default value of type T
                    if (CurrentIndex == -1)
                    {
                        return default(T);
                    }
                    // Otherwise, return the element at the current index in the Vector's data array
                    return Vector.data[CurrentIndex];
                }
            }
            // Explicit interface implementation for IEnumerator.Current property
            object IEnumerator.Current
            {
                get
                {
                    if (CurrentIndex == -1)
                    {
                        return default(T);
                    }
                    return Vector.data[CurrentIndex];

                }
            }

            // Method to move to the next element in the iteration
            public bool MoveNext()
            {
                // Check if there are more elements to iterate over
                if (CurrentIndex < Vector.Count - 1)
                {
                    CurrentIndex++;
                    return true;
                }
                return false;
            }
            // Method to reset the iterator to the initial state
            public void Reset()
            {
                CurrentIndex = -1;
            }
            // Method to release any resources held by the iterator
            public void Dispose()
            {
                Reset();
                Vector = null;
            }
        }   

    }

}