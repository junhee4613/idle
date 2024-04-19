using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powder_anim_temp : MonoBehaviour
{
    public Animator an;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (an.GetCurrentAnimatorStateInfo(0).IsName("Powder_keg_boom"))
        {
            if(an.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Managers.Pool.Push(this.gameObject);
            }
        }
    }
    private void OnEnable()
    {
        an.Play("Powder_keg_idle");
    }
}
