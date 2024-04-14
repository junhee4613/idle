using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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
    public void Pattern_function(ref List<Pattern_json_date> pattern_json_data, ref bool pattern_ending, ref float pattern_duration_time, ref sbyte pattern_count, 
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
    public void Warning_box_fade(Vector3 size, Vector3 pos, bool fade_option, sbyte count = 0, float minute = 0)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = size;
        if (fade_option)
        {
            warning_box.GetComponent<SpriteRenderer>().DOFade(1, 0);
            warning_box.GetComponent<SpriteRenderer>().DOFade(0, minute).SetLoops(count, LoopType.Yoyo).OnComplete(() =>
            {
                Managers.Pool.Push(warning_box);
            });
        }
    }
    public void Warning_box_scale(Vector3 init_size, Vector3 pos, float dotween_dration, Vector3 end_value, bool only_vertical = false, bool only_horizontal = false, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = init_size;
        if (only_horizontal)
        {
            warning_box.transform.DOScaleX(end_value.x, dotween_dration).OnComplete(() => dotween_end_function());
        }
        else if (only_vertical)
        {
            warning_box.transform.DOScaleY(end_value.y, dotween_dration).OnComplete(() => dotween_end_function());
        }
        else
        {
            warning_box.transform.DOScale(end_value, dotween_dration).OnComplete(() => dotween_end_function());
        }
    }
    public GameObject Warning_box_punch_scale(Vector3 pos, Vector3 init_size, Vector3 first_dotween, float first_dotween_duration, Vector3 end_size, float seconde_dotween_duration
        , bool only_vertical = false, bool only_horizontal = false, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = init_size;
        if (only_horizontal)
        {
            warning_box.transform.DOScaleX(first_dotween.x, first_dotween_duration).OnComplete(() => 
            {
                warning_box.transform.DOScaleX(end_size.x, seconde_dotween_duration).OnComplete(() => dotween_end_function()); 
            });
        }
        else if (only_vertical)
        {
            warning_box.transform.DOScaleY(first_dotween.y, first_dotween_duration).OnComplete(() =>
            {
                warning_box.transform.DOScaleY(end_size.y, seconde_dotween_duration).OnComplete(() => dotween_end_function());
            });
        }
        else
        {
            warning_box.transform.DOScale(first_dotween, first_dotween_duration).OnComplete(() =>
            {
                warning_box.transform.DOScale(end_size, seconde_dotween_duration).OnComplete(() => dotween_end_function());
            });
        }
        return warning_box;
    }
    public virtual void Pattern_processing()
    {

        
    }
}
