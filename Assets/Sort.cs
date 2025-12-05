using System.Collections;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System.Collections.Generic;
using System;

public static class Sort_Animated
{
    static UIManager manager = GameObject.FindAnyObjectByType<UIManager>();
    static SortStats sortStats;

    public static IEnumerator BubbleSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);

        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool sorted = true;
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, j);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }

                sortStats.read_count += 2;
                if (arr[j] > arr[j + 1])
                {
                    int t = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = t;
                    sorted = false;
                    sortStats.write_count += 2;
                    sortStats.read_count += 2;
                }
            }
            if (sorted) break;

            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, arr.Length - i);
                manager.UpdateStats(sortStats);
                yield return null;
            }
        }
        visualizer.VisualizeArray(arr, -1);
    }
    public static IEnumerator InsertionSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);
        for (int i = 1; i < arr.Length; i++)
        {
            sortStats.read_count++;
            int key = arr[i];
            int j = i - 1;

            sortStats.read_count++;
            while (j >= 0 && arr[j] > key)
            {
                sortStats.write_count++;
                sortStats.read_count++;
                arr[j + 1] = arr[j];
                j--;

                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, j + 1);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }
            }

            sortStats.write_count++;
            arr[j + 1] = key;

            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, i);
                manager.UpdateStats(sortStats);
                yield return null;
            }
        }

        visualizer.VisualizeArray(arr, -1);
    }
    public static IEnumerator SelectionSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);
        for (int i = 0; i < arr.Length - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < arr.Length; j++)
            {
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, j);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }

                sortStats.read_count += 2;
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                sortStats.write_count += 2;
                sortStats.read_count += 2;
                int temp = arr[i];
                arr[i] = arr[minIndex];
                arr[minIndex] = temp;
            }

            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, i);
                manager.UpdateStats(sortStats);
                yield return null;
            }
        }

        visualizer.VisualizeArray(arr, -1);
    }
public static IEnumerator QuickSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
{
    sortStats = new SortStats(0, 0, Time.time);
    manager.UpdateStats(sortStats);

    IEnumerator Sort(int left, int right)
    {
        if (left < right)
        {
            // --- Find median value of subarray ---
            int length = right - left + 1;
            int[] sub = new int[length];
            Array.Copy(arr, left, sub, 0, length);

            Array.Sort(sub); // O(n log n), but ensures exact median
            int pivot = sub[length / 2]; // median value

            // --- Partition around median value ---
            int i = left;
            int j = right;
            while (i <= j)
            {
                while (arr[i] < pivot) { sortStats.read_count++; i++; }
                while (arr[j] > pivot) { sortStats.read_count++; j--; }

                if (i <= j)
                {
                    sortStats.read_count += 2;
                    sortStats.write_count += 2;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                    i++;
                    j--;
                }

                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, i);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }
            }

            // Visualization rule for outer loop
            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, (left + right) / 2);
                manager.UpdateStats(sortStats);
                yield return null;
            }

            // Recurse left and right halves
            IEnumerator leftSort = Sort(left, j);
            while (leftSort.MoveNext()) yield return leftSort.Current;

            IEnumerator rightSort = Sort(i, right);
            while (rightSort.MoveNext()) yield return rightSort.Current;
        }
    }

    IEnumerator mainSort = Sort(0, arr.Length - 1);
    while (mainSort.MoveNext()) yield return mainSort.Current;

    visualizer.VisualizeArray(arr, -1);
}


    public static IEnumerator BogoSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);
        System.Random rng = new System.Random();

        // Helper: check if array is sorted
        bool IsSorted(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                sortStats.read_count += 2;
                if (a[i] > a[i + 1]) return false;
            }
            return true;
        }

        while (!IsSorted(arr))
        {
            // Shuffle the array
            for (int i = arr.Length - 1; i > 0; i--)
            {
                sortStats.read_count += 2;
                sortStats.write_count += 2;
                int j = rng.Next(i + 1);
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;

                // Inner visualization only if step_time_ms > 0
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, i);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }
            }

            // Outer visualization only if step_time_ms == 0
            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, 0);
                manager.UpdateStats(sortStats);
                yield return null;
            }
        }

        // Final visualization: fully sorted
        visualizer.VisualizeArray(arr, -1);
        manager.UpdateStats(sortStats);
    }
    public static IEnumerator MergeSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);
        // Recursive local function
        IEnumerator Sort(int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;

                // Sort left half
                IEnumerator leftSort = Sort(left, mid);
                while (leftSort.MoveNext()) yield return leftSort.Current;

                // Sort right half
                IEnumerator rightSort = Sort(mid + 1, right);
                while (rightSort.MoveNext()) yield return rightSort.Current;

                // Merge halves
                IEnumerator merge = Merge(arr, left, mid, right, visualizer, step_time_ms);
                while (merge.MoveNext()) yield return merge.Current;

                // Outer visualization only if step_time_ms == 0
                if (step_time_ms == 0)
                {
                    visualizer.VisualizeArray(arr, mid);
                    manager.UpdateStats(sortStats);
                    yield return null;
                }
            }
        }

        // Start recursion
        IEnumerator mainSort = Sort(0, arr.Length - 1);
        while (mainSort.MoveNext()) yield return mainSort.Current;

        // Final visualization
        visualizer.VisualizeArray(arr, -1);
    }
    private static IEnumerator Merge(int[] arr, int left, int mid, int right, ArrayVisualizer visualizer, int step_time_ms)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;
        sortStats.write_count += n1 + n2;
        sortStats.read_count += n1 + n2;

        int[] L = new int[n1];
        int[] R = new int[n2];

        for (int i = 0; i < n1; i++) L[i] = arr[left + i];
        for (int j = 0; j < n2; j++) R[j] = arr[mid + 1 + j];

        int iIndex = 0, jIndex = 0, k = left;

        while (iIndex < n1 && jIndex < n2)
        {
            sortStats.read_count++;
            if (L[iIndex] <= R[jIndex])
            {
                arr[k] = L[iIndex];
                iIndex++;
            }
            else
            {
                arr[k] = R[jIndex];
                jIndex++;
            }
            sortStats.write_count++;

            // Inner visualization only if step_time_ms > 0
            if (step_time_ms != 0)
            {
                visualizer.VisualizeArray(arr, k);
                manager.UpdateStats(sortStats);
                yield return new WaitForSeconds(step_time_ms / 1000f);
            }

            k++;
        }

        // Copy remaining elements
        while (iIndex < n1)
        {
            sortStats.write_count++;
            arr[k] = L[iIndex];
            iIndex++;
            k++;

            if (step_time_ms != 0)
            {
                visualizer.VisualizeArray(arr, k - 1);
                manager.UpdateStats(sortStats);
                yield return new WaitForSeconds(step_time_ms / 1000f);
            }
        }

        while (jIndex < n2)
        {
            sortStats.write_count++;
            arr[k] = R[jIndex];
            jIndex++;
            k++;

            if (step_time_ms != 0)
            {
                visualizer.VisualizeArray(arr, k - 1);
                manager.UpdateStats(sortStats);
                yield return new WaitForSeconds(step_time_ms / 1000f);
            }
        }
    }
    public static IEnumerator HeapSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);
        int n = arr.Length;

        // Build max heap
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            IEnumerator heapify = Heapify(arr, n, i, visualizer, step_time_ms);
            while (heapify.MoveNext()) yield return heapify.Current;
        }

        // Extract elements one by one
        for (int i = n - 1; i > 0; i--)
        {
            sortStats.write_count += 2;
            sortStats.read_count += 2;
            // Swap root (max) with last element
            int temp = arr[0];
            arr[0] = arr[i];
            arr[i] = temp;

            // Heapify reduced heap
            IEnumerator heapify = Heapify(arr, i, 0, visualizer, step_time_ms);
            while (heapify.MoveNext()) yield return heapify.Current;

            // Outer visualization only if step_time_ms == 0
            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, i);
                manager.UpdateStats(sortStats);
                yield return null;
            }
        }

        // Final visualization
        visualizer.VisualizeArray(arr, -1);
        manager.UpdateStats(sortStats);
    }
    private static IEnumerator Heapify(int[] arr, int n, int i, ArrayVisualizer visualizer, int step_time_ms)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        sortStats.write_count += 4;
        if (left < n && arr[left] > arr[largest])
            largest = left;

        if (right < n && arr[right] > arr[largest])
            largest = right;

        if (largest != i)
        {
            sortStats.write_count += 2;
            sortStats.read_count += 2;
            int swap = arr[i];
            arr[i] = arr[largest];
            arr[largest] = swap;

            // Inner visualization only if step_time_ms > 0
            if (step_time_ms != 0)
            {
                visualizer.VisualizeArray(arr, largest);
                manager.UpdateStats(sortStats);
                yield return new WaitForSeconds(step_time_ms / 1000f);
            }

            // Recursively heapify the affected subtree
            IEnumerator heapify = Heapify(arr, n, largest, visualizer, step_time_ms);
            while (heapify.MoveNext()) yield return heapify.Current;
        }
    }
    public static IEnumerator CocktailSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        sortStats = new SortStats(0, 0, Time.time);
        manager.UpdateStats(sortStats);
        bool swapped = true;
        int start = 0;
        int end = arr.Length - 1;

        while (swapped)
        {
            swapped = false;

            // Forward pass
            for (int i = start; i < end; i++)
            {
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, i);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }

                sortStats.read_count += 2;
                if (arr[i] > arr[i + 1])
                {
                    sortStats.read_count += 2;
                    sortStats.write_count += 2;
                    int temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                    swapped = true;
                }
            }

            // Outer visualization if step_time_ms == 0
            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, end);
                manager.UpdateStats(sortStats);
                yield return null;
            }

            if (!swapped) break;

            swapped = false;
            end--;

            // Backward pass
            for (int i = end - 1; i >= start; i--)
            {
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, i);
                    manager.UpdateStats(sortStats);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }

                sortStats.read_count += 2;
                if (arr[i] > arr[i + 1])
                {
                    sortStats.read_count += 2;
                    sortStats.write_count += 2;
                    int temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                    swapped = true;
                }
            }

            // Outer visualization if step_time_ms == 0
            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, start);
                manager.UpdateStats(sortStats);
                yield return null;
            }

            start++;
        }

        // Final visualization
        visualizer.VisualizeArray(arr, -1);
    }

}

public static class Sort_Normal
{        // Bubble Sort
    public static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        bool swapped;
        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                }
            }
            if (!swapped) break; // optimization: stop if already sorted
        }
    }

    // Selection Sort
    public static void SelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
                if (arr[j] < arr[minIndex]) minIndex = j;

            if (minIndex != i)
                (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
        }
    }

    // Insertion Sort
    public static void InsertionSort(int[] arr)
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

    // Quick Sort
    public static void QuickSort(int[] arr) => QuickSort(arr, 0, arr.Length - 1);

    private static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high);
            QuickSort(arr, low, pi - 1);
            QuickSort(arr, pi + 1, high);
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }

    // Merge Sort
    public static void MergeSort(int[] arr) => MergeSort(arr, 0, arr.Length - 1);

    private static void MergeSort(int[] arr, int left, int right)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;
        MergeSort(arr, left, mid);
        MergeSort(arr, mid + 1, right);
        Merge(arr, left, mid, right);
    }

    private static void Merge(int[] arr, int left, int mid, int right)
    {
        int n1 = mid - left + 1, n2 = right - mid;
        int[] L = new int[n1], R = new int[n2];
        Array.Copy(arr, left, L, 0, n1);
        Array.Copy(arr, mid + 1, R, 0, n2);

        int i = 0, j = 0, k = left;
        while (i < n1 && j < n2)
            arr[k++] = (L[i] <= R[j]) ? L[i++] : R[j++];
        while (i < n1) arr[k++] = L[i++];
        while (j < n2) arr[k++] = R[j++];
    }

    // Heap Sort
    public static void HeapSort(int[] arr)
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

    private static void Heapify(int[] arr, int n, int i)
    {
        int largest = i, l = 2 * i + 1, r = 2 * i + 2;
        if (l < n && arr[l] > arr[largest]) largest = l;
        if (r < n && arr[r] > arr[largest]) largest = r;
        if (largest != i)
        {
            (arr[i], arr[largest]) = (arr[largest], arr[i]);
            Heapify(arr, n, largest);
        }
    }

    // Cocktail Sort (Bidirectional Bubble Sort)
    public static void CocktailSort(int[] arr)
    {
        int start = 0, end = arr.Length - 1;
        bool swapped = true;
        while (swapped)
        {
            swapped = false;
            for (int i = start; i < end; i++)
            {
                if (arr[i] > arr[i + 1])
                {
                    (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]);
                    swapped = true;
                }
            }
            if (!swapped) break;
            swapped = false;
            end--;
            for (int i = end; i > start; i--)
            {
                if (arr[i] < arr[i - 1])
                {
                    (arr[i], arr[i - 1]) = (arr[i - 1], arr[i]);
                    swapped = true;
                }
            }
            start++;
        }
    }

}