using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    /*[Header("스킬 범위")]
    public float slow_skill_range;
    [Header("슬로우 스킬 적용할 대상 레이어")]
    public LayerMask slow_skill_targets;*/
    public AudioSource au;
    [Header("느려졌을 때 속도")]
    public float slow_speed = 0.2f;
    public bool q_down;
    //public Collider2D[] targets;
    //public GameObject Test2;
    public Button init_button;
    //public HashSet<Collider2D> slow_Obstacle = new HashSet<Collider2D>();
    // Start is called before the first frame update
    private void Awake()
    {
        /*init_button.onClick.AddListener(() => {
            slow_Obstacle.Clear();
        });*/
    }
    void Start()
    {
        if (au == null)
        {
            GameObject managers = GameObject.FindAnyObjectByType<Managers>().gameObject;
            if (managers.TryGetComponent<AudioSource>(out AudioSource temp_au))
            {
                au = temp_au;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Managers.GameManager.game_start)
        {
            Key_press();
        }
    }
    public void FixedUpdate()
    {
        Skill();
    }
    public void Skill()
    {
        if (!Managers.GameManager.game_stop)
        {
            if (q_down) 
            {
                if (Time.timeScale != slow_speed)
                {
                    Time.timeScale = slow_speed;
                    au.pitch = slow_speed;
                    Time.fixedDeltaTime = Time.deltaTime * slow_speed;
                }
            }
            else
            {
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.02f;
                    au.pitch = 1;
                }
            }
        }
        else
        {
            au.pitch = 0;
        }
    }
    public void Key_press()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            q_down = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            q_down = false;
        }
    }
    
}
