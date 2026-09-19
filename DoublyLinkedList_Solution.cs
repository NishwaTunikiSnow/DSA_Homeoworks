using System;

namespace DataStructures
{
    /// <summary>
    /// Solution key for the Doubly Linked List assignment:
    ///   Problem 1: Reverse()
    ///   Problem 2: PartitionList(int x)
    /// Node has value, next, and prev. The list tracks head, tail, and length.
    /// </summary>
    public class Node
    {
        public int value;
        public Node next;
        public Node prev;

        public Node(int value)
        {
            this.value = value;
            this.next = null;
            this.prev = null;
        }
    }

    public class DoublyLinkedList
    {
        public Node head;
        public Node tail;
        public int length;

        public DoublyLinkedList(int value)
        {
            Node newNode = new Node(value);
            head = newNode;
            tail = newNode;
            length = 1;
        }

        // --- Helper methods used to build/print the list for testing ---

        public void Append(int value)
        {
            Node newNode = new Node(value);
            if (length == 0)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.next = newNode;
                newNode.prev = tail;
                tail = newNode;
            }
            length++;
        }

        public void PrintList()
        {
            Node temp = head;
            while (temp != null)
            {
                Console.Write(temp.value);
                if (temp.next != null) Console.Write(" <-> ");
                temp = temp.next;
            }
            Console.WriteLine();
        }

        // =====================================================================
        // PROBLEM 1: Reverse()
        // Reverses the list in place by swapping each node's next/prev, then
        // swapping head and tail.
        //
        // Time complexity:  O(n) - visits every node exactly once
        // Space complexity: O(1) - only a few extra pointers, no new nodes
        // =====================================================================
        public void Reverse()
        {
            // Empty list or single node: nothing to reverse
            if (head == null || head == tail)
            {
                return;
            }

            Node current = head;

            while (current != null)
            {
                // Save the "old next" before we overwrite anything,
                // otherwise we lose our way forward through the list.
                Node temp = current.next;

                // Swap this node's pointers.
                current.next = current.prev;
                current.prev = temp;

                // Advance using the saved reference.
                current = temp;
            }

            // Finally, swap head and tail for the whole list.
            Node oldHead = head;
            head = tail;
            tail = oldHead;
        }

        // =====================================================================
        // PROBLEM 2: PartitionList(int x)
        // Rearranges the list so every value < x comes before every value >= x,
        // preserving relative order within each group. Uses two dummy-headed
        // chains built in a single pass, then joins them.
        //
        // Time complexity:  O(n) - one pass through the original list
        // Space complexity: O(1) - reuses existing nodes, only extra pointers
        // =====================================================================
        public void PartitionList(int x)
        {
            if (head == null)
            {
                return;
            }

            // Dummy nodes give both chains a stable starting point so we
            // never need to special-case "is this the first node?".
            Node dummy1 = new Node(0); // will hold values < x
            Node dummy2 = new Node(0); // will hold values >= x
            Node prev1 = dummy1;
            Node prev2 = dummy2;

            Node current = head;

            while (current != null)
            {
                if (current.value < x)
                {
                    prev1.next = current;
                    current.prev = prev1;
                    prev1 = current;
                }
                else
                {
                    prev2.next = current;
                    current.prev = prev2;
                    prev2 = current;
                }

                current = current.next;
            }

            // Terminate the tail end of whichever chain ends up last,
            // so we don't leave a stray pointer back into the old list.
            prev2.next = null;

            // Join the two chains: end of "less than" chain connects to
            // the start of the "greater than or equal to" chain.
            prev1.next = dummy2.next;

            if (dummy2.next != null)
            {
                dummy2.next.prev = prev1;
            }

            // The new head is whatever follows dummy1 (could be the
            // "less than" chain, or if that chain is empty, the
            // "greater than or equal to" chain).
            head = dummy1.next;
            if (head != null)
            {
                head.prev = null;
            }

            // The new tail is the last node touched in the "greater than
            // or equal to" chain, unless that chain was empty, in which
            // case the tail is the end of the "less than" chain.
            tail = (prev2 != dummy2) ? prev2 : prev1;
        }
    }

    // ---------------------------------------------------------------------
    // Simple test harness demonstrating both methods against the sample
    // inputs used in the assignment.
    // ---------------------------------------------------------------------
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Problem 1: Reverse()");
            DoublyLinkedList list1 = new DoublyLinkedList(3);
            list1.Append(8);
            list1.Append(12);
            list1.Append(5);
            Console.Write("Before: "); list1.PrintList();
            list1.Reverse();
            Console.Write("After:  "); list1.PrintList();
            Console.WriteLine();

            Console.WriteLine("Problem 2: PartitionList(x)");
            DoublyLinkedList list2 = new DoublyLinkedList(3);
            list2.Append(8);
            list2.Append(5);
            list2.Append(2);
            list2.Append(10);
            list2.Append(1);
            Console.Write("Before, x = 5: "); list2.PrintList();
            list2.PartitionList(5);
            Console.Write("After:          "); list2.PrintList();
        }
    }
}
