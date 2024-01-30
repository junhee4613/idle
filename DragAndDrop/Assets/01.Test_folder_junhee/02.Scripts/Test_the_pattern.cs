using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test_the_pattern : MonoBehaviour
{
    [Header("처음에 날아가는 최대 각도")]
    public float rotation_z;
    [Header("초기에 날아가는 힘")]
    public float shoot_power;
    float rotation_init;
    [Header("z축이 0으로 돌아오는 속도 ")]
    public float rotation_zero_speed;
    bool positive_num;
    [Header("1을 설정하면 초기 속도가 초당 1미터 이동")]
    public float test_num;

    private void Start()
    {
        rotation_init = Random.Range(-rotation_z, rotation_z);
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        Debug.Log(transform.rotation.eulerAngles.z);
        if (transform.rotation.eulerAngles.z < 90)
        {
            positive_num = true;
            //양수
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            positive_num = false;
            //음수
        }
    }

    private void FixedUpdate()
    {
        //transform.position = new Vector3(transform.position.x + Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), transform.position.y + Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) * (transform.position.magnitude + test_num + Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x + Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * Time.fixedDeltaTime * shoot_power, transform.position.y - (test_num += Time.fixedDeltaTime) * Time.fixedDeltaTime);
        if (positive_num)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z - rotation_zero_speed * Time.fixedDeltaTime, 0, 90));
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z + rotation_zero_speed * Time.fixedDeltaTime, 0, 359.99f));
        }
    }
}
/*public float bpm;       //0이 안나올라면 float / float를 해야된다
    public float pattern_end_time;
    public AudioSource au;
    public AudioClip clip;
    float pattern_duration;     //패턴이 지속된 시간
    float beat;                     //비트(박자)
    float time_gone;        //흘러간 시간
    Test_patterns_enum current_pattern;
    bool pattern_start;
    public Pattern1_tle test;
    public class Pattern1_tle 
    {
       public sbyte rain_drop_num;
    }


    private void Awake()
    {
    }
    private void Start()
    {
        beat = 60 / bpm;
    }
    private void FixedUpdate()
    {
        if (au.clip != clip)
        {
            au.clip = clip;
            au.Play();
        }
        time_gone += Time.fixedDeltaTime;

        if (beat <= time_gone)
        {
            time_gone -= beat;
            if (!pattern_start)
            {
                pattern_start = true;
                sbyte pattern_num = (sbyte)Random.Range(0, 3);
                current_pattern = (Test_patterns_enum)pattern_num;
            }
        }
        if (pattern_start)
        {
            Pattern_in_progress();
        }
    }
    public void Pattern_in_progress()
    {
        switch (current_pattern)
        {
            case Test_patterns_enum.PATTERN1:
                Pattern1();
                break;
            case Test_patterns_enum.PATTERN2:
                Pattern2();
                break;
            case Test_patterns_enum.PATTERN3:
                Pattern3();
                break;
            default:
                break;
        }
        

    }
    
    public void Pattern1()
    {
        if (pattern_duration <= pattern_end_time)
        {

            pattern_duration += Time.fixedDeltaTime;
        }
        else if (beat <= time_gone)
        {
            pattern_duration = 0;
            pattern_start = false;
        }
    }
    public void Pattern2()
    {
        if (pattern_duration <= pattern_end_time)
        {

            pattern_duration += Time.fixedDeltaTime;

        }
        else if (beat <= time_gone)
        {
            pattern_duration = 0;
            pattern_start = false;
        }
    }
    public void Pattern3()
    {
        if (pattern_duration <= pattern_end_time)
        {

            pattern_duration += Time.fixedDeltaTime;

        }
        else if (beat <= time_gone)
        {
            pattern_duration = 0;
            pattern_start = false;
        }
    }*/
