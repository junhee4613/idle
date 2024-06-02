using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using DG.Tweening;

public class Bitto : BossController
{
    public Tutorial tutorial;
    public Hammer hammer;
    public Bitto_trnasform bitto_trnasform;
    public Ping_pong ping_pong;
    public Obstacle obstacle;

    Collider2D[] censer;
    public GameObject bitto_obj;
    public Bitto_box bitto_box;
    string[] anims = new string[] { "idle", "left_hammer", "right_hammer", "face_trans", "angry_right_hammer", "angry_left_hammer", "angry_bitto_idle" };
    string[] bitto_box_anims = new string[] { "face_angry_trans", "face_nomal_trans", "face_on", "trans_ping_pong", "blank_face", "ping_pong_change_normal" };
    public GameObject player_box;       //FIX : 여기 나중에 풀링으로 수정
    protected Dictionary<string, Anim_stage_state> bitto_box_anim_state = new Dictionary<string, Anim_stage_state>();

    bool test = false;
    // Start is called before the first frame update
    protected override void Awake()
    {
        anim_state.Anim_processing2(ref an, anims);
        bitto_box_anim_state.Anim_processing2(ref bitto_box.an, bitto_box_anims);
        hammer.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_hammer_data").text);
        ping_pong.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_bitto_ping_pong_data").text);
        bitto_trnasform.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_bitto_transform_data").text);
        obstacle.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_bitto_obstacle_data").text);
        Cursor.visible = false;
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
        if (tutorial.player_move)
        {
            Pattern_function(ref hammer.pattern_data, ref hammer.pattern_ending, ref hammer.duration,ref hammer.pattern_count, Hammer_pattern);
            Pattern_function(ref bitto_trnasform.pattern_data, ref bitto_trnasform.pattern_ending, ref bitto_trnasform.duration,ref bitto_trnasform.pattern_count, Bitto_transform_pattern);
            Pattern_function(ref ping_pong.pattern_data, ref ping_pong.pattern_ending, ref ping_pong.duration,ref ping_pong.pattern_count, Bitto_ping_pong_pattern);
            Pattern_function(ref obstacle.pattern_data, ref obstacle.pattern_ending, ref obstacle.duration,ref obstacle.pattern_count, Obstacle_pattern);
        }
        else if (!Managers.UI_jun.fade_start)
        {
            if(tutorial.tutorial_end == false)
            {
                StartCoroutine(Bitto_appearence());
                tutorial.tutorial_end = true;
            }
            else if (test)
            {
                Managers.GameManager.operate = true;
                if (Input.GetMouseButtonUp(0))
                {
                    tutorial.player_move = true;
                }
            }
        }
    }

    IEnumerator Bitto_appearence()
    {
        yield return new WaitForSeconds(tutorial.bitto_appearence_time);
        bitto_obj.SetActive(true);
        Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>("Tutorial_stage"), false);            //이거 나중에 옮겨야됨- 이건 비토가 화났을 때 재생시켜야돼서
        //파티클 넣기
        yield return new WaitForSeconds(tutorial.mouse_cursor_appearence_time);
        StartCoroutine(Mouse_cursor_appearence());
    }
    IEnumerator Mouse_cursor_appearence()
    {
        Anim_state_machin2(anim_state["idle"], false);
        test = true;
        Cursor.visible = true;
        yield return null;
    }
    public void Hammer_pattern()
    {
        switch (hammer.pattern_data[hammer.pattern_count].action_num)
        {
            case 0:     //처음 플레이어 박스 및 해머 생성
                Managers.GameManager.Player.transform.position = new Vector3(0, -2, 0);
                ParticleSystem warp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warp")).GetComponent<ParticleSystem>();
                warp.transform.position = Managers.GameManager.Player_character.position;
                warp.Play();
                for (int i = 0; i < 2; i++)
                {
                    hammer.hammer_action[i] = Managers.Pool.Pop(hammer.hammer_obj).transform;
                }
                hammer.hammer_action[0].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer.hammer_action[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer.hammer_action[0].position = new Vector3(-12.5f, -2, 0);
                hammer.hammer_action[1].position = new Vector3(12.5f, -2, 0);
                player_box.SetActive(true);
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
    public void Hammer_swing(Transform[] hammer, float right_hammer_rot = 270, float left_hammer_rot = -90, float duration = 1, Action action = null)
    {
        switch (this.hammer.dir)
        {
            case 1:
                Anim_state_machin2(anim_state["angry_left_hammer"], false);
                hammer[0].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer[0].DORotate(new Vector3(0, 0, left_hammer_rot), duration).OnComplete(() =>
                {
                    Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                    if(action != null)
                    {
                        action();
                    }
                });
                break;
            case -1:
                Anim_state_machin2(anim_state["angry_right_hammer"], false);
                hammer[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                hammer[1].DORotate(new Vector3(0, 0, right_hammer_rot), duration).OnComplete(() =>
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
                Hammer_swing(hammer.hammer_action,-180, 180, 0.5f, () => 
                { 
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
                Anim_state_machin2(bitto_box_anim_state["face_on"], false);
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
                break;
            case 1:     //가로로 있는 장애물
                break;
            case 2:     //화면이 떨리고
                break;
            case 3:     //비토 얼굴이 등장
                break;
            case 4:     //비토랑 플레이어 박스가 점점 투명해짐
                break;
            case 5:     //비토 등장
                break;

        }
    }
    [Serializable]
    public class Tutorial
    {
        public Vector3 player_init_pos;
        public Vector3 bitto_init_pos;
        public bool player_move = false;
        public float bitto_appearence_time;
        public float mouse_cursor_appearence_time;
        public bool tutorial_end = false;
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
