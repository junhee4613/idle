using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager
{
    public List<Pattern_state> pattern_data = new List<Pattern_state>();
    PlayerController player;
    Transform player_character;
    GameObject beat_box;
    public GameObject Beat_box 
    { 
        get
        { 
            if(beat_box == null)
            {
                beat_box = GameObject.FindGameObjectWithTag("Beat_box");
            }
            return beat_box; 
        } 
    }
    public bool splash = false;             //스플레시 화면일 때 쓰는 불값
    public string scene_name;
    public bool load_end = false;
    public bool operate = false;
    public bool option_window_on = false;
    public Dictionary<string, bool> stage_clear = new Dictionary<string, bool>()
    { { "Tutorial_stage", false },  {"Chapter1_boss_stage", false}, { "Chapter2_boss_stage", false }, { "Chapter2_general_stage1", false }};
    public PlayerController Player 
    { 
        get { 
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>();
            } 
            return player; 
        }
    }
    public Transform Player_character
    {
        get
        {
            if(player_character == null)
            {
                player_character = GameObject.FindGameObjectWithTag("Player_character").transform;
            }
            return player_character;
        }
    }

    public GameObject boss;
    public bool player_die = false;
    public Action gameover;
    public bool game_start = false;
    public bool game_stop = false;
    public bool tutorial = false;
    public bool tutorial_hit = false;
    //public event Action portal_init;
    // Start is called before the first frame update
    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene_name = scene.name;
        if (!string.IsNullOrEmpty(scene_name))
        {
            Sound_init(scene_name);
        }
        if (scene_name == "Tutorial_stage")
        {
            operate = false;
        }
        else
        {
            operate = true;
        }
        Stage_setting();
        UI_init();
    }
    
    public void Stage1()
    {
        //FIX : 나중에 여기에 플레이어 박스 파츠들 변경하는 로직 넣기
    }
    public void Sound_init(string scene_name)
    {
        if (scene_name == "Main_screen")
        {
            Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>(scene_name), true);

        }
        else if (scene_name != "Tutorial_stage")
        {
            Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>(scene_name), false);
        }
        else
        {
            Managers.Sound.bgSound.Stop();
        }
    }
    public void UI_init()
    {
        Managers.UI_jun.option_window_on = false;           //FIX : 이거 왜 있는지 모르겠음 
        if (Managers.UI_jun.UI_window_on["Game_over"].activeSelf)
        {
            Managers.UI_jun.UI_window_on["Game_over"].SetActive(false);
        }
        if (Managers.UI_jun.UI_window_off.Count != 0)
        {
            for (int i = 0; i < Managers.UI_jun.UI_window_off.Count; i++)
            {
                Managers.UI_jun.UI_window_off.Peek().SetActive(false);
            }
        }
    }
    public void Stage_setting()
    {
        Managers.Pool.Clear();
        switch (scene_name)
        {
            case "Main_screen":
                Setting_main_stage();
                break;
            default:
                break;
        }
    }
    void Setting_main_stage()
    {

    }

}
