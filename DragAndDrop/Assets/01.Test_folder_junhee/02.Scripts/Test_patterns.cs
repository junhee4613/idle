using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test_boss;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class Test_patterns : MonoBehaviour
{
    [Header("한 패턴에 떨어지는 빗물의 갯수")]
    public sbyte rain_drop_num;
    [Header("생성되는 높이")]
    public float pos_y;
    [Header("생성되는 빗물 오브젝트 할당")]
    public GameObject rain_drop_obj;
    [Header("생성되는 x축 양의 범위")]
    public float pos_x;
    sbyte rain_drop_count;
    public float test_time;
    private void FixedUpdate()
    {
        test_time += Time.fixedDeltaTime;
        if (rain_drop_num > rain_drop_count && test_time >= 1)
        {
            test_time -= 1;
            Instantiate(rain_drop_obj).transform.position = new Vector2(Random.Range(-pos_x, pos_x), pos_y);
            rain_drop_count++;
        }
    }
}


//번개 패턴
/*[Header("패턴이 등장하는 위치(높이)")]
    public float pos_y;
    [Header("패턴이 등장하는 x축 위치들")]
    public float[] pos_min_x;
    Stack<float> pos_x = new Stack<float>();
    List<float> temp_x = new List<float>();
    [Header("패턴이 등장하는 횟수")]
    public sbyte pattern_num;
    sbyte pattern_count;
    Vector2 pos;
    bool pattern_setting = false;
    public GameObject lightning;
    int pos_select_range;
    public float time;


    // Start is called before the first frame update
    void Start()
    {
        pattern_count = pattern_num;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (0 < pattern_count)
        {
            if (time > 1)
            {
                if (!pattern_setting)
                {
                    pos_select_range = pos_min_x.Length - 1;
                    pattern_setting = true;
                    pattern_count = pattern_num;
                    for (int i = pos_min_x.Length - 1; i >= 0; i--)
                    {
                        temp_x.Add(pos_min_x[i]);
                    }

                    for (int i = 0; i < pattern_num; i++)
                    {
                        Debug.Log(pos_select_range);
                        int num = Random.Range(0, pos_select_range);
                        pos_x.Push(temp_x[num]);
                        temp_x.RemoveAt(num);
                        pos_select_range--;
                    }
                }
                pos = new Vector2(pos_x.Pop(), pos_y);
                GameObject obj = Instantiate(lightning);
                obj.transform.position = pos;
                time -= 1;
                pattern_count--;
            }
        }
        *//*else
        {
            pattern_setting = false;
            pattern_count = pattern_num;
        }*//*
    }*/