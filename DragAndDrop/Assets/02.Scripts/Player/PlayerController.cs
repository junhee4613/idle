using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Unit
{
    
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    public float speed;
    public int player_life = 3;
    public float speed_down;
    #region 클래스 안에서 해결할것들
    Vector2 mouse_pos;
    Vector2 player_pos; 
    Vector2 mouse_button_up_pos;
    public float player_move_pos;
    public Vector2 player_move_dir;
    bool player_sence = false;
    RaycastHit2D ray_hit;
    Player_statu player_statu = Player_statu.IDLE;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButton(0)) //순서 2번 이거 실행 도중에 버튼 누렀을 때 실행되는 함수가 실행이 됨
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))    //순서 1번
            {
                Mouse_button_down();
            }
            
        }
        else if (Input.GetMouseButtonUp(0) && player_sence)     //순서 3번
        {
            Mouse_button_up();
        }

        Player_Statu();
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            player_pos = ray_hit.collider.gameObject.transform.position;
            player_sence = true;
        }
        else
        {
            player_sence = false;
        }
    }
    public void Mouse_button_up()
    {
        Debug.Log("버튼 뗌");
        player_move_pos = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); // - 마우스를 땐 위치의 크기를 가져와서 그만큼 뺀다
        player_move_dir = new Vector2(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y).normalized;
        rb.AddForce(player_move_dir * player_move_pos * speed, ForceMode2D.Impulse);
        player_statu = Player_statu.RUN;

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

    public void Run()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime * speed_down, 0, rb.velocity.x), Mathf.Clamp(rb.velocity.y - Time.deltaTime * speed_down, 0, rb.velocity.y));
    }
}
//플레이어가 날아갈 크기랑 방향 정하기 nomalize랑 magnitude를 사용
//좌표 값을 가져오는 걸 클릭한 좌표를 가져올 건지 플레이어 좌표를 가져올 건지 결정
//플레이어 드래그 표시를 어떻게 할지 결정
//드래그 차징이 얼마나 됐는지 표현할 방법
//그외 효과같은건 어떻게 줄지 결정
//skwnddp wpwkr
