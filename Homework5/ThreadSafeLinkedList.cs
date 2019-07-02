using System.Threading;

namespace Homework5
{
    public sealed class ThreadSafeLinkedList<T>
    {
        private Node<T> head;
        private readonly Node<T> tail;

        public Node<T> Head { get => head; }

        public ThreadSafeLinkedList()
        {
            tail = head;
        }

        public void Add(T nodeValue)
        {
            Node<T> newHeadNode = null;
            Node<T> initialHeadNode = null;

            do
            {
                initialHeadNode = head;

                newHeadNode = new Node<T>()
                {
                    Item = nodeValue,
                    Next = head
                };

            } while (initialHeadNode != Interlocked.CompareExchange(ref head, newHeadNode, initialHeadNode));
        }
    }
}
