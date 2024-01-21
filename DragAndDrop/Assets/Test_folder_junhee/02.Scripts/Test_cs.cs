using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_cs : slow_eligibility
{
    /*public collider2d[] test;
    public boxcollider2d bc;*/
    public int dir = 1;
    public Button apply_button;
    public Button init_button;
    public float max_speed;
    private void Awake()
    {
        apply_button.onClick.AddListener(() =>
        {
            dir = dir * -1;
        });
        init_button.onClick.AddListener(() =>
        {
            gameObject.layer = 10;
            speed = max_speed;
        });
    }
    private void Start()
    {

    }
    private void Update()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x - Time.deltaTime * dir * speed, -5, 5),0);
    }
    public void Dir_trans()
    {
        dir = dir * -1;
    }
    public override void Slow_apply()
    {
        base.Slow_apply();
    }
}
