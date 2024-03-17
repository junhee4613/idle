using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossController : Stage_base_controller
{
    public sbyte gimmick_num = 4;
    protected sbyte boss_hp = 5;
    public SpriteRenderer boss_image;
    public Action patterns;
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
    public void Pattern_function(List<Pattern_json_date> pattern_json_data, bool pattern_ending, float pattern_duration_time, sbyte pattern_count, 
         Action not_duration_pattern, bool pattern_duration_obj_enable = false, float pattern_time = 0f, float time = 0f, Action duration_pattern = null)
    {
        if (!pattern_ending)
        {
            if ((pattern_json_data[pattern_count].time <= Managers.Sound.bgSound.time || pattern_duration_time != 0))
            {
                if (pattern_duration_time == 0)
                {
                    pattern_duration_time = pattern_json_data[pattern_count].duration;
                }
                
                if (pattern_duration_obj_enable)
                {
                    pattern_time -= Time.fixedDeltaTime;
                    if (pattern_time <= 0)
                    {
                        pattern_time += time;
                        duration_pattern();
                    }
                }
                not_duration_pattern();
                pattern_duration_time = Mathf.Clamp(pattern_duration_time - Time.fixedDeltaTime, 0, pattern_json_data[pattern_count].duration);
                if (pattern_duration_time == 0)
                {
                    pattern_count++;
                    if (pattern_json_data.Count == pattern_count)
                    {
                        pattern_ending = true;
                    }
                }
            }
        }
    }
    public virtual void Pattern_processing()
    {

        
    }
}
