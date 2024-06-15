using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;
using Newtonsoft.Json;
using DG.Tweening;


public class ChaInGureumi : BossController          //비트는 80dlek
{
    public Transform boss_trans;
    /*[Header("비 패턴")]
    public Rain_drop_pattern rain_drop;*/
    [Header("비바람 패턴")]
    public Rain_storm_pattern rain_storm;
    [Header("소나기 패턴")]
    public Shower_pattern shower;
    [Header("돌진 패턴")]
    public Rush_pattern rush;
    [Header("단일 번개 패턴")]
    public Lightning_pattern lightning;
    [Header("2페이즈 시작")]
    public Phase_2_pattern phase_2;
    [Header("전기 구체 패턴")]
    public Electric_ball_pattern electric_ball_pattern;
    [Header("전기 구체 회전")]
    public Electric_ball_rotation_pattern electric_ball_rotation_pattern;
    
    protected override void Awake()
    {
        base.Awake();
        rain_storm.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_rain_storm_data").text);
        rush.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_rush_pattern_data").text);
        shower.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_shower_data").text);
        lightning.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_lightning_data").text);
        phase_2.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage_1_2_phase_start_data").text);
        electric_ball_rotation_pattern.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_electric_ball_rotation_data").text);
        electric_ball_pattern.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_Electric_ball_pattern_data").text);
        
    }
    public void Start()
    {
        Anim_state_machin(anim_state["1_phase_idle"]);
        Managers.GameManager.game_start = true;
    }
    public override void Pattern_processing()
    {
        Pattern_function(ref rain_storm.pattern_data, ref rain_storm.pattern_ending, ref rain_storm.duration, ref rain_storm.pattern_count,ref rain_storm.time, Rain_storm_rotation, true, 0.375f, Rain_storm);
        Pattern_function(ref shower.pattern_data, ref shower.pattern_ending, ref shower.duration, ref shower.pattern_count, Shower);
        Pattern_function(ref rush.pattern_data, ref rush.pattern_ending, ref rush.duration, ref rush.pattern_count, Rush);
        if (!lightning.pattern_ending)
        {
            if(lightning.pattern_data[lightning.pattern_count].time <= Managers.Sound.bgSound.time)
            {
                Anim_state_machin(anim_state["2_phase_idle"]);
                Lightning();
            }
        }
        Pattern_function(ref phase_2.pattern_data, ref phase_2.pattern_ending, ref phase_2.duration, ref phase_2.pattern_count, Phase_2);
        Pattern_function(ref electric_ball_pattern.pattern_data, ref electric_ball_pattern.pattern_ending, ref electric_ball_pattern.duration, ref electric_ball_pattern.pattern_count, Electric_ball);
        Pattern_function(ref electric_ball_rotation_pattern.pattern_data, ref electric_ball_rotation_pattern.pattern_ending, ref electric_ball_rotation_pattern.duration, ref electric_ball_rotation_pattern.pattern_count, Electric_ball_rotation);
    }
    public void Rain_storm_rotation()
    {
        switch (rain_storm.pattern_data[rain_storm.pattern_count].action_num)
        {
            case 0:         //가운데(바람 빨아들이는 애니메이션)
                Anim_state_machin(anim_state["simple_pattern2"]);
                if (rain_storm.pos_x_critaria != 0)
                {
                    rain_storm.pos_x_critaria = 0;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 0)
                {
                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.identity, rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            case 1:         //왼쪽(왼쪽 방향으로 부는 애니메이션)
                Anim_state_machin(anim_state["simple_pattern3"]);
                if (boss_trans.transform.localScale.x != -1)
                {
                    boss_trans.transform.localScale = new Vector3(-1f, 1f, 0f);
                }
                if (rain_storm.pos_x_critaria != rain_storm.pos_x_criteria_option)
                {
                    rain_storm.pos_x_critaria = -rain_storm.pos_x_criteria_option;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 315)
                {

                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.Euler(0, 0, 315), rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            case 2:     //오른쪽(오른쪽으로 부는 애니메이션)
                Anim_state_machin(anim_state["simple_pattern3"]);
                if (boss_trans.transform.localScale.x != 1)
                {
                    boss_trans.transform.localScale = new Vector3(1f, 1f, 0f);
                }
                if (rain_storm.pos_x_critaria != rain_storm.pos_x_criteria_option)
                {
                    rain_storm.pos_x_critaria = rain_storm.pos_x_criteria_option;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 45)
                {
                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.Euler(0, 0, 45), rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            case 3:
                Anim_state_machin(anim_state["simple_pattern3"]);
                if (rain_storm.pos_x_critaria != 0)
                {
                    rain_storm.pos_x_critaria = 0;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 0)
                {
                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.identity, rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            case 4:
                Anim_state_machin(anim_state["2_phase_idle"]);
                if (rain_storm.pos_x_critaria != 0)
                {
                    rain_storm.pos_x_critaria = 0;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 0)
                {
                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.identity, rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            default:
                break;
        }
    }
   
    public void Idle()
    {
        Anim_state_machin(anim_state["simple_pattern0"]);
    }
    public void Rain_storm()
    {
        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_drop"));
        temp.transform.rotation = rain_storm.criteria;
        switch (rain_storm.pattern_data[rain_storm.pattern_count].action_num)
        {
            case 0:         //가운데(바람 빨아들이는 애니메이션)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 1:         //왼쪽(왼쪽 방향으로 부는 애니메이션)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria + 4f, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 2:     //오른쪽(오른쪽으로 부는 애니메이션)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria - 4f, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 3:
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 4:
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            default:
                break;
        }
        if(rain_storm.pos_x_count == rain_storm.pos_x.Length - 1)
        {
            rain_storm.pos_x_count = 0;
        }
        else
        {
            rain_storm.pos_x_count++;
        }
    }
    
    public void Shower()
    {
        //2~-4.5
        switch (shower.pattern_data[shower.pattern_count].action_num)
        {
            case 0:
                //빨간 박스 생김
                Anim_state_machin(anim_state["simple_pattern1"]);
                Warning_box_fade(new Vector3(6.5f, 4, 0), new Vector3(0.75f, -2, 0), true, 3, 0.25f);
                break;
            case 1:
                //울면서 플레이어 박스 끝쪽으로 이동
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(2.5f, 2.4f), 2.5f * Time.fixedDeltaTime);
                Anim_state_machin(anim_state["simple_pattern1"]);
                if (!shower.shower_obj.activeSelf && transform.position.x == 2.5f)
                {
                    shower.shower_obj.SetActive(true);
                }
                break;
            case 2:
                Anim_state_machin(anim_state["simple_pattern0"]);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1f, transform.position.y), 12 * Time.fixedDeltaTime);
                if (shower.duration == 0)
                {
                    Anim_state_machin(anim_state["1_phase_idle"]);
                    shower.shower_obj.SetActive(false);
                }
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 0);
                }
                //지정한 위치까지 돌진 후 소나기 사라짐
                break;
            case 3:
                transform.Translate(Vector3.up * Time.fixedDeltaTime * 12);
                break;
            default:
                break;
        }
    }
    public void Rush()
    {
        
        switch (rush.pattern_data[rush.pattern_count].action_num)
        {
            case 0:
                //Warning_box(new Vector3(8, Mathf.Abs(4 - (Mathf.Abs(Mathf.Abs(rush.pos_y) - 2) * 2)), 0), new Vector3(0, rush.pos_y, 0), 3, 0.25f);
                //여기 수정해야됨 
                Warning_box_fade(new Vector3(8, Mathf.Abs(4 - (Mathf.Abs(rush.pos_y) - 2)), 0), new Vector3(0, (rush.pos_y / 2f - 1), 0), true, 3, 0.25f);
                //Warning_box(new Vector3(8, Mathf.Abs(4 - (Mathf.Abs(Mathf.Abs(rush.pos_y) - 2))), 0), new Vector3(0, rush.pos_y + Mathf.Abs(rush.pos_y / 2f - 1), 0), 3, 0.25f);
                break;
            case 1:
                if (!rush.rush_start)
                {
                    transform.localScale = new Vector3(rush.dir, 1, 0);
                    rush.rush_start = true;
                    transform.position = new Vector3(rush.init_pos_x * rush.dir, -2f);
                }
                transform.DOMoveX(rush.target_pos_x * rush.dir, 0.5f);
                if (Managers.GameManager.Player.transform.position.y != transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, Managers.GameManager.Player.transform.position.y, Time.fixedDeltaTime * 3), 0);
                    rush.pos_y = transform.position.y;
                }
                break;
            case 2:
                Anim_state_machin(anim_state["1_phase_idle"]);
                if (Managers.GameManager.Player.transform.position.y != transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, Managers.GameManager.Player.transform.position.y, Time.fixedDeltaTime * 2), 0);
                    rush.pos_y = transform.position.y;
                }
                break;
            case 3:
                transform.DOMoveX(rush.target_rush_pos_x * rush.dir, 0.75f);
                Anim_state_machin(anim_state["simple_pattern0"]);

                if (rush.duration == 0)
                {
                    rush.rush_start = false;
                    rush.dir = rush.dir * -1;
                }
                //돌진

                break;
            case 4:
                //여기 수정해야됨 
                Anim_state_machin(anim_state["hard_pattern2"]);
                //탄환
                GameObject bullet = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_bullet"));
                bullet.transform.position = rush.rain_muzzle.position;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rush.bullet_roataion * rush.dir));
                Rain_bullet_controller temp = bullet.GetComponent<Rain_bullet_controller>();
                if(temp == null)
                {
                    temp = bullet.AddComponent<Rain_bullet_controller>();
                    temp.speed = rush.bullet_speed;
                    temp.push_time = rush.bullet_push_time;
                }
                break;
            case 5:
                if (!rush.rush_start)
                {
                    rush.rush_start = true;
                    transform.localScale = new Vector3(rush.dir, 1, 0);
                    transform.position = new Vector3(rush.init_pos_x * rush.dir, rush.pos_y);
                }
                transform.DOMoveX(rush.target_rush_pos_x * rush.dir, 0.75f);
                Anim_state_machin(anim_state["simple_pattern0"]);

                if (rush.duration == 0)
                {
                    rush.rush_start = false;
                    rush.dir = rush.dir * -1;
                }
                //돌진
                break;
            case 6:
                if (!rush.rush_start)
                {
                    rush.rush_start = true;
                    transform.localScale = new Vector3(rush.dir, 1, 0);
                    Anim_state_machin(anim_state["simple_pattern0"]);

                    transform.position = new Vector3(rush.init_pos_x * rush.dir, 2.7f);
                }
                transform.DOMoveX(0, 0.4f);
                //11.5에서 시작 상단 돌진
                break;
            case 7:
                Anim_state_machin(anim_state["hard_pattern1"]);
                break;
            case 8:
                if(Managers.GameManager.Player.transform.position.y >= -2)
                {
                    rush.pos_y = -1.25f;
                }
                else
                {
                    rush.pos_y = -2.75f;
                }
                Warning_box_fade(new Vector3(8, Mathf.Abs(4 - Mathf.Abs(rush.pos_y + 2)), 0), new Vector3(0, (rush.pos_y / 2f - 1), 0), true, 3, 0.25f);
                break;
            case 9:
                transform.DOMoveX(rush.target_rush_pos_x * rush.dir, 0.4f);
                Anim_state_machin(anim_state["simple_pattern0"]);
                if (rush.duration == 0)
                {
                    rush.rush_start = false;
                    rush.dir = rush.dir * -1;
                }
                break;
            case 10:
                Anim_state_machin(anim_state["1_phase_idle"]);
                break;
        }
    }
    public void Electric_ball_rotation()
    {
        switch (electric_ball_rotation_pattern.pattern_data[electric_ball_rotation_pattern.pattern_count].action_num)
        {
            case 0:
                electric_ball_rotation_pattern.lightning_ball.Rotate(0, 0, electric_ball_rotation_pattern.electric_bullet_rotation_speed * Time.fixedDeltaTime);
                break;
            case 1:
                electric_ball_rotation_pattern.lightning_ball.Rotate(0, 0, electric_ball_rotation_pattern.electric_bullet_reverse_rotation_speed * Time.fixedDeltaTime);
                break;
            case 2:
                electric_ball_rotation_pattern.lightning_ball.Rotate(0, 0, -electric_ball_rotation_pattern.electric_bullet_rotation_speed * Time.fixedDeltaTime);
                break;
            default:
                break;
        }
    }
    public void Electric_ball()
    {
        

        switch (electric_ball_pattern.pattern_data[electric_ball_pattern.pattern_count].action_num)
        {
            case 0:
                Sequence sequence = DOTween.Sequence();
                electric_ball_pattern.electric_ball_obj.SetActive(true);
                sequence.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one * 1.5f, 0.5f));
                sequence.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.5f));
                break;
            case 1:
                Sequence sequence1 = DOTween.Sequence();
                sequence1.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one * 0.5f, 0.1f));
                sequence1.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.1f)).OnComplete(() => 
                {
                    electric_ball_pattern.muzzle.SetActive(true);
                });
                break;
            case 2:
                foreach (var item in electric_ball_pattern.muzzles_trans)
                {
                    GameObject bullet = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Electric_bullet"));
                    Electric_bullet_obj electric_bullet = bullet.GetComponent<Electric_bullet_obj>();
                    if (electric_bullet == null)
                    {
                        electric_bullet = bullet.AddComponent<Electric_bullet_obj>();
                        electric_bullet.bullet_speed = electric_ball_pattern.speed;
                        electric_bullet.bullet_push_time = electric_ball_pattern.push_time;

                    }
                    bullet.transform.position = item.position;
                    bullet.transform.rotation = item.rotation;
                }
                break;
            case 3:
                Sequence sequence2 = DOTween.Sequence();

                sequence2.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one * 0.5f, 0.1f)).OnComplete(() =>
                {
                    electric_ball_pattern.muzzle.SetActive(false);
                }); ;
                sequence2.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.1f));
                //지속시간 0.5초
                break;
            case 4:
                electric_ball_pattern.electric_line.SetActive(true);
                break;
            case 5:
                electric_ball_pattern.ball.DOScale(new Vector3(electric_ball_pattern.ball.localScale.x + 0.1f, electric_ball_pattern.ball.localScale.y + 0.1f, electric_ball_pattern.ball.localScale.z), 0.2f);
                break;
            case 6:
                electric_ball_pattern.electric_line.SetActive(false);
                break;
            case 7:
                electric_ball_pattern.electric_ball_obj.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void Lightning()
    {
        switch (lightning.pattern_data[lightning.pattern_count].action_num)
        {
            case 0:
                GameObject single_lightning = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Single_lightning"));
                single_lightning.transform.position = new Vector3(lightning.single_pos_x[lightning.single_count], 0.5f, 0);
                if(lightning.single_count != lightning.single_pos_x.Length - 1)
                {
                    lightning.single_count++;
                }
                break;
            case 1:
                GameObject board_lightning = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Broad_lightning"));
                board_lightning.transform.position = new Vector3(lightning.board_pos_x[lightning.board_count], 0.5f, 0);
                if (lightning.board_count != lightning.board_pos_x.Length - 1)
                {
                    lightning.board_count++;
                }
                break;
            default:
                break;
        }
        lightning.pattern_count++;
        if(lightning.pattern_data.Count == lightning.pattern_count)
        {
            lightning.pattern_ending = true;
        }
    }
    public void Phase_2()
    {
        switch (phase_2.pattern_data[phase_2.pattern_count].action_num)
        {
            case 0:
                transform.position = new Vector3(1.5f, 8, 0);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - Time.fixedDeltaTime * phase_2.speed, 2.7f, 8), 0);
                break;
            case 2:
            Anim_state_machin(anim_state["hard_pattern1"]);
                break;
            case 3:
            Anim_state_machin(anim_state["2_phase_idle"]);
                Managers.Main_camera.Shake_move();
                break;
            default:
                break;
        }
    }
    [Serializable]
    public class Lightning_pattern : Pattern_base_data
    {
        [Header("단일 번개 x 값 순서대로 할당")]
        public float[] single_pos_x;
        [Header("광범위 번개 x 값 순서대로 할당")]
        public float[] board_pos_x;
        [HideInInspector]
        public sbyte single_count;
        [HideInInspector]
        public sbyte board_count;
    }

    [Serializable]
    public class Rain_storm_pattern : Pattern_base_data
    {
        [Header("생성되는 높이")]
        public float pos_y;
        [Header("생성되는 x축 위치들")]
        public float[] pos_x;
        [Header("회전하는 속도")]
        public float rotation_speed;
        [Header("바람 방향이 바뀔 때 생성해야되는 위치가 원점으로 부터 몇이나 떨어져야되는지 할당")]
        public float pos_x_criteria_option;
        [HideInInspector]
        public sbyte pos_x_count;
        [HideInInspector]
        public List<Transform> rain_trans = new List<Transform>();
        [HideInInspector]
        public HashSet<string> rain_hash = new HashSet<string>();
        [HideInInspector]
        public Quaternion criteria;
        [HideInInspector]
        public float pos_x_critaria;
        [HideInInspector]
        public float time;
        
    }
    [Serializable]
    public class Shower_pattern : Pattern_base_data
    {
        [Header("이동 속도")]
        public float speed;
        [Header("소나기 오브젝트")]
        public GameObject shower_obj;
        
    }
    [Serializable]
    public class Rush_pattern : Pattern_base_data
    {
        [HideInInspector]
        public float pos_y;
        public int dir = 1;
        public float target_pos_x = -8f;
        public float init_pos_x = -12.5f;
        public float target_rush_pos_x = 18f;
        public float bullet_speed = 20f;
        public float bullet_roataion = 90;
        public bool rush_start = false;
        public float bullet_push_time = 5f;
        public Transform rain_muzzle;
        public GameObject[] bullets;
        public HashSet<string> temp;
        public int temp2 = 0;

    }
    [Serializable]
    public class Phase_2_pattern : Pattern_base_data
    {
        public float speed = 2;
    }
    [Serializable]
    public class Electric_ball_pattern : Pattern_base_data
    {
        public GameObject electric_ball_obj;
        public Transform ball;
        public GameObject muzzle;
        public GameObject electric_line;
        public Transform[] muzzles_trans;
        public float speed;
        public float push_time;
    }
    [Serializable]
    public class Electric_ball_rotation_pattern : Pattern_base_data
    {
        
        public Transform lightning_ball;
        [Header("전기탄 나갈 때의 시계반향 속도")]
        public float electric_bullet_rotation_speed;
        [Header("전기탄 나갈 때의 반시계반향 속도")]
        public float electric_bullet_reverse_rotation_speed;
    }
}
