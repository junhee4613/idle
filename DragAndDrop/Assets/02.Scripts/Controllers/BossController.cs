using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    #region 나중에 private로 바꿀 것들
    public int pattern_num;
    public bool Pattern_in_progress = false;
    public int hp = 100;
    public int hard_pattern_start_hp;
    public GameObject[] simple_patterns;        //이건 나중에 어드레서블로 저장해서 패턴을 불러오는 식으로 하자
    public GameObject[] hard_patterns;          //이건 나중에 어드레서블로 저장해서 패턴을 불러오는 식으로 하자
    #endregion

    // Start is called before the first frame update
}
