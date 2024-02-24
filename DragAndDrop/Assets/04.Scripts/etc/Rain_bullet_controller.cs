using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_bullet_controller : MonoBehaviour
{
    public float speed;
    public float push_time;
    public float rotation;
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
        transform.Translate(new Vector3(0, -speed, 0) * Time.fixedDeltaTime);
        time += Time.fixedDeltaTime;
        if (time > push_time)
        {
            Managers.Pool.Push(this.gameObject);
            time = 0;
        }
    }
}
