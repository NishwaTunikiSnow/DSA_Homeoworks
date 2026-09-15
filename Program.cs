using System.Diagnostics;

// =====================================================================
// Reads an array size and its elements directly from the console, then
// runs Bubble Sort, Selection Sort, and Insertion Sort on that exact
// array (each on a fresh copy). For each sort, it ACTUALLY COUNTS the
// real number of operations performed while it runs:
//   - comparisons made
//   - swaps / element-moves made
//   - total operations (comparisons + swaps/moves)
// plus wall-clock time and memory allocated. Nothing here is a
// hardcoded textbook formula -- every number is tallied live as the
// algorithm executes.
// =====================================================================

Console.Write("Enter array size: ");
int n = ReadPositiveInt();

int[] original = new int[n];
Console.WriteLine($"Enter {n} element(s), one at a time:");
for (int i = 0; i < n; i++)
{
    Console.Write($"  Element [{i}]: ");
    original[i] = ReadInt();
}

Console.WriteLine();
Console.WriteLine($"You entered: [{string.Join(", ", original)}]");
Console.WriteLine();

// A small struct to carry back every operation count from a sort call.
// (comparisons, swaps) -- kept separate so we can see both individually
// and add them together for a total.

/// <summary>
/// Bubble Sort. Counts every comparison and every swap as it happens.
/// </summary>
(long comparisons, long swaps) BubbleSortCounting(int[] arr)
{
    long comparisons = 0, swaps = 0;
    int len = arr.Length;
    for (int pass = 0; pass < len - 1; pass++)
    {
       // bool swapped = false; // ignoring optimized version of bubble sort
        for (int i = 0; i < len - 1 - pass; i++)
        {
            comparisons++;                      // every neighbor check counts
            if (arr[i] > arr[i + 1])
            {
                (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]);
                swaps++;
               // swapped = true;
            }
        }
        //if (!swapped) break;
    }
    return (comparisons, swaps);
}

/// <summary>
/// Selection Sort. Counts every comparison made while hunting for the
/// minimum, plus every real swap once the minimum is found.
/// </summary>
(long comparisons, long swaps) SelectionSortCounting(int[] arr)
{
    long comparisons = 0, swaps = 0;
    int len = arr.Length;
    for (int i = 0; i < len - 1; i++)
    {
        int minIndex = i;
        for (int j = i + 1; j < len; j++)
        {
            comparisons++;                      // every candidate check counts
            if (arr[j] < arr[minIndex]) minIndex = j;
        }
        if (minIndex != i)
        {
            (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
            swaps++;
        }
    }
    return (comparisons, swaps);
}

/// <summary>
/// Insertion Sort. Counts every comparison made while looking left for
/// the insertion point, plus every single-element shift performed.
/// </summary>
(long comparisons, long moves) InsertionSortCounting(int[] arr)
{
    long comparisons = 0, moves = 0;
    int len = arr.Length;
    for (int i = 1; i < len; i++)
    {
        int current = arr[i];
        int j = i - 1;
        while (j >= 0)
        {
            comparisons++;                      // count the check itself...
            if (arr[j] > current)
            {
                arr[j + 1] = arr[j];
                j--;
                moves++;
            }
            else
            {
                break;                          // ...including the one that stops us
            }
        }
        arr[j + 1] = current;
    }
    return (comparisons, moves);
}

bool IsSorted(int[] arr)
{
    for (int i = 0; i < arr.Length - 1; i++)
        if (arr[i] > arr[i + 1]) return false;
    return true;
}

var algorithms = new (string Name, Func<int[], (long comparisons, long swaps)> Sort)[]
{
    ("Bubble Sort",    BubbleSortCounting),
    ("Selection Sort", SelectionSortCounting),
    ("Insertion Sort", InsertionSortCounting),
};

Console.WriteLine($"{"Algorithm",-16}{"Result",-26}{"Time(ms)",-10}{"Comparisons",-13}{"Swaps/Moves",-13}{"Total Ops",-11}{"Bytes",-8}");
Console.WriteLine(new string('-', 97));

var results = new List<(string Name, long Comparisons, long Swaps, long Total)>();

foreach (var (name, sortFn) in algorithms)
{
    int[] working = (int[])original.Clone();

    long allocBefore = GC.GetAllocatedBytesForCurrentThread();
    Stopwatch sw = Stopwatch.StartNew();
    var (comparisons, swaps) = sortFn(working);
    sw.Stop();
    long allocAfter = GC.GetAllocatedBytesForCurrentThread();

    if (!IsSorted(working))
    {
        throw new InvalidOperationException($"{name} failed to sort the array!");
    }

    long bytesAllocated = allocAfter - allocBefore;
    long totalOps = comparisons + swaps;
    string resultText = "[" + string.Join(",", working) + "]";

    results.Add((name, comparisons, swaps, totalOps));

    Console.WriteLine(
        $"{name,-16}{resultText,-26}{sw.Elapsed.TotalMilliseconds,-10:F4}{comparisons,-13}{swaps,-13}{totalOps,-11}{bytesAllocated,-8}");
}

// ---- What the measured operation counts actually tell us ----

Console.WriteLine();
Console.WriteLine("What these counts mean:");
foreach (var (name, comparisons, swaps, total) in results)
{
    Console.WriteLine($"  {name}: {comparisons} comparisons + {swaps} swaps/moves = {total} total operations for n = {n}");
}
Console.WriteLine();
Console.WriteLine("For reference, n*(n-1)/2 for this array size is " + ((long)n * (n - 1) / 2) +
                   " -- that's the maximum possible comparisons/inversions for a reverse-sorted");
Console.WriteLine("input of this size. Compare it to the counts above to see how close each");
Console.WriteLine("algorithm came to that worst-case ceiling on the array you entered.");

// ---- Helpers for robust console input ----

int ReadPositiveInt()
{
    while (true)
    {
        string? line = Console.ReadLine();
        if (int.TryParse(line, out int value) && value > 0)
        {
            return value;
        }
        Console.Write("Please enter a positive whole number: ");
    }
}

int ReadInt()
{
    while (true)
    {
        string? line = Console.ReadLine();
        if (int.TryParse(line, out int value))
        {
            return value;
        }
        Console.Write("Please enter a whole number: ");
    }
}
