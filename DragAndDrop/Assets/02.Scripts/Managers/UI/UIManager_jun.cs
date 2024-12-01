using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using UnityEditor;

public class UIManager_jun
{
    public Stack<GameObject> UI_window_off = new Stack<GameObject>();
    public bool option_window_on;
    public Dictionary<string, GameObject> UI_window_on = new Dictionary<string, GameObject>();
    public GameObject canvas;
    public Slider[] slider_object;
    public Button[] button_object;

    public Slider timer;
    //나중에 연출 때 hp바를 없앨것같아 미리 이렇게 처리함 어 근데 그냥 stage 한정 ui들 모두를 없애면 이거 필요없음
    public GameObject player_hp_bar;            //FIX : 여기 나중에 사용 안하면 없애자
    public GameObject boss_hp_bar;
    public GameObject[] player_hp = new GameObject[3];
    public GameObject[] boss_hp = new GameObject[3];
    public bool fade_start = false;

    private bool _init = false;
    public void Init()
    {
        timer = Managers.Resource.Load<GameObject>("Timer").GetComponent<Slider>();

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

        if(button_object == null)
            button_object = GameObject.FindObjectsOfType<Button>();

        SetButtonStatus();

        foreach (var item in UI_window_on)
            item.Value.SetActive(false);

        _init = true;
    }

    public void SetButtonStatus()
    {
        foreach (var item in button_object)
        {
            switch (item.name)
            {
                case "Exit_button":
                    {
                        if(_init == false)
                        {
                            item.onClick.AddListener(() =>
                            {
                                if (Managers.GameManager.sceneName != SceneName.Main)
                                {
                                    if (Time.timeScale == 0)
                                        Managers.instance.OptionUIController();

                                    Managers.GameManager.InitPos = new Vector3(Managers.GameManager.InitPos.x, -2, 0);
                                    Managers.UI_jun.Fade_out_next_in("Black", 0, 1, SceneName.Main, 1, Managers.Pool.Clear);
                                }
                            });
                        }

                        item.gameObject.SetActive(false);
                    }
                    break;
                case "Retry_button":
                    {
                        if (_init == false)
                        {
                            item.onClick.AddListener(() =>
                            {
                                if (Managers.GameManager.sceneName != SceneName.Main)
                                {
                                    if(Time.timeScale == 0)
                                        Managers.instance.OptionUIController();

                                    Managers.UI_jun.Fade_out_next_in("Black", 0, 1, Managers.GameManager.sceneName, 1, Managers.Pool.Clear);
                                }

                            });
                        }

                        item.gameObject.SetActive(false);
                    }
                    break;
                case "Init":
                    {
                        if (_init == false)
                        {
                            item.onClick.AddListener(() =>
                            {
                                if(Time.timeScale == 0)
                                    Managers.instance.OptionUIController();

                                Managers.UI_jun.Fade_out_next_in("Black", 0, 1, SceneName.Lobby, 1, Managers.GameManager.stage_clear_init);
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

    public void Fade_out_next_in(string color, float out_delay, float out_duration,SceneName nextScene , float in_duration, Action action = null)
    {
        fade_start = true;
        UI_window_on[color].SetActive(true);
        Image temp_image = UI_window_on[color].GetComponent<Image>();
        Color origin_color = temp_image.color;
        temp_image.color = Color.clear;
        temp_image.DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() =>
        {
            Managers.Sound.bgSound.pitch = 0;
            Managers.GameManager.sceneName = nextScene;
            SceneManager.LoadScene(Managers.instance.SceneAssetDic[nextScene]);
            temp_image.DOFade(0, in_duration).OnComplete(() =>
            {
                fade_start = false;
                Managers.Sound.bgSound.pitch = 1;
                temp_image.color = origin_color;
                UI_window_on[color].SetActive(false);
                Managers.Pool.Clear();
                action?.Invoke();
            });
        });
    }

    public void Game_over_ui()
    {
        UI_window_on["Game_over"].SetActive(true);
    }
    

}
