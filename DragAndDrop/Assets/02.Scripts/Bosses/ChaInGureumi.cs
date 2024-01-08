using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ChaInGureumi : BossController
{
    public Cha_in_gureumi_simple_patterns simple_pattern;

    private void Awake()
    {
        Managers.GameManager.boss = this.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        pattern_num = Random.Range(1, Enum.GetNames(typeof(Cha_in_gureumi_simple_patterns)).Length);
    }

    // Update is called once per frame
    void Update()
    {
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
            case Cha_in_gureumi_simple_patterns.RANDOM_MULTIPLE_LIGHTNING:
                Random_multiple_lightning();
                break;
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
    void Random_multiple_lightning()
    {

    }
    void Raindrops()
    {

    }
    void Broad_based_lightning()
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
