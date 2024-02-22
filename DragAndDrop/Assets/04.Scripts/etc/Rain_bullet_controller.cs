using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_bullet_controller : MonoBehaviour
{
    public float rotation;
    public float speed;
    public float push_time;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 0, rotation));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.up * speed * Time.fixedDeltaTime);
        time += Time.fixedDeltaTime;
        if (time > push_time)
        {
            Managers.Pool.Push(this.gameObject);
            time = 0;
        }
    }
    private void OnEnable()
    {
        transform.Rotate(new Vector3(0, 0, rotation));
    }
}
