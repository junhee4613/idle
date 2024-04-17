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
    public string scene_name;
    public bool option_window_on = false;
    public Dictionary<string, bool> stage_clear = new Dictionary<string, bool>() 
    { {"Stage1", false}, { "Stage2", false }, { "Stage3", false }, { "Stage4", false } };

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
    public bool boss_die = false;
    public bool player_die = false;
    public Action gameover;
    public float beat;
    public float bgm_length;        //���� ���� �ð�
    public bool game_start = false;
    public sbyte pattern_num;
    public bool game_stop = false;
    public void Init()
    {
    }
    // Start is called before the first frame update
    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        game_start = false;
        scene_name = scene.name;
        Managers.Pool.Clear();
        switch (scene_name)
        {
            case "Stage1":
                Stage1();
                break;
            default:
                break;
        }
        if (scene_name.Contains("Stage"))
        {
        }
        Managers.UI_jun.option_window_on = false;           //FIX : �̰� �� �ִ��� �𸣰��� 
    }
    public void Stage1()
    {
        //FIX : ���߿� ���⿡ �÷��̾� �ڽ� ������ �����ϴ� ���� �ֱ�
    }
}