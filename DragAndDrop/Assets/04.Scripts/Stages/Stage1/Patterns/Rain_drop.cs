using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_drop : MonoBehaviour
{
    [Header("처음에 날아가는 최대 각도")]
    public float rotation_z;
    [Header("초기에 날아가는 힘")]
    public float shoot_power;
    float rotation_init;
    [Header("z축이 0으로 돌아오는 속도(높을수록 빨리 0으로 되돌아감)")]
    public float rotation_zero_speed;
    bool positive_num;
    [Header("중력 값")]
    public float temp_gravity;
    float gravity;
    [Header("사라지는 시간")]
    public float push_time;
    float time;
    private void Start()
    {
        gravity = temp_gravity;
        rotation_init = Random.Range(-rotation_z, rotation_z);
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        if (transform.rotation.eulerAngles.z < 90)
        {
            positive_num = true;
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            positive_num = false;
        }
    }

    private void FixedUpdate()
    {
        if(push_time <= time)
        {
            time = 0;
            Managers.Pool.Push(this.gameObject);
            gravity = temp_gravity;
        }
        time += Time.fixedDeltaTime;
        transform.position = new Vector3(transform.position.x + Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * Time.fixedDeltaTime * shoot_power, transform.position.y - (gravity += Time.fixedDeltaTime) * Time.fixedDeltaTime);
        if (positive_num)
        {
            if(transform.rotation.eulerAngles.z != 0)
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
    private void OnEnable()
    {

        rotation_init = Random.Range(-rotation_z, rotation_z);
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        if (transform.rotation.eulerAngles.z < 90)
        {
            positive_num = true;
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            positive_num = false;
        }
    }
}
