using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;

public class ChaInGureumi : BossController
{
    public Cha_in_gureumi_simple_patterns simple_pattern;

    private void Awake()
    {
        Managers.GameManager.boss = this.gameObject;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        pattern_num = Random.Range(1, Enum.GetNames(typeof(Cha_in_gureumi_simple_patterns)).Length);
        gimmick_count = gimmick_num - 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if(사운드 매니저에서 불러온 노래 길이 / gimmick_count() * gimmick_num 패턴 등장 횟수이거 올림으로 해서 딱 맞아 떨어지게 하기 == 스테이지 진행도(노래의 최대 길이에서 점차 감소할 때))
        Current_pattern();
    }
    public void Current_pattern()
    {
        if (!Pattern_in_progress)
        {
            if (hp <= hard_pattern_start_hp)
            {
                Simple_patterns();
            }
            else
            {
                Hard_patterns();
            }
        }
    }
    #region 패턴들
    #region 심플 패턴들
    void Simple_patterns()
    {
        switch (simple_pattern)
        {
            case Cha_in_gureumi_simple_patterns.SINGLE_LIGHTNING:
                Single_lightning();
                break;
           /* case Cha_in_gureumi_simple_patterns.RANDOM_MULTIPLE_LIGHTNING:
                Random_multiple_lightning();
                break;*/
            case Cha_in_gureumi_simple_patterns.RAINDROPS:
                Raindrops();
                break;
            case Cha_in_gureumi_simple_patterns.BROAD_BASED_LIGHTNING:
                Broad_based_lightning();
                break;
            default:
                break;
        }
    }
    void Single_lightning()
    {

    }
    /*void Random_multiple_lightning()
    {

    }*/
    void Raindrops()                //빗방울
    {

    }
    void Broad_based_lightning()    //차징 번개
    {

    }
    #endregion
    #region 하드패턴들
    void Hard_patterns()
    {
        
    }
    #endregion
    #endregion
    
    void Player_hit()
    {
        Managers.GameManager.player.Hit(1);
    }

    public override void Hit(float damage)
    {
        throw new NotImplementedException();
    }
}
