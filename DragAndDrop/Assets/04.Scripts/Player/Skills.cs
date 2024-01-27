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
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Skill();
    }
    public void FixedUpdate()
    {
    }
    public void Skill()
    {
        Key_Press();
        if (q_down)
        {
            Managers.GameManager.game_speed = slow_speed;
            Time.timeScale = slow_speed;
            Time.fixedDeltaTime = Time.deltaTime * slow_speed;
        }
        else
        {
            Time.fixedDeltaTime = Time.deltaTime;
            Time.timeScale = 1f;
            Managers.GameManager.game_speed = 1;
        }
    }
    public void Key_Press()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Test2.SetActive(true);
            q_down = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            //Test2.SetActive(false);
            q_down = false;
        }
    }
}
