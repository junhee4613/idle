using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Unit
{
    //보니까 addForce랑 velocity랑 값을 대입해줘서 실행되는 원리가 다른 것 같음 velrocity로 지정하면 바운스 머테리얼 넣은 채로 벽에 닿았을 때 튕겨지지가 않고 반대로 addForce는 머테리얼을 안넣고 로직으로 처리하면 작동이 안됨
    //Drag_statu_walls_collider(); 이 함수가 실행이 생각대로 안됨
    // 스킬을 구현해야되는데 이거 어떻게 데이터를 정리할지 고민중 스킬들의 정보는 json으로 저장
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    [Header("날라갈 때의 초기 속도")]
    public float shoot_speed;
    [Header("드래그 할 때의 속도")]
    public float slow_speed;
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
    public Vector2 drag_before_dir;
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
        if (Managers.developer_mode)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit(1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Time.timeScale = 0.2f;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                Time.timeScale = 1;
            }
        }
        
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            drag_before_dir = rb.velocity.normalized;
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
        Drag_statu_walls_collider();
        if (speed != slow_speed)
        {
            speed = slow_speed;
            rb.velocity = drag_before_dir * speed;
        }
        shoot_dir_image.SetActive(true);
        player_pos = ray_hit.collider.gameObject.transform.position;
        drag_dis = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); 
        player_move_dir = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0).normalized;
        player_rotation_z = Mathf.Atan2(player_move_dir.y, player_move_dir.x) * Mathf.Rad2Deg;
        arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        if (Input.GetMouseButtonUp(0))
        {
            shoot_dir_image.SetActive(false);
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
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

    public void Drag_statu_walls_collider()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, cc.radius, 1 << 9);
        for (int i = 0; i < targets.Length; i++)
        {
            switch (targets[i].tag)
            {
                case "Horizontal":
                    rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
                        break;
                case "Virtical":
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
                    break;
                default:
                    break;
            }
        }
        
    }
    
}
