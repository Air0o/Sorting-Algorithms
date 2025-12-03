using System.Collections;
using System.Threading;
using UnityEngine;

public static class Sort
{
    public static IEnumerator BubbleSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool sorted = true;
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, j);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }

                if (arr[j] > arr[j + 1])
                {
                    int t = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = t;
                    sorted = false;
                }
            }
            if (sorted) break;

            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, arr.Length - i);
                yield return null;
            }
        }
        visualizer.VisualizeArray(arr, -1);
    }
    public static IEnumerator InsertionSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i];
            int j = i - 1;

            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;

                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, j + 1);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }
            }

            arr[j + 1] = key;

            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, i);
                yield return null;
            }
        }

        visualizer.VisualizeArray(arr, -1);
    }
    public static IEnumerator SelectionSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < arr.Length; j++)
            {
                if (step_time_ms != 0)
                {
                    visualizer.VisualizeArray(arr, j);
                    yield return new WaitForSeconds(step_time_ms / 1000f);
                }

                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                int temp = arr[i];
                arr[i] = arr[minIndex];
                arr[minIndex] = temp;
            }

            if (step_time_ms == 0)
            {
                visualizer.VisualizeArray(arr, i);
                yield return null;
            }
        }

        visualizer.VisualizeArray(arr, -1);
    }
    public static IEnumerator QuickSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
    {
        IEnumerator Sort(int left, int right)
        {
            if (left < right)
            {
                int pivot = arr[right];
                int i = left - 1;

                for (int j = left; j < right; j++)
                {
                    if (step_time_ms != 0)
                    {
                        visualizer.VisualizeArray(arr, j);
                        yield return new WaitForSeconds(step_time_ms / 1000f);
                    }

                    if (arr[j] <= pivot)
                    {
                        i++;
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }

                // Place pivot in correct position
                int t = arr[i + 1];
                arr[i + 1] = arr[right];
                arr[right] = t;

                int pivotIndex = i + 1;

                // If step_time_ms == 0, visualize once per partition step
                if (step_time_ms == 0)
                {
                    visualizer.VisualizeArray(arr, pivotIndex);
                    yield return null;
                }

                IEnumerator leftSort = Sort(left, pivotIndex - 1);
                while (leftSort.MoveNext()) yield return leftSort.Current;

                IEnumerator rightSort = Sort(pivotIndex + 1, right);
                while (rightSort.MoveNext()) yield return rightSort.Current;
            }
        }

        IEnumerator mainSort = Sort(0, arr.Length - 1);
        while (mainSort.MoveNext()) yield return mainSort.Current;

        visualizer.VisualizeArray(arr, -1);
    }

public static IEnumerator BogoSort(ArrayVisualizer visualizer, int[] arr, int step_time_ms)
{
    System.Random rng = new System.Random();

    // Helper: check if array is sorted
    bool IsSorted(int[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
            if (a[i] > a[i + 1]) return false;
        return true;
    }

    while (!IsSorted(arr))
    {
        // Shuffle the array
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;

            // Inner visualization only if step_time_ms > 0
            if (step_time_ms != 0)
            {
                visualizer.VisualizeArray(arr, i);
                yield return new WaitForSeconds(step_time_ms / 1000f);
            }
        }

        // Outer visualization only if step_time_ms == 0
        if (step_time_ms == 0)
        {
            visualizer.VisualizeArray(arr, 0);
            yield return null;
        }
    }

    // Final visualization: fully sorted
    visualizer.VisualizeArray(arr, -1);
}

}
