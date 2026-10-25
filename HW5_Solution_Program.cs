// Homework 5: Trees  (INSTRUCTOR SOLUTION - do not post)
// 1. Finish every method marked TODO. Use loops, not recursion.
// 2. Write your answers for Questions 1 to 5 in the comment box at the bottom of this file.
// Do not change the tests in Main. Run with: dotnet run
using System;

public class Program
{
    public static void Main(string[] args)
    {
        // ======================= TESTS (do not change) =======================
        var empty = new BinarySearchTree();
        var one = new BinarySearchTree();
        one.Insert(7);
        var t1 = new BinarySearchTree();
        foreach (int v in new[] { 50, 30, 70, 20, 40, 80, 35, 45 }) t1.Insert(v);
        var line = new BinarySearchTree();
        foreach (int v in new[] { 10, 20, 30, 40, 50 }) line.Insert(v);

        Console.WriteLine("== Min ==");
        Console.WriteLine(t1.Min());
        Console.WriteLine(line.Min());
        Console.WriteLine(one.Min());
        Console.WriteLine();

        Console.WriteLine("== Max ==");
        Console.WriteLine(t1.Max());
        Console.WriteLine(line.Max());
        Console.WriteLine(one.Max());
        Console.WriteLine();

        Console.WriteLine("== Depth ==");
        Console.WriteLine(t1.Depth(50));
        Console.WriteLine(t1.Depth(70));
        Console.WriteLine(t1.Depth(35));
        Console.WriteLine(t1.Depth(99));
        Console.WriteLine(line.Depth(50));
        Console.WriteLine(empty.Depth(5));
        Console.WriteLine();
    }
}

// ======================= CLASSES =======================
public class Node
{
    public int Value;
    public Node? Left;
    public Node? Right;
    public Node(int value) { Value = value; }
}

public class BinarySearchTree
{
    private Node? root;
    public Node? Root => root;

    public bool Insert(int value)
    {
        Node newNode = new Node(value);
        if (root == null) { root = newNode; return true; }
        Node temp = root;
        while (true)
        {
            if (newNode.Value == temp.Value) return false;
            if (newNode.Value < temp.Value)
            {
                if (temp.Left == null) { temp.Left = newNode; return true; }
                temp = temp.Left;
            }
            else
            {
                if (temp.Right == null) { temp.Right = newNode; return true; }
                temp = temp.Right;
            }
        }
    }

    public bool Contains(int value)
    {
        Node? temp = root;
        while (temp != null)
        {
            if (value < temp.Value) temp = temp.Left;
            else if (value > temp.Value) temp = temp.Right;
            else return true;
        }
        return false;
    }

    public int Min()
    {
        Node temp = root!;
        while (temp.Left != null)       // smallest value is the leftmost node
            temp = temp.Left;
        return temp.Value;
    }

    public int Max()
    {
        Node temp = root!;
        while (temp.Right != null)      // biggest value is the rightmost node
            temp = temp.Right;
        return temp.Value;
    }

    public int Depth(int value)
    {
        Node? temp = root;
        int depth = 0;
        while (temp != null)
        {
            if (value == temp.Value) return depth;
            if (value < temp.Value) temp = temp.Left;
            else temp = temp.Right;
            depth++;
        }
        return -1;                      // not in the tree
    }
}

/* ======================= WRITTEN ANSWERS (Questions 1 to 5) =======================

   ---- Question 1 ----
   a) root: 50
   b) leaves: 20, 35, 45, 80
   c) internal nodes: 50, 30, 70, 40
   d) parent of 35: 40         sibling of 20: 40
   e) depth of 35: 3           height of node 30: 2   (30 -> 40 -> 35)
   f) height of tree: 3        size: 8
   g) degree of 70: 1
   h) valid BST? Yes. At 30, the left side (20) is smaller and the right side
      (40, 35, 45) is bigger. All of them are also smaller than 50.

   ---- Question 2 ----
                 Tree A    Tree B    Tree C
   Full?         Yes       Yes       Yes
   Perfect?      No        Yes       No
   Complete?     Yes       Yes       No
   Reasons: A is not perfect because leaf 12 is one level higher than leaves 2 and 6.
            C is not perfect because leaf 4 is one level higher than 10 and 14.
            C is not complete because the last level has empty spots on the left (under 4).

   ---- Question 3 ----
   a) tree:
                  41
               /      \
             20        65
            /  \      /  \
          11    29   50   91
                  \      /  \
                  32    72   99

   b) height: 3                number of leaves: 5  (11, 32, 50, 72, 99)
   c) Contains(32) visits: 41, 20, 29, 32      returns: true
      Contains(60) visits: 41, 65, 50          returns: false

   ---- Question 4 ----
   a) tree:
      10
        \
         20
           \
            30
              \
               40
                 \
                  50
      height: 4
   b) nodes visited by Contains(50): 5     Big O: O(n)     acts like: a linked list

   ---- Question 5 ----
   a) tree:
                  90
                /    \
              70      85
             /  \    /  \
           40   65  80   30
          /  \
        10    35

   b) children of 70: 70 is at index 1. Left = 2(1)+1 = 3 -> 40. Right = 2(1)+2 = 4 -> 65.
   c) parent of 35: 35 is at index 8. (8 - 1) / 2 = 3 -> 40.
   d) 65 is at index 4. Its children would be at 2(4)+1 = 9 and 2(4)+2 = 10,
      but the last index is 8, so 65 has no children.
*/
