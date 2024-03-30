using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : playerData
{
    public Rigidbody2D rb;
    public CapsuleCollider2D cc;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    public float obj_size;
    public AudioClip bounce_sound;
    public GameObject player;
    public Transform character;
    [Header("드래그 할 때의 속도 및 크기 배율(1이 기본값)")]
    public float drag_dis_magnification = 1;
    public Animator animator;
    #region 클래스 안에서 해결할것들
    sbyte break_num = 0;
    public Vector2 mouse_current_pos;
    public Vector2 mouse_click_pos;
    public Vector2 drag_dis;
    Vector2 drag_min_shoot_dir;
    float time;
    float player_rotation_z;
    float shoot_power_range;
    Managers Managers => Managers.instance;                 //지금 드래그 상태일 때 발사가 안되는 버그 있음
    [SerializeField]
    public Player_statu player_statu = Player_statu.IDLE;
    public Collider2D[] interation_obj;
    public ParticleSystem move_fragments;
    public ParticleSystem move_wave;
    public ParticleSystem wavelength;
    public Particle_figure move_fragments_figurel;
    #endregion
    #region 테스트용
    //[Header("테스트용")]
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.gameover += Player_die_setActive;
        move_fragments_figurel.module = move_fragments.emission;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()           //게임 오브젝트의 상태 업데이트나 비물리적인 움직임은 여기서 처리하는게 좋음
    {
        if (!Managers.GameManager.option_window_on)
        {
            if(rb.velocity == Vector2.zero)
            {
                animator.speed = 1;
                if(player_statu != Player_statu.HIT)
                {
                    player_statu = Player_statu.IDLE;
                }
            }
            Key_operate();
            Drag();
            Player_Statu();


        }
    }
    public void Mouse_button_down()
    {
    }
    public void Player_Statu()
    {
        switch (player_statu)
        {
            case Player_statu.IDLE:
                Debug.Log("idle");
                animator.Play("Idle");
                Interaction_obj();
                break;
            case Player_statu.HIT:
                Debug.Log("hit");
                time += Time.deltaTime;
                if (time > invincibility_time)
                {
                    player_statu = Player_statu.HIT;
                    time = 0;
                }
                break;
            default:
                break;
        }
    }
    private void FixedUpdate()      //물리적인건 여기서 처리하는게 좋음
    {
            Run();
    }
    public void Key_operate()
    {
        
        if (Input.GetMouseButtonDown(0))    //순서 1번
        {
            animator.speed = 1;
            shoot_dir_image.SetActive(true);
            mouse_click_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animator.Play("Drag_statu");
            wavelength.gameObject.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Space) && break_num == 1)
        {
            break_num = 0;
            rb.velocity = Vector2.zero;
            
        }
        
        
    }
    public void Drag()
    {
        mouse_current_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        drag_dis = new Vector3(mouse_click_pos.x - mouse_current_pos.x, mouse_click_pos.y - mouse_current_pos.y, 0) * drag_dis_magnification;
        if(drag_dis != Vector2.zero)
        {
            player_rotation_z = Mathf.Atan2(drag_dis.normalized.y, drag_dis.normalized.x) * Mathf.Rad2Deg;
        }
        else
        {
            drag_min_shoot_dir = new Vector2(mouse_click_pos.x - transform.position.x, mouse_click_pos.y - transform.position.y).normalized;
            player_rotation_z = Mathf.Atan2(drag_min_shoot_dir.y, drag_min_shoot_dir.x) * Mathf.Rad2Deg;
        }
        transform.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        animator.SetBool("Drag", true);
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("Shoot");
            animator.SetBool("Drag", false);
            if (break_num == 0)
            {
                break_num = 1;
            }
            shoot_dir_image.SetActive(false);
            shoot_power_range = Mathf.Clamp(drag_dis.magnitude, Mathf.Abs(slow_speed), Mathf.Abs(shoot_speed));
            animator.speed = 5f / shoot_power_range;
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            move_fragments.Play();
            move_fragments_figurel.module.SetBurst(0, new ParticleSystem.Burst(0, 60 * shoot_power_range / Mathf.Abs(shoot_speed)));
            move_wave.Play();
            Drag_shoot();
            
        }
    }
    public void Drag_shoot()
    {
        rb.velocity = transform.up * shoot_power_range;
        wavelength.gameObject.SetActive(false);

    }
    public void Run()
    {
        rb.velocity = rb.velocity.normalized * (Mathf.Clamp(rb.velocity.magnitude - Time.fixedDeltaTime * gradually_down_speed, 0, shoot_speed));
    }
    
    public void Hit()
    {
        if (player_statu != Player_statu.HIT)
        {
            player_statu = Player_statu.HIT;
            if (!Managers.invincibility)
            {
                player_life -= 1;
                Managers.UI_jun.player_hp[player_life].SetActive(false);
            }
            if (player_life <= 0)
            {
                Managers.GameManager.player_die = true;
                gameObject.SetActive(false);
                Managers.GameManager.gameover();
                return;
            }
        }
    }
    public void Player_die_setActive()
    {
        //플레이어 죽은 후 처리 애니메이션이 작동한다든지 등등
    }
    public void Interaction_obj()
    {
        interation_obj = Physics2D.OverlapCircleAll(transform.position, obj_size, 1 << 8);

        foreach (var item in interation_obj)
        {
            IInteraction_obj obj = item.GetComponent<IInteraction_obj>();
            if (obj != null)
            {
                obj.practice();
            }
            else
            {
                Debug.Log("공격 당함");
                Hit();
            }
        }
    }
    [System.Serializable]
    public class Particle_figure 
    {
        public ParticleSystem.EmissionModule module;
        public float origin_min_speed;
        public float origin_max_speed;
        public float origin_start_size;
        public float origin_life_time;
    }
}
