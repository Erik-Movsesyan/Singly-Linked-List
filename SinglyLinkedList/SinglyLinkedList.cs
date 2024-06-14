using SinglyLinkedList;
using System.Collections;

namespace MyCollections.SinglyLinkedList
{
    public class SinglyLinkedList<T>: IEnumerable<T>
    {
        public SinglyLinkedListNode<T>? First { get; private set; }
        public SinglyLinkedListNode<T>? Last { get; private set; }
        public int Length { get; private set; }

        public SinglyLinkedList() { }

        public SinglyLinkedList(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        public SinglyLinkedListNode<T> AddLast(T value)
        {
            var newNode = new SinglyLinkedListNode<T>(this, value);
            return AddLastInternal(newNode);
        }

        public SinglyLinkedListNode<T> AddLast(SinglyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddLastInternal(newNode);
        }

        public SinglyLinkedListNode<T> AddFirst(T? value)
        {
            var newNode = new SinglyLinkedListNode<T>(this, value);
            return AddFirstInternal(newNode);
        }

        public SinglyLinkedListNode<T> AddFirst(SinglyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddFirstInternal(newNode);
        }

        public SinglyLinkedListNode<T> AddAfter(SinglyLinkedListNode<T>? node, T? value)
        {
            ValidateNode(node);
            var newNode = new SinglyLinkedListNode<T>(this, value);

            return AddAfterInternal(node!, newNode);
        }

        public SinglyLinkedListNode<T> AddAfter(SinglyLinkedListNode<T>? node, SinglyLinkedListNode<T>? newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddAfterInternal(node!, newNode);
        }

        public SinglyLinkedListNode<T> AddBefore(SinglyLinkedListNode<T>? node, T? value)
        {
            var newNode = new SinglyLinkedListNode<T>(this, value);

            return AddBeforeInternal(node!, newNode);
        }

        public SinglyLinkedListNode<T> AddBefore(SinglyLinkedListNode<T>? node, SinglyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddBeforeInternal(node!, newNode);  
        }

        public bool Remove(T? value)
        {
            var nodeToRemove = Find(value);
            if(nodeToRemove != null)
            {
                RemoveNodeInternal(nodeToRemove);
                return true;
            }

            return false;
        }

        public void Remove(SinglyLinkedListNode<T>? nodeToRemove)
        {
            ValidateNode(nodeToRemove);
            RemoveNodeInternal(nodeToRemove);
        }

        public void RemoveFirst()
        {
            if (First == null)
                throw new InvalidOperationException("The SinglyLinkedList is empty");

            RemoveNodeInternal(First);
        }

        public void RemoveLast()
        {
            if (First == null)
                throw new InvalidOperationException("The SinglyLinkedList is empty");

            RemoveNodeInternal(Last);
        }

        public void Clear()
        {
            SinglyLinkedListNode<T>? current = First;
            while (current != null)
            {
                SinglyLinkedListNode<T> temp = current;
                current = current.Next;
                temp.Invalidate();
            }

            First = null;
            Last = null;
            Length = 0;
        }

        public bool Contains(T? value) => Find(value) != null;

        public SinglyLinkedListNode<T>? Find(T? value)
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;

            if (node != null)
            {
                if (value != null)
                {
                    SinglyLinkedListNode<T>? nextNode;
                    do
                    {
                        if (comparer.Equals(node!.Value, value))
                        {
                            return node;
                        }
                        nextNode = node.Next;
                        node = nextNode;

                    } while (nextNode != null);
                }
                else
                {
                    do
                    {
                        if (node.Value == null)
                        {
                            return node;
                        }
                        node = node.Next;

                    } while (node != null);
                }
            }
            return null;
        }

        public SinglyLinkedListNode<T>? FindLast(T? value)
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;
            SinglyLinkedListNode<T>? resultNode = null;

            if (node != null)
            {
                if (value != null)
                {
                    SinglyLinkedListNode<T>? nextNode;
                    do
                    {
                        if (comparer.Equals(node!.Value, value))
                        {
                            resultNode = node;
                        }
                        nextNode = node.Next;
                        node = nextNode;

                    } while (nextNode != null);

                    return resultNode;
                }
                else
                {
                    do
                    {
                        if (node.Value == null)
                        {
                            resultNode = node;
                        }
                        node = node.Next;

                    } while (node != null);

                    return resultNode;
                }
            }
            return null;
        }

        public SinglyLinkedListNode<T>? FindBefore(SinglyLinkedListNode<T>? node)
        {
            ValidateNode(node);
            if (node!.List!.Length < 2)
                return null;

            var currentNode = First!.Next;

            SinglyLinkedListNode<T>? previousNode = First;
            SinglyLinkedListNode<T>? resultNode = null;
            do
            {
                if (currentNode == node)
                {
                    resultNode = previousNode;
                    break;
                }

                previousNode = currentNode;
                currentNode = currentNode?.Next;

            } while (currentNode != null);

            return resultNode;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SinglyLinkedListEnumerator<T>(this);
        }

        private SinglyLinkedListNode<T> AddFirstInternal(SinglyLinkedListNode<T> newNode)
        {
            if (First == null)
            {
                First = newNode;
                Last = newNode;
            }
            else
            {
                newNode.Next = First;
                First = newNode;
            }

            Length++;
            return newNode;
        }

        private SinglyLinkedListNode<T> AddLastInternal(SinglyLinkedListNode<T> newNode)
        {
            if (First == null)
            {
                First = newNode;
                Last = newNode;
            }
            else
            {
                Last!.Next = newNode;
                Last = newNode;
            }

            Length++;
            return newNode;
        }

        private SinglyLinkedListNode<T> AddAfterInternal(SinglyLinkedListNode<T> node, SinglyLinkedListNode<T> newNode)
        {
            if (Last == node)
            {
                Last = newNode;
            }
            else
            {
                newNode.Next = node!.Next;
            }
            node.Next = newNode;

            Length++;
            return newNode;
        }

        private SinglyLinkedListNode<T> AddBeforeInternal(SinglyLinkedListNode<T> node, SinglyLinkedListNode<T> newNode)
        {
            var precedingNode = FindBefore(node);

            if (precedingNode == null)
            {
                First = newNode;
            }
            else
            {
                precedingNode.Next = newNode;
            }
            newNode.Next = node;

            Length++;
            return newNode;
        }

        private void RemoveNodeInternal(SinglyLinkedListNode<T>? nodeToRemove)
        {
            var previousNode = FindBefore(nodeToRemove);
            if (previousNode != null)
            {
                previousNode.Next = nodeToRemove!.Next;

                if (nodeToRemove == Last)
                    Last = previousNode;
            }
            else
            {
                if(nodeToRemove!.Next != null)
                {
                    First = nodeToRemove.Next;
                }
                else
                {
                    First = null;
                    Last = null;
                }
            }

            nodeToRemove.Invalidate();
            Length--;
        }

        private static void ValidateNewNode(SinglyLinkedListNode<T>? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (node.List != null)
            {
                throw new InvalidOperationException("Node is already part of a different SinglyLinkedList");
            }
        }

        private void ValidateNode(SinglyLinkedListNode<T>? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (node.List != this)
            {
                throw new InvalidOperationException("The node does not belong to the current SinglyLinkedList");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class SinglyLinkedListNode<T>
    {
        public SinglyLinkedList<T>? List { get; internal set; }
        public SinglyLinkedListNode<T>? Next { get; internal set; }
        public T? Value { get; }

        internal SinglyLinkedListNode(SinglyLinkedList<T> list, T? value)
        {
            List = list;
            Value = value;
        }

        public SinglyLinkedListNode(T? value)
        {
            Value = value;
        }

        public void Invalidate()
        {
            List = null;
            Next = null;
        }
    }
}