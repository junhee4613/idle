using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_base_controller : MonoBehaviour
{
    protected Dictionary<string, Anim_stage_state> anim_state = new Dictionary<string, Anim_stage_state>();
    public Anim_stage_state now_anim;
    public Animator an;
    public sbyte simple_pattern_num;
    public sbyte hard_pattern_num;
    protected virtual void Awake()
    {
        anim_state.Anim_processing(ref an, gameObject, simple_pattern_num, hard_pattern_num);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Anim_state_machin(Anim_stage_state clip_name)
    {
        if (an.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && now_anim != clip_name)
        {
            clip_name.On_state_exit();
            now_anim = clip_name;
            clip_name.On_state_enter();
        }

    }
}
