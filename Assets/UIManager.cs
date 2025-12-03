using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject menu_screen, animation_screen;
    public Slider speed_slider;
    public TMP_InputField element_count_field;

    SortingManager manager => FindAnyObjectByType<SortingManager>();

    public void ChooseAlgorithm(int algorithm_index)
    {
        manager.array_size = int.Parse(element_count_field.text);
        manager.value_range.y = manager.array_size;
        
        manager.step_time_ms = (int)Mathf.Pow(speed_slider.value, 2);
        menu_screen.SetActive(false);
        animation_screen.SetActive(true);
        manager.StartSorting(algorithm_index);
    }

    public void BackToMenu()
    {
        menu_screen.SetActive(true);
        manager.Stop();
        animation_screen.SetActive(false);
    }
}
