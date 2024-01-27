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
        SINGLE_LIGHTNING,
        /*RANDOM_MULTIPLE_LIGHTNING,*/
        RAINDROPS,
        BROAD_BASED_LIGHTNING
    }
    public enum Cha_in_gureumi_hard_patterns
    {
        LIGHTNING_SPHERE,
        REINFORCED_RAINDROPS,
        SHOWER
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



