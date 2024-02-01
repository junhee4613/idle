using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    public sbyte gimmick_num = 4;
    protected sbyte boss_hp = 5;
    public sbyte pattern_num;
    public void FixedUpdate()
    {
        if (Managers.GameManager.game_start)
        {
            if (boss_hp > 0)
            {
                Pattern_processing();
            }
            else
            {
                Managers.GameManager.boss_die = true;
            }
        }
    }
    public void Pattern_processing()
    {
        Debug.Log(Managers.Sound.bgSound.time);
        if (Managers.GameManager.pattern_data[pattern_num].time <= Managers.Sound.bgSound.time)
        {
            if (boss_hp > 3)
            {
                Simple_pattern();
            }
            else
            {
                Hard_pattern();
            }
            if(Managers.GameManager.pattern_data.Count - 1 > pattern_num)
            {
                pattern_num++;
            }
        }
    }
    public virtual void Simple_pattern()
    {

    }
    public virtual void Hard_pattern()
    {

    }
}
