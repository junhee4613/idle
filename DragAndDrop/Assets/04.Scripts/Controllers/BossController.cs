using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    [Header("이 보스에 약점 패턴을 성공해야되는 횟수")]
    public int gimmick_complete_num;
    [Header("보스 피는 정수만 사용함")]
    public int hp;
    [Header("이 보스에 약점 패턴이 등장하는 횟수")]
    public int gimmick_num;
    [Header("이 보스에 패턴이 빨라지는 속도")]
    public int patterns_speed;
    #region 나중에 private로 바꿀 것들
    public int pattern_num;
    public int gimmick_complete_count = 0;      //보스 약점기믹 성공한 횟수
    public bool Pattern_in_progress = false;
    public float hard_pattern_start_hp;
    public GameObject[] simple_patterns;        //이건 나중에 어드레서블로 저장해서 패턴을 불러오는 식으로 하자
    public GameObject[] hard_patterns;          //이건 나중에 어드레서블로 저장해서 패턴을 불러오는 식으로 하자
    public bool weakness_pattern_start = false; //기믹이 시작했는지
    
    protected int gimmick_count;  //기믹이 등장 할 때마다 숫자가 내려감
    
    public void Weakness_pattern()
    {

    }
    public virtual void Start()
    {
        gimmick_count = gimmick_num - 1;
    }
    public void die()
    {
        if(hp <= 0)
        {
            if(gimmick_complete_num <= gimmick_complete_count)
            {
                //보스의 패턴 속도가 더 빨라짐
            }
            else
            {
                //die(클리어)로직
            }

        }
    }
    #endregion

    // Start is called before the first frame update
}
