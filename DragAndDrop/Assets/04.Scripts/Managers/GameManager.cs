using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour        //여기서 비트를 관리
{
    public List<Pattern_state> pattern_data = new List<Pattern_state>();
    PlayerController player;
    public string scene_name;

    public PlayerController Player 
    { 
        get { 
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>();
                if(player == null)
                {
                    Managers.Resource.Load<GameObject>("Player");
                }
            } 
            return player; 
        }
    }
    public GameObject boss;
    public bool boss_die = false;
    public bool player_die = false;
    public Action gameover;
    public float beat;
    public float bmg_length;        //음악 진행 시간
    public bool game_start = false;
    public sbyte pattern_num;
    public bool game_stop = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

    }
    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene_name = scene.name;
        switch (scene_name)
        {
            case "Stage1":
                Stage1();
                break;
            default:
                break;
        }
        Managers.UI_jun.option_window_on = false;
    }
    public void Stage1()
    {
        TextAsset temp = Managers.Resource._resources["Stage1_data"] as TextAsset;
        if(temp == null)
        {
            Debug.Log("널ㅇ이ㅑ");
        }
        else
        {
            Debug.Log(temp.text);
        }
        pattern_data = JsonConvert.DeserializeObject<List<Pattern_state>>(temp.text);
    }
}
