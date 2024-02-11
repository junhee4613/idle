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
    public Button[] button_object;

    public Slider timer;
    //나중에 연출 때 hp바를 없앨것같아 미리 이렇게 처리함 어 근데 그냥 stage 한정 ui들 모두를 없애면 이거 필요없음
    public GameObject player_hp_bar;            //FIX : 여기 나중에 사용 안하면 없애자
    public GameObject boss_hp_bar;
    public GameObject[] player_hp = new GameObject[3];
    public GameObject[] boss_hp = new GameObject[3];
    public void Init()
    {
        timer = Managers.Resource.Load<GameObject>("Timer").GetComponent<Slider>();
        if (event_system == null)
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
            if(child.gameObject.name == "Stage_base")
            {
                for (int j = 0; j < child.childCount; j++)
                {
                    GameObject child_child = child.GetChild(j).gameObject;
                    if (child_child.name == "Player_hp_bar")
                    {
                        player_hp_bar = child_child;
                        for (int p = 0; p < player_hp_bar.transform.childCount; p++)
                        {
                            player_hp[p] = player_hp_bar.transform.GetChild(p).gameObject;
                        }
                    }
                    else if (child_child.name == "Boss_hp_bar")
                    {
                        boss_hp_bar = child_child;
                        for (int b = 0; b < boss_hp_bar.transform.childCount; b++)
                        {
                            boss_hp[b] = boss_hp_bar.transform.GetChild(b).gameObject;
                        }

                    }
                }
            }
        }
        slider_object = GameObject.FindObjectsOfType<Slider>();
        foreach (var item in slider_object)
        {
            switch (item.name)
            {
                case "BGM_Slider":
                    item.onValueChanged.AddListener((float value) => Managers.Sound.SetBGMVolume(value));
                    Debug.Log("넣음");
                    break;
                case "SFX_Slider":
                    item.onValueChanged.AddListener((float value) => Managers.Sound.SetSFXVolume(value));
                    break;
                default:
                    break;
            }
        }
        button_object = GameObject.FindObjectsOfType<Button>();
        foreach (var item in button_object)
        {
            switch (item.name)
            {
                case "Exit_button":
                    item.onClick.AddListener(() => SceneManager.LoadScene("Main_screen"));
                    break;
                case "Retry_button":
                    item.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
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
    public void Game_over_ui()
    {
        UI_window_on["Game_over"].SetActive(true);
    }
    public void UI_on_scene_loaded(Scene arg0, LoadSceneMode arg1)
    {
        if (UI_window_on["Game_over"].activeSelf)
        {
            UI_window_on["Game_over"].SetActive(false);
        }
        if (UI_window_off.Count != 0)
        {
            for (int i = 0; i < UI_window_off.Count; i++)
            {
                UI_window_off.Peek().SetActive(false);
            } 
        }
        if (Managers.GameManager.scene_name.Contains("Stage"))
        {
            //Managers.Resource.Load<GameObject>("Timer_canvas").GetComponent<Canvas>().worldCamera = GameObject.FindObjectOfType<Camera>();
            UI_window_on["Stage_base"].SetActive(true);
            foreach (var item in player_hp)
            {
                item.SetActive(true);
            }
            foreach (var item in boss_hp)
            {
                item.SetActive(true);
            }
        }
        else
        {
            UI_window_on["Stage_base"].SetActive(false);
        }
    }

}
