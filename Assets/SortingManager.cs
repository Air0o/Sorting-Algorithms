using UnityEngine;

public class SortingManager : MonoBehaviour
{
    public enum SortType
    {
        BubbleSort,
        SelectionSort,
        InsertionSort,
        QuickSort,
        MergeSort,
        BogoSort
    }

    public SortType sort_type;
    public int array_size = 16;
    public Vector2Int value_range = new Vector2Int(0, 16);
    public int step_time_ms = 100;
    ArrayVisualizer visualizer => FindAnyObjectByType<ArrayVisualizer>();

    private void Start() {
        if(value_range.x <= 0) value_range.x = 1;

        int[] arr = new int[array_size];
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = Random.Range(value_range.x, value_range.y);
        }

        switch (sort_type)
        {
            case SortType.BubbleSort:
            StartCoroutine(Sort.BubbleSort(visualizer, arr, step_time_ms));
            break;
            case SortType.SelectionSort:
            StartCoroutine(Sort.SelectionSort(visualizer, arr, step_time_ms));
            break;
            case SortType.InsertionSort:
            StartCoroutine(Sort.InsertionSort(visualizer, arr, step_time_ms));
            break;
            case SortType.QuickSort:
            StartCoroutine(Sort.QuickSort(visualizer, arr, step_time_ms));
            break;
            case SortType.BogoSort:
            StartCoroutine(Sort.BogoSort(visualizer, arr, step_time_ms));
            break;
        }
    }
}
