using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_jun : MonoBehaviour
{
    public Stack<GameObject> UI_window_off = new Stack<GameObject>();
    public bool option_window_on;
    public Dictionary<string, GameObject> UI_window_on = new Dictionary<string, GameObject>();
    public GameObject canvas;
    public GameObject event_system;
    public Slider[] slider_object;
    public void Init()
    {
        if(event_system == null)
        {
            GameObject temp = GameObject.Find("EventSystem");
            if (!temp)
            {
                temp = Managers.Resource.Instantiate("EventSystem", null);
            }
            event_system = temp;
            DontDestroyOnLoad(event_system);
        }
        if (canvas == null)
        {
            GameObject temp = GameObject.Find("Canvas");
            if (!temp)
            {
                temp = Managers.Resource.Instantiate("Canvas", null);
            }
            canvas = temp;
            DontDestroyOnLoad(canvas);
        }
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            
            Transform child = canvas.transform.GetChild(i);
            UI_window_on.Add(child.name, child.gameObject);
        }
        slider_object = GameObject.FindObjectsOfType<Slider>();
        foreach (var item in slider_object)
        {
            switch (item.name)
            {
                case "BGM_Slider":
                    item.onValueChanged.AddListener((float value) => Managers.Sound.SetBGMVolume(value));
                    Debug.Log("³ÖÀ½");
                    break;
                case "SFX_Slider":
                    item.onValueChanged.AddListener((float value) => Managers.Sound.SetSFXVolume(value));
                    break;
                default:
                    break;
            }
        }
        foreach (var item in UI_window_on)
        {
            item.Value.SetActive(false);
        }
    }
    public void UI_on_scene_loaded(Scene arg0, LoadSceneMode arg1)
    {
        if(UI_window_off.Count != 0)
        {
            for (int i = 0; i < UI_window_off.Count; i++)
            {
                UI_window_off.Peek().SetActive(false);
            } 
        }
    }

}
