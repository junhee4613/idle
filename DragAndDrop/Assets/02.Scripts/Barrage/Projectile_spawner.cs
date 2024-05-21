using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_spawner : MonoBehaviour
{
    List<GameObject> projectiles = new List<GameObject>();
    int repeat_num = 0;
    float projectile_speed = 0;
    Color projectile_color = Color.red;
    GameObject projectile_obj;
    Spawner_mode spawner_mode = Spawner_mode.NON;
    Projectile_moving_mode moving_mode = Projectile_moving_mode.NON;

    public Projectile_spawner(int repeat_num, float projectile_speed, Color projectile_color, 
        GameObject obj, Spawner_mode spawner_mode, Projectile_moving_mode moving_mode)
    {
        this.repeat_num = repeat_num;
        this.projectile_speed = projectile_speed;
        this.projectile_color = projectile_color;
        this.projectile_obj = obj;
        this.spawner_mode = spawner_mode;
        this.moving_mode = moving_mode;
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
            GameObject projectile = this.projectile_obj;
            projectile.transform.position = transform.position;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (360 / repeat_num) * i));
            projectile.transform.localScale = Vector3.one;
            projectiles.Add(projectile);
            Managers.Pool.Pop(projectile);
        }
        Barrage_end();
    }
    void Barrage_end()
    {
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
