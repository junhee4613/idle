using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Newtonsoft.Json;

public class The_most_angry_gunman : BossController
{
    public Gun_shoot gun_shoot;
    protected override void Awake()
    {
        //base.Awake();
        gun_shoot.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_shoot_data").text);
    }
    // Start is called before the first frame update
    void Start()
    {
        Managers.GameManager.game_start = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Pattern_processing()
    {
        base.Pattern_processing();
        Pattern_function(ref gun_shoot.pattern_data, ref gun_shoot.pattern_ending, ref gun_shoot.duration,ref gun_shoot.pattern_count, Gun_shoot_pattern);
    }
    public void Gun_shoot_pattern()
    {
        Debug.Log(gun_shoot.pattern_count);
        switch (gun_shoot.pattern_data[gun_shoot.pattern_count].action_num)
        {
            case 0:     //에임 생성
                if(gun_shoot.aims[0] == null)
                {
                    gun_shoot.aims[0] = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Scope"));
                    gun_shoot.aims[0].transform.position = gun_shoot.pop_pos[0];
                    gun_shoot.aims[0].SetActive(false);
                }
                else
                {
                    gun_shoot.aims[1] = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Scope"));
                    gun_shoot.aims[1].transform.position = gun_shoot.pop_pos[1];
                    gun_shoot.aims[1].SetActive(false);
                }
                if (!gun_shoot.aims[0].activeSelf)
                {
                    gun_shoot.aims[0].SetActive(true);
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(gun_shoot.aims[0].transform.DOScale(Vector3.one * 1.5f, 0.2f));
                    sequence.Append(gun_shoot.aims[0].transform.DOScale(Vector3.one * 1f, 0.2f));
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
            case 1:     //에임들이 바깥쪽에서 움직임
                Debug.Log("시작");
                if (gun_shoot.aims[0].activeSelf)
                {
                    gun_shoot.aims[0].transform.position = new Vector3(Mathf.Clamp(transform.position.x + Time.deltaTime * Mathf.Sin(45 * Mathf.Deg2Rad) * gun_shoot.aims_data[0].criteria_dir_x, gun_shoot.pop_pos[0].x - gun_shoot.criteria_x, gun_shoot.init_pos[0].x + gun_shoot.criteria_x),
                    Mathf.Clamp(transform.position.y + Time.deltaTime * Mathf.Cos(315 * Mathf.Deg2Rad) * gun_shoot.aims_data[0].criteria_dir_y, gun_shoot.pop_pos[0].y - gun_shoot.criteria_y, gun_shoot.pop_pos[0].y + gun_shoot.criteria_y));
                    if (gun_shoot.aims[0].transform.position.x == gun_shoot.pop_pos[0].x + gun_shoot.criteria_x || gun_shoot.aims[0].transform.position.x == gun_shoot.pop_pos[0].x - gun_shoot.criteria_x)
                    {
                        gun_shoot.aims_data[0].criteria_dir_x = -gun_shoot.aims_data[0].criteria_dir_x;
                    }
                    if (transform.localPosition.y == gun_shoot.pop_pos[0].y + gun_shoot.criteria_y || transform.localPosition.y == gun_shoot.pop_pos[0].y - gun_shoot.criteria_y)
                    {
                        gun_shoot.aims_data[0].criteria_dir_y = -gun_shoot.aims_data[0].criteria_dir_y;
                    }
                }
                /*if (gun_shoot.aims[1] != null)
                {
                    gun_shoot.aims[1].transform.position = new Vector3(Mathf.Clamp(transform.position.x + Time.deltaTime * Mathf.Sin(45 * Mathf.Deg2Rad) * gun_shoot.aims_data[1].criteria_dir_x, -gun_shoot.criteria_x, gun_shoot.criteria_x),
                    Mathf.Clamp(transform.position.y + Time.deltaTime * Mathf.Cos(315 * Mathf.Deg2Rad) * gun_shoot.aims_data[1].criteria_dir_y, -gun_shoot.criteria_y, gun_shoot.criteria_y));
                    if (transform.localPosition.x == criteria_x || transform.localPosition.x == -criteria_x)
                    {
                        criteria_dir_x = -criteria_dir_x;
                    }
                    if (transform.localPosition.y == criteria_y || transform.localPosition.y == -criteria_y)
                    {
                        criteria_dir_y = -criteria_dir_y;
                    }
                }*/
                break;
            case 2:     //에임들이 플레이어 위치로 이동
                if (gun_shoot.aims[1] == null)
                {
                    gun_shoot.move_befor_pos[0] = gun_shoot.aims[0].transform.position;
                    transform.DOLocalMove(Managers.GameManager.Player.transform.position, 1);
                }
                else
                {
                    if (gun_shoot.right_aim_start)
                    {
                        gun_shoot.move_befor_pos[1] = gun_shoot.aims[1].transform.position;
                        transform.DOLocalMove(Managers.GameManager.Player.transform.position, 1);
                    }
                    else
                    {
                        gun_shoot.move_befor_pos[0] = gun_shoot.aims[0].transform.position;
                        transform.DOLocalMove(Managers.GameManager.Player.transform.position, 1);
                    }
                    
                }
                break;
            case 3:     //해당 위치에서 쏜 후 0.3초 뒤에 출발 지점으로 돌아감
                if (gun_shoot.aims[1] == null)
                {
                    gun_shoot.aims[0].transform.DOPunchScale(Vector3.one * 1.1f, 0.3f).OnComplete(() => gun_shoot.aims[0].transform.DOLocalMove(gun_shoot.move_befor_pos[0], 0.5f));
                }

                break;
            default:
                break;
        }
        
    }
    [Serializable]
    public class Gun_shoot : Pattern_base_data
    {
        public GameObject[] aims;       //에임 오브젝트들
        public Vector3[] init_pos;     //에임들이 플레이어를 따라가기 전 마지막 위치
        public Vector3[] pop_pos;     //에임들이 플레이어를 따라가기 전 마지막 위치
        public Vector3[] move_befor_pos;     //에임들이 플레이어를 따라가기 전 마지막 위치
        public float aim_speed = 5f;    //에임 스피드
        public Aim_data[] aims_data;    //에임들이 바깥 쪽에 위치할 때 이동하는 방향
        public float criteria_x;        //에임들의 움직이는 x축 범위 
        public float criteria_y;        //에임들의 움직이는 y축 범위
        public bool right_aim_start = false;    //에임들이 공격할 때 오른쪽부터 공격을 시작하는지
        [Serializable]
        public class Aim_data
        {
            public float criteria_dir_x = 1;
            public float criteria_dir_y = 1;
        }
    }

}
    
