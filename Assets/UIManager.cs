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
        Benchmark.benchmarking = false;
        
        menu_screen.SetActive(true);
        manager.Stop();
        animation_screen.SetActive(false);
    }

    public void UpdateStats(SortStats stats)
    {
        stats_text.text = $"Read operations: {stats.read_count}\nWrite operations: {stats.write_count}\nCurrent time: {Time.time-stats.start_time}";
    }

    public void StartBenchmark()
    {
        Benchmark.benchmarking = true;
        StartCoroutine(Benchmark.Benchmark_Coroutine(benchmark_text));

        menu_screen.SetActive(false);
        animation_screen.SetActive(true);
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