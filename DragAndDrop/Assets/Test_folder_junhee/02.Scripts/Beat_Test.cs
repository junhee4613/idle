using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test_boss;
using DG.Tweening;

public class Beat_Test : MonoBehaviour
{
    public float pattern_end_time;
    public float test_pattern_end_time;
    float pattern_current_time;
    float test_pattern_current_time;
    float beat;
    public float bpm;       //0이 안나올라면 float / float를 해야된다
    float beat_time;
    public AudioSource au;
    public AudioClip clip;
    public Test_patterns_enum current_pattern;
    bool pattern_start;
    sbyte pattern_num;

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
        beat_time += Time.fixedDeltaTime;
        test_pattern_current_time += Time.fixedDeltaTime;

        if (beat <= beat_time)
        {
            beat_time -= beat;
            if (!pattern_start)
            {
                pattern_start = true;
                pattern_num = (sbyte)Random.Range(0, 3);
                current_pattern = (Test_patterns_enum)pattern_num;
            }
            else if(test_pattern_current_time <= test_pattern_end_time)   //이거 테스트용이라 else로 해야됨
            {
                Test_beat_patterns();
            }
            else
            {
                test_pattern_current_time = 0;
                pattern_start = false;
            }
        }
        /*if (pattern_start)
        {
            Pattern_in_progress();
        }*/
    }
    public void Test_beat_patterns()
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
    public void Pattern_in_progress() 
    {

        if (pattern_current_time <= pattern_end_time)
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
            pattern_current_time += Time.fixedDeltaTime;

        }
        else if(beat <= beat_time)
        {
            pattern_current_time = 0;
            pattern_start = false;
        }

    }
    public void Pattern1()
    {
        transform.position = new Vector2(transform.position.x , transform.position.y * -1);
    }
    public void Pattern2()
    {
        transform.position = new Vector2(transform.position.x * -1, transform.position.y);
    }
    public void Pattern3()
    {
        transform.Rotate(0, 0, 15);
    }
}
