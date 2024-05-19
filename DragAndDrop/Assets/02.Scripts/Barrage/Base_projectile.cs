using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_projectile : MonoBehaviour
{
    Vector3 init_pos 
    { 
        get 
        { 
            if(Managers.Barrage.Projectile_spawners.Count != 0)
            {
                return Managers.Barrage.Projectile_spawners[Managers.Barrage.Projectile_spawners.Count].transform.position;
            }
            else
            {
                return Vector3.zero;
            }
        } 
    }
    Vector3 init_scale = Vector3.one;
    Quaternion init_rotation;
    public float speed;
    bool init_complete;
    bool addtion_mode;

    
    // Start is called before the first frame update
    private void Awake()
    {
        Trans_init();
        if(gameObject.layer != 8)
        {
            gameObject.layer = 8;
        }
        gameObject.AddComponent<CircleCollider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected void FixedUpdate()
    {
        if (init_complete == true)
        {
            transform.position = transform.position + Vector3.zero * speed;
            Debug.Log(Mathf.Acos(0.5f));
            //Vector3.zero 대신 나중에 방향을 나타내서 하기
        }
        //여기에 이동하는 로직
    }
    private void OnEnable()
    {
        Trans_init();
    }
    public void Trans_init()
    {
        transform.position = init_pos;
        transform.localScale = init_scale;
        transform.rotation = init_rotation;
        if (!Managers.Barrage.addtional_option)
        {
            addtion_mode = false;
        }
        else
        {
            addtion_mode = true;
            if (addtion_mode == true)
            {

            }
        }
        
        init_complete = true;
    }
    private void OnDisable()
    {
        init_complete = false;
    }
}
