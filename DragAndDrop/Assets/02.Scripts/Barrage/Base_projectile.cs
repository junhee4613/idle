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
    Rigidbody2D rb;
    public void Init(float push_time,float speed, Projectile_moving_mode moving_move)
    {
        this.push_time = push_time;
        this.moving_mode = moving_move;
        this.speed = speed;
        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        init = true;
    }
    public void Init(float push_time, float speed, float rot_speed, Projectile_moving_mode moving_move)
    {
        this.push_time = push_time;
        this.moving_mode = moving_move;
        this.speed = speed;
        this.rot_speed = rot_speed;
        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        init = true;
    }
    private void Update()
    {
        if(time > push_time)
        {
            Managers.Pool.Push(this.gameObject);
        }
        else
        {
            time += Time.deltaTime;
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
    void Projectile_move()
    {
        switch (moving_mode)
        {
            case Projectile_moving_mode.GENERAL:
                General_move();
                break;
            case Projectile_moving_mode.GUIDED_X:
                Guided('x');
                General_move();
                break;
            case Projectile_moving_mode.GUIDED_Y:
                Guided('y');
                General_move();
                break;
            case Projectile_moving_mode.NON:
                Debug.LogError("모드 설정을 안했음");
                break;
            default:
                break;
        }
    }
    void General_move()
    {
        rb.velocity = transform.up * speed;
    }
    void Guided(char dir)
    {
        switch (dir)
        {
            case 'x':
                Rotation(Managers.GameManager.Player_character.position.x,transform.position.x);
                break;
            case 'y':
                Rotation(Managers.GameManager.Player_character.position.y, transform.position.y);
                break;
        }
    }
    float Look_at_target(Vector3 target)
    {
        return Mathf.Atan2(transform.position.y - target.y, transform.position.x - target.x) * Mathf.Rad2Deg + 90;
    }
    void Rotation(float target_pos, float this_pos)
    {
        if(target_pos > this_pos)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, Look_at_target(Managers.GameManager.Player_character.transform.position)), rot_speed * Time.deltaTime);
        }
    }
}
