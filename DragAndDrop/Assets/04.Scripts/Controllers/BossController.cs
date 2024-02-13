using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Stage_base_controller
{
    public sbyte gimmick_num = 4;
    protected sbyte boss_hp = 5;
    public SpriteRenderer boss_image;
    //public Animator an;
    protected override void Awake()
    {
        base.Awake();
    }
    public void FixedUpdate()
    {
        if (Managers.GameManager.game_start)
        {
            Managers.UI_jun.timer.value = Mathf.Clamp(Managers.UI_jun.timer.value - Time.fixedDeltaTime, 0, Managers.UI_jun.timer.value);
            Managers.UI_jun.timer.value -= 1f / Managers.GameManager.bgm_length * Time.fixedDeltaTime;
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
    public virtual void Pattern_processing()
    {

        /*if (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].time <= Managers.Sound.bgSound.time)
        {
            if (boss_hp > 3)
            {
                Simple_pattern();
            }
            else
            {
                Hard_pattern();
            }
            if(Managers.GameManager.pattern_data.Count - 1 > Managers.GameManager.pattern_num)
            {
                Managers.GameManager.pattern_num++;
            }
        }*/
    }
    /*public virtual void Simple_pattern()
    {

    }
    public virtual void Hard_pattern()
    {

    }*/
    public void Anim_state_machin(string clip_name)
    {
        /*if (an.GetCurrentAnimatorStateInfo(0). != clip_name)
        {

        }*/
        Debug.Log("����");
        if (an.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("����");
            an.Play(clip_name);
        }

    }
}
