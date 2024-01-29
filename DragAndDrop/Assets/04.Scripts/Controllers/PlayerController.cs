using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : playerData
{
    //벽의 옆면에 맞으면 다른 벽
    //최저 속도는 플레이어가 드래그 상태일 때의 속도로 기준을 잡고 최고 속도는 
    //보니까 addForce랑 velocity랑 값을 대입해줘서 실행되는 원리가 다른 것 같음 velrocity로 지정하면 바운스 머테리얼 넣은 채로 벽에 닿았을 때 튕겨지지가 않고 반대로 addForce는 머테리얼을 안넣고 로직으로 처리하면 작동이 안됨
    //Drag_statu_walls_collider(); 이 함수가 실행이 생각대로 안됨
    // 스킬을 구현해야되는데 이거 어떻게 데이터를 정리할지 고민중 스킬들의 정보는 json으로 저장
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    public float obj_size;

    #region 클래스 안에서 해결할것들
    sbyte break_num = 0;
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 drag_dis;
    bool hit_statu = false;
    float time;
    float player_rotation_z;
    float shoot_power_range;
    Vector2 spacebar_dir;
    float spacebar_mag;
    //string wall_name;
    //public Collider2D[] walls_sence;
    Managers Managers => Managers.instance;                 //지금 드래그 상태일 때 발사가 안되는 버그 있음
    [SerializeField]
    RaycastHit2D ray_hit;
    Ray mouse_pos_ray_pos;
    public Player_statu player_statu = Player_statu.IDLE;
    public Collider2D[] interation_obj;
    #endregion
    #region 테스트용
    //[Header("테스트용")]
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.gameover += Player_die_setActive;
    }
    public void Player_slow_skill_down()
    {
        //rb.velocity
    }
    public void Player_slow_skill_up()
    {

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
        //Drag_statu_walls_collider();
        Interaction_obj();
        Key_operate();
        #region 여기부턴 개발자용 
        /*if (Managers.developer_mode)
        {
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                Time.timeScale = 0.2f;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit(1);
            }
        }*/

        #endregion
    }
    public void Mouse_button_down()
    {
        //wall_name = string.Empty;
        player_statu = Player_statu.DRAG;
    }
   
    public void Player_Statu()
    {
        switch (player_statu)
        {
            case Player_statu.IDLE:
                break;
            case Player_statu.DRAG:
                mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Drag();
                
                break;
            case Player_statu.RUN:
                break;
            case Player_statu.HIT:
                break;
            default:
                break;
        }
    }
    private void FixedUpdate()      //감속은 여기서 처리해야 같은 범위까지만 움직여지는 것 같음
    {
        Run();
    }
    public void Key_operate()
    {
        Player_Statu();
        
        if (Input.GetMouseButtonDown(0))    //순서 1번
        {
            mouse_pos_ray_pos = Camera.main.ScreenPointToRay(Input.mousePosition);
            ray_hit = Physics2D.Raycast(mouse_pos_ray_pos.origin, mouse_pos_ray_pos.direction, 1, 1 << 6);
            if (ray_hit.collider != null)
            {
                shoot_dir_image.SetActive(true);
                player_pos = ray_hit.collider.gameObject.transform.position;
                Mouse_button_down();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && break_num == 1)
        {
            break_num = 0;
            spacebar_dir = rb.velocity.normalized;
            spacebar_mag = rb.velocity.magnitude;
            rb.velocity = spacebar_dir * Mathf.Clamp(spacebar_mag - 4, 0, shoot_speed);
            if (rb.velocity == Vector2.zero)
            {
                player_statu = Player_statu.IDLE;
            }
        }
    }
    public void Drag()
    {
        if (ray_hit.collider != null)
        {
            drag_dis = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0);
            player_rotation_z = Mathf.Atan2(drag_dis.normalized.y, drag_dis.normalized.x) * Mathf.Rad2Deg;
            arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90); 
            if (Input.GetMouseButtonUp(0))      //여기에 누르는 동안 시간이 흘러가고 너무 많이 흐르면 피해를 입는 로직으로 일단 추가
            {
                if (break_num == 0)
                {
                    break_num = 1;
                }
                shoot_dir_image.SetActive(false);
                shoot_power_range = Mathf.Clamp(drag_dis.magnitude, Mathf.Abs(slow_speed), Mathf.Abs(shoot_speed));
                transform.rotation = arrow_rotation_base.rotation;
                rb.velocity = Vector2.zero;
                Drag_shoot();
            }
        }
    }
    public void Drag_shoot()
    {
        rb.velocity = transform.up * shoot_power_range;
        player_statu = Player_statu.RUN;
    }
    public void Run()
    {
        rb.velocity = rb.velocity.normalized * (Mathf.Clamp(rb.velocity.magnitude - Time.fixedDeltaTime * gradually_down_speed, 0, shoot_speed));
    }
    
    public override void Hit(float damage)
    {
        if (!hit_statu)
        {
            hit_statu = true;
            if (!Managers.invincibility)
                player_life -= (byte)damage;

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
            time += Time.fixedDeltaTime;
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
    public void Interaction_obj()
    {
        interation_obj = Physics2D.OverlapCircleAll(transform.position, obj_size, 1 << 8);
        
        foreach (var item in interation_obj)
        {
            if(item.TryGetComponent<IInteraction_obj>(out IInteraction_obj interaction))
            {
                interaction.practice();
            }
        }
        
    }

    /*public void Drag_statu_walls_collider()
    {
        walls_sence = Physics2D.OverlapCircleAll(transform.position, cc.radius, 1 << 9);
        for (int i = 0; i < walls_sence.Length; i++)
        {
            switch (walls_sence[i].tag)
            {
                case "Virtical":
                    if (wall_name != walls_sence[i].name)
                    {
                        rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
                        wall_name = walls_sence[i].name;
                    }
                    break;
                case "Horizontal":
                    if (wall_name != walls_sence[i].name)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
                        wall_name = walls_sence[i].name;
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }*/
    
}
