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
    public GameObject bitto_obj;
    string[] anims = new string[] { "idle", "left_hammer", "right_hammer" };
    public GameObject player_box;       //FIX : 여기 나중에 풀링으로 수정
    public Color[] color = new Color[2];


    bool test = false;
    // Start is called before the first frame update
    protected override void Awake()
    {
        anim_state.Anim_processing2(ref an, anims);
        hammer.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Tutorial_hammer_data").text);
        Cursor.visible = false;
        Managers.GameManager.game_start = true;
    }
    void Start()
    {
        tutorial.player_init_pos = Managers.GameManager.Player_character.position;
        hammer.hammer_obj = Managers.Resource.Load<GameObject>("Hammer");
    }

    // Update is called once per frame
    public override void Pattern_processing()
    {
        if (tutorial.player_move)
        {
            Pattern_function(ref hammer.pattern_data, ref hammer.pattern_ending, ref hammer.duration,ref hammer.pattern_count, Hammer_pattern);
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
        Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>("Tutorial_stage"), false);            //이거 나중에 옮겨야됨
        //파티클 넣기
        yield return new WaitForSeconds(tutorial.mouse_cursor_appearence_time);
        StartCoroutine(Mouse_cursor_appearence());
    }
    IEnumerator Mouse_cursor_appearence()
    {
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
               /* ParticleSystem warp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warp")).GetComponent<ParticleSystem>();
                warp.transform.position = Managers.GameManager.Player_character.position;
                warp.Play();*/
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
                Doscale_y__warning_box(Vector3.one * 5, new Vector3(-2.5f * hammer.dir, -2, 0), new Vector3(-2.5f * hammer.dir, 0.5f, 0), color[0], color[1], 1);
                break;
            case 2:     //해머 휘두르며 경고장판 생성
                switch (hammer.dir)
                {
                    case 1:
                        hammer.hammer_action[0].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        hammer.hammer_action[0].DORotate(new Vector3(0, 0, -90), 1).OnComplete(() =>
                        {
                           Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                        });
                        break;
                    case -1:
                        hammer.hammer_action[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        hammer.hammer_action[1].DORotate(new Vector3(0, 0, 270), 1).OnComplete(() =>
                        {
                            Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);

                        });
                        break;
                }
                hammer.dir *= -1;
                Doscale_y__warning_box(Vector3.one * 5, new Vector3(-2.5f * hammer.dir, -2, 0), new Vector3(-2.5f * hammer.dir, 0.5f, 0), color[0], color[1], 1);
                break;
            case 3:     //해머만 휘두름
                Debug.Log(1);
                switch (hammer.dir)
                {
                    case 1:
                        hammer.hammer_action[0].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        hammer.hammer_action[0].DORotate(new Vector3(0, 0, -90), 1).OnComplete(() =>
                        {
                            Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                        });
                        break;
                    case -1:
                        hammer.hammer_action[1].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        hammer.hammer_action[1].DORotate(new Vector3(0, 0, 270), 1).OnComplete(() =>
                        {
                            Managers.Main_camera.Move_y(-0.1f, 0.1f, 0, 0.1f);
                        });
                        break;
                }
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
}
