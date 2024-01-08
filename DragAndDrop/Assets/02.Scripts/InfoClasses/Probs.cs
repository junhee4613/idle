using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public abstract void Hit(float damage);
}

public enum Player_statu
{
    IDLE,
    DRAG,
    RUN,
    HIT,
    SKILLS
}
public enum Cha_in_gureumi_simple_patterns
{
    SINGLE_LIGHTNING,
    RANDOM_MULTIPLE_LIGHTNING,
    RAINDROPS,
    BROAD_BASED_LIGHTNING
}

