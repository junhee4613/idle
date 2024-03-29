using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Temp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;


public class ChaInGureumi : BossController          //비트는 80dlek
{
    public Cha_in_gureumi_simple_patterns simple_pattern;
    [SerializeField]
    Lightning_pattern lightning;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    public override void Start()
    {
        Managers.GameManager.boss = this.gameObject;
        Managers.GameManager.beat = 60f / 80f;
        Debug.Log(Managers.GameManager.beat);
        gimmick_count = gimmick_num - 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pattern_note = Physics2D.Raycast(note_pos.position, transform.up, 1, pattern_kind);
        time += Time.fixedDeltaTime;
        //if(사운드 매니저에서 불러온 노래 길이 / gimmick_count() * gimmick_num 패턴 등장 횟수이거 올림으로 해서 딱 맞아 떨어지게 하기 == 스테이지 진행도(노래의 최대 길이에서 점차 감소할 때))
        if (Managers.GameManager.beat <= time && pattern_note)
        {
            time -= Managers.GameManager.beat;
            if (hp <= hard_pattern_start_hp)
            {
                Simple_patterns();
            }
            else
            {
                Hard_patterns();
            }

        }
    }
    public void Current_pattern()
    {
        if (hp <= hard_pattern_start_hp)
        {
            Simple_patterns();
        }
        else
        {
            Hard_patterns();
        }
    }
    #region 패턴들
    #region 심플 패턴들
    void Simple_patterns()
    {
        switch (pattern_note.collider.gameObject.layer)
        {
            case 20:
                Raindrops();
                break;
            case 21:
                Broad_based_lightning();
                break;
            case 22:
                Single_lightning();
                break;
            default:
                break;
        }
        *//*pattern_num = Random.Range(1, Enum.GetNames(typeof(Cha_in_gureumi_simple_patterns)).Length);
        simple_pattern = (Cha_in_gureumi_simple_patterns)pattern_num;
        switch (simple_pattern)
        {
            case Cha_in_gureumi_simple_patterns.SINGLE_LIGHTNING:
                Single_lightning();
                break;
            case Cha_in_gureumi_simple_patterns.RAINDROPS:
                Raindrops();
                break;
            case Cha_in_gureumi_simple_patterns.BROAD_BASED_LIGHTNING:
                Broad_based_lightning();
                break;
            default:
                break;
        }*//*
    }
    void Single_lightning()
    {
        if (!lightning.pattern_setting)
        {
            lightning.pos_select_range = lightning.pos_min_x.Length - 1;
            lightning.pattern_setting = true;
            lightning.pattern_count = lightning.pattern_num;
            for (int i = lightning.pos_min_x.Length - 1; i >= 0; i--)
            {
                lightning.temp_x.Add(lightning.pos_min_x[i]);
            }

            for (int i = 0; i < lightning.pattern_num; i++)
            {
                int num = Random.Range(0, lightning.pos_select_range);
                lightning.pos_x.Push(lightning.temp_x[num]);
                lightning.temp_x.RemoveAt(num);
                lightning.pos_select_range--;
            }
        }

        if (0 < lightning.pattern_count)
        {
            lightning.pos = new Vector2(lightning.pos_x.Pop(), lightning.pos_y);
            GameObject obj = Instantiate(lightning.lightning);
            obj.transform.position = lightning.pos;
            lightning.pattern_count--;
            if (lightning.pattern_count <= 0)
            {
                lightning.temp_x.Clear();
                lightning.pos_x.Clear();
                lightning.pattern_setting = false;
            }
        }
    }
    *//*void Random_multiple_lightning()
    {

    }*//*
    void Raindrops()                //빗방울
    {

    }
    void Broad_based_lightning()    //차징 번개
    {

    }
    #endregion
    #region 하드패턴들
    void Hard_patterns()
    {

    }
    #endregion
    #endregion

    void Player_hit()
    {
        Managers.GameManager.Player.Hit(1);
    }

    public override void Hit(float damage)
    {
        throw new NotImplementedException();
    }

    [Serializable]
    public class Lightning_pattern
    {
        [Header("패턴이 등장하는 위치(높이)")]
        public float pos_y;
        [Header("패턴이 등장하는 x축 위치들")]
        public float[] pos_min_x;
        [Header("패턴이 등장하는 횟수")]
        public sbyte pattern_num;
        [HideInInspector]
        public Stack<float> pos_x = new Stack<float>();
        [HideInInspector]
        public List<float> temp_x = new List<float>();
        [HideInInspector]
        public sbyte pattern_count;
        [HideInInspector]
        public Vector2 pos;
        [HideInInspector]
        public bool pattern_setting = false;
        [Header("번개 오브젝트 할당")]
        public GameObject lightning;
        [HideInInspector]
        public int pos_select_range;
    }

}*/


/*[Header("이 보스에 약점 패턴을 성공해야되는 횟수")]
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
        if (hp <= 0)
        {
            if (gimmick_complete_num <= gimmick_complete_count)
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

    // Start is called before the first frame update*/

