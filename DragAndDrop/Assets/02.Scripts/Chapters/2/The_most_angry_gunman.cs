using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;
using static The_most_angry_gunman;

public class The_most_angry_gunman : BossController
{
    public Gun_shoot gun_shoot;
    public Dynamite dynamite;
    public Tumbleweed tumbleweed;
    public Powder_keg powder_keg;
    public Animator[] weapon_anim_controller;
    public GameObject boss_character;
    public GameObject[] hands = new GameObject[2];
    Dictionary<string, Anim_stage_state> right_hand_state = new Dictionary<string, Anim_stage_state>();
    Dictionary<string, Anim_stage_state> left_hand_state = new Dictionary<string, Anim_stage_state>();
    string[] anims = new string[] {"idle", "reload", "right_move","right_shot","left_move", "left_shot", "stage2_dead", "dynamite_throw", "change_weapon", "right_dynamite_throw", "left_dynamite_throw" };
    string[] weapon_anims = new string[] { "gun_idle", "reload", "shot", "look_on", "gun_shot_init", "dynamite_idle", "dynamite_boom", "dynamite_instance", "gun_base", "change_weapon" };
    protected override void Awake()
    {
        weapon_anim_controller[0] = hands[0].GetComponent<Animator>();
        weapon_anim_controller[1] = hands[1].GetComponent<Animator>();
        anim_state.Anim_processing2(ref an, anims);
        right_hand_state.Anim_processing2(ref weapon_anim_controller[0], weapon_anims);
        left_hand_state.Anim_processing2(ref weapon_anim_controller[1], weapon_anims);
        gun_shoot.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_shot_data").text);
        dynamite.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_dynamite_data").text);
        powder_keg.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_powder_keg_data").text);
        tumbleweed.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_tumbleweed_data").text);
        Managers.GameManager.game_start = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Anim_state_machin2(right_hand_state["gun_idle"], true);
        Anim_state_machin2(left_hand_state["gun_idle"], true);
        Anim_state_machin2(anim_state["idle"], true);
    }

    // Update is called once per frame
    
    public override void Pattern_processing()
    {
        Pattern_function(ref gun_shoot.pattern_data, ref gun_shoot.pattern_ending, ref gun_shoot.duration,ref gun_shoot.pattern_count, Gun_shoot_pattern);
        Aim_moving();
        Pattern_function(ref dynamite.pattern_data, ref dynamite.pattern_ending, ref dynamite.duration, ref dynamite.pattern_count, Dynamite_pattern);
        Pattern_function(ref tumbleweed.pattern_data, ref tumbleweed.pattern_ending, ref tumbleweed.duration, ref tumbleweed.pattern_count, Tumbleweed_pattern);
        Pattern_function(ref powder_keg.pattern_data, ref powder_keg.pattern_ending, ref powder_keg.duration, ref powder_keg.pattern_count, Powder_keg_pattern);

    }
    public void Aim_moving()    //활성화된 에임들이 바깥쪽에서 움직이는 코드
    {
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
                if (gun_shoot.aim_idle_state[0] && gun_shoot.aims[0].activeSelf && !gun_shoot.right_shoot)
                {
                    Lock_on(ref gun_shoot, 0);
                    gun_shoot.aims_data[0].attack_action = true;
                    if (gun_shoot.aims[1].activeSelf)
                    {
                        gun_shoot.right_shoot = true;
                    }
                }
                else if(gun_shoot.aim_idle_state[1] && gun_shoot.aims[1].activeSelf)
                {
                    Lock_on(ref gun_shoot, 1);
                    gun_shoot.aims_data[1].attack_action = true;
                    gun_shoot.right_shoot = false;
                }
                break;
            case 2:     //해당 위치에서 쏜 후 0.3초 뒤에 출발 지점으로 돌아감
                if (!gun_shoot.aim_idle_state[0] && gun_shoot.aims_data[0].attack_action)
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                    Shoot_after_init_pos((value) => gun_shoot.aim_idle_state[0] = value, 0,ref gun_shoot.aims_data[0].attack_action);
                }
                else if (!gun_shoot.aim_idle_state[1] && gun_shoot.aims_data[1].attack_action)
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                    Shoot_after_init_pos((value) => gun_shoot.aim_idle_state[1] = value, 1, ref gun_shoot.aims_data[1].attack_action);
                }
                break;
            case 3:     //공격하면서 또 하나의 에임 생성
                Scope_appearance(gun_shoot.aims[1], (value) => gun_shoot.aim_idle_state[1] = value);
                Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                Shoot_after_init_pos((value) => gun_shoot.aim_idle_state[0] = value, 0, ref gun_shoot.aims_data[0].attack_action);
                break;
            case 4:     //4연발   0.15초 간격으로 총을 쏜다.
                gun_shoot.aim_idle_state[0] = false;
                gun_shoot.aims[0].transform.position = gun_shoot.aims[0].transform.position;
                Chasing_shoot(0, (value) => gun_shoot.aim_idle_state[0] = value);
                break;
            case 5:     //장전
                if (!hands[0].activeSelf)           //FIX : 여기 애니메이션 받으면 없애야됨
                {
                    foreach (var item in hands)
                    {
                        item.SetActive(true);
                    }
                }
                Anim_state_machin2(right_hand_state["reload"], false);
                Anim_state_machin2(left_hand_state["reload"], false);
                Anim_state_machin2(anim_state["reload"], true);
                break;
            case 6:     //에임들 사라짐
                gun_shoot.aim_idle_state[0] = false;
                gun_shoot.aim_idle_state[1] = false;
                gun_shoot.aims[0].transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
                {
                    gun_shoot.aims[0].SetActive(false);

                    Anim_state_machin2(right_hand_state["change_weapon"], false);
                    Anim_state_machin2(left_hand_state["change_weapon"], false);
                    Anim_state_machin2(anim_state["change_weapon"], false);
                    gun_shoot.aims[1].transform.DOScale(Vector3.zero, 0.35f).OnComplete(() => gun_shoot.aims[1].SetActive(false));
                });
                break;
            case 7:
                gun_shoot.aim_idle_state[1] = false;
                gun_shoot.aims[1].transform.position = gun_shoot.aims[1].transform.position;
                Chasing_shoot(1, (value) => gun_shoot.aim_idle_state[1] = value);
                break;
            default:
                break;
        }
    }
    public void Scope_side_move(ref GameObject aim, ref float dir_x, ref float dir_y, float range_x, float range_y, float pop_pos_x , float pop_pos_y, float speed)
    {                   //에임들이 외각에서 움직이는 코드
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
    public void Chasing_shoot(sbyte gun_num, Action<bool> scope_action_end)     //연발
    {
        gun_shoot.aims[gun_num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.2f).OnComplete(() =>
        {
            Managers.Main_camera.Punch(4.8f, 5, 0.1f);
            if (!hands[0].activeSelf)
            {
                foreach (var item in hands)
                {
                    item.SetActive(true);
                }
            }
            switch (gun_num)
            {
                case 0:
                    Anim_state_machin2(anim_state["right_shot"], false, true);
                    Anim_state_machin2(right_hand_state["shot"], false, true);
                    break;
                case 1:
                    Anim_state_machin2(anim_state["left_shot"], false, true);
                    Anim_state_machin2(left_hand_state["shot"], false, true);
                    break;
            }
            gun_shoot.aims[gun_num].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
            {
                Bullet_mark_ceate(gun_shoot.aims[gun_num].transform.position);
                gun_shoot.aims[gun_num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.2f).OnComplete(() =>
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.1f);
                    switch (gun_num)
                    {
                        case 0:
                            Anim_state_machin2(anim_state["right_shot"], false, true);
                            Anim_state_machin2(right_hand_state["shot"], false, true);
                            break;
                        case 1:
                            Anim_state_machin2(anim_state["left_shot"], false, true);
                            Anim_state_machin2(left_hand_state["shot"], false, true);
                            break;
                    }
                    gun_shoot.aims[gun_num].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
                    {
                        Bullet_mark_ceate(gun_shoot.aims[gun_num].transform.position);
                        gun_shoot.aims[gun_num].transform.DOLocalMove(gun_shoot.move_befor_pos[gun_num], 0.2f).OnComplete(() => 
                        { 
                            scope_action_end(true);
                        });
                    });
                });
            });
        });
    }
    public void Scope_create(ref GameObject scope, Vector3 pop_pos)         //스코프 생성
    {
        scope = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Scope"));
        scope.transform.position = pop_pos;
        scope.SetActive(false);
    }
    public void Scope_appearance(GameObject scope, Action<bool> scope_action_end)   //스코프 등장
    {
        scope.SetActive(true);
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(scope.transform.DOScale(Vector3.one * 1.5f, 0.2f));
        sequence.Append(scope.transform.DOScale(Vector3.one * 1f, 0.2f).OnComplete(() => 
        { 
            scope_action_end(true);
        }));
    }
    public void Shoot_after_init_pos(Action<bool> scope_action_end, sbyte num, ref bool attack)
    {               //스코프 발사 후 원래 위치로 이동
        if (!hands[0].activeSelf)
        {
            foreach (var item in hands)
            {
                item.SetActive(true);
            }
        }
        attack = false;
        switch (num)
        {
            case 0:
                Anim_state_machin2(anim_state["right_shot"], false);
                Anim_state_machin2(right_hand_state["shot"], false);
                break;
            case 1:
                Anim_state_machin2(anim_state["left_shot"], false);
                Anim_state_machin2(left_hand_state["shot"], false);
                break;
        }
        gun_shoot.aims[num].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
        {
            Bullet_mark_ceate(gun_shoot.aims[num].transform.position);
            switch (num)
            {
                case 0:
                    Anim_state_machin2(right_hand_state["gun_shot_init"], false);
                    break;
                case 1:
                    Anim_state_machin2(left_hand_state["gun_shot_init"], false);
                    break;
            }
            gun_shoot.aims[num].transform.DOLocalMove(gun_shoot.move_befor_pos[num], 0.2f).OnComplete(() => 
            { 
                scope_action_end(true);
            });
        });
    }
    public void Lock_on(ref Gun_shoot gun_Shoot, sbyte num)         //스코프 플레이어 위치로 이동
    {
        if (!hands[0].activeSelf)
        {
            foreach (var item in hands)
            {
                item.SetActive(true);
            }
        }
        switch (num)
        {
            case 0:
                Anim_state_machin2(anim_state["right_move"], false);
                Anim_state_machin2(right_hand_state["look_on"], false);
                break;
            case 1:
                Anim_state_machin2(anim_state["left_move"], false);
                Anim_state_machin2(left_hand_state["look_on"], false);
                break;
        }
        gun_Shoot.move_befor_pos[num] = gun_shoot.aims[num].transform.position;
        gun_Shoot.aims[num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.3f);
        gun_Shoot.aim_idle_state[num] = false;
    }
    public void Bullet_mark_ceate(Vector3 create_pos)
    {
        GameObject tmep = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Bullet_mark"));
        tmep.transform.position = create_pos;
    }
    public void Dynamite_pattern()
    {
        switch (dynamite.pattern_data[dynamite.pattern_count].action_num)
        {
            case 1:     //다이너마이트 들고 움직임 1초간 움직임
                dynamite.dir = -dynamite.dir;
                transform.transform.DOMoveX(Random.Range(0, 3) * dynamite.dir, 0.7f);
                break;
            case 2:     //다이너 마이트 생성 애니메이션
                dynamite.throw_dynamite = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Dynamite"));
                dynamite.dynamite_landing_pos_x = Random.Range(transform.position.x, 3.5f * dynamite.dir);
                DG.Tweening.Sequence sequence = DOTween.Sequence();
                switch (dynamite.dir)
                {                   //FIX : 수정
                    case 1:
                        //다이너마이트 다시 생기는 애니메이션 작동
                        dynamite.throw_dynamite.transform.localScale = new Vector3(1, 1, 1);
                        dynamite.throw_dynamite.transform.position = hands[1].transform.position;
                        Anim_state_machin2(left_hand_state["dynamite_instance"], false);
                        Anim_state_machin2(anim_state["left_dynamite_throw"], false);
                        sequence.Join(dynamite.throw_dynamite.transform.DOLocalJump(new Vector3(dynamite.dynamite_landing_pos_x, -3, 0), 5, 1, 0.5f).SetEase(Ease.InSine));
                        sequence.Join(dynamite.throw_dynamite.transform.DORotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(2));
                        break;
                    case -1:
                        dynamite.throw_dynamite.transform.position = hands[0].transform.position;
                        dynamite.throw_dynamite.transform.localScale = new Vector3(-1, 1, 1);
                        Anim_state_machin2(right_hand_state["dynamite_instance"], false);
                        Anim_state_machin2(anim_state["right_dynamite_throw"], false);
                        sequence.Join(dynamite.throw_dynamite.transform.DOLocalJump(new Vector3(dynamite.dynamite_landing_pos_x, -3, 0), 5, 1, 0.5f).SetEase(Ease.InSine));
                        sequence.Join(dynamite.throw_dynamite.transform.DORotate(new Vector3(0, 0, -360), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(2));
                        break;
                }
                //경고판 생성하는 로직 추가
                break;
            case 3:     //다이너마이트 터짐
                dynamite.throw_dynamite.transform.rotation = Quaternion.identity;
                if (!anim_ongoing_obj.ContainsKey(dynamite.throw_dynamite))
                {
                    anim_ongoing_obj.Add(dynamite.throw_dynamite, dynamite.throw_dynamite.GetComponent<Animator>());
                }                   //애니메이션이 다음 프레임에 실행됨
                anim_ongoing_obj[dynamite.throw_dynamite].Play("dynamite_boom");
                /*AnimatorClipInfo[] clipInfos = anim_ongoing_obj[dynamite.throw_dynamite].GetCurrentAnimatorClipInfo(0);
                Debug.Log(clipInfos[0].clip.name);*/        // 이 코드 이용해서 하기
                Managers.Main_camera.Shake_move();
                Managers.Pool.Push(dynamite.throw_dynamite);
                break;
            case 4:     //섬광탄 던져서 0.7 터짐 위치 :(0, 0, 0) 스케일 : (1.5, 1.5, 1.5)
                GameObject flash_bang = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Flashbang"));
                flash_bang.transform.DOMoveY(0f, 0.7f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    Managers.UI_jun.Fade_out_in("White", 0f, 0.1f, 0.4f, 0.2f, Beat_box_size_up);
                    Managers.Pool.Push(flash_bang);
                });
                break;
            default:
                break;
        }
        if(anim_ongoing_obj.ContainsKey(dynamite.throw_dynamite))
        {
            Anim_end_push("dynamite_boom", anim_ongoing_obj[dynamite.throw_dynamite].GetCurrentAnimatorClipInfo(0));
        }
    }
    public void Beat_box_size_up()
    {
        boss_character.SetActive(false);
        Managers.GameManager.Beat_box.transform.position = Vector3.zero;
        Managers.GameManager.Beat_box.transform.localScale = Vector3.one * 1.5f;
    }
    public void Tumbleweed_pattern()
    {
        //방향 정해주는 로직
        tumbleweed.dir = Random.Range(0, 2) == 1 ? 1 : -1;
        switch (tumbleweed.pattern_data[tumbleweed.pattern_count].action_num)
        {
            case 0:     //굴러감
                foreach (var item in tumbleweed.horizontal_tumbleweed_instance)
                {
                    if (tumbleweed.small_turn)
                    {
                        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Small_tumbleweed"));
                        temp.transform.localScale = new Vector3(temp.transform.localScale.x * tumbleweed.dir, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.position = new Vector3(10 * tumbleweed.dir, item, 0);
                        temp.transform.DOMoveX(-10 * tumbleweed.dir, 1.8f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            Managers.Pool.Push(temp);
                            Managers.Pool.Push(tumbleweed.warning[0]);
                            tumbleweed.warning.RemoveAt(0);
                        }); ;
                        //여기에 경고장판 사라지는 로직
                        tumbleweed.small_tumbleweed.Add(item);
                    }
                    else
                    {
                        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Big_tumbleweed"));
                        temp.transform.localScale = new Vector3(temp.transform.localScale.x * tumbleweed.dir, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.position = new Vector3(10 * tumbleweed.dir, item, 0);
                        temp.transform.DOMoveX(-10 * tumbleweed.dir, 1.8f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            Managers.Pool.Push(temp);
                            Managers.Pool.Push(tumbleweed.warning[0]);
                            tumbleweed.warning.RemoveAt(0);
                        });
                        //여기에 경고장판 사라지는 로직
                        tumbleweed.big_tumbleweed.Add(item);
                    }
                }
                tumbleweed.horizontal_tumbleweed_instance.Clear();
                break;
            case 1:     //큰 경고판 1.5
                tumbleweed.small_turn = false;
                int big_num = Random.Range(0, tumbleweed.big_tumbleweed.Count - 1);
                tumbleweed.warning.Add(Warning_box_punch_scale(new Vector3(0, tumbleweed.big_tumbleweed[big_num], 0), new Vector3(15, 1.25f, 0), new Vector3(15, 1.4f, 0), 0.1f, new Vector3(15, 1.5f, 0), 0.05f, false, true));
                tumbleweed.horizontal_tumbleweed_instance.Add(tumbleweed.big_tumbleweed[big_num]);
                tumbleweed.big_tumbleweed.RemoveAt(big_num);
                break;
            case 2:    //(작은 장판)) 1.25
                tumbleweed.small_turn = true;
                int small_num = Random.Range(0, tumbleweed.small_tumbleweed.Count - 1);
                tumbleweed.warning.Add(Warning_box_punch_scale(new Vector3(0, tumbleweed.small_tumbleweed[small_num], 0), new Vector3(15, 0.8f, 0), new Vector3(15, 1, 0), 0.1f, new Vector3(15, 1.25f, 0), 0.05f, false, true));
                tumbleweed.horizontal_tumbleweed_instance.Add(tumbleweed.small_tumbleweed[small_num]);
                tumbleweed.small_tumbleweed.RemoveAt(small_num);
                break;
            case 3:     //위에서 회전초 떨어지고 비트박스 아랫부분에 닿으면 바람 부는 방향으로 굴러감(0.7초동안 경고장판이 생긴뒤 떨어짐) 1.5
                float pos_x = tumbleweed.vertical_tumbleweed[Random.Range(0, tumbleweed.vertical_tumbleweed.Count - 1)];
                tumbleweed.vertical_tumbleweed.Remove(pos_x);
                GameObject warning = Warning_box_punch_scale(new Vector3(pos_x, 0, 0), new Vector3(1.5f, 7.5f, 0),
                    new Vector3(1.8f, 7.5f, 0), 0.5f, new Vector3(2.5f, 7.5f, 0), 0.1f, true, false, true, () =>
                    {
                        Vertical_tumble(Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Vertical_tumbleweed")), pos_x);
                    });
                break;
            default:
                break;
        }
    }
    public void Vertical_tumble(GameObject temp, float pos_x)
    {
        temp.transform.position = new Vector3(pos_x, 7, 0);
        temp.transform.DOMoveY(-7, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            tumbleweed.vertical_tumbleweed.Add(temp.transform.position.x);
            Managers.Pool.Push(temp);
        });
    }
    public void Powder_keg_pattern()
    {
        switch (powder_keg.pattern_data[powder_keg.pattern_count].action_num)
        {
            case 0:     //화약통 생성 후 조건 충족 시 폭발
                powder_keg.num = Random.Range(0, powder_keg.deployable_pos.Count - 1);
                GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Powder_keg"));
                temp.transform.localScale = Vector3.zero;
                temp.transform.position = powder_keg.deployable_pos[powder_keg.num];
                temp.transform.DOScale(Vector3.one, 0.35f);
                powder_keg.deployable_pos.RemoveAt(powder_keg.num);
                powder_keg.objs.Add(temp.transform);
                if (powder_keg.objs.Count != 0)
                {
                    foreach (var item in powder_keg.objs)
                    {
                        if (item.transform.position != temp.transform.position)
                        {
                            if (item.position.x == temp.transform.position.x && !powder_keg.boom.Contains(item))
                            {
                                Warning_box_fade(new Vector3(5f, 7.5f, 0), new Vector3(item.position.x, 0, 0), true, 3, 0.23f,() => Powder_keg_pattern_boom(item, temp, true));

                                //FIX : 나중엔 여길 터지는 애니메이션 작동되게 변경
                            }
                            else if (item.position.y == temp.transform.position.y && !powder_keg.boom.Contains(item))
                            {
                                Warning_box_fade(new Vector3(15f, 2.5f, 0), new Vector3(0, item.position.y, 0), true, 3, 0.23f,() => Powder_keg_pattern_boom(item, temp, false));
                            }
                        }
                    }
                }
                break;
            case 1:     //화약통 다 사라짐 0.4초 dotween 그리고 박스 작아짐: 0, -1.5, 0의 위치와 1의 스케일
                foreach (var item in powder_keg.objs)
                {
                    if (item.gameObject.activeSelf)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                Managers.UI_jun.Fade_out_in("White", 0f, 0.3f, 0.1f, 0.3f, Boss_die);
                Managers.GameManager.Beat_box.transform.position = new Vector3(0, -1.5f, 0);
                Managers.GameManager.Player.transform.position = Vector3.zero;
                Managers.GameManager.Beat_box.transform.localScale = Vector3.one;
                break;
            default:
                break;
        }
    }
    public void Boss_die()
    {
        boss_character.SetActive(true);
        boss_character.transform.position = new Vector3(0, 0.25f, 0);
        foreach (var item in hands)
        {
            item.SetActive(false);
        }
        Anim_state_machin2(anim_state["stage2_dead"], false);
    }
    public void Powder_keg_pattern_boom(Transform item, GameObject temp, bool criteria_pos_x)
    {
        Managers.Main_camera.Shake_move();
        powder_keg.boom.Add(item);
        powder_keg.boom.Add(temp.transform);
        foreach (var item2 in powder_keg.boom)
        {
            if (!anim_ongoing_obj.ContainsKey(item2.gameObject))
            {
                anim_ongoing_obj.Add(item2.gameObject, item2.GetComponent<Animator>());
            }
            anim_ongoing_obj[item2.gameObject].Play("Powder_keg_boom");
            powder_keg.deployable_pos.Add(item2.position);
            powder_keg.objs.Remove(item2);
        }
        powder_keg.boom.Clear();
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
        public bool right_shoot = false;        //두개의 에임중에 공격할 에임이 뭔지 판별하기 위한 변수
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
        public GameObject throw_dynamite;
        public float dynamite_landing_pos_x;
        public float dynamite_throw_pos_x_range;
        public int dir = 1;
        public Dictionary<GameObject, Animator> dynamite_anim = new Dictionary<GameObject, Animator>();
    }
    [Serializable]
    public class Tumbleweed : Pattern_base_data
    {
        public List<float> small_tumbleweed = new List<float>();            //x축 방향으로 굴러가는 회전초
        public List<float> big_tumbleweed = new List<float>();              //x축 방향으로 굴러가는 회전초
        public List<float> vertical_tumbleweed = new List<float>();         //Y축 방향으로 떨어지는 회전초
        public int dir = 1;                 //바람 부는 방향의 기준이 되는 값
        public bool small_turn = false;                                     //x축 방향으로 굴러가는 회전초 2개 중 뭐가 굴러가는지 정해주는 불값
        public List<float> horizontal_tumbleweed_instance = new List<float>();                  //생성된 x축 방향으로 굴러가는 회전초를 넣어두는 리스트
        public List<GameObject> warning = new List<GameObject>();                               //경고판을 관리하는 변수
    }
    [Serializable]
    public class Powder_keg : Pattern_base_data
    {
        public List<Vector3> deployable_pos = new List<Vector3>();      //배치 가능한 위치
        public HashSet<Transform> objs = new HashSet<Transform>();            //배치된 오브젝트들
        public HashSet<Transform> boom = new HashSet<Transform>();          //터진 오브젝트들
        public int num;                     //List(deployable_pos)에 있는 값들중 랜덤으로 고르기 위한 변수
    }
}

