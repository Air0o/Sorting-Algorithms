using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
public enum SortType
{
    BubbleSort,
    SelectionSort,
    InsertionSort,
    QuickSort,
    MergeSort,
    HeapSort,
    CocktailSort,
    BogoSort,
    last
}

public enum ArrayType
{
    Inverted,
    Random,
}
public class SortingManager : MonoBehaviour
{
    public ArrayVisualizer visualizer => FindAnyObjectByType<ArrayVisualizer>();

    int [] InitArray(int array_size, ArrayType type)
    {
        int[] arr = new int[array_size];
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = array_size-i;
        }

        if(type == ArrayType.Random)
        {               
            System.Random rng = new System.Random();
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        return arr;
    }
    public void StartSortingAnimation(SortSettings settings) {
        if(settings.array_size <= 0) settings.array_size = 1;

        int[] arr = InitArray(settings.array_size, settings.array_type);

        switch (settings.sort_type)
        {
            case SortType.BubbleSort:
            StartCoroutine(Sort_Animated.BubbleSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.SelectionSort:
            StartCoroutine(Sort_Animated.SelectionSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.InsertionSort:
            StartCoroutine(Sort_Animated.InsertionSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.QuickSort:
            StartCoroutine(Sort_Animated.QuickSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.MergeSort:
            StartCoroutine(Sort_Animated.MergeSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.HeapSort:
            StartCoroutine(Sort_Animated.HeapSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.CocktailSort:
            StartCoroutine(Sort_Animated.CocktailSort(visualizer, arr, settings.step_time_ms));
            break;
            case SortType.BogoSort:
            StartCoroutine(Sort_Animated.BogoSort(visualizer, arr, settings.step_time_ms));
            break;
        }
    }
    public long StartSortingNormal(SortSettings settings) {
        if(settings.array_size <= 0) return 0;

        int[] arr = InitArray(settings.array_size, settings.array_type);

        Stopwatch stopwatch = Stopwatch.StartNew();
        switch (settings.sort_type)
        {
            case SortType.BubbleSort:
            Sort_Normal.BubbleSort(arr);
            break;
            case SortType.SelectionSort:
            Sort_Normal.SelectionSort(arr);
            break;
            case SortType.InsertionSort:
            Sort_Normal.InsertionSort(arr);
            break;
            case SortType.QuickSort:
            Sort_Normal.QuickSort(arr);
            break;
            case SortType.MergeSort:
            Sort_Normal.MergeSort(arr);
            break;
            case SortType.HeapSort:
            Sort_Normal.HeapSort(arr);
            break;
            case SortType.CocktailSort:
            Sort_Normal.CocktailSort(arr);
            break;
        }
        return stopwatch.ElapsedMilliseconds;
    }
    public void Stop()
    {
        StopAllCoroutines();
        visualizer.Reset();
    }
}

public struct SortSettings
{
    public SortType sort_type;
    public ArrayType array_type;
    public int array_size;
    public int step_time_ms;
    
    public SortSettings(SortType sort_type, ArrayType array_type, int array_size, int step_time_ms)
    {
        this.sort_type = sort_type;
        this.array_type = array_type;
        this.array_size = array_size;
        this.step_time_ms = step_time_ms;
    }
}