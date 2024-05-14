using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public abstract class BossController : Stage_base_controller
{
    public Dictionary<GameObject, Animator> anim_end_push_objs = new Dictionary<GameObject, Animator>();
    protected override void Awake()
    {
        base.Awake();
    }
    public void FixedUpdate()
    {
        if (Managers.GameManager.game_start)
        {
            Pattern_processing();
            if(Managers.Sound.bgSound.time >= Managers.Sound.bgSound.clip.length - 0.2f)
            {
                Game_clear();
            }
            Anim_end_push();
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
    public void Warning_box_fade(Vector3 size, Vector3 pos, bool fade_option, sbyte count = 0, float minute = 0, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = size;
        if (fade_option)
        {
            warning_box.GetComponent<SpriteRenderer>().DOFade(1, 0);
            warning_box.GetComponent<SpriteRenderer>().DOFade(0, minute).SetLoops(count, LoopType.Yoyo).OnComplete(() =>
            {
                if(dotween_end_function != null)
                {
                    dotween_end_function();
                }
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
            warning_box.transform.DOScaleX(end_value.x, dotween_dration).OnComplete(() =>
            {
                dotween_end_function();
                Managers.Pool.Push(warning_box);
            });
        }
        else if (only_vertical)
        {
            warning_box.transform.DOScaleY(end_value.y, dotween_dration).OnComplete(() =>
            {
                dotween_end_function();
                Managers.Pool.Push(warning_box);
            });
        }
        else
        {
            warning_box.transform.DOScale(end_value, dotween_dration).OnComplete(() =>
            {
                dotween_end_function();
                Managers.Pool.Push(warning_box);
            });
        }
    }
    public GameObject Warning_box_punch_scale(Vector3 pos, Vector3 init_size, Vector3 first_dotween, float first_dotween_duration, Vector3 end_size, float seconde_dotween_duration
        ,bool auto_disable, bool only_vertical = false, bool only_horizontal = false, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = init_size;
        if (only_horizontal)
        {
            warning_box.transform.DOScaleX(first_dotween.x, first_dotween_duration).OnComplete(() => 
            {
                warning_box.transform.DOScaleX(end_size.x, seconde_dotween_duration).OnComplete(() => 
                { 
                    if(dotween_end_function != null)
                    {
                        dotween_end_function();
                    }
                    if (auto_disable)
                    {
                        Managers.Pool.Push(warning_box);
                    }
                }); 
            });
        }
        else if (only_vertical)
        {
            warning_box.transform.DOScaleY(first_dotween.y, first_dotween_duration).OnComplete(() =>
            {
                warning_box.transform.DOScaleY(end_size.y, seconde_dotween_duration).OnComplete(() =>
                {
                    if (dotween_end_function != null)
                    {
                        dotween_end_function();
                    }
                    if (auto_disable)
                    {
                        Managers.Pool.Push(warning_box);
                    }
                });
            });
        }
        else
        {
            warning_box.transform.DOScale(first_dotween, first_dotween_duration).OnComplete(() =>
            {
                warning_box.transform.DOScale(end_size, seconde_dotween_duration).OnComplete(() =>
                {
                    if (dotween_end_function != null)
                    {
                        dotween_end_function();
                    }
                    if (auto_disable)
                    {
                        Managers.Pool.Push(warning_box);
                    }
                });
            });
        }
        return warning_box;
    }
    public void Game_clear()
    {
        Managers.GameManager.game_start = false;
        Managers.UI_jun.Fade_out_next_in("Black", 0, 1, "Main_screen", 1);
    }
    public abstract void Pattern_processing();
    public void Anim_end_push()
    {
        if(anim_end_push_objs.Count != 0)
        {
            foreach (var item in anim_end_push_objs)
            {
                if(item.Value.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    Managers.Pool.Push(item.Key);
                }
            }
        }
    }
}
