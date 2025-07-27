using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    public class Heap<K, D> where K : IComparable<K>
    {

        // This is a nested Node class whose purpose is to represent a node of a heap.
        private class Node : IHeapifyable<K, D>
        {
            // The Data field represents a payload.
            public D Data { get; set; }
            // The Key field is used to order elements with regard to the Binary Min (Max) Heap Policy, i.e. the key of the parent node is smaller (larger) than the key of its children.
            public K Key { get; set; }
            // The Position field reflects the location (index) of the node in the array-based internal data structure.
            public int Position { get; set; }

            public Node(K key, D value, int position)
            {
                Data = value;
                Key = key;
                Position = position;
            }

            // This is a ToString() method of the Node class.
            // It prints out a node as a tuple ('key value','payload','index')}.
            public override string ToString()
            {
                return "(" + Key.ToString() + "," + Data.ToString() + "," + Position + ")";
            }
        }

        // ---------------------------------------------------------------------------------
        // Here the description of the methods and attributes of the Heap<K, D> class starts

        public int Count { get; private set; }

        // The data nodes of the Heap<K, D> are stored internally in the List collection. 
        // Note that the element with index 0 is a dummy node.
        // The top-most element of the heap returned to the user via Min() is indexed as 1.
        private List<Node> data = new List<Node>();

        // We refer to a given comparer to order elements in the heap. 
        // Depending on the comparer, we may get either a binary Min-Heap or a binary  Max-Heap. 
        // In the former case, the comparer must order elements in the ascending order of the keys, and does this in the descending order in the latter case.
        private IComparer<K> comparer;

        // We expect the user to specify the comparer via the given argument.
        public Heap(IComparer<K> comparer)
        {
            this.comparer = comparer;

            // We use a default comparer when the user is unable to provide one. 
            // This implies the restriction on type K such as 'where K : IComparable<K>' in the class declaration.
            if (this.comparer == null) this.comparer = Comparer<K>.Default;

            // We simplify the implementation of the Heap<K, D> by creating a dummy node at position 0.
            // This allows to achieve the following property:
            // The children of a node with index i have indices 2*i and 2*i+1 (if they exist).
            data.Add(new Node(default(K), default(D), 0));
        }

        // This method returns the top-most (either a minimum or a maximum) of the heap.
        // It does not delete the element, just returns the node casted to the IHeapifyable<K, D> interface.
        public IHeapifyable<K, D> Min()
        {
            if (Count == 0) throw new InvalidOperationException("The heap is empty.");
            return data[1];
        }

        // Insertion to the Heap<K, D> is based on the private UpHeap() method
        public IHeapifyable<K, D> Insert(K key, D value)
        {
            Count++;
            Node node = new Node(key, value, Count);
            data.Add(node);
            UpHeap(Count);
            return node;
        }

        private void UpHeap(int start)
        {
            int position = start;
            while (position != 1)
            {
                if (comparer.Compare(data[position].Key, data[position / 2].Key) < 0) Swap(position, position / 2);
                position = position / 2;
            }
        }

        // This method swaps two elements in the list representing the heap. 
        // Use it when you need to swap nodes in your solution, e.g. in DownHeap() that you will need to develop.
        private void Swap(int from, int to)
        {
            Node temp = data[from];
            data[from] = data[to];
            data[to] = temp;
            data[to].Position = to;
            data[from].Position = from;
        }

        public void Clear()
        {
            for (int i = 0; i <= Count; i++) data[i].Position = -1;
            data.Clear();
            data.Add(new Node(default(K), default(D), 0));
            Count = 0;
        }

        public override string ToString()
        {
            if (Count == 0) return "[]";
            StringBuilder s = new StringBuilder();
            s.Append("[");
            for (int i = 0; i < Count; i++)
            {
                s.Append(data[i + 1]);
                if (i + 1 < Count) s.Append(",");
            }
            s.Append("]");
            return s.ToString();
        }
        public void DownHeap(int position)
        {
            int parent = position;
            int leftChild = 2 * parent;
            int rightChild = leftChild + 1;

            // While the parent has at least one child
            while (leftChild <= Count)
            {
                int smallerChild = leftChild; // Assume left child is smaller

                // Check if right child exists and if it's smaller than the left child
                if (rightChild <= Count && comparer.Compare(data[rightChild].Key, data[leftChild].Key) < 0)
                {
                    smallerChild = rightChild; // If so, update the smallerChild index
                }

                // Compare the parent with the smaller child
                if (comparer.Compare(data[parent].Key, data[smallerChild].Key) <= 0)
                {
                    // If the parent is smaller or equal to the smaller child, heap property is satisfied
                    break;
                }

                // Swap parent with the smaller child
                Swap(parent, smallerChild);

                // Move down the heap
                parent = smallerChild;
                leftChild = 2 * parent;
                rightChild = leftChild + 1;
            }
        }

        public IHeapifyable<K, D> Delete()
        {
            if (Count == 0)
                throw new InvalidOperationException("The heap is empty.");

            // Save the top-most element to return later
            IHeapifyable<K, D> deletedNode = data[1];

            // Swap the top-most element with the last element
            Swap(1, Count);

            // Remove the last element (previously the top-most element)
            data.RemoveAt(Count);
            Count--;

            // If the heap is now empty, no further action is needed
            if (Count == 0)
                return deletedNode;

            // Perform down-heap to restore heap property starting from the root
            DownHeap(1);

            return deletedNode;

        }

        // Builds a minimum binary heap using the specified data according to the bottom-up approach.
        public IHeapifyable<K, D>[] BuildHeap(K[] keys, D[] data)
        {
            // Checks if the heap is empty or not
            if (Count != 0)
                throw new InvalidOperationException("Heap is not empty.");

            //Sets the count to the number of defined keys in the heap
            Count = keys.Length;

            //Initialsing a heap array in which we build the heap as per the required structure 
            IHeapifyable<K, D>[] heapArray = new IHeapifyable<K, D>[Count];

            for (int i = 1; i < Count + 1; i++)
            {
                //Initialsing a new node to add in the to the heap 
                Node node = new Node(keys[i - 1], data[i - 1], i);
                //Adding the node in the heap
                this.data.Add(node);
                heapArray[i - 1] = node;
            }

            //Restructures the whole heap
            for (int i = Count; i > 0; i--)
            {
                DownHeap(i);
            }


            return heapArray;

        }

        public void DecreaseKey(IHeapifyable<K, D> element, K new_key)
        {
            //Initialises the position of the element in the heap
            int position = element.Position;

            //Checks for the element position in the data
            if (data[position] != element)
            {
                throw new InvalidOperationException("Given element is inconsistent with the current state of the heap.");
            }
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element), "Element cannot be null.");
            }
            //Updates the data element position int he heap
            data[element.Position].Key = new_key;

            // Perform up-heapify operation
            UpHeap(element.Position);
        }
        public IHeapifyable<K, D> DeleteElement(IHeapifyable<K, D> element)
        {
            // Get the position of the element in the heap
            int position = element.Position;

            // Swap the element with the last element in the heap
            Swap(position, Count);
            data[position].Position = position;

            // Remove the last element (previously the one we want to delete)
            data.RemoveAt(Count);
            Count--;

            // If the deleted element was the last one, no further action is needed
            if (position > Count)
            {
                // Update the position of the element to -1 to mark it as removed
                //Updates the data element position in the heap
                data[element.Position].Position = -1;
                return element;
            }

            // Update the position of the element to -1 to mark it as removed
            data[element.Position].Position = -1;

            // Perform down-heapify and up-heapify operations to restore heap property
            DownHeap(position);
            UpHeap(position);

            return element;
        }

        public IHeapifyable<K, D> KthMinElement(int k)
        {
            if (k < 1 || k > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "K must be between 1 and the number of elements in the heap.");
            }

            // Create a temporary min-heap
            var tempHeap = new Heap<K, D>(comparer);

            // Copy all elements from the original heap to the temporary heap
            foreach (var node in data.Skip(1))
            {
                tempHeap.Insert(node.Key, node.Data);
            }

            // Perform k-1 delete operations on the temporary heap
            for (int i = 0; i < k - 1; i++)
            {
                tempHeap.Delete();
            }

            // Return the kth minimum element (top element of the temporary heap)
            return tempHeap.Min();
        }

    }
}
