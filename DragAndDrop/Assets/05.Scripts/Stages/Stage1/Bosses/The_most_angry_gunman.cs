using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using static The_most_angry_gunman;
using static UnityEditor.Progress;

public class The_most_angry_gunman : BossController
{
    public Gun_shoot gun_shoot;
    public Dynamite dynamite;
    public Tumbleweed tumbleweed;
    protected override void Awake()
    {
        base.Awake();
        gun_shoot.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_shoot_data").text);
        dynamite.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_dynamite_data").text);
    }
    // Start is called before the first frame update
    void Start()
    {
        Anim_state_machin(anim_state["1_phase_idle"]);
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
        //활성화된 에임들이 바깥쪽에서 움직이는 코드
        if (gun_shoot.aim_idle_state[0] && gun_shoot.aims[0] != null && gun_shoot.aims[0].activeSelf)
        {
            Scope_side_move(ref gun_shoot.aims[0], ref gun_shoot.aims_data[0].criteria_dir_x, ref gun_shoot.aims_data[0].criteria_dir_y
                , gun_shoot.criteria_x, gun_shoot.criteria_y, gun_shoot.pop_pos[0].x, gun_shoot.pop_pos[0].y, gun_shoot.aim_speed);
        }
        if (gun_shoot.aim_idle_state[1] && gun_shoot.aims[1] != null && gun_shoot.aims[1].activeSelf)
        {
            Scope_side_move(ref gun_shoot.aims[1], ref gun_shoot.aims_data[1].criteria_dir_x, ref gun_shoot.aims_data[1].criteria_dir_y
                , gun_shoot.criteria_x, gun_shoot.criteria_y, gun_shoot.pop_pos[1].x, gun_shoot.pop_pos[1].y, gun_shoot.aim_speed);
        }
        Pattern_function(ref dynamite.pattern_data, ref dynamite.pattern_ending, ref dynamite.duration, ref dynamite.pattern_count, Dynamite_pattern);
        if (dynamite.dynamite_obj != null && dynamite.dynamite_rotate)
        {
            dynamite.dynamite_obj.transform.Rotate(new Vector3(0, 0, 720) * Time.deltaTime);
        }
    }
    public void Gun_shoot_pattern()
    {   
        
        //패턴별 행동들
        switch (gun_shoot.pattern_data[gun_shoot.pattern_count].action_num)
        {
            case 0:     //에임 생성
                if(gun_shoot.aims[0] == null && gun_shoot.aims[1] == null)
                {
                    Scope_create(ref gun_shoot.aims[0], gun_shoot.pop_pos[0]);
                    Scope_create(ref gun_shoot.aims[1], gun_shoot.pop_pos[1]);
                }
                Scope_appearance(gun_shoot.aims[0], (value) => gun_shoot.aim_idle_state[0] = value);
                break;
            case 1:     //에임들이 플레이어 위치로 이동
                if (gun_shoot.aim_idle_state[0] && gun_shoot.aims[0].activeSelf)
                {
                    Lock_on(ref gun_shoot, 0);
                    gun_shoot.aims_data[0].attack_action = true;
                }
                else if(gun_shoot.aim_idle_state[1] && gun_shoot.aims[1].activeSelf)
                {
                    Lock_on(ref gun_shoot, 1);
                    gun_shoot.aims_data[1].attack_action = true;
                }
                break;
            case 2:     //해당 위치에서 쏜 후 0.3초 뒤에 출발 지점으로 돌아감
                if (!gun_shoot.aim_idle_state[0] && gun_shoot.aims_data[0].attack_action)
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                    Shoot_after_init_pos(gun_shoot.aims[0], (value) => gun_shoot.aim_idle_state[0] = value, 0,ref gun_shoot.aims_data[0].attack_action);
                }
                else if (!gun_shoot.aim_idle_state[1] && gun_shoot.aims_data[1].attack_action)
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                    Shoot_after_init_pos(gun_shoot.aims[1], (value) => gun_shoot.aim_idle_state[1] = value, 1, ref gun_shoot.aims_data[1].attack_action);
                }
                break;
            case 3:     //공격하면서 또 하나의 에임 생성
                Scope_appearance(gun_shoot.aims[1], (value) => gun_shoot.aim_idle_state[1] = value);
                Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                Shoot_after_init_pos(gun_shoot.aims[0], (value) => gun_shoot.aim_idle_state[0] = value, 0, ref gun_shoot.aims_data[0].attack_action);
                break;
            case 4:     //4연발   0.15초 간격으로 총을 쏜다.
                gun_shoot.aim_idle_state[0] = false;
                gun_shoot.aims[0].transform.position = gun_shoot.aims[0].transform.position;
                Chasing_shoot(gun_shoot.aims[0], (value) => gun_shoot.aim_idle_state[0] = value, 0);
                break;
            case 5:     //장전
                break;
            case 6:     //에임들 사라짐
                gun_shoot.aim_idle_state[0] = false;
                gun_shoot.aim_idle_state[1] = false;
                gun_shoot.aims[0].transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
                {
                    gun_shoot.aims[0].SetActive(false);
                    gun_shoot.aims[1].transform.DOScale(Vector3.zero, 0.35f).OnComplete(() => gun_shoot.aims[1].SetActive(false));
                });
                break;
            case 7:
                gun_shoot.aim_idle_state[1] = false;
                gun_shoot.aims[1].transform.position = gun_shoot.aims[1].transform.position;
                Chasing_shoot(gun_shoot.aims[1], (value) => gun_shoot.aim_idle_state[1] = value, 1);
                break;
            default:
                break;
        }
    }
    public void Scope_side_move(ref GameObject aim, ref float dir_x, ref float dir_y, float range_x, float range_y, float pop_pos_x , float pop_pos_y, float speed)
    {
        aim.transform.position = new Vector3(Mathf.Clamp(aim.transform.position.x + Time.deltaTime * Mathf.Sin(45 * Mathf.Deg2Rad) * dir_x * speed, pop_pos_x - range_x, pop_pos_x + range_x),
                    Mathf.Clamp(aim.transform.position.y + Time.deltaTime * Mathf.Cos(315 * Mathf.Deg2Rad) * dir_y * speed, pop_pos_y - range_y, pop_pos_y + range_y));
        if (aim.transform.position.x == pop_pos_x + range_x || aim.transform.position.x == pop_pos_x - range_x)
        {
            dir_x = -dir_x;
        }
        if (aim.transform.position.y == pop_pos_y + range_y || aim.transform.position.y == pop_pos_y - range_y)
        {
            dir_y = -dir_y;
        }
    }
    public void Chasing_shoot(GameObject aim, Action<bool> scope_action_end, sbyte num)
    {
        aim.transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.2f).OnComplete(() =>
        {
            Managers.Main_camera.Punch(4.8f, 5, 0.1f);
            aim.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
            {
                Bullet_mark_ceate(aim.transform.position);
                aim.transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.2f).OnComplete(() =>
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.1f);
                    aim.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
                    {
                        Bullet_mark_ceate(aim.transform.position);
                        aim.transform.DOLocalMove(gun_shoot.move_befor_pos[num], 0.2f).OnComplete(() => scope_action_end(true));
                    });
                });
            });
        });
    }
    public void Scope_create(ref GameObject scope, Vector3 pop_pos)
    {
        scope = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Scope"));
        scope.transform.position = pop_pos;
        scope.SetActive(false);
    }
    public void Scope_appearance(GameObject scope, Action<bool> scope_action_end)
    {
        scope.SetActive(true);
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(scope.transform.DOScale(Vector3.one * 1.5f, 0.2f));
        sequence.Append(scope.transform.DOScale(Vector3.one * 1f, 0.2f).OnComplete(() => scope_action_end(true)));
    }
    public void Shoot_after_init_pos(GameObject aim, Action<bool> scope_action_end, sbyte num, ref bool attack)
    {
        attack = false;
        aim.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
        {
            Bullet_mark_ceate(aim.transform.position);
            aim.transform.DOLocalMove(gun_shoot.move_befor_pos[num], 0.2f).OnComplete(() => scope_action_end(true));
        });
        
    }
    public void Lock_on(ref Gun_shoot gun_Shoot, sbyte num)
    {
        gun_Shoot.move_befor_pos[num] = gun_shoot.aims[num].transform.position;
        gun_Shoot.aims[num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.3f);
        gun_Shoot.aim_idle_state[num] = false;
    }
    public void Bullet_mark_ceate(Vector3 create_pos)
    {
        Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Bullet_mark")).transform.position = create_pos;
    }
    public void Dynamite_pattern()
    {
        
        switch (dynamite.pattern_data[dynamite.pattern_count].action_num)
        {
            case 0:     //다이너마이트 생성 0.3초뒤에 생성
                foreach (var item in dynamite.dynamite_objs)
                {
                    if (!item.activeSelf)
                    {
                        item.SetActive(true);
                        if(dynamite.dynamite_obj == null)
                        {
                            dynamite.dynamite_obj = item;
                        }
                        item.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
                break;
            case 1:     //다이너마이트 들고 움직임 1초간 움직임
                break;
            case 2:     //다이너마이트 경고판 0.7
                dynamite.dynamite_landing_pos = new Vector3(Random.Range(transform.position.x, 3.5f * dynamite.dir), -3f, 0);
                dynamite.dynamite_obj.transform.DOJump(dynamite.dynamite_landing_pos, 5, 1, 0.5f).SetEase(Ease.InSine).OnComplete(() => dynamite.dynamite_rotate = false);
                dynamite.warning = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
                dynamite.warning.transform.position = new Vector3(dynamite.dynamite_landing_pos.x, -1.6f, 0);
                dynamite.warning.transform.localScale = new Vector3(2, 5, 0);
                dynamite.dynamite_rotate = true;
                break;
            case 3:     //다이너마이트 던짐
                Managers.Pool.Push(dynamite.warning);
                dynamite.dynamite_obj.SetActive(false);
                foreach (var item in dynamite.dynamite_objs)
                {
                    if (item.activeSelf)
                    {
                        dynamite.dynamite_obj = item;
                        break;
                    }
                }
                break;
            case 4:     //섬광탄 던져서 0.6초뒤에 착지하고 그 후 0.8초뒤에 터짐
            default:
                break;
        }
    }
    public void Tumbleweed_pattern()
    {
        switch (tumbleweed.pattern_data[gun_shoot.pattern_count].action_num)
        {
            case 0:     //경고판
                break;
            case 1:     //굴러감
                break;
            case 2:     //큰 장판
                break;
            case 3:     //섬광탄 던짐 0.8초뒤에 터짐
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
        public Aims_dir[] aims_data;    //에임들이 바깥 쪽에 위치할 때 이동하는 방향
        public bool[] aim_idle_state = new bool[2]; //에임이 플레이어를 쫒아 공격까지 했는지
        public float aim_speed = 5f;    //에임 스피드
        public float criteria_x;        //에임들의 움직이는 x축 범위 
        public float criteria_y;        //에임들의 움직이는 y축 범위
        [Serializable]
        public class Aims_dir           //에임들이 공통적으로 독립적으로 갖는 값들
        {
            public float criteria_dir_x = 1;
            public float criteria_dir_y = 1;
            public bool attack_action = false;
        }
    }
    [Serializable]
    public class Dynamite : Pattern_base_data
    {
        public Transform left_hand;
        public GameObject[] dynamite_objs;
        public GameObject dynamite_obj;
        public GameObject warning;
        public Vector3 dynamite_landing_pos;
        public int dir = 1;
        public bool dynamite_rotate;
    }
    [Serializable]
    public class Tumbleweed : Pattern_base_data
    {

    }

}

