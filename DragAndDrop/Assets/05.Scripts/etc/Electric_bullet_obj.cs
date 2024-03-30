using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_bullet_obj : MonoBehaviour
{
    public float bullet_speed;
    public float bullet_push_time;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.Translate(0, bullet_speed * Time.fixedDeltaTime, 0);
        if(bullet_push_time < time)
        {
            Managers.Pool.Push(this.gameObject);
            time = 0;
        }
        time += Time.fixedDeltaTime;

    }
}
