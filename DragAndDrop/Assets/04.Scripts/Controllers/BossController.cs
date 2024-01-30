using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    [Header("이 보스에 약점 패턴을 성공해야되는 횟수")]
    public int gimmick_complete_num;
    [Header("노래 진행도")]
    public float hp;
    [Header("하드패턴 시작 기준 진행도")]
    public float hard_pattern_start_hp;
    [Header("이 보스에 약점 패턴이 등장하는 횟수")]
    public int gimmick_num;
    [Header("이 보스에 패턴이 빨라지는 속도")]       //이거 뺄 수도 있음
    public int patterns_speed;
    [Header("진행해야되는 패턴 노트 검출")]
    public RaycastHit2D pattern_note;
    [Header("패턴 종류들")]
    public LayerMask pattern_kind;
    public Transform note_pos;
    #region 나중에 private로 바꿀 것들
    protected int gimmick_complete_count = 0;      //보스 약점기믹 성공한 횟수
    //protected bool Pattern_in_progress = false;
    protected float time;
    protected bool weakness_pattern_start = false; //기믹이 시작했는지
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
                //게임 클리어
            }
            else
            {
                //즉사기 시전
            }
        }
    }
    #endregion

    // Start is called before the first frame update
}
