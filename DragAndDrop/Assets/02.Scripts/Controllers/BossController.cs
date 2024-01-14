using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    #region 나중에 private로 바꿀 것들
    public int pattern_num;
    public bool Pattern_in_progress = false;
    public int hp;
    public int hard_pattern_start_hp;
    public GameObject[] simple_patterns;        //이건 나중에 어드레서블로 저장해서 패턴을 불러오는 식으로 하자
    public GameObject[] hard_patterns;          //이건 나중에 어드레서블로 저장해서 패턴을 불러오는 식으로 하자
    public bool weakness_pattern_start = false; //기믹이 시작했는지
    [Header("이 보스에 약점 패턴이 등장하는 횟수")]
    public int gimmick_num;
    protected int gimmick_count;  //기믹이 등장 할 때마다 숫자가 내려감
    public void Weakness_pattern()
    {

    }
    public virtual void Start()
    {
        gimmick_count = gimmick_num - 1;
    }
    #endregion

    // Start is called before the first frame update
}
