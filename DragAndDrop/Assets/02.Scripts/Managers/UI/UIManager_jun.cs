using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class UIManager_jun
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
    public bool fade_start = false;
    public void Init()
    {
        timer = Managers.Resource.Load<GameObject>("Timer").GetComponent<Slider>();


        if (event_system == null)
        {
            GameObject eventSystem = GameObject.Find("EventSystem");

            if (!eventSystem)
            {
                eventSystem = Managers.Resource.Instantiate("EventSystem", null);
            }
            event_system = eventSystem;
            UnityEngine.MonoBehaviour.DontDestroyOnLoad(event_system);
        }

        if (canvas == null)
        {
            GameObject temp = GameObject.Find("Canvas");
            if (!temp)
            {
                temp = Managers.Resource.Instantiate("Canvas", null);
            }
            canvas = temp;
            UnityEngine.MonoBehaviour.DontDestroyOnLoad(canvas);
        }
        for (int i = 0; i < canvas.transform.childCount; i++)       //GetChild는 자식 오브젝트 중 최상위 오브젝트만 가져옴
        {
            Transform child = canvas.transform.GetChild(i);
            UI_window_on.Add(child.name, child.gameObject);
            /*if(child.gameObject.name == "Stage_base")
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
            }*/
        }
        slider_object = GameObject.FindObjectsOfType<Slider>();
        foreach (var item in slider_object)
        {
            switch (item.name)
            {
                case "BGM_Slider":
                    item.onValueChanged.AddListener((float value) => Managers.Sound.SetBGMVolume(value));
                    break;
                case "SFX_Slider":
                    item.onValueChanged.AddListener((float value) => Managers.Sound.SetSFXVolume(value));
                    break;
                default:
                    break;
            }
        }
        button_object = GameObject.FindObjectsOfType<Button>();
        SetButtonStatus();

        foreach (var item in UI_window_on)
            item.Value.SetActive(false);
    }

    public void SetButtonStatus()
    {
        foreach (var item in button_object)
        {
            switch (item.name)
            {
                case "Exit_button":
                    {
                        if(item.onClick.GetPersistentEventCount() == 0)
                        {
                            item.onClick.AddListener(() =>
                            {
                                if (Managers.GameManager.scene_name != "Main_screen")
                                {
                                    Managers.GameManager.InitPos = new Vector3(Managers.GameManager.InitPos.x, -2, 0);
                                    Managers.UI_jun.Fade_out_next_in("Black", 0, 1, Managers.instance.SceneAssetDic[SceneName.Main].name, 1, Managers.instance.OptionUIController);
                                }
                            });
                        }

                        item.gameObject.SetActive(false);
                    }
                    break;
                case "Retry_button":
                    {
                        if (item.onClick.GetPersistentEventCount() == 0)
                        {
                            item.onClick.AddListener(() =>
                            {
                                if (Managers.GameManager.scene_name != "Main_screen")
                                    Managers.UI_jun.Fade_out_next_in("Black", 0, 1, Managers.GameManager.scene_name, 1, Managers.instance.OptionUIController);

                            });
                        }

                        item.gameObject.SetActive(false);
                    }
                    break;
                case "Init":
                    {
                        if (item.onClick.GetPersistentEventCount() == 0)
                        {
                            item.onClick.AddListener(() =>
                            {
                                Managers.instance.OptionUIController();
                                Managers.UI_jun.Fade_out_next_in("Black", 0, 1, Managers.instance.SceneAssetDic[SceneName.Lobby].name, 1, InitGameMode);
                            });
                        }

                        item.gameObject.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

        }
    }

    private void InitGameMode()
    {
        Managers.GameManager.InitGameMode();
        Managers.GameManager.splash = false;
        SetButtonStatus();
    }

    public void Fade_out_in(string color, float out_delay, float out_duration, float in_delay, float in_duration, Action first_delay = null, Action second_delay = null)
    {
        UI_window_on[color].SetActive(true);
        UI_window_on[color].GetComponent<Image>().DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() =>
        {
            if (first_delay != null)
                first_delay();
            UI_window_on[color].GetComponent<Image>().DOFade(0, in_duration).SetDelay(in_delay).OnComplete(() =>
            {
                UI_window_on[color].GetComponent<Image>().DOFade(1, 0);
                UI_window_on[color].SetActive(false);
                if (second_delay != null)
                    second_delay();
            });
        });
    }
    public void Fade_out_in(string color, float out_delay, float out_duration, float in_delay, float in_duration, string next_scene)
    {
        UI_window_on[color].SetActive(true);
        UI_window_on[color].GetComponent<Image>().DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() =>
        {

            UI_window_on[color].GetComponent<Image>().DOFade(0, in_duration).SetDelay(in_delay).OnComplete(() =>
            {
                UI_window_on[color].GetComponent<Image>().DOFade(1, 0);
                UI_window_on[color].SetActive(false);
                SceneManager.LoadScene(next_scene);
            });
        });
    }
    public void Fade_out_in(string color, float out_delay, float out_duration, float in_delay, float in_duration, string next_scene, Action out_end_action = null, Action in_end_action = null)
    {
        UI_window_on[color].SetActive(true);
        Image temp_image = UI_window_on[color].GetComponent<Image>();
        Color origin_color = temp_image.color;
        temp_image.color = Color.clear;
        temp_image.DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() =>
        {
            if (out_end_action != null)
                out_end_action();
            temp_image.DOFade(0, in_duration).SetDelay(in_delay).OnComplete(() =>
            {
                temp_image.color = origin_color;
                UI_window_on[color].SetActive(false);
                if (in_end_action != null)
                    in_end_action();

                SceneManager.LoadScene(next_scene);
            });
        });
    }
    public void Fade_out_next_in(string color, float out_delay, float out_duration, string next_scene, float in_duration, Action action = null)
    {
        fade_start = true;
        UI_window_on[color].SetActive(true);
        Image temp_image = UI_window_on[color].GetComponent<Image>();
        Color origin_color = temp_image.color;
        temp_image.color = Color.clear;
        temp_image.DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() =>
        {
            Managers.Sound.bgSound.pitch = 0;
            SceneManager.LoadScene(next_scene);
            temp_image.DOFade(0, in_duration).OnComplete(() =>
            {
                fade_start = false;
                Managers.Sound.bgSound.pitch = 1;
                temp_image.color = origin_color;
                UI_window_on[color].SetActive(false);
                if (action != null)
                {
                    action();
                }
            });
        });
    }
    public void Game_over_ui()
    {
        UI_window_on["Game_over"].SetActive(true);
    }
    

}
