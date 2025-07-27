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

    public class MergeSortTopDown : ISorter
    {
        public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
        {
            // when Merge Sort is called, we need to break the array into singular elements 
            // so we call MergeSortRecursive

            // we send the array of elements, left index which is initially 0, 
            // right index which is length of the array - 1, and comparer if any.
            MergeSortRecursive(sequence, 0, sequence.Length - 1, comparer);
        }

        private void MergeSortRecursive<K>(K[] sequence, int left, int right, IComparer<K> comparer) where K : IComparable<K>
        {
            // we need to break the array into singular elements, 
            // so we break till we have 1 element (left == right) or 0 lement ( left > right)
            if (left < right)
            {
                int middle = (left + right) / 2;
                //calculate the middle and using this we divide the array into two parts, 
                //left to middle and middle+1 to right

                MergeSortRecursive(sequence, left, middle, comparer);
                MergeSortRecursive(sequence, middle + 1, right, comparer);

                //we take the sorted arrays from above and merge them.
                Merge(sequence, left, middle, right, comparer);
            }
        }

        private void Merge<K>(K[] sequence, int left, int middle, int right, IComparer<K> comparer) where K : IComparable<K>
        {
            // We find the size of both the arrays
            int leftSize = middle - left + 1;
            int rightSize = right - middle;

            //Creating new arrays to store the elements
            K[] L = new K[leftSize];
            K[] R = new K[rightSize];

            //Copying the elements from the original array to the new arrays
            Array.Copy(sequence, left, L, 0, leftSize);
            Array.Copy(sequence, middle + 1, R, 0, rightSize);


            int i = 0;          // index for iterating through left subarray.
            int j = 0;          // index for iterating through right subarray
            int k = left;       // index for inserting back into the main array.


            while (i < leftSize && j < rightSize)       // we iterate until both the arrays are not completed.
            {
                if (comparer.Compare(L[i], R[j]) <= 0)  // if element in left subarray is smaller than right subarray
                {
                    sequence[k] = L[i];                 // inserting back the smaller element into the main array
                    i++;                                // moving on to the next element in left subarray
                }
                else                                    // if element in right subarray is smaller than left subarray
                {
                    sequence[k] = R[j];                 // inserting back the smaller element into the main array
                    j++;                                // moving on to the next element in right subarray
                }
                k++;                                    // since an element has been inserted, we move onto next index
            }

            while (i < leftSize)                        // if we have finished iterating through the right subarray, but elements still left in left subarray
            {
                sequence[k] = L[i];                     // since there is no elements in right subarray, we just insert the elements into the main array.
                i++;
                k++;
            }

            while (j < rightSize)                       // if we have finished iterating through the left subarray, but elements still left in right subarray
            {
                sequence[k] = R[j];                     // since there is no elements in left subarray, we just insert the elements into the main array.
                j++;
                k++;
            }
        }
    }
    public class MergeSortBottomUp : ISorter
    {
        public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
        {
            int n = sequence.Length;

            // Merge subarrays of size 1, then subarrays of size 2, 4, 8, and so on
            for (int size = 1; size < n; size *= 2)
            {
                // Merge subarrays of the current size
                for (int left = 0; left < n - size; left += 2 * size)
                {
                    int middle = left + size - 1;
                    int right = Math.Min(left + 2 * size - 1, n - 1);
                    Merge(sequence, left, middle, right, comparer);
                }
            }
        }

       private void Merge<K>(K[] sequence, int left, int middle, int right, IComparer<K> comparer) where K : IComparable<K>
        {
            // We find the size of both the arrays
            int leftSize = middle - left + 1;
            int rightSize = right - middle;

            //Creating new arrays to store the elements
            K[] L = new K[leftSize];
            K[] R = new K[rightSize];

            //Copying the elements from the original array to the new arrays
            Array.Copy(sequence, left, L, 0, leftSize);
            Array.Copy(sequence, middle + 1, R, 0, rightSize);


            int i = 0;          // index for iterating through left subarray.
            int j = 0;          // index for iterating through right subarray
            int k = left;       // index for inserting back into the main array.


            while (i < leftSize && j < rightSize)       // we iterate until both the arrays are not completed.
            {
                if (comparer.Compare(L[i], R[j]) <= 0)  // if element in left subarray is smaller than right subarray
                {
                    sequence[k] = L[i];                 // inserting back the smaller element into the main array
                    i++;                                // moving on to the next element in left subarray
                }
                else                                    // if element in right subarray is smaller than left subarray
                {
                    sequence[k] = R[j];                 // inserting back the smaller element into the main array
                    j++;                                // moving on to the next element in right subarray
                }
                k++;                                    // since an element has been inserted, we move onto next index
            }

            while (i < leftSize)                        // if we have finished iterating through the right subarray, but elements still left in left subarray
            {
                sequence[k] = L[i];                     // since there is no elements in right subarray, we just insert the elements into the main array.
                i++;
                k++;
            }

            while (j < rightSize)                       // if we have finished iterating through the left subarray, but elements still left in right subarray
            {
                sequence[k] = R[j];                     // since there is no elements in left subarray, we just insert the elements into the main array.
                j++;
                k++;
            }
        }
    }


   

    public class RandomizedQuickSort : ISorter
    {
        private readonly Random random;

        //generating random indices during the sorting process
        public RandomizedQuickSort()
        {
            random = new Random();
        }

        // Sort method: Sorts the elements in the input array using the Quick Sort algorithm.
        public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
        {
            // Checks if the input array is null or empty, and if so, returns without sorting.
            if (sequence == null || sequence.Length == 0)
                return;

            // Initiates the Quick Sort process by calling the QuickSort method with the entire array range.
            QuickSort(sequence, 0, sequence.Length - 1, comparer);
        }

        // QuickSort method: Recursively sorts the elements in the array using the Randomized Quick Sort algorithm.
        private void QuickSort<K>(K[] sequence, int low, int high, IComparer<K> comparer) where K : IComparable<K>
        {
            // Base case: Checks if the lower index is less than the higher index, indicating valid sub-array partitions.
            if (low < high)
            {
                // Selects a random pivot index and partitions the array around this pivot.
                int pivotIndex = RandomizedPartition(sequence, low, high, comparer);

                // Recursively sorts the sub-arrays before and after the pivot.
                QuickSort(sequence, low, pivotIndex - 1, comparer);
                QuickSort(sequence, pivotIndex + 1, high, comparer);
            }
        }

        // RandomizedPartition method: Selects a random pivot index and partitions the array around this pivot.
        private int RandomizedPartition<K>(K[] sequence, int low, int high, IComparer<K> comparer) where K : IComparable<K>
        {

            int randomIndex = random.Next(low, high + 1); // Generate random index within the range [low, high]
            Swap(sequence, randomIndex, high); // Swap the random element with the last element
            return Partition(sequence, low, high, comparer); // Calls the Partition method to partition the array based on the selected pivot.
        }

        // Partition method: Partitions the array around a pivot element.
        private int Partition<K>(K[] sequence, int low, int high, IComparer<K> comparer) where K : IComparable<K>
        {
            // Selects the pivot element as the last element in the partition.
            K pivot = sequence[high];
            int i = low - 1;

            // Iterates through the array and reorganizes elements based on their relation to the pivot.
            for (int j = low; j < high; j++)
            {
                // Compares the current element with the pivot using the provided comparer.
                if (comparer.Compare(sequence[j], pivot) <= 0)
                {
                    i++;        // Moves the current element to the left side of the partition.
                    Swap(sequence, i, j);
                }
            }

            // Swaps the pivot element to its correct position in the partition.
            Swap(sequence, i + 1, high);
            return i + 1;           // Returns the index of the pivot element after partitioning.
        }

        private void Swap<K>(K[] sequence, int i, int j) where K : IComparable<K>
        {
            K temp = sequence[i];
            sequence[i] = sequence[j];
            sequence[j] = temp;
        }
    }
}