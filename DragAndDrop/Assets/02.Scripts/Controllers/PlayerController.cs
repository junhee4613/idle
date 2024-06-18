using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : playerData
{
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    public float obj_size;
    public AudioClip bounce_sound;
    public GameObject player;
    public Transform character;
    [Header("플레이어 사이즈")]
    public float player_size;
    [Header("플레이어 사이즈 배율(1이 기본 값)")]
    public float player_size_magnification;
    [Header("드래그 할 때의 속도 및 크기 배율(1이 기본값)")]
    public float drag_dis_magnification = 1;
    public Animator animator;
    #region 클래스 안에서 해결할것들
    sbyte break_num = 0;
    public Vector2 mouse_current_pos;
    public Vector2 mouse_click_pos;
    Vector2 drag_dis;
    Vector2 drag_min_shoot_dir;
    public bool hit_statu = false;
    float time;
    public float player_rotation_z;
    float shoot_power_range;
    Managers Managers => Managers.instance;                 //지금 드래그 상태일 때 발사가 안되는 버그 있음
    [SerializeField]
    public Player_statu player_statu = Player_statu.IDLE;
    public Collider2D[] interation_obj;
    public ParticleSystem[] move_particles;
    public ParticleSystem wavelength;
    public ParticleSystem hit_particle;
    public Particle_figure move_fragments_figurel;
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.gameover += Player_die_setActive;
        move_fragments_figurel.module = move_particles[0].emission;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Managers.GameManager.option_window_on && Managers.GameManager.operate)
        {
            Interaction_obj();
            Key_operate();
        }
        if (Managers.GameManager.scene_name == "Main_screen")        //0 -18
        {
            if (transform.position.x > -9 && Managers.Main_camera.state != Camera_focus_state.CHAPTER2)
            {
                Managers.Main_camera.state = Camera_focus_state.CHAPTER2;
                Managers.Main_camera.Focus_move(new Vector3(0, 0, -10));
            }
            else if (transform.position.x < -9 && Managers.Main_camera.state != Camera_focus_state.CHAPTER1)
            {
                Managers.Main_camera.state = Camera_focus_state.CHAPTER1;
                Managers.Main_camera.Focus_move(new Vector3(-18, 0, -10));
            }
        }
    }
    public void Mouse_button_down()
    {
        player_statu = Player_statu.DRAG;
    }
    public void Player_Statu()
    {
        switch (player_statu)
        {
            case Player_statu.IDLE:
                break;
            case Player_statu.DRAG:
                mouse_current_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                wavelength.gameObject.SetActive(true);
                Drag();
                break;
            case Player_statu.RUN:
                wavelength.gameObject.SetActive(false);
                break;
            case Player_statu.HIT:
                break;
            default:
                break;
        }
    }
    private void FixedUpdate()      //감속은 여기서 처리해야 같은 범위까지만 움직여지는 것 같음
    {
        if (hit_statu)
        {
            time += Time.fixedDeltaTime;
            if (time > invincibility_time)
            {
                hit_statu = false;
                time = 0;
            }
        }
        Run();
    }
    public void Key_operate()
    {
        Player_Statu();

        if (Input.GetMouseButtonDown(0))    //순서 1번
        {
            shoot_dir_image.SetActive(true);
            mouse_click_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animator.Play("Drag_statu");
            Mouse_button_down();
        }
        if (Input.GetKeyDown(KeyCode.Space) && break_num == 1)
        {
            break_num = 0;
            rb.velocity = Vector2.zero;
        }
        if (rb.velocity == Vector2.zero && player_statu == Player_statu.RUN)
        {
            animator.Play("Idle");
            player_statu = Player_statu.IDLE;
        }
        if (player_statu != Player_statu.RUN)
        {
            animator.speed = 1;
        }
    }
    public void Drag()
    {
        drag_dis = new Vector3(mouse_click_pos.x - mouse_current_pos.x, mouse_click_pos.y - mouse_current_pos.y, 0) * drag_dis_magnification;
        if (drag_dis.sqrMagnitude > Vector2.one.sqrMagnitude * 0.5f)
        {
            player_rotation_z = Mathf.Atan2(drag_dis.normalized.y, drag_dis.normalized.x) * Mathf.Rad2Deg;
            shoot_power_range = Mathf.Clamp(drag_dis.magnitude, Mathf.Abs(slow_speed), Mathf.Abs(shoot_speed));
        }
        else
        {
            drag_min_shoot_dir = new Vector2(mouse_click_pos.x - transform.position.x, mouse_click_pos.y - transform.position.y).normalized;
            player_rotation_z = Mathf.Atan2(drag_min_shoot_dir.y, drag_min_shoot_dir.x) * Mathf.Rad2Deg;
            shoot_power_range = 3;
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
            animator.speed = 5f / shoot_power_range;
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            move_particles[0].Play();
            move_fragments_figurel.module.SetBurst(0, new ParticleSystem.Burst(0, 60 * shoot_power_range / Mathf.Abs(shoot_speed)));
            move_particles[1].Play();
            Drag_shoot();

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

    public void Hit()
    {
        if (!hit_statu)
        {
            //Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Hit_particle"));
            hit_particle.gameObject.SetActive(true);
            hit_particle.Play();
            hit_statu = true;
            if (!Managers.invincibility)
            {
                if (!Managers.GameManager.tutorial)
                {
                    player_life -= 1;
                }
                else if(!Managers.GameManager.tutorial_hit)
                {
                    Managers.GameManager.tutorial_hit = true;
                }
                //Managers.UI_jun.player_hp[player_life].SetActive(false);
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
    public void Interaction_obj()       //DEL : 나중에 불필요하면 제거
    {
        if (!hit_statu)
        {
            interation_obj = Physics2D.OverlapCircleAll(Managers.GameManager.Player_character.transform.position, cc.radius, 1 << 14);
            foreach (var item in interation_obj)
            {
                if (item.TryGetComponent<IInteraction_obj>(out IInteraction_obj interaction_obj))
                {
                    interaction_obj.practice();
                }
                else
                {
                    Hit();
                }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Managers.GameManager.option_window_on)
        {
            if (collision.TryGetComponent<IInteraction_obj>(out IInteraction_obj interaction_obj))
            {
                interaction_obj.practice();
            }
            else
            {
                Hit();
            } 
        }
    }
}