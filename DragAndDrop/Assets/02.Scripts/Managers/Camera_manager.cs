using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Camera_manager
{
    float time;
    Camera main_camera;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shake_move()
    {
        Main_camera.transform.DOShakePosition(0.3f, 0.3f, 100, 90, false, true);
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
    public void Fade_out_in(string color,float out_delay, float out_duration, float in_delay, float in_duration, Action action1 = null, Action action2 = null)
    {
        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>(color + "_fade"));
        temp.GetComponent<SpriteRenderer>().DOFade(1, out_duration).SetDelay(out_delay).OnComplete(() => 
        {
            Managers.Pool.Push(temp);
            if (action1 != null)
                action1();
            temp.GetComponent<SpriteRenderer>().DOFade(0, in_duration).SetDelay(in_delay).OnComplete(() => 
            { 
                if(action2 != null)
                    action2();
            });
        });
    }
}
