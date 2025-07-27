using System;
using System.Collections.Generic;
using System.Text;

namespace Vector
{
    public class Vector<T> where T : IComparable<T>
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

    }
    public class BubbleSort : ISorter
    {   // Created a BubbleSort class extending ISorter
        public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
        {
            //Defining the lenght of the array
            int n = sequence.Length;

            //   // Iterate over the array to select each element for sortings
            for (int i = 0; i < n - 1; i++)
            {
                /*
                 * Iterate over the unsorted portion of the array
                 * The last i elements are already in place
                 */

                for (int j = 0; j < n - i - 1; j++)
                {
                    // Compare adjacent elements
                    if (comparer.Compare(sequence[j], sequence[j + 1]) > 0)
                    {
                        // Swap if they're in the wrong order
                        K temp = sequence[j];
                        sequence[j] = sequence[j + 1];
                        sequence[j + 1] = temp;
                    }
                }
            }
        }
    }

    public class SelectionSort : ISorter
    {   // Created a SelectionSort class extending ISorter
        public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
        {
            //Defining the lenght of the array
            int n = sequence.Length;

            /*Checking the array till the second last index 
             * As we proceed with cheecking the elements of the array, we start with comparing the 
             * minimum unsorted index with the elements of the array and find the minimum element in the array
             */
            for (int i = 0; i < n - 1; i++)
            {
                // This is the minimum index which holds the unsorted element 
                int min_unsorted = i;

                for (int j = i + 1; j < n; j++)
                {
                    // Compare adjacent elements
                    // below is inbuilt function which takes a,b
                    // if a > b => -1 else 1
                    if (comparer.Compare(sequence[j], sequence[min_unsorted]) < 0)
                    {
                        // Swap if they're in the wrong
                        // Saving the index of the minimum element
                        min_unsorted = j;
                    }
                }
                // Placing the minimum element at the sorted position
                K temp = sequence[min_unsorted];
                sequence[min_unsorted] = sequence[i];
                sequence[i] = temp;
            }
        }
    }

    public class InsertionSort : ISorter
    {
        public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
        {
            //Defining the length of the array
            int n = sequence.Length;

            //Looping through the array till the last element starting from the second index
            //Since the first element is already sorted.
            for (int i = 1; i < n; i++)
            {
                //The element that needs to be placed at the sorted position in the array 
                K key = sequence[i];

                //Assigning the previous index element to i th element
                int j = i - 1;

                /*Compare the element at index j with the key
                 * Move elements of sequence[0..i-1], that are greater than key, 
                 * to one position ahead of their current position
                 */
                while (j >= 0 && comparer.Compare(sequence[j], key) > 0)
                {
                    sequence[j + 1] = sequence[j];
                    j = j - 1;
                }
                //  Place the key at its correct position in the sorted sequence
                sequence[j + 1] = key;
            }
        }
    }
}