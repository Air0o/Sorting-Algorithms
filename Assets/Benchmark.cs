using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

public static class Benchmark 
{
    public static bool benchmarking;
    public static IEnumerator Benchmark_Coroutine(TextMeshProUGUI benchmark_text)
    {
        SortingManager manager = GameObject.FindAnyObjectByType<SortingManager>();
        int max_array_length = 100000;
        int run_count = 5;
        int test_count = 4;
        

        string text = $"Benchmark results (Random array)):\n";
        benchmark_text.text = text;

        int step = Math.Max(1, max_array_length / run_count);

        for (int run = 1; run <= run_count; run++)
        {
            int currentSize = step * run;
            SortSettings sort_settings = new SortSettings(0, ArrayType.Random, 0, 0);
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
