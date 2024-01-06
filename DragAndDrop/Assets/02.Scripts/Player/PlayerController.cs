using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public int player_life = 3;
    public float speed_down;
    public LayerMask collision_target;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    #region 클래스 안에서 해결할것들
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 player_move_dir;
    bool wall_collider;
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
    
   

    public override void Hit(float damage)
    {
        if(player_statu != Player_statu.HIT)
        {
            player_life -= (int)damage;
            player_statu = Player_statu.HIT;
        }
        
        //여기에 몇초동안 피격 받은 상태로 만들고 그에 따른 처리를 하면 됨
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
        rb.drag = 0;
        if(rb.velocity != Vector2.zero)
        {
            rb.velocity = drag_before_speed * speed;
            //지금 이 상태에서 벽에 부딪히면 낌 여기에 입사각과 반사각의 값을 넣는 식으로 해서 처리
        }
        shoot_dir_image.SetActive(true);
        drag_dis = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); // - 마우스를 땐 위치의 크기를 가져와서 그만큼 뺀다
        player_move_dir = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0).normalized;
        player_rotation_z = Mathf.Atan2(player_move_dir.y, player_move_dir.x) * Mathf.Rad2Deg;
        arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        if (Input.GetMouseButtonUp(0))     //순서 3번
        {
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            rb.drag = 1;
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
        /*if(Mathf.Abs(transform.rotation.eulerAngles.z) >= 90)
        {
            player_velocity_dir_y = -1;
            min_speed_y = -drag_dis * shoot_speed;
            max_speed_y = 0;
        }
        else
        {
            player_velocity_dir_y = 1;
            min_speed_y = 0;
            max_speed_y = drag_dis * shoot_speed;
        }

        if (transform.rotation.eulerAngles.z >= 0)
        {
            player_velocity_dir_x = -1;
            min_speed_x = -drag_dis * shoot_speed;
            max_speed_x = 0;
        }
        else
        {
            player_velocity_dir_x = 1;
            min_speed_x = 0;
            max_speed_x = drag_dis * shoot_speed;
        }
        shoot_dir_image.SetActive(false);
        if (rb.velocity.y <= 0)
        {
            Debug.Log(rb.velocity.y);
        }
        else if (rb.velocity.x <= 0)
        {
            Debug.Log(rb.velocity.x);
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime * speed_down * player_velocity_dir_x, -min_speed_x, max_speed_x), 
            Mathf.Clamp(rb.velocity.y - Time.deltaTime * speed_down * player_velocity_dir_y, -max_speed_x, min_speed_x));*/
        //if(속도가 0이 된다면 idle 상태로 바꿔주는 로직)
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
                    //장애물에 대한 처리
                    break;
                case "Special_zone":
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
            //입사각과 반사각을 구해 그 값을 이용하여 회전을 준다
        }
        //벽에 붙이힌다면
        //속력 * -1을 해주기 또는 회전을 줘서 반댓 방향으로 날아가게 하면서 y축 방향으로 전진함

    }
}
//플레이어가 날아갈 크기랑 방향 정하기 nomalize랑 magnitude를 사용
//좌표 값을 가져오는 걸 클릭한 좌표를 가져올 건지 플레이어 좌표를 가져올 건지 결정
//플레이어 드래그 표시를 어떻게 할지 결정
//드래그 차징이 얼마나 됐는지 표현할 방법
//그외 효과같은건 어떻게 줄지 결정
//skwnddp wpwkr
