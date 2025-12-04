using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.ComponentModel;

public class UIManager : MonoBehaviour
{
    public GameObject menu_screen, animation_screen;
    public Slider speed_slider;
    public TMP_InputField element_count_field;
    public TextMeshProUGUI stats_text, benchmark_text;
    public TMP_Dropdown array_type_dropdown;

    bool benchmarking = false;

    SortingManager manager;
    
    private void Start() {
        manager = FindAnyObjectByType<SortingManager>();
    }
    public void ChooseAlgorithm(int algorithm_index)
    {
        benchmark_text.text = "";

        SortSettings sort_settings = new SortSettings();
        sort_settings.array_size = int.Parse(element_count_field.text);
        sort_settings.step_time_ms = (int)Mathf.Pow(speed_slider.value, 2);
        sort_settings.array_type = (ArrayType)array_type_dropdown.value;
        sort_settings.sort_type = (SortType)algorithm_index;
        
        menu_screen.SetActive(false);
        animation_screen.SetActive(true);
        manager.StartSortingAnimation(sort_settings);
    }

    public void BackToMenu()
    {
        benchmarking = false;
        
        menu_screen.SetActive(true);
        manager.Stop();
        animation_screen.SetActive(false);
    }

    public void UpdateStats(SortStats stats)
    {
        stats_text.text = $"Read operations: {stats.read_count}\nWrite operations: {stats.write_count}\nCurrent time: {Time.time-stats.start_time}";
    }

    public void Benchmark()
    {
        benchmarking = true;
        StartCoroutine(Benchmark_Coroutine());
    }

    public IEnumerator Benchmark_Coroutine()
    {
        int max_array_length = 100000;
        int run_count = 5;
        int test_count = 4;
        

        menu_screen.SetActive(false);
        animation_screen.SetActive(true);

        string text = $"Benchmark results ({((ArrayType)array_type_dropdown.value==ArrayType.Inverted? "Inverted" : "Random")}):\n";
        benchmark_text.text = text;

        int step = Math.Max(1, max_array_length / run_count);

        for (int run = 1; run <= run_count; run++)
        {
            int currentSize = step * run;
            SortSettings sort_settings = new SortSettings(0, (ArrayType)array_type_dropdown.value, 0, 0);
            sort_settings.array_size = currentSize;

            text += $"\n=== Array size {currentSize:N0} ===\n";
            benchmark_text.text = text;

            var results = new ConcurrentQueue<string>();
            var threads = new List<Thread>();

            for (int i = 0; i < (int)SortType.last; i++)
            {
                if ((SortType)i == SortType.BogoSort)
                    continue;

                SortSettings local_settings = sort_settings;
                local_settings.sort_type = (SortType)i;

                Thread t = new Thread(() =>
                {
                    // Warm-up
                        _ = manager.StartSortingNormal(local_settings);

                        long[] times = new long[test_count];
                        for (int j = 0; j < test_count && benchmarking; j++)
                            times[j] = manager.StartSortingNormal(local_settings);

                        string algoName = (int)local_settings.sort_type switch
                        {
                            (int)SortType.BubbleSort => "BubbleSort",
                            (int)SortType.SelectionSort => "SelectionSort",
                            (int)SortType.InsertionSort => "InsertionSort",
                            (int)SortType.QuickSort => "QuickSort",
                            (int)SortType.MergeSort => "MergeSort",
                            (int)SortType.HeapSort => "HeapSort",
                            (int)SortType.CocktailSort => "CocktailSort",
                            _ => "Unknown"
                        };

                        double avg = times.Average();

                        // Highlight average in bold and green
                        string avgPretty = avg >= 1000
                            ? $"<b><color=#00FF00>{avg:N0} ms ({avg / 1000:F2} s)</color></b>"
                            : $"<b><color=#00FF00>{avg:N0} ms</color></b>";

                        // Make individual runs smaller and gray
                        string runTimes = string.Join(", ", times.Select(ti =>
                            $"<color=#888888>{ti:N0} ms</color>"));

                        string result = $"{algoName,-12}: {avgPretty}\n   Tries: {runTimes}\n";
                        results.Enqueue(result);
                        
                });

                threads.Add(t);
            }

            foreach (Thread t in threads)
                t.Start();

            int finishedCount = 0;
            while (finishedCount < threads.Count)
            {
                if (!benchmarking)
                {
                    benchmark_text.text = "";
                    yield break;
                }

                while (results.TryDequeue(out string result))
                {
                    text += result;
                    benchmark_text.text = text;
                    finishedCount++;
                }
                
                yield return null;
            }

            foreach (var t in threads)
                t.Join();
        }
    }
}

public struct SortStats
{
    public int read_count;
    public int write_count;
    public float start_time, end_time;

    public SortStats(int read_count, int write_count, float start_time, float end_time = 0)
    {
        this.read_count = read_count;
        this.write_count = write_count;
        this.start_time = start_time;
        this.end_time = end_time;
    }
}