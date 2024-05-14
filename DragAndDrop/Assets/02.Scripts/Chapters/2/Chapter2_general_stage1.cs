using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Chapter2_general_stage1 : BossController
{
    public Cactus_climb_up cactus_climb_up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Pattern_processing()
    {
        Pattern_function(ref cactus_climb_up.pattern_data, ref cactus_climb_up.pattern_ending, ref cactus_climb_up.duration,ref cactus_climb_up.pattern_count, Cactus_climb_up_pattern);
    }
    public void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:             //선인장 하나 생성
                Managers.Pool.Pop(Managers.Resource.Load<GameObject>("여기에 나중에 선인장")).transform;
                break;
            case 1:             //선인장 생성하면서 움직임
                break;
            case 2:
                break;          //선인장 사라짐
        }
    }
    [Serializable]
    public class Cactus_climb_up : Pattern_base_data
    {
        public float speed;         //선인장 움직이는 속도
        public float[] cactus_size;     //움직이는 선인장 사이즈들
        public int appearance_dir = 0;      //선인장 등장하는 방향
        public GameObject[] moving_cactus;  //움직이는 선인장들 넣어두는 곳
        public sbyte[] array_num;

    }
}
