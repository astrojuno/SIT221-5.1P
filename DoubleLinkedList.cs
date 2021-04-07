using System;
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

        /// <summary>Returns the node (casted to INode<T>) before the specified node in the list</summary>
        /// <param><c>INode<T></c> the node after the node you want returned</param>
        /// <returns><c>INode<T></c> the code before the node paramater</returns>
        /// <exception cref = "NullReferenceException"> thrown when the parameter node is null</exception>
        /// <exception cref = "InvalidOperationException"> thrown when the parameter node is not in the list</exception>
        public INode<T> Before(INode<T> node)
        {
            // if the node is null, throw exception
            if (node == null) throw new NullReferenceException();
            // get the node as a node (not INode)
            Node<T> node_current = node as Node<T>;
            // if the list doesn't contain the node, throw exception
            if (node_current.Previous == null || node_current.Next == null) throw new InvalidOperationException("The node referred as 'before' is no longer in the list");
            // if the previous node is the head, return null
            if (node_current.Previous.Equals(Head)) return null;
            // otherwise, return the previous node
            else return node_current.Previous;
        }

        /// <summary>Adds a new node at the beginning of the list with the value given</summary>
        /// <param><c>T</c> the value to be the payload of the new node</param>
        /// <returns><c>Inode<T></c> the new node that has been added, cast as INode<T></returns>
        public INode<T> AddFirst(T value)
        {
            // hand the work off to AddBetween, with the head as the previous
            return AddBetween(value, Head, Head.Next);
        }

        /// <summary>Adds a new node before the specified node in the linked list</summary>
        /// <param><c>Inode<T><c> the node to insert before</param>
        /// <param><c>T</c> the value that is to be the payload of the new node</param>
        /// <returns><c>Inode<T></c> the new node that has been added, cast as INode<T></returns>
        /// <exception cref = "NullReferenceException"> thrown when the parameter node is null</exception>
        /// <exception cref = "InvalidOperationException"> thrown when the parameter node is not in the list</exception>
        public INode<T> AddBefore(INode<T> before, T value)
        {
            // if the passed node is null, throw exception
            if (before == null) throw new NullReferenceException();
            // get the node as a node (not INode)
            Node<T> add_before_me = before as Node<T>;
            // if the list doesn't contain the node, throw exception
            if (add_before_me.Previous == null || add_before_me.Next == null) throw new InvalidOperationException("The node referred as 'before' is no longer in the list");
            // get the node before the new node
            Node<T> node_before = add_before_me.Previous;
            // make the new node
            Node<T> new_node = new Node<T>(value, node_before, add_before_me);
            // wire up the new node to its place in the list
            node_before.Next = new_node;
            add_before_me.Previous = new_node;
            // keep track of count
            Count++;
            
            // return the new node
            return new_node;
        }

        /// <summary>Adds a new node after the specified node in the linked list</summary>
        /// <param><c>Inode<T><c> the node to insert after</param>
        /// <param><c>T</c> the value that is to be the payload of the new node</param>
        /// <returns><c>Inode<T></c> the new node that has been added, cast as INode<T></returns>
        /// <exception cref = "NullReferenceException"> thrown when the parameter node is null</exception>
        /// <exception cref = "InvalidOperationException"> thrown when the parameter node is not in the list</exception>
        public INode<T> AddAfter(INode<T> after, T value)
        {
            // if the passed node is null, throw exception
            if (after == null) throw new NullReferenceException();
            // get the node as a node (not INode)
            Node<T> add_after_me = after as Node<T>;
            // if the list doesn't contain the node, throw exception
            if (add_after_me.Previous == null || add_after_me.Next == null) throw new InvalidOperationException("The node referred as 'before' is no longer in the list");
            // get the node after the new node
            Node<T> node_after = add_after_me.Next;
            // make the new node
            Node<T> new_node = new Node<T>(value, add_after_me, node_after);
            // wire up the new node to its place in the list
            add_after_me.Next = new_node;
            node_after.Previous = new_node;
            // keep track of count
            Count++;

            // return the new node
            return new_node;
        }

        /// <summary>Clears the array of all elements, resets the count to zero</summary>
        public void Clear()
        {
            // if the list is already empty, return
            if (Count == 0) return;
            // start at the first node
            Node<T> node = Head.Next;
            // temporary node for holding the next node
            Node<T> next_node;
            // loop through all nodes until the tail, setting pointers to null, and moving on
            while (node.Next != Tail) {
                node.Previous = null;
                next_node = node.Next;
                node.Next = null;
                node = next_node; 
            }

            // link the head and tail (empty list)
            Head.Next = Tail;
            Tail.Previous = Head;
            // reset count
            Count = 0;
        }

        /// <summary>Removes the specified node from the list</summary>
        /// <param><c>INode<T></c> the node to remove</param>
        public void Remove(INode<T> node)
        {
            // if the list doesn't contain the node, throw exception
            if (node == null) throw new NullReferenceException();
            // get the node as a node (not INode)
            Node<T> node_current = node as Node<T>;
            // if the list doesn't contain the node, throw exception
            if (node_current.Previous == null || node_current.Next == null) throw new InvalidOperationException("The node referred as 'before' is no longer in the list");
            // wire up the next and previous node together
            node_current.Previous.Next = node_current.Next;
            node_current.Next.Previous = node_current.Previous;
            // set the node to delete pointers to null
            node_current.Previous = null;
            node_current.Next = null;
            // update the count
            Count--;
        }

        /// <summary>Removes the first element in the list</summary>
        /// <exception cref = "InvalidOperationException"> thrown when attempting to remove first on
        ///     an empty list</exception>
        public void RemoveFirst()
        {
            // if the array is empty, throw the exception
            if (Count == 0) throw new InvalidOperationException();
            // otherwise, remove the first node
            Remove(Head.Next);
        }

        /// <summary>Removes the last element in the list</summary>
        /// <exception cref = "InvalidOperationException"> thrown when attempting to remove last on
        ///     an empty list</exception>
        public void RemoveLast()
        {
            // if the array is empty, throw the exception
            if (Count ==0) throw new InvalidOperationException();
            // otherwise, remove the first node
            Remove(Tail.Previous);
        }
    }
}
