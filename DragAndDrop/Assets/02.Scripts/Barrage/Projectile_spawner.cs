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
    float rot_speed;
    bool init_end = false;
    Color projectile_color = Color.red;
    Vector3 projectile_pos;
    Vector3 projectile_scale;
    Transform projectile_parent;
    GameObject projectile_obj;
    Projectile_moving_mode moving_mode = Projectile_moving_mode.NON;

    public void Init(int projectile_spawn_count, int repeat, float spawn_time, float projectile_speed, float projectile_push_time, Color projectile_color, 
        GameObject obj, Spawner_mode spawner_mode, Projectile_moving_mode moving_mode, Vector3 spanw_pos, Vector3 projectile_rot, Vector3 projectile_scale, Transform parent)
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
        projectile_parent = parent;
        push_time = projectile_push_time;
        init_end = true;

    }
    public void Init(int projectile_spawn_count, int repeat, float spawn_time, float projectile_speed, float projectile_push_time, Color projectile_color,
        GameObject obj, Spawner_mode spawner_mode, Projectile_moving_mode moving_mode, Vector3 spanw_pos, Vector3 projectile_rot, Vector3 projectile_scale, Transform parent, float rot_speed)
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
        projectile_parent = parent;
        push_time = projectile_push_time;
        this.rot_speed = rot_speed;
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
                time -= spawn_time;
            }
            else
            {
                time += Time.deltaTime;
            }
        }
        else
        {
            init_end = false;
            repeat_count = 0;
            Managers.Pool.Push(gameObject);
        }
    }
    private void OnEnable()
    {
        
    }
    void Projectile_spawn()
    {
        projectile_parent = Managers.Pool.Pop(projectile_parent.gameObject).transform;

        switch (moving_mode)
        {
            case Projectile_moving_mode.GENERAL:
                for (int i = 0; i < projectile_spawn_count; i++)
                {
                    GameObject projectile = Managers.Pool.Pop(this.projectile_obj);
                    projectile.GetComponent<SpriteRenderer>().color = projectile_color;
                    projectile.transform.parent = projectile_parent;
                    projectile.transform.position = projectile_pos;
                    projectile.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, projectile_parent.rotation.z + (360 / projectile_spawn_count) * i));
                    projectile.transform.localScale = projectile_scale;
                    projectile.GetOrAddComponent<Base_projectile>().Init(push_time, projectile_speed, moving_mode);
                }
                break;
            case Projectile_moving_mode.GUIDED_X:
                for (int i = 0; i < projectile_spawn_count; i++)
                {
                    GameObject projectile = Managers.Pool.Pop(this.projectile_obj);
                    projectile.GetComponent<SpriteRenderer>().color = projectile_color;
                    projectile.transform.parent = projectile_parent;
                    projectile.transform.position = projectile_pos;
                    projectile.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, projectile_parent.rotation.z + (360 / projectile_spawn_count) * i));
                    projectile.transform.localScale = projectile_scale;
                    projectile.GetOrAddComponent<Base_projectile>().Init(push_time, projectile_speed, rot_speed, moving_mode);
                }
                break;
            case Projectile_moving_mode.GUIDED_Y:
                for (int i = 0; i < projectile_spawn_count; i++)
                {
                    GameObject projectile = Managers.Pool.Pop(this.projectile_obj);
                    projectile.GetComponent<SpriteRenderer>().color = projectile_color;
                    projectile.transform.parent = projectile_parent;
                    projectile.transform.position = projectile_pos;
                    projectile.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, projectile_parent.rotation.z + (360 / projectile_spawn_count) * i));
                    projectile.transform.localScale = projectile_scale;
                    projectile.GetOrAddComponent<Base_projectile>().Init(push_time, projectile_speed, rot_speed, moving_mode);
                }
                break;
            case Projectile_moving_mode.NON:
                break;
            default:
                break;
        }
        
    }
}
