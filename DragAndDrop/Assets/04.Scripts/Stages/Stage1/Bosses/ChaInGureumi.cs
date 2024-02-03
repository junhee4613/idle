using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;


public class ChaInGureumi : BossController          //비트는 80dlek
{
    public Cha_in_gureumi_simple_patterns simple_pattern;
    public Cha_in_gureumi_hard_patterns hard_pattern;
    Lightning_pattern lightning;
    public Rain_drop_pattern rain_drop;
    public override void Simple_pattern()
    {
        base.Simple_pattern();
        switch (Managers.GameManager.pattern_data[pattern_num].simple_pattern_type)
        {
            case (sbyte)Cha_in_gureumi_hard_patterns.IDLE:
                Idle();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.RAINDROPS:
                Rain_drop();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.RAIN_STORM:
                Rain_storm();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.SHOWER:
                Shower();
                break;
            default:
                break;
        }
    }
    public override void Hard_pattern()
    {
        base.Hard_pattern();
        switch (Managers.GameManager.pattern_data[pattern_num].hard_pattern_type)
        {
            case (sbyte)Cha_in_gureumi_hard_patterns.IDLE:
                Idle();
                break;
            case (sbyte)Cha_in_gureumi_hard_patterns.BROAD_LIGHTNING:
                Broad_lightning();
                break;
            case (sbyte)Cha_in_gureumi_hard_patterns.LIGHTNING_BALL:
                Lightning_ball();
                break;
            case (sbyte)Cha_in_gureumi_hard_patterns.SINGLE_LIGHTNING:
                Single_lightning();
                break;
            default:
                break;
        }
    }
    public void Idle()
    {
        Anim_state_machin(anim_state["simple_pattern0"]);
    }
    public void Rain_drop()
    {
        Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_drop")).transform.position = new Vector2(rain_drop.center_pos.position.x + Random.Range(-rain_drop.pos_x, rain_drop.pos_x), rain_drop.pos_y.position.y);
        Anim_state_machin(anim_state["simple_pattern1"]);
    }
    public void Rain_storm()
    {

    }
    public void Shower()
    {

    }
    public void Broad_lightning()
    {

    }
    public void Lightning_ball()
    {

    }
    public void Single_lightning()
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
    [Serializable]
    public class Rain_drop_pattern
    {
        [Header("생성되는 높이")]
        public Transform pos_y;
        [Header("생성되는 x축 양의 범위")]
        public float pos_x;
        public Transform center_pos;
    }
}
