using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_spawner : MonoBehaviour
{
    int projectile_spawn_count = 0;
    int repeat;
    int repeat_count;
    float projectile_speed = 0;
    float spawn_time;
    float time;
    float push_time;
    bool init_end = false;
    Color projectile_color = Color.red;
    Vector3 projectile_pos;
    Vector3 projectile_scale;
    GameObject barrage_obj;
    GameObject projectile_obj;
    Projectile_moving_mode moving_mode = Projectile_moving_mode.NON;

    public void Init(int projectile_spawn_count, int repeat, float spawn_time, float projectile_speed, float projectile_push_time, Color projectile_color, 
        GameObject obj, Spawner_mode spawner_mode, Projectile_moving_mode moving_mode, Vector3 spanw_pos, Vector3 projectile_rot, Vector3 projectile_scale, GameObject parent)
    {
        this.projectile_spawn_count = projectile_spawn_count;
        this.repeat = repeat;
        this.spawn_time = spawn_time;
        this.projectile_speed = projectile_speed;
        this.projectile_color = projectile_color;
        this.projectile_obj = obj;
        this.moving_mode = moving_mode;
        this.projectile_pos = spanw_pos;
        this.projectile_scale = projectile_scale;
        barrage_obj = parent;
        push_time = projectile_push_time;
        init_end = true;

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
        if (repeat >= repeat_count)
        {
            if (spawn_time < time && init_end)
            {
                Projectile_spawn();
                repeat_count++;
            } 
        }
        else
        {
            Managers.Pool.Push(gameObject);
        }
    }
    private void OnEnable()
    {
        
    }
    void Projectile_spawn()
    {
        for (int i = 0; i < projectile_spawn_count; i++)
        {
            GameObject projectile = Managers.Pool.Pop(this.projectile_obj);
            projectile.GetComponent<SpriteRenderer>().color = projectile_color;
            projectile.transform.parent = Managers.Pool.Pop(barrage_obj).transform;
            projectile.transform.position = projectile_pos;
            projectile.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, barrage_obj.transform.rotation.z + (360 / projectile_spawn_count) * i));
            projectile.transform.localScale = projectile_scale;
            projectile.GetOrAddComponent<Base_projectile>().Init(push_time, projectile_speed, moving_mode);
        }
    }
}
