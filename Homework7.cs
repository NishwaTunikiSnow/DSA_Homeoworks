// ============================================================
// ANSWER KEY - Homework: Balanced Trees and AVL Trees
// Balance factor (BF) = height of left subtree - height of right subtree
// Height of an empty tree = 0, height of a single node = 1
// ============================================================
//
// PART 1 - SHORT ANSWERS
//
// Q1. A balanced tree is a tree where the left and right sides of every
//     node are about the same height. Nothing hangs down much deeper on one
//     side. This keeps the tree short, so we never have to walk far to
//     find a value.
//
// Q2. Plain BST after inserting 10, 20, 30, 40, 50:
//
//     10
//       \
//        20
//          \
//           30
//             \
//              40
//                \
//                 50
//
//     Height = 5. Finding 50 takes 5 steps (we visit every node).
//     It is really just a linked list. A balanced tree with 5 nodes has
//     height 3, so finding any value takes at most 3 steps.
//
// Q3. The balance factor of a node is (height of left subtree) minus
//     (height of right subtree). In an AVL tree every node must have a
//     BF of -1, 0, or +1.
//
// Q4. Because every node stays balanced, the height of an AVL tree stays
//     close to log n. Search, insert, and delete only walk from the root
//     down one path, so they take O(log n) steps. A plain BST can grow
//     to height n, which makes these O(n).
//
// Q5. Node : height, BF
//     10 : 1, 0      20 : 2, +1     40 : 1, 0     30 : 3, +1
//     90 : 1, 0      80 : 2, -1     70 : 3, -2    50 : 4, 0
//     It is NOT an AVL tree. Node 70 has BF -2 (left height 0, right height 2).
//
// PART 2 - AVL INSERTION TRACES
//
// Q6. Insert 33, 18, 7, 25, 29
//     Insert 7: 33 has BF +2, path left, left -> LL rotation at 33.
//          18
//         /  \
//        7    33
//     Insert 25: no rotation.
//     Insert 29: 33 has BF +2, path left (25), right (29) -> LR rotation at 33.
//     Final:
//          18
//         /  \
//        7    29
//            /  \
//          25    33
//
// Q7. Insert 14, 26, 38, 44, 41
//     Insert 38: 14 has BF -2, path right, right -> RR rotation at 14.
//          26
//         /  \
//       14    38
//     Insert 44: no rotation.
//     Insert 41: 38 has BF -2, path right (44), left (41) -> RL rotation at 38.
//     Final:
//          26
//         /  \
//       14    41
//            /  \
//          38    44
//
// Q8. Insert 42 into the given tree.
//     42 goes right of 39. Both 48 and 71 have BF +2.
//     Fix the LOWEST one (48). Path from 48: left (39), right (42) -> LR at 48.
//     After the fix, 71 is balanced again (BF +1).
//     Final:
//            71
//           /  \
//         42    90
//        /  \
//      39    48
//
// Q9. Insert 52, 37, 68, 29, 44, 41, 75, 80, 61, 78
//     52, 37, 68, 29, 44: no rotations.
//     Insert 41: 52 has BF +2, path left (37), right (44) -> LR at 52.
//            44
//           /  \
//         37    52
//        /  \     \
//      29    41    68
//     Insert 75: 52 has BF -2, path right, right -> RR at 52.
//            44
//           /  \
//         37    68
//        /  \   /  \
//      29   41 52   75
//     Insert 80: no rotation (44 is -1, 68 is -1, 75 is -1).
//     Insert 61: no rotation (52 is -1, 68 is 0, 44 is -1).
//     Insert 78: 75 has BF -2, path right (80), left (78) -> RL at 75.
//     Final:
//              44
//            /    \
//          37      68
//         /  \    /   \
//       29   41  52    78
//                  \   /  \
//                  61 75   80
//
// PART 3 - EXPECTED OUTPUT FROM MAIN
//     The program prints: LR rotation at 52, RR rotation at 52,
//     RL rotation at 75. This matches the Q9 trace.
//     Height of the AVL tree after inserting 1 to 15 in order: 4.
//     A plain BST with the same inserts would have height 15.
// ============================================================

using System;

class AvlNode
{
    public int Value;
    public AvlNode Left;
    public AvlNode Right;
    public int Height;

    public AvlNode(int value)
    {
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
}

class Program
{
    // Height of a node. An empty spot (null) has height 0.
    static int GetHeight(AvlNode node)
    {
        if (node == null)
        {
            return 0;
        }
        return node.Height;
    }

    // Balance factor = left height - right height.
    static int GetBalance(AvlNode node)
    {
        if (node == null)
        {
            return 0;
        }
        return GetHeight(node.Left) - GetHeight(node.Right);
    }

    // Recompute a node's height from its children.
    static void UpdateHeight(AvlNode node)
    {
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    // Right rotation (used for LL). The left child moves up.
    static AvlNode RotateRight(AvlNode a)
    {
        AvlNode b = a.Left;
        AvlNode moved = b.Right;

        b.Right = a;
        a.Left = moved;

        UpdateHeight(a);   // a is now lower, so update it first
        UpdateHeight(b);
        return b;          // b is the new top of this subtree
    }

    // Left rotation (used for RR). The right child moves up.
    static AvlNode RotateLeft(AvlNode a)
    {
        AvlNode b = a.Right;
        AvlNode moved = b.Left;

        b.Left = a;
        a.Right = moved;

        UpdateHeight(a);
        UpdateHeight(b);
        return b;
    }

    // Insert a value and return the (possibly new) top of this subtree.
    static AvlNode Insert(AvlNode node, int value)
    {
        // 1. Normal BST insert
        if (node == null)
        {
            return new AvlNode(value);
        }
        if (value < node.Value)
        {
            node.Left = Insert(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = Insert(node.Right, value);
        }
        else
        {
            return node; // no duplicates
        }

        // 2. Update height on the way back up
        UpdateHeight(node);

        // 3. Check balance. Because this runs on the way back up,
        //    the lowest unbalanced node is always fixed first.
        int balance = GetBalance(node);

        // LL
        if (balance > 1 && value < node.Left.Value)
        {
            Console.WriteLine("  LL rotation at " + node.Value);
            return RotateRight(node);
        }
        // RR
        if (balance < -1 && value > node.Right.Value)
        {
            Console.WriteLine("  RR rotation at " + node.Value);
            return RotateLeft(node);
        }
        // LR
        if (balance > 1 && value > node.Left.Value)
        {
            Console.WriteLine("  LR rotation at " + node.Value);
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        // RL
        if (balance < -1 && value < node.Right.Value)
        {
            Console.WriteLine("  RL rotation at " + node.Value);
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    // Left, root, right. Should print the values in sorted order.
    static void PrintInOrder(AvlNode node)
    {
        if (node == null)
        {
            return;
        }
        PrintInOrder(node.Left);
        Console.Write(node.Value + " ");
        PrintInOrder(node.Right);
    }

    // Prints the tree sideways (root on the left, right side on top).
    static void PrintTree(AvlNode node, string indent)
    {
        if (node == null)
        {
            return;
        }
        PrintTree(node.Right, indent + "        ");
        Console.WriteLine(indent + node.Value + " (h=" + node.Height + ", bf=" + GetBalance(node) + ")");
        PrintTree(node.Left, indent + "        ");
    }

    // Measures real heights (does not trust the stored Height field).
    // Returns -1 if any node has a BF outside -1..+1.
    static int CheckHeight(AvlNode node)
    {
        if (node == null)
        {
            return 0;
        }
        int left = CheckHeight(node.Left);
        if (left == -1)
        {
            return -1;
        }
        int right = CheckHeight(node.Right);
        if (right == -1)
        {
            return -1;
        }
        if (Math.Abs(left - right) > 1)
        {
            return -1;
        }
        return 1 + Math.Max(left, right);
    }

    static bool IsBalanced(AvlNode root)
    {
        return CheckHeight(root) != -1;
    }

    static AvlNode BuildTree(int[] values)
    {
        AvlNode root = null;
        foreach (int v in values)
        {
            Console.WriteLine("Insert " + v);
            root = Insert(root, v);
        }
        return root;
    }

    static void ShowTree(AvlNode root)
    {
        Console.Write("In order: ");
        PrintInOrder(root);
        Console.WriteLine();
        Console.WriteLine("Root: " + root.Value);
        Console.WriteLine("Height: " + GetHeight(root));
        Console.WriteLine("Balanced: " + IsBalanced(root));
        PrintTree(root, "");
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        Console.WriteLine("=== Q6 ===");
        ShowTree(BuildTree(new int[] { 33, 18, 7, 25, 29 }));

        Console.WriteLine("=== Q7 ===");
        ShowTree(BuildTree(new int[] { 14, 26, 38, 44, 41 }));

        Console.WriteLine("=== Q8 (build the starting tree, then insert 42) ===");
        ShowTree(BuildTree(new int[] { 71, 48, 90, 39, 42 }));

        Console.WriteLine("=== Q9 ===");
        ShowTree(BuildTree(new int[] { 52, 37, 68, 29, 44, 41, 75, 80, 61, 78 }));

        Console.WriteLine("=== Insert 1 to 15 in order ===");
        AvlNode big = null;
        for (int i = 1; i <= 15; i++)
        {
            big = Insert(big, i);
        }
        Console.WriteLine("Height: " + GetHeight(big) + " (a plain BST would be 15)");
        Console.WriteLine("Balanced: " + IsBalanced(big));
    }
}
