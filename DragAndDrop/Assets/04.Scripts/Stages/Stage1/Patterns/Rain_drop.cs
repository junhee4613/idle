using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_drop : MonoBehaviour, IInteraction_obj
{
    [Header("떨어지는 속도")]
    public float gravity_value;
    [Header("사라지는 시간")]
    public float push_time;
    float time = 0;
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        transform.position += new Vector3(gravity_value * Time.fixedDeltaTime * Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), -gravity_value * Time.fixedDeltaTime * Mathf.Abs(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)), 0);
        if(time >= push_time)
        {
            Managers.Pool.Push(this.gameObject);
        }

    }
    private void OnEnable()
    {
        time = 0;
    }
    public void practice()
    {
        //time = 0;
        Managers.GameManager.Player.Hit();
        Managers.Pool.Push(this.gameObject);
    }
}
/*[Header("처음에 날아가는 최대 각도")]
    public float rotation_z;
    [Header("초기에 날아가는 힘")]
    public float shoot_power;
    float rotation_init;
    public float simple_pattern_2_rotation;
    [Header("z축이 0으로 돌아오는 속도(높을수록 빨리 0으로 되돌아감)")]
    public float rotation_zero_speed;
    bool right_tilt;
    [Header("중력 값")]
    public float temp_gravity;
    float gravity;
    [Header("사라지는 시간")]
    public float push_time;
    float time;
    private void Start()
    {
        gravity = temp_gravity;
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].simple_pattern_type)
        {
            case 1:
                rotation_init = Random.Range(-rotation_z, rotation_z);
                break;
            case 2:
                simple_pattern_2_init();
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        if (transform.rotation.eulerAngles.z < 90)
        {
            right_tilt = true;
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            right_tilt = false;
        }
    }

    private void FixedUpdate()
    {
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].simple_pattern_type)
        {
            case 1:
                Simple_pattern1_();
                break;
            case 2:
                Simple_pattern_2();
                break;
            default:
                break;
        }
        transform.position = new Vector3(transform.position.x + Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * Time.fixedDeltaTime * shoot_power, transform.position.y - (gravity += Time.fixedDeltaTime) * Time.fixedDeltaTime);
        if (push_time <= time)
        {
            time = 0;
            Managers.Pool.Push(this.gameObject);
        }
        else
        {
            time += Time.fixedDeltaTime;
        }


    }
    public void Simple_pattern1_()
    {
        Init_tilt();
    }
    public void Simple_pattern_2()
    {
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].action_num)
        {
            case 0:
                Init_tilt();
                break;
            case 1:
                if (right_tilt)
                {
                    right_tilt = false;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 359.99f);
                }
                if (transform.rotation.eulerAngles.z != 315)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z - rotation_zero_speed * Time.fixedDeltaTime, 315, 359.99f));
                }
                break;
            case 2:
                if (!right_tilt)
                {
                    right_tilt = true;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                }
                if (transform.rotation.eulerAngles.z != 45)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z + rotation_zero_speed * Time.fixedDeltaTime, 0, 45));
                }
                break;
            default:
                break;
        }
    }
    public void Init_tilt()
    {
        if (right_tilt)
        {
            if (transform.rotation.eulerAngles.z != 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z - rotation_zero_speed * Time.fixedDeltaTime, 0, 90));
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.z != 359.99f)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z + rotation_zero_speed * Time.fixedDeltaTime, 0, 359.99f));

            }
        }
    }
    public void simple_pattern_2_init()
    {
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].action_num)
        {
            case 0:
                rotation_init = 0;
                break;
            case 1:
                rotation_init = -simple_pattern_2_rotation;

                break;
            case 2:
                rotation_init = simple_pattern_2_rotation;

                break;
            default:
                break;
        }
    }
    private void OnEnable()
    {
        gravity = temp_gravity;
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].simple_pattern_type)
        {
            case 1:
                rotation_init = Random.Range(-rotation_z, rotation_z);
                break;
            case 2:
                simple_pattern_2_init();
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        if (transform.rotation.eulerAngles.z < 90)
        {
            right_tilt = true;
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            right_tilt = false;
        }
    }*/
