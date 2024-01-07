using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Unit
{
    //지금 피직스 머테리얼로 하니까 튕겨지는 각도가 이상함
    //발사 방향을 y축으로 해놓고 벽에 부딪히면 날아온 각도에 따라 그에 맞는 반향으로 날아가게 해야(입사각과 반사각이 일정해야됨) 이렇게 해서 velocity를 줄이면 문제 없어짐
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    [Header("날라갈 때의 초기 속도")]
    public float shoot_speed;
    [Header("드래그 할 때의 속도")]
    public float slow_speed;
    public float speed_down;
    public int player_life = 3;
    public LayerMask collision_target;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    public float invincibility_time;
    #region 클래스 안에서 해결할것들
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 player_move_dir;
    bool wall_collider;
    public bool hit_statu = false;
    float time;
    float speed;
    float drag_dis;
    float player_rotation_z;
    public Vector2 drag_before_speed;
    public Collider2D[] targets;
    RaycastHit2D ray_hit;
    public Player_statu player_statu = Player_statu.IDLE;
    #endregion
    // Start is called before the first frame update
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
        targets = Physics2D.OverlapCircleAll(transform.position, cc.radius, collision_target);
        Collider_target();
       if (Input.GetMouseButton(0)) //순서 2번 이거 실행 도중에 버튼 누렀을 때 실행되는 함수가 실행이 됨
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))    //순서 1번
            {
                Mouse_button_down();
            }
        }
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            player_pos = ray_hit.collider.gameObject.transform.position;
            drag_before_speed = rb.velocity;
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
            rb.velocity = drag_before_speed * speed; //이거 속도 일정하게 바꾸기
        }
        shoot_dir_image.SetActive(true);
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
        
    }
    public void Collider_target()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            switch (targets[i].gameObject.tag)
            {
                case "Wall":
                    Wall();
                    //벽에 대한 처리
                    break;
                case "Obstacle":
                    Obstacle();
                    //장애물에 대한 처리
                    break;
                case "Special_zone":
                    Special_zone();
                    //히트영역이 아닌 곳에 대한 거 정리
                    break;
                default:
                    break;
            }
        }
        wall_collider = false;
        
    }
    public void Wall()
    {
        if (!wall_collider)
        {
            wall_collider = true;
        }
    }
    public void Obstacle()
    {
        if(!hit_statu)
        {
            Hit(1);
            Debug.Log("공격당함");
            //히트 당해도 효과는 받게 하기
        }
    }
    public override void Hit(float damage)
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
    public void Special_zone()
    {
        //여기엔 특정 영역에서 얻을 수 있는 능력들 넣기
    }
}
