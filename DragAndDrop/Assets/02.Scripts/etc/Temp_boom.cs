using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_boom : MonoBehaviour
{
    public Animator anim;
    public BoxCollider2D bc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Managers.Pool.Push(this.gameObject);
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
        {
            bc.enabled = false;
        }
    }
    private void OnEnable()
    {
        bc.enabled = true;
    }
}
