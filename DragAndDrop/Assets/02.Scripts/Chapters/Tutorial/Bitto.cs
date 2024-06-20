using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;
using static Bitto;

public class Bitto : BossController
{
    public Tutorial tutorial;
    public Operation operation;
    public Hammer hammer;
    public Bitto_trnasform bitto_trnasform;
    public Ping_pong ping_pong;
    public Obstacle obstacle;

    Collider2D[] censer;
    public GameObject bitto_obj;
    public Bitto_box bitto_box;
    string[] anims = new string[] { "idle", "left_hammer", "right_hammer", "face_trans", "angry_right_hammer", "angry_left_hammer", "angry_bitto_idle", "both_hit_box", "angry_both_hammer"};
    string[] bitto_box_anims = new string[] { "face_angry_trans", "face_nomal_trans", "face_on1", "face_on2", "trans_ping_pong", "blank_face", "ping_pong_change_normal", "blank_box_face_on"};
    public GameObject player_box;       //FIX : 여기 나중에 풀링으로 수정
    protected Dictionary<string, Anim_stage_state> bitto_box_anim_state = new Dictionary<string, Anim_stage_state>();
    bool tutorial_bgm_start = false;
    bool rurotial_operation_bgm_start = false;
    AudioClip tutorial_clip;
    // Start is called before the first frame update
    protected override void Awake()
    {
        anim_state.Anim_processing2(ref an, anims);
        bitto_box_anim_state.Anim_processing2(ref bitto_box.an, bitto_box_anims);
        operation.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_operation_data").text);
        hammer.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_hammer_data").text);
        ping_pong.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_bitto_ping_pong_data").text);
        bitto_trnasform.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_bitto_transform_data").text);
        obstacle.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_bitto_obstacle_data").text);
        if (Managers.GameManager.base_tutorial_end)
        {
            Cursor.visible = true;
            bitto_obj.SetActive(true);
            Anim_state_machin2(anim_state["angry_bitto_idle"], false);
        }
        else
        {
            bitto_obj.SetActive(false);
            Cursor.visible = false;
            tutorial_clip = Managers.Resource.Load<AudioClip>("Operation_tutorial");
            Managers.GameManager.tutorial = true;
        }
        Managers.GameManager.game_start = true;
    }
    void Start()
    {
        tutorial.player_init_pos = Managers.GameManager.Player_character.position;
        hammer.hammer_obj = Managers.Resource.Load<GameObject>("Hammer");
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ping_pong.ping_pong_obj.activeSelf)
        {
            ping_pong.walls[ping_pong.turn].transform.position = Following_move_pos_y(ping_pong.walls[ping_pong.turn].transform.position, ping_pong.ball_rb.gameObject.transform.position,6);
            censer = Physics2D.OverlapCircleAll(ping_pong.ball_rb.transform.position, ping_pong.ball_cc.radius + 0.01f, 1 << 8);
            if (censer.Length != 0)
            {
                if (!ping_pong.bounce)
                {
                    switch (ping_pong.turn)
                    {
                        case 1:
                            ping_pong.turn = 0;
                            break;
                        case 0:
                            ping_pong.turn = 1;
                            break;
                    } 
                }
                ping_pong.bounce = true;
            }
            else
            {
                ping_pong.bounce = false;
            }
        }
    }
    public Vector3 Following_move_pos_y(Vector3 pos, Vector3 target, float speed)
    {
        pos = Vector3.MoveTowards(pos, new Vector3(pos.x, target.y, 0), speed * Time.fixedDeltaTime);
        return pos;
    }
    // Update is called once per frame
    public override void Pattern_processing()
    {
        if (!Managers.GameManager.player_move)
        {
            if (!Managers.UI_jun.fade_start)
            {
                if (tutorial.tutorial_start == false)
                {
                    tutorial.tutorial_start = true;
                    StartCoroutine(Bitto_appearence());
                }
                else if (tutorial.moveable)
                {
                    Managers.GameManager.operate = true;
                    if (Input.GetMouseButtonUp(0))
                    {
                        Managers.GameManager.player_move = true;
                    }
                } 
            }
        }
        else if (!Managers.GameManager.base_tutorial_end)
        {
            if (!rurotial_operation_bgm_start)
            {
                rurotial_operation_bgm_start = true;
                Managers.Sound.BGMSound(tutorial_clip, true);
            }
            else if (Managers.GameManager.tutorial_hit && !operation.pattern_ending)         //튜토리얼 진행 중 한대라도 맞으면 다시 노래 시작
            {
                
                bitto_obj.transform.DOMoveX(0, 0.1f).SetDelay(0.25f).OnComplete(() => 
                {
                    Managers.GameManager.tutorial_hit = false;
                    operation.pattern_count = 0;
                    Managers.Sound.BGMSound(tutorial_clip, true);
                });
            }
            else
            {
                Pattern_function(ref operation.pattern_data, ref operation.pattern_ending, ref operation.duration, ref operation.pattern_count, Operation_pattern);
            }
        }
        else
        {
            if(!tutorial_bgm_start)
            {
                Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>("Tutorial_stage"), false);
                tutorial_bgm_start = true;
                Managers.GameManager.tutorial = false;
            }
            Boss_pattern_start();
        }
    }
    public void Boss_pattern_start()
    {
        Pattern_function(ref hammer.pattern_data, ref hammer.pattern_ending, ref hammer.duration, ref hammer.pattern_count, Hammer_pattern);
        Pattern_function(ref bitto_trnasform.pattern_data, ref bitto_trnasform.pattern_ending, ref bitto_trnasform.duration, ref bitto_trnasform.pattern_count, Bitto_transform_pattern);
        Pattern_function(ref ping_pong.pattern_data, ref ping_pong.pattern_ending, ref ping_pong.duration, ref ping_pong.pattern_count, Bitto_ping_pong_pattern);
        Pattern_function(ref obstacle.pattern_data, ref obstacle.pattern_ending, ref obstacle.duration, ref obstacle.pattern_count, Obstacle_pattern);
    }
    public void Operation_pattern()     //이 패턴이 끝날 때까지 안맞아야 성공
    {
        switch (operation.pattern_data[operation.pattern_count].action_num)
        {
            case 0:
                //왼쪽 오른쪽 번갈아가면서 패턴 나오기
                bitto_obj.transform.DOMoveX(-2 * operation.num, 0.1f).SetDelay(0.25f).OnComplete(() =>
                {
                    switch (operation.num)
                    {
                        case 1:
                            Anim_state_machin2(anim_state["right_hammer"], false);
                            break;
                        case -1:
                            Anim_state_machin2(anim_state["left_hammer"], false);
                            break;
                    }
                    GameObject hit_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Hit_box"));
                    Hit_box_appearence(hit_box, 4.5f * operation.num);
                    hit_box.transform.DOScaleY(2, 0.55f).SetEase(Ease.OutQuint).OnComplete(() =>
                    {
                        hit_box.transform.DOScaleY(12, 0.1f).OnComplete(() =>
                        {
                            Managers.Main_camera.Move_y(1f, 0.1f, 0, 0.1f);
                            CoroutineManager.StartCoroutine(Hit_box_push(hit_box, 0.25f));
                        });
                    });
                });
                break;
            case 1:
                //가운데로 나와서 비토 맞고 고장나기
                bitto_obj.transform.DOMoveX(0, 0.1f).SetDelay(0.25f).OnComplete(() => 
                {
                    Anim_state_machin2(anim_state["both_hit_box"], false);
                    GameObject hit_box2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Hit_box"));
                    Hit_box_appearence(hit_box2, 0);
                    hit_box2.transform.DOScaleY(2, 0.55f).SetEase(Ease.OutQuint).OnComplete(() =>
                    {
                        hit_box2.transform.DOScaleY(12, 0.1f).OnComplete(() =>
                        {
                            Anim_state_machin2(anim_state["angry_bitto_idle"], false);
                            Managers.Main_camera.Move_y(1f, 0.1f, 0, 0.1f);
                            CoroutineManager.StartCoroutine(Hit_box_push(hit_box2, 0.25f));
                            Managers.GameManager.base_tutorial_end = true;
                        });
                    });
                });
                break;
        }
    }
    public void Hit_box_appearence(GameObject obj, float pos_x)
    {
        obj.transform.localScale = new Vector3(9, 0, 0);
        obj.transform.position = new Vector3(pos_x, 6, 0);
        operation.num *= -1;

    }
    IEnumerator Hit_box_push(GameObject temp, float time)
    {
        yield return new WaitForSeconds(time);
        Managers.Pool.Push(temp);
    }
    IEnumerator Bitto_appearence()
    {
        yield return new WaitForSeconds(tutorial.bitto_appearence_wait_time);
        GameObject particle1 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boss_warp"));
        GameObject particle2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boss_appearence_ball"));
        particle1.transform.position = bitto_obj.transform.position;
        particle2.transform.position = bitto_obj.transform.position;
        particle1.GetComponent<ParticleSystem>().Play();
        particle2.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        bitto_obj.SetActive(true);
        yield return new WaitForSeconds(tutorial.mouse_cursor_appearence_wait_time);
        StartCoroutine(Mouse_cursor_appearence());
    }
    IEnumerator Mouse_cursor_appearence()
    {
        Anim_state_machin2(anim_state["idle"], false);
        tutorial.moveable = true;
        Cursor.visible = true;
        yield return null;
    }
    public void Hammer_pattern()
    {
        switch (hammer.pattern_data[hammer.pattern_count].action_num)
        {
            case 0:     //처음 플레이어 박스 및 해머 생성
                Managers.GameManager.Player_character.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
                {
                    Managers.GameManager.Player_character.transform.localScale = Vector3.one * 0.4f;
                    Managers.GameManager.Player.transform.position = new Vector3(0, -2, 0);
                    ParticleSystem warp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warp")).GetComponent<ParticleSystem>();
                    warp.transform.position = Managers.GameManager.Player_character.position;
                    warp.Play();
                });
                for (int i = 0; i < 2; i++)
                {
                    hammer.hammer_action[i] = Managers.Pool.Pop(hammer.hammer_obj).transform;
                }
                hammer.hammer_action[0].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer.hammer_action[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer.hammer_action[0].position = new Vector3(-12.5f, -2, 0);
                hammer.hammer_action[1].position = new Vector3(12.5f, -2, 0);
                player_box.SetActive(true);
                Managers.Main_camera.Shake_move();
                break;
            case 1:     //처음 경고장판 생성
                Charging_doscale_y_warning_box(Vector3.one * 5, new Vector3(-2.5f * hammer.dir, -2, 0), new Vector3(-2.5f * hammer.dir, 0.5f, 0),Warning_box_pivots.UP, color, 1);
                break;
            case 2:     //해머 휘두르며 경고장판 생성
                Hammer_swing(hammer.hammer_action);
                hammer.dir *= -1;
                Charging_doscale_y_warning_box(Vector3.one * 5, new Vector3(-2.5f * hammer.dir, -2, 0), new Vector3(-2.5f * hammer.dir, 0.5f, 0),Warning_box_pivots.UP, color, 1);
                break;
            case 3:     //해머만 휘두름
                Hammer_swing(hammer.hammer_action);
                hammer.dir *= -1;
                break;
        }
    }
    public void Hammer_swing(Transform[] hammer, float right_hammer_rot = 270, float left_hammer_rot = -90, float duration = 1, Action anim_start = null, Action action = null)
    {
        switch (this.hammer.dir)
        {
            case 1:
                if (anim_start == null)
                {
                    Anim_state_machin2(anim_state["angry_left_hammer"], false);
                }
                else
                {
                    anim_start();
                }
                hammer[0].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer[0].DORotate(new Vector3(0, 0, left_hammer_rot), duration).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                    if(action != null)
                    {
                        action();
                    }
                });
                break;
            case -1:
                if (anim_start == null)
                {
                    Anim_state_machin2(anim_state["angry_right_hammer"], false);
                }
                else
                {
                    anim_start();
                }
                hammer[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer[1].DORotate(new Vector3(0, 0, right_hammer_rot), duration).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                    if (action != null)
                    {
                        action();
                    }
                });
                break;
        }
    }
    public void Bitto_transform_pattern()
    {
        switch (bitto_trnasform.pattern_data[bitto_trnasform.pattern_count].action_num)
        {
            case 0:     //비토가 망치에 맞고 사라짐 (회전값 줘서 1초 동안 주기)
                hammer.hammer_action[1].position = new Vector3(10, 4);
                hammer.hammer_action[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                Hammer_swing(hammer.hammer_action,-180, 180, 0.5f, () => Anim_state_machin2(anim_state["angry_both_hammer"], false), () => 
                {
                    Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Hammer_particle"));
                    bitto_obj.SetActive(false);
                    hammer.hammer_action[1].DORotate(new Vector3(0, 0, 90), 1).SetDelay(0.5f).OnComplete(() => 
                    {
                        for (int i = 0; i < hammer.hammer_action.Length; i++)
                        {
                            hammer.hammer_action[i].gameObject.SetActive(false);
                        }
                    });
                });
                break;
            case 1:     //화면이 일렁거림
                Managers.Main_camera.Shake_move(4, 0.5f, 4, 90, false, true);
                player_box.transform.DOScale(bitto_box.player_box_scale, 4);
                player_box.transform.DOMoveY(bitto_box.player_box_pos.y, 4);
                break;
            case 2:     //비토 얼굴 등장하면서
                bitto_box.obj.SetActive(true);
                Anim_state_machin2(bitto_box_anim_state["face_on1"], false);
                bitto_box.obj.GetComponent<SpriteRenderer>().DOFade(1, 4);
                break;
            case 3:
                Anim_state_machin2(bitto_box_anim_state["face_on2"], false);
                break;
        }
    }
    public void Bitto_ping_pong_pattern()
    {
        switch (ping_pong.pattern_data[ping_pong.pattern_count].action_num)
        {
            case 0:     //얼굴이 변하면서 공 튕길 바들을 생성
                Anim_state_machin2(bitto_box_anim_state["trans_ping_pong"], false);
                break;
            case 1:     //비트에 맞춰 화면이 움직임이고 이때 공 움직이기 시작
                if (ping_pong.start == false)
                {
                    ping_pong.ping_pong_obj.SetActive(true);
                    ping_pong.ball_rb.AddForce(new Vector2(1, 1).normalized * ping_pong.ball_speed,ForceMode2D.Impulse);
                    ping_pong.start = true;
                }
                Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                Anim_state_machin2(bitto_box_anim_state["blank_face"], false);
                
                //Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                break;
            case 2:     //공이 다시 입의 위치로 돌아오면서(dotween으로 처리) 얼굴로 다시 돌아옴(0.5초만에 이동)
                ping_pong.ball_rb.gameObject.transform.DOMove(ping_pong.face_pos[0], 0.3f);
                ping_pong.walls[0].transform.DOMove(ping_pong.face_pos[1], 0.3f);
                ping_pong.walls[1].transform.DOMove(ping_pong.face_pos[2], 0.3f).OnComplete(() => 
                {
                    Anim_state_machin2(bitto_box_anim_state["ping_pong_change_normal"], false);
                    ping_pong.ping_pong_obj.SetActive(false);
                });
                break;
            case 3:     //화난 얼굴로 변경
                Anim_state_machin2(bitto_box_anim_state["face_angry_trans"], false);
                break;
        }
    }
    public void Obstacle_pattern()
    {
        switch (obstacle.pattern_data[obstacle.pattern_count].action_num)
        {
            case 0:     //새로로 있는 장애물
                GameObject vertical_pillar = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Vertical_pillar"));
                vertical_pillar.transform.position = new Vector3(12, obstacle.spawn_pos_y[obstacle.num]);
                vertical_pillar.transform.DOMoveX(-12, 2.5f).SetEase(Ease.Linear).OnComplete(() => Managers.Pool.Push(vertical_pillar));
                obstacle.num = obstacle.num == 1 ? 0 : 1;
                break;
            case 1:     //가로로 있는 장애물 X축 12에서 시작
                GameObject horizontal_pillar = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Horizontal_pillar"));
                int num = Random.Range(0, obstacle.horizontal_pillar.Count);
                horizontal_pillar.transform.position = new Vector3(12, obstacle.horizontal_pillar[num]);
                obstacle.horizontal_pillar.RemoveAt(num);
                horizontal_pillar.transform.DOMoveX(-12, 2.5f).SetEase(Ease.Linear).OnComplete(() => 
                {
                    obstacle.horizontal_pillar.Add(horizontal_pillar.transform.position.y);
                    Managers.Pool.Push(horizontal_pillar);
                });
                break;
            case 2:     //화면이 떨리고
                Managers.Main_camera.Shake_move();
                break;
            case 3:     //비토 얼굴이 등장
                Anim_state_machin2(bitto_box_anim_state["blank_box_face_on"], false);
                break;
            case 4:     //비토랑 플레이어 박스가 점점 투명해짐
                bitto_box.obj.GetComponent<SpriteRenderer>().DOFade(0, 2).OnComplete(() => bitto_box.obj.SetActive(false));
                player_box.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0, 2).OnComplete(() => player_box.SetActive(false));
                break;
            case 5:
                GameObject particle1 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boss_warp"));
                GameObject particle2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boss_appearence_ball"));
                particle1.transform.position = bitto_obj.transform.position;
                particle2.transform.position = bitto_obj.transform.position;
                particle1.GetComponent<ParticleSystem>().Play();
                particle2.GetComponent<ParticleSystem>().Play();
                break;
            case 6:     //비토 등장
                bitto_obj.SetActive(true);
                Anim_state_machin2(anim_state["idle"], false);
                break;

        }
    }
    [Serializable]
    public class Tutorial
    {
        public Vector3 player_init_pos;
        public Vector3 bitto_init_pos;
        internal float bitto_appearence_wait_time;
        internal float mouse_cursor_appearence_wait_time;
        internal bool tutorial_start = false;
        internal bool moveable = false;

    }
    [Serializable]
    public class Operation : Pattern_base_data 
    {
        internal sbyte num = 1;
    }
    [Serializable]
    public class Hammer : Pattern_base_data
    {
        public GameObject hammer_obj;
        public Transform[] hammer_action = new Transform[2];
        public Vector3 hammer_pos;
        public float rot_speed;
        public sbyte dir = 1;
        public sbyte attack_count = 2;      //2번 공격 후에 3번째 공격은 랜덤한 방향에서 공격
    }
    [Serializable]
    public class Bitto_trnasform : Pattern_base_data
    {

    }
    [Serializable]
    public class Ping_pong : Pattern_base_data
    {
        public Vector3[] face_pos = new Vector3[3];
        public Rigidbody2D ball_rb;
        public GameObject[] walls;
        public GameObject ping_pong_obj;
        internal float ball_speed = 12;
        public bool start = false;
        internal int turn = 1;
        public CircleCollider2D ball_cc;
        public bool bounce = false;
    }
    [Serializable]
    public class Obstacle : Pattern_base_data
    {
        internal int num = 1;
        internal float[] spawn_pos_y = new float[2] {3, -4};
        internal List<float> horizontal_pillar = new List<float>() {-3.2f, -1.85f, -0.5f, 0.85f, 2.2f};
    }
    [Serializable]
    public class Bitto_box 
    {
        public GameObject obj;
        public Animator an;
        public Vector3 bitto_box_pos = new Vector3(0, -1, 0);
        public Vector3 player_box_scale = new Vector3(1.6f, 1.45f, 0);
        public Vector3 player_box_pos = new Vector3(0, -0.5f, 0);
    }

}
