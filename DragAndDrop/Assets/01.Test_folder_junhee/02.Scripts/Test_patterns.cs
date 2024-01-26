using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test_boss;

public class Test_patterns : MonoBehaviour
{
    public float bpm;       //0이 안나올라면 float / float를 해야된다
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
    }
}
