
using System.Text;

namespace DoublyLinkedList
{
    public class DoublyLinkedList<T>
    {

        // Here is the the nested Node<K> class 
        private class Node<K> : INode<K>
        {
            public K Value { get; set; }
            public Node<K> Next { get; set; }
            public Node<K> Previous { get; set; }

            public Node(K value, Node<K> previous, Node<K> next)
            {
                Value = value;
                Previous = previous;
                Next = next;
            }

            // This is a ToString() method for the Node<K>
            // It represents a node as a tuple {'the previous node's value'-(the node's value)-'the next node's value')}. 
            // 'XXX' is used when the current node matches the First or the Last of the DoublyLinkedList<T>
            public override string ToString()
            {
                StringBuilder s = new StringBuilder();
                s.Append("{");
                s.Append(Previous.Previous == null ? "XXX" : Previous.Value.ToString());
                s.Append("-(");
                s.Append(Value);
                s.Append(")-");
                s.Append(Next.Next == null ? "XXX" : Next.Value.ToString());
                s.Append("}");
                return s.ToString();
            }

        }

        // Here is where the description of the methods and attributes of the DoublyLinkedList<T> class starts

        // An important aspect of the DoublyLinkedList<T> is the use of two auxiliary nodes: the Head and the Tail. 
        // The both are introduced in order to significantly simplify the implementation of the class and make insertion functionality reduced just to a AddBetween(...)
        // These properties are private, thus are invisible to a user of the data structure, but are always maintained in it, even when the DoublyLinkedList<T> is formally empty. 
        // Remember about this crucial fact when you design and code other functions of the DoublyLinkedList<T> in this task.
        private Node<T> Head { get; set; }
        private Node<T> Tail { get; set; }
        public int Count { get; private set; } = 0;

        public DoublyLinkedList()
        {
            Head = new Node<T>(default(T), null, null);
            Tail = new Node<T>(default(T), Head, null);
            Head.Next = Tail;
        }

        public INode<T> First
        {
            get
            {
                if (Count == 0) return null;
                else return Head.Next;
            }
        }

        public INode<T> Last
        {
            get
            {
                if (Count == 0) return null;
                else return Tail.Previous;
            }
        }

        public INode<T> After(INode<T> node)
        {
            if (node == null) throw new NullReferenceException();
            Node<T> node_current = node as Node<T>;
            if (node_current.Previous == null || node_current.Next == null) throw new InvalidOperationException("The node referred as 'before' is no longer in the list");
            if (node_current.Next.Equals(Tail)) return null;
            else return node_current.Next;
        }

        public INode<T> AddLast(T value)
        {
            return AddBetween(value, Tail.Previous, Tail);
        }

        // This is a private method that creates a new node and inserts it in between the two given nodes referred as the previous and the next.
        // Use it when you wish to insert a new value (node) into the DoublyLinkedList<T>
        private Node<T> AddBetween(T value, Node<T> previous, Node<T> next)
        {
            Node<T> node = new Node<T>(value, previous, next);
            previous.Next = node;
            next.Previous = node;
            Count++;
            return node;
        }

        public INode<T> Find(T value)
        {
            Node<T> node = Head.Next;
            while (!node.Equals(Tail))
            {
                if (node.Value.Equals(value)) return node;
                node = node.Next;
            }
            return null;
        }

        public override string ToString()
        {
            if (Count == 0) return "[]";
            StringBuilder s = new StringBuilder();
            s.Append("[");
            int k = 0;
            Node<T> node = Head.Next;
            while (!node.Equals(Tail))
            {
                s.Append(node.ToString());
                node = node.Next;
                if (k < Count - 1) s.Append(",");
                k++;
            }
            s.Append("]");
            return s.ToString();
        }

        // TODO: Your task is to implement all the remaining methods.
        // Read the instruction carefully, study the code examples from above as they should help you to write the rest of the code.

        //Returns the node cast into the INode<T> that precedes the specified node in the DoublyLinkedList<T>.
        public INode<T> Before(INode<T> node)
        {
            // given node is null, the method throws the ArgumentNullException.
            // If the node is not in the current DoublyLinkedList<T>, the method throws the InvalidOperationException.

            if (node == null)
            {
                throw new ArgumentNullException();
            }
            Node<T> Currentnode = node as Node<T>;

            if (Currentnode.Previous == null || Currentnode.Next == null)
            {
                throw new InvalidOperationException("Node described as 'after' is no longer in the list");
            }
            if (Currentnode.Previous.Equals(Head))
            {
                return null;
            }
            else
            {
                return Currentnode.Previous;
            }
        }
        //Adds a new node containing the specified value at the start of the DoublyLinkedList<T> and
        //returns the new node cast into the INode<T>
        public INode<T> AddFirst(T value)
        {
            // You should replace this plug by your code.
            return AddBetween(value, Head, Head.Next);
        }
        //Adds a new node before the specified node of the DoublyLinkedList<T>
        // records the given value as its payload. 
        public INode<T> AddBefore(INode<T> before, T value)
        {
            // You should replace this plug by your code.
            Node<T> Currentnode = before as Node<T>;

            if (Currentnode.Previous == null || Currentnode.Next == null)
            {
                throw new InvalidOperationException("Node described as 'after' is no longer in the list");
            }
            else
            {
                return AddBetween(value, Currentnode.Previous, Currentnode);
            }
        }
        //Adds a new node after the specified node of the DoublyLinkedList<T> and records the given value as its payload.
        //It returns the newly created node cast into the INode<T>.
        public INode<T> AddAfter(INode<T> after, T value)
        {
            // You should replace this plug by your code.
            Node<T> Currentnode = after as Node<T>;

            if (Currentnode.Previous == null || Currentnode.Next == null)
            {
                throw new InvalidOperationException("Node described as 'before' is no longer in the list");
            }
            else
            {
                return AddBetween(value, Currentnode, Currentnode.Next);
            }
        }
        /*
         * Removes all nodes from the DoublyLinkedList<T>. 
         * Count is set to zero. For each of the nodes, links to the previous and the next nodes must be nullified.
         */
        public void Clear()
        {
            // You should replace this plug by your code.
            Node<T> node = Head.Next;

            while (node.Next != Tail)
            {
                Node<T> next = node.Next;

                node.Next = null;
                node.Previous = null;
                node = next;

            }
            Count = 0;
        }
        /*
         * Removes the specified node from the DoublyLinkedList<T>. 
         * If node is null, the method throws the ArgumentNullException. 
         * If the node does not exist in the DoublyLinkedList<T>, 
         * the method throws the InvalidOperationException.
         */
        public void Remove(INode<T> node)
        {
            // You should replace this plug by your code.
            if (node == null)
            {
                throw new ArgumentNullException();
            }
            Node<T> Currentnode = node as Node<T>;

            if (Currentnode.Previous == null || Currentnode.Next == null) throw new InvalidOperationException("The node referred as 'before' is no longer in the list");

            Node<T> predecessor = Currentnode.Previous;
            Node<T> successor = Currentnode.Next;

            predecessor.Next = successor;
            successor.Previous = predecessor;

            Currentnode.Next = null;
            Currentnode.Previous = null;
            Count--;
        }
        //Removes the node at the start of the DoublyLinkedList<T>.
        //If the DoublyLinkedList<T> is empty, it throws the InvalidOperationException.
        public void RemoveFirst()
        {
            // You should replace this plug by your code.
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }
            Remove(Head.Next);
        }
        //Removes the node at the end of the DoublyLinkedList<T>.
        //If the DoublyLinkedList<T> is empty, it throws the InvalidOperationException.
        public void RemoveLast()
        {
            // You should replace this plug by your code.
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }
            Remove(Tail.Previous);
        }

    }
}
