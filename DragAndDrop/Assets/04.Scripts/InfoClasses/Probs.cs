using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Unit : MonoBehaviour
{
    public abstract void Hit(float damage);
}


public class playerData : Unit
{
    [Header("날아가는 최대 속도")]
    public float shoot_speed;
    [Header("점차 감소하는 속도의 세기 1로 설정 시 초당 속력 1씩 줄어듬")]
    public float gradually_down_speed;
    [Header("드래그 상태의 속도 및 날아가는 최소 속도")]
    public float slow_speed;
    [Header("피격 당한 후 무적 시간")]
    public float invincibility_time;
    [Header("처음 목숨 갯수")]
    public byte player_life = 3;
    


    public override void Hit(float damage)
    {

    }
}
public enum Player_statu
{
    IDLE,
    DRAG,
    RUN,
    HIT,
}
public class slow_eligibility : MonoBehaviour
{
    public float slow_speed;
    public float speed;
    public virtual void Slow_apply()
    {
        speed = slow_speed;
    }
}

namespace Cha_in_gureumi
{
    public enum Cha_in_gureumi_simple_patterns
    {
        IDLE,
        RAINDROPS,
        RAIN_STORM,
        SHOWER,
        RUSH
    }
    public enum Cha_in_gureumi_hard_patterns
    {
        IDLE,
        BROAD_LIGHTNING,
        SINGLE_LIGHTNING,
        LIGHTNING_BALL
    }
}
namespace Test_boss
{
    public enum Test_patterns_enum
    {
        PATTERN1,
        PATTERN2,
        PATTERN3,
    }

}
interface IInteraction_obj
{
    void practice();
}
[System.Serializable]
public class Pattern_state
{
    [SerializeField]public float time;
    [SerializeField]public sbyte simple_pattern_type;
    [SerializeField]public sbyte hard_pattern_type;
    [SerializeField]public sbyte action_num;
}
public class Stage_setting
{
    public float beat;
    public float bgm_length;
}
public abstract class Anim_stage_state
{
    public Animator an;
    public string temp_name;
    public abstract void On_state_enter();
    public abstract void On_state_update(sbyte loop_num);
    public abstract void On_state_exit();
}
namespace Stage_FSM
{
    public class Simple_pattern : Anim_stage_state
    {
        public Simple_pattern(string anim_name, Animator temp_an)
        {
            this.an = temp_an;
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }
    /*public class Simple_pattern1 : Anim_stage_state
    {
        public Simple_pattern1(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }

    }
    public class Simple_pattern2 : Anim_stage_state
    {
        public Simple_pattern2(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }*/
    public class Hard_pattern : Anim_stage_state
    {
        public Hard_pattern(string anim_name, Animator temp_an)
        {
            an = temp_an;
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }
    /*public class Hard_pattern1 : Anim_stage_state
    {
        public Hard_pattern1(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }
    public class Hard_pattern3 : Anim_stage_state
    {
        public Hard_pattern3(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }*/
}



