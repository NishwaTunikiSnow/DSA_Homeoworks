using System;

// ============================================================
// Node class — kept OUTSIDE the LinkedList class, as before.
// ============================================================
public class Node
{
    public int value;
    public Node next;

    public Node(int value)
    {
        this.value = value;
    }
}

// ============================================================
// LinkedList class — holds head/tail/length plus every method,
// including the 3 assignment solutions at the bottom.
// ============================================================
public class LinkedList
{
    private Node head;
    private Node tail;
    private int length;

    public LinkedList(int value)
    {
        Node newNode = new Node(value);
        head = newNode;
        tail = newNode;
        length = 1;
    }

    // Private constructor used internally (e.g. by DecimalToBinary)
    // to build a list that starts out empty.
    private LinkedList()
    {
        head = null;
        tail = null;
        length = 0;
    }

    public Node GetHead() { return head; }
    public Node GetTail() { return tail; }
    public int GetLength() { return length; }

    public void PrintList()
    {
        Node temp = head;
        while (temp != null)
        {
            Console.Write(temp.value);
            if (temp.next != null) Console.Write(" -> ");
            temp = temp.next;
        }
        Console.WriteLine();
    }

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
            tail = newNode;
        }
        length++;
    }

    public void Prepend(int value)
    {
        Node newNode = new Node(value);
        if (length == 0)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            newNode.next = head;
            head = newNode;
        }
        length++;
    }

    // ========================================================
    // PROBLEM 1 — Remove Duplicates from a Sorted Linked List
    // Approach: the list is sorted, so duplicates are always
    // neighbors. Walk with one pointer (current); whenever the
    // next node has the same value, splice it out. Only advance
    // current when the next value is different, since there
    // could be more than one duplicate in a row.
    // Time: O(n)   Space: O(1)
    // ========================================================
    public void RemoveDuplicates()
    {
        if (head == null) return; // empty list, nothing to do

        Node current = head;
        while (current != null && current.next != null)
        {
            if (current.value == current.next.value)
            {
                current.next = current.next.next;
                length--; // one node was removed
            }
            else
            {
                current = current.next;
            }
        }

        tail = current; // current now sits on the true last node
    }

    // ========================================================
    // PROBLEM 2, Part A — Binary Linked List -> Decimal
    // Approach: Horner's method. Walk head to tail, and at each
    // node do total = total * 2 + node.value. Multiplying by 2
    // shifts every bit seen so far one place left; adding the
    // new bit slots it into the ones place.
    // Time: O(n)   Space: O(1)
    // ========================================================
    public int BinaryToDecimal()
    {
        int total = 0;
        Node temp = head;
        while (temp != null)
        {
            total = total * 2 + temp.value;
            temp = temp.next;
        }
        return total;
    }

    // ========================================================
    // PROBLEM 2, Part B — Decimal -> Binary Linked List
    // Approach: repeatedly divide by 2 and record the remainder.
    // The first remainder produced is the LEAST significant bit,
    // so each new remainder is Prepended (added to the front)
    // instead of Appended — that naturally reverses the order
    // so the final list reads most-significant-bit first.
    // Time: O(log n)   Space: O(log n) for the new list
    // ========================================================
    public static LinkedList DecimalToBinary(int number)
    {
        if (number == 0)
        {
            return new LinkedList(0); // special case: 0 in binary is just "0"
        }

        LinkedList result = new LinkedList(); // starts empty

        while (number > 0)
        {
            int remainder = number % 2;
            number = number / 2;
            result.Prepend(remainder);
        }

        return result;
    }

    // ========================================================
    // PROBLEM 3 — Partition List Around a Value
    // Approach: build two brand-new lists while walking the
    // original once — one for values < x ("before"), one for
    // values >= x ("after") — using a dummy head for each so
    // there's no special case for the first node. Then splice
    // the "after" list onto the end of the "before" list.
    // Time: O(n)   Space: O(1) extra (nodes are relinked, not copied)
    // ========================================================
    public void PartitionList(int x)
    {
        Node beforeDummy = new Node(0);
        Node afterDummy = new Node(0);
        Node beforeTail = beforeDummy;
        Node afterTail = afterDummy;

        Node temp = head;
        while (temp != null)
        {
            Node next = temp.next; // save before we relink temp.next
            temp.next = null;

            if (temp.value < x)
            {
                beforeTail.next = temp;
                beforeTail = temp;
            }
            else
            {
                afterTail.next = temp;
                afterTail = temp;
            }
            temp = next;
        }

        beforeTail.next = afterDummy.next; // stitch the two lists together
        afterTail.next = null;             // guard against an accidental cycle

        head = beforeDummy.next;
        tail = (afterTail == afterDummy) ? beforeTail : afterTail;
        // (length is unchanged — same nodes, just reordered)
    }
}

// ============================================================
// Demo
// ============================================================
public class Program
{
    // helper just for building demo lists quickly
    private static LinkedList BuildList(int[] values)
    {
        LinkedList list = new LinkedList(values[0]);
        for (int i = 1; i < values.Length; i++)
        {
            list.Append(values[i]);
        }
        return list;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("===== Problem 1: RemoveDuplicates =====");
        LinkedList dupList = BuildList(new int[] { 1, 1, 2, 3, 3, 3, 4 });
        Console.Write("Before: ");
        dupList.PrintList();
        dupList.RemoveDuplicates();
        Console.Write("After:  ");
        dupList.PrintList();

        Console.WriteLine("\n===== Problem 2a: BinaryToDecimal =====");
        LinkedList binList = BuildList(new int[] { 1, 0, 1 });
        Console.Write("Binary list: ");
        binList.PrintList();
        Console.WriteLine("Decimal value: " + binList.BinaryToDecimal());

        Console.WriteLine("\n===== Problem 2b: DecimalToBinary =====");
        int decimalInput = 13;
        LinkedList newBinList = LinkedList.DecimalToBinary(decimalInput);
        Console.Write("Decimal " + decimalInput + " -> Binary list: ");
        newBinList.PrintList();

        Console.WriteLine("\n===== Problem 3: PartitionList =====");
        LinkedList partList = BuildList(new int[] { 3, 8, 5, 10, 2, 1 });
        Console.Write("Before (x=8): ");
        partList.PrintList();
        partList.PartitionList(8);
        Console.Write("After:        ");
        partList.PrintList();

        Console.WriteLine("\nDone. Press any key to exit.");
        Console.ReadKey();
    }
}
