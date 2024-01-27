using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    public PlayerController Player 
    { 
        get { 
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>();
                if(player == null)
                {
                    Debug.LogError("플레이어가 없어");
                }
            } 
            return player; 
        }
    }
    public GameObject boss;
    public bool boss_die = false;
    public bool player_die = false;
    public Action gameover;
    public float game_speed = 1;
    public Action slow_skill_down;
    public Action slow_skill_up;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
