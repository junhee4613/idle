using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Camera_manager
{
    Camera main_camera;
    float time;
    public Camera_focus_state state;
    public Camera Main_camera { 
        get 
        {
            if (main_camera == null)
            {
                main_camera = GameObject.FindObjectOfType<Camera>();
            }
            return main_camera; 
        }
    }
    public void Shake_move(float duration = 0.3f, float strength = 0.3f, int vibrato = 100, float randomness = 90, bool snapping = false, bool fade_out = true)
    {
        if(snapping == false)
        {
            Main_camera.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fade_out).OnComplete(() =>
            {
                Main_camera.transform.DOMove(new Vector3(0, 0, Main_camera.transform.position.z), 1f / vibrato);
            });
        }
        else
        {
            Main_camera.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fade_out);
        }
    }
    public void Move_y(float move_pos,float move_pos_time, float go_back_pos, float go_back_pos_time)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Main_camera.transform.DOMoveY(move_pos, move_pos_time));
        sequence.Append(Main_camera.transform.DOMoveY(go_back_pos, go_back_pos_time));
    }
    public void Punch(float expansion, float reduction, float time)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Main_camera.DOOrthoSize(expansion, time));
        sequence.Append(Main_camera.DOOrthoSize(reduction, time));
    }
    
    public void Focus_move(Vector3 pos)
    {
        Main_camera.transform.DOMove(pos, 0.3f).SetEase(Ease.OutQuint);
    }
    /*public void Fade_out_in(string color,float out_delay, float out_duration, float in_delay, float in_duration, Action first_delay = null, Action second_delay = null)
    {
        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>(color + "_fade"));
        temp.GetComponent<SpriteRenderer>().DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() => 
        {
            if (first_delay != null)
                first_delay();
            temp.GetComponent<SpriteRenderer>().DOFade(0, in_duration).SetDelay(in_delay).OnComplete(() => 
            {

                Managers.Pool.Push(temp);
                if (second_delay != null)
                    second_delay();
            });
        });
    }*/
}
