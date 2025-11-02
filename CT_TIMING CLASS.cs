using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        int[] sizes = { 5000, 10000, 50000, 100000 }; // số lượng phần tử
        string[] names = {
            "Selection Sort", "Exchange Sort", "Insertion Sort", "Bubble Sort",
            "Quick Sort", "Merge Sort", "Shell Sort", "Heap Sort"
        };

        foreach (int n in sizes)
        {
            Console.WriteLine($"\n===== Kết quả cho mảng {n} phần tử =====");

            int[] baseArray = GenerateArray(n);

            // chạy từng thuật toán
            TestSort("Selection Sort", baseArray, SelectionSort);
            TestSort("Exchange Sort", baseArray, ExchangeSort);
            TestSort("Insertion Sort", baseArray, InsertionSort);
            TestSort("Bubble Sort", baseArray, BubbleSort);
            TestSort("Quick Sort", baseArray, arr => QuickSort(arr, 0, arr.Length - 1));
            TestSort("Merge Sort", baseArray, arr => MergeSort(arr, 0, arr.Length - 1));
            TestSort("Shell Sort", baseArray, ShellSort);
            TestSort("Heap Sort", baseArray, HeapSort);
        }

        Console.WriteLine("\nHoàn thành!");
    }

    // ---------- HÀM ĐO THỜI GIAN ----------
    static void TestSort(string name, int[] baseArray, Action<int[]> sortMethod)
    {
        int[] arr = (int[])baseArray.Clone();
        Stopwatch sw = new Stopwatch();

        sw.Start();
        sortMethod(arr);
        sw.Stop();

        Console.WriteLine($"{name,-15}: {sw.ElapsedMilliseconds,8} ms");
    }

    // ---------- SINH MẢNG NGẪU NHIÊN ----------
    static int[] GenerateArray(int n)
    {
        Random rand = new Random();
        int[] arr = new int[n];
        for (int i = 0; i < n; i++)
            arr[i] = rand.Next(0, 100000);
        return arr;
    }

    // ---------- THUẬT TOÁN SẮP XẾP ----------

    static void SelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int min = i;
            for (int j = i + 1; j < n; j++)
                if (arr[j] < arr[min]) min = j;
            (arr[i], arr[min]) = (arr[min], arr[i]);
        }
    }

    static void ExchangeSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
            for (int j = i + 1; j < n; j++)
                if (arr[i] > arr[j])
                    (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    static void InsertionSort(int[] arr)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i];
            int j = i - 1;
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = key;
        }
    }

    static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (arr[j] > arr[j + 1])
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
    }

    static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high);
            QuickSort(arr, low, pi - 1);
            QuickSort(arr, pi + 1, high);
        }
    }

    static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }

    static void MergeSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int mid = (left + right) / 2;
            MergeSort(arr, left, mid);
            MergeSort(arr, mid + 1, right);
            Merge(arr, left, mid, right);
        }
    }

    static void Merge(int[] arr, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;
        int[] L = new int[n1];
        int[] R = new int[n2];
        Array.Copy(arr, left, L, 0, n1);
        Array.Copy(arr, mid + 1, R, 0, n2);

        int i = 0, j = 0, k = left;
        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
                arr[k++] = L[i++];
            else
                arr[k++] = R[j++];
        }
        while (i < n1) arr[k++] = L[i++];
        while (j < n2) arr[k++] = R[j++];
    }

    static void ShellSort(int[] arr)
    {
        int n = arr.Length;
        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i++)
            {
                int temp = arr[i];
                int j;
                for (j = i; j >= gap && arr[j - gap] > temp; j -= gap)
                    arr[j] = arr[j - gap];
                arr[j] = temp;
            }
        }
    }

    static void HeapSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = n / 2 - 1; i >= 0; i--)
            Heapify(arr, n, i);
        for (int i = n - 1; i > 0; i--)
        {
            (arr[0], arr[i]) = (arr[i], arr[0]);
            Heapify(arr, i, 0);
        }
    }

    static void Heapify(int[] arr, int n, int i)
    {
        int largest = i;
        int l = 2 * i + 1;
        int r = 2 * i + 2;

        if (l < n && arr[l] > arr[largest]) largest = l;
        if (r < n && arr[r] > arr[largest]) largest = r;

        if (largest != i)
        {
            (arr[i], arr[largest]) = (arr[largest], arr[i]);
            Heapify(arr, n, largest);
        }
    }
}
