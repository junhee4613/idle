using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_spawner : MonoBehaviour
{
    List<GameObject> projectiles = new List<GameObject>();
    int repeat_num = 0;
    float projectile_speed = 0;
    Color projectile_color = Color.red;
    Projectile_shape shape = Projectile_shape.NON;
    Spawner_mode spawner_mode = Spawner_mode.NON;
    Projectile_moving_mode moving_mode = Projectile_moving_mode.NON;

    public void Init(int repeat_num, float projectile_speed, Color projectile_color, 
        Projectile_shape shape, Spawner_mode spawner_mode, Projectile_moving_mode moving_mode)
    {

    }
    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Projectile_spawn();
    }
    void FixedUpdate()
    {
        Projectile_move();
    }
    void Projectile_spawn()
    {
        for (int i = 0; i < repeat_num; i++)
        {
            switch (shape)
            {
                case Projectile_shape.CIRCLE:
                    GameObject circel = Managers.Resource.Load<GameObject>("원");
                    circel.transform.position = transform.position;
                    circel.transform.rotation = Quaternion.Euler(new Vector3(0, 0 ,(360 / repeat_num) * i));
                    circel.transform.localScale = Vector3.one;
                    projectiles.Add(circel);
                    Managers.Pool.Pop(circel);
                    break;
                case Projectile_shape.BOX:
                    GameObject box = Managers.Resource.Load<GameObject>("박스");
                    box.transform.position = transform.position;
                    box.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (360 / repeat_num) * i));
                    box.transform.localScale = Vector3.one;
                    projectiles.Add(box);
                    Managers.Pool.Pop(box);
                    break;
                case Projectile_shape.TRIANGLE:
                    GameObject triangle = Managers.Resource.Load<GameObject>("삼각형");
                    triangle.transform.position = transform.position;
                    triangle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (360 / repeat_num) * i));
                    triangle.transform.localScale = Vector3.one;
                    projectiles.Add(triangle);
                    Managers.Pool.Pop(triangle);
                    break;
                case Projectile_shape.CAPSULE:
                    GameObject capsule = Managers.Resource.Load<GameObject>("캡슐");
                    capsule.transform.position = transform.position;
                    capsule.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (360 / repeat_num) * i));
                    capsule.transform.localScale = Vector3.one;
                    projectiles.Add(capsule);
                    Managers.Pool.Pop(capsule);
                    break;
                case Projectile_shape.NON:
                    Debug.Log("설정 안함");
                    break;
                default:
                    Debug.Log("제공하지 않는 모양");
                    break;
            }
        }
        switch (spawner_mode)
        {
            case Spawner_mode.REPEAT_END:
                Managers.Pool.Push(gameObject);
                break;
            case Spawner_mode.NON:
                Debug.Log("설정 안함");
                break;
            default:
                break;
        }


    }
    void Projectile_move()
    {
        if (projectiles.Count != 0)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                switch (moving_mode)
                {
                    case Projectile_moving_mode.GENERAL:
                        projectiles[i].transform.position = projectiles[i].transform.forward * projectile_speed;
                        break;
                    case Projectile_moving_mode.NON:
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    void OnDisable()
    {
        if(TryGetComponent<Projectile_spawner>(out Projectile_spawner component))
        {
            Destroy(component);
        }
    }
}
