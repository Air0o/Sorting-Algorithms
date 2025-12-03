using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArrayVisualizer : MonoBehaviour
{
    public Vector2 view_size = Vector2.one;
    AudioSource audio_source => FindAnyObjectByType<AudioSource>();
    public Sprite sprite;

    List<GameObject> array_elements = new List<GameObject>();
    int current_highlighted_index = -1;

    public void VisualizeArray(int[] arr, int highlight_index)
    {
        if(current_highlighted_index > arr.Length) current_highlighted_index = -1;
        for(int i = 0; i < arr.Length; i++)
        {
            if(i >= array_elements.Count)
            {
                array_elements.Add(CreateNewArrayElement(i, arr[i], arr.Max(), arr.Length));
            }
            else
            {
                UpdateArrayElement(array_elements[i], i, arr[i], arr.Max(), arr.Length);
            }

            if(highlight_index == -1)
            {                
                if(current_highlighted_index != -1)
                {
                    array_elements[current_highlighted_index].GetComponent<SpriteRenderer>().color = Color.white;
                }
                current_highlighted_index = -1;
            } 
            else if(i == highlight_index)
            {
                array_elements[i].GetComponent<SpriteRenderer>().color = Color.red;
                if(current_highlighted_index != -1)
                {
                    array_elements[current_highlighted_index].GetComponent<SpriteRenderer>().color = Color.white;
                }
                current_highlighted_index = i;
            }
        }

        // val:max = pitch:maxpitch
        //pitch = (val*maxpitch)/max

        if(highlight_index >= 0 && highlight_index < arr.Length){
            audio_source.pitch = ((float)arr[highlight_index]*4f)/(float)arr.Max() - 1f;
            audio_source.Play();
        }
    }

    GameObject CreateNewArrayElement(int index, int value, int max_value, int max_count)
    {
        GameObject element = new GameObject($"Array element ({value})");
        element.transform.SetParent(transform);

        element.AddComponent(typeof(SpriteRenderer));
        SpriteRenderer renderer = element.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = Color.white;

        UpdateArrayElement(element, index, value, max_value, max_count);

        return element;
    }

    void UpdateArrayElement(GameObject element, int index, int value, int max_value, int max_count)
    {
        float size_x = view_size.x/max_count;
        float size_y = view_size.y/max_value;
        float x = -view_size.x/2 + size_x/2 + size_x*index;
        float y = -view_size.y/2 + size_y*value/2;
        element.transform.localPosition = new Vector2(x, y);
        element.transform.localScale = new Vector3(size_x, size_y*value);
    }

    public void Reset()
    {
        foreach (GameObject obj in array_elements)
        {
            Destroy(obj);
        }
        array_elements = new List<GameObject>();
        current_highlighted_index = -1;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, view_size);
    }
}
