using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class The_most_angry_gunman : BossController
{
    public Gun_shoot gun_shoot;
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Pattern_processing()
    {
        base.Pattern_processing();
        Pattern_function(gun_shoot.pattern_data, gun_shoot.pattern_ending, gun_shoot.duration, gun_shoot.pattern_count, Gun_shoot_pattern);
    }
    public void Gun_shoot_pattern()
    {
        switch (gun_shoot.pattern_data[gun_shoot.pattern_count].action_num)
        {
            case 0:
                if (!gun_shoot.aims[0].activeSelf)
                {
                    gun_shoot.aims[0].SetActive(true);
                }
                else if(!gun_shoot.aims[1].activeSelf)
                {
                    gun_shoot.aims[1].SetActive(true);
                }
                else
                {
                    foreach (var item in gun_shoot.aims)
                    {
                        item.SetActive(false);
                    }
                }
                break;
            case 1:
                gun_shoot.aim_trajectory += Time.fixedDeltaTime;
                if (gun_shoot.aims[0].activeSelf)
                {
                    gun_shoot.aims[0].transform.Translate(Managers.GameManager.player_obj.transform.position * gun_shoot.aim_speed);
                    gun_shoot.aims[0].transform.position = new Vector3(gun_shoot.aims[0].transform.position.x, gun_shoot.aims[0].transform.position.y, gun_shoot.aims[0].transform.position.z);
                }
                if (gun_shoot.aims[1].activeSelf)
                {
                    gun_shoot.aims[1].transform.Translate(Managers.GameManager.player_obj.transform.position * gun_shoot.aim_speed);
                    gun_shoot.aims[1].transform.position = new Vector3(gun_shoot.aims[1].transform.position.x, gun_shoot.aims[1].transform.position.y, gun_shoot.aims[1].transform.position.z);
                }
                break;
            case 2:
                break;
            default:
                break;
        }
        
    }
    [Serializable]
    public class Gun_shoot : Pattern_base_data
    {
        public GameObject[] aims;
        public float aim_speed = 5f;
        public float aim_trajectory = 0f;
    }
}
