using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_projectile : MonoBehaviour
{
    public float push_time;
    float time;
    bool init = false;
    float speed;
    float rot_speed;
    Projectile_moving_mode moving_mode;
    public void Init(float push_time,float speed, Projectile_moving_mode moving_move)
    {
        this.push_time = push_time;
        this.moving_mode = moving_move;
        this.speed = speed;
        init = true;
    }
    public void Init(float push_time, float speed, float rot_speed, Projectile_moving_mode moving_move)
    {
        this.push_time = push_time;
        this.moving_mode = moving_move;
        this.speed = speed;
        this.rot_speed = rot_speed;
        init = true;
    }
    private void Update()
    {
        if(time > push_time)
        {
            Managers.Pool.Push(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (init)
        {
            Projectile_move();
        }
    }
    private void OnDisable()
    {
        time = 0;
        init = false;
    }
    private void OnEnable()
    {

    }
    public void Projectile_move()
    {
        switch (moving_mode)
        {
            case Projectile_moving_mode.GENERAL:
                General_move();
                break;
            case Projectile_moving_mode.GUIDED_X:
                Guided('x');
                break;
            case Projectile_moving_mode.GUIDED_Y:
                Guided('y');
                break;
            case Projectile_moving_mode.NON:
                Debug.LogError("모드 설정을 안했음");
                break;
            default:
                break;
        }
    }
    public void General_move()
    {
        transform.Translate(transform.up * (speed * Time.deltaTime));
    }
    public void Guided(char dir)
    {
        switch (dir)
        {
            case 'x':
                break;
            case 'y':
                break;
        }
    }
}
