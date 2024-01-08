using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Unit
{
    // 스킬을 구현해야되는데 이거 어떻게 데이터를 정리할지 고민중 스킬들의 정보는 json으로 저장
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    [Header("날라갈 때의 초기 속도")]
    public float shoot_speed;
    [Header("드래그 할 때의 속도")]
    public float slow_speed;
    [Header("이동할 때 감속되는 속도")]
    public float speed_down;
    [Header("무적 시간")]
    public float invincibility_time;
    public int player_life = 3;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    #region 클래스 안에서 해결할것들
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 player_move_dir;
    public bool hit_statu = false;
    float time;
    float speed;
    float drag_dis;
    float player_rotation_z;
    public Vector2 drag_before_speed;
    public Collider2D[] targets;
    Managers Managers => Managers.instance;
    RaycastHit2D ray_hit;
    public Player_statu player_statu = Player_statu.IDLE;
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.player = gameObject.GetComponent<PlayerController>();
        Managers.GameManager.gameover += Player_die_setActive;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.GameManager.player_die)
        {
            return;
        }
        Player_Statu();
        if (Input.GetMouseButton(0)) //순서 2번 이거 실행 도중에 버튼 누렀을 때 실행되는 함수가 실행이 됨
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))    //순서 1번
            {
                Mouse_button_down();
            }
        }
        if (Input.GetKey(KeyCode.H))
        {
            Hit(1);
        }
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            drag_before_speed = rb.velocity.normalized;
        }
    }
   
    public void Player_Statu()
    {
        switch (player_statu)
        {
            case Player_statu.IDLE:
                break;
            case Player_statu.DRAG:
                Drag();
                break;
            case Player_statu.RUN:
                Run();
                break;
            case Player_statu.HIT:
                break;
            case Player_statu.SKILLS:
                Skills();   //스킬이 키 별로 따로 있어서 굳이 이 상태를 안넣어도 될 것 같음
                break;
            default:
                break;
        }
    }
    public void Drag()
    {
        speed = slow_speed;
        if(rb.drag != 0)
        {
            rb.drag = 0;
            rb.velocity = drag_before_speed * speed;
        }
        shoot_dir_image.SetActive(true);
        player_pos = ray_hit.collider.gameObject.transform.position;
        drag_dis = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); 
        player_move_dir = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0).normalized;
        player_rotation_z = Mathf.Atan2(player_move_dir.y, player_move_dir.x) * Mathf.Rad2Deg;
        arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        if (Input.GetMouseButtonUp(0))     //순서 3번
        {
            shoot_dir_image.SetActive(false);
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            rb.drag = speed_down;
            speed = shoot_speed;
            Drag_shoot();
        }
    }
    public void Drag_shoot()
    {
        rb.AddForce(transform.up * drag_dis * speed, ForceMode2D.Impulse);
        player_statu = Player_statu.RUN;
    }
    public void Run()
    {
        //이 상태 빼도 될것같음
    }
    public void Skills()
    {

    }
    
    public override void Hit(float damage)
    {
        if (!hit_statu && !Managers.developer_mode)
        {
            hit_statu = true;
            player_life -= (int)damage;
            if (player_life <= 0)
            {
                Managers.GameManager.player_die = true;
                Managers.GameManager.gameover();
                return;
            }
            StartCoroutine(invincibility());
        }
    }
    IEnumerator invincibility()
    {
        if(player_life != 0)
        {
            transform.DOPunchScale(Vector2.one * 1.5f, 2f, 3, 0.5f);
        }

        while (time < invincibility_time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        hit_statu = false;
        time = 0;
    }
    public void Player_die_setActive()
    {
        gameObject.SetActive(false);
        Managers.GameManager.gameover -= Player_die_setActive;
    }
    
}
