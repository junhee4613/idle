using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    [Header("스킬 범위")]
    public float slow_skill_range;
    [Header("슬로우 스킬 적용할 대상 레이어")]
    public LayerMask slow_skill_targets;
    public bool q_down;
    public Collider2D[] targets;
    public GameObject Test2;
    public Button init_button;
    public HashSet<Collider2D> slow_Obstacle = new HashSet<Collider2D>();
    // Start is called before the first frame update
    private void Awake()
    {
        init_button.onClick.AddListener(() => {
            slow_Obstacle.Clear();
        });
    }
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skill()
    {
        Key_Press();
        if (q_down)
        {
            targets = Physics2D.OverlapCircleAll(transform.position, slow_skill_range, slow_skill_targets);
            foreach (var item in targets)
            {
                Debug.Log("적용");
                if (!slow_Obstacle.Contains(item))
                {
                    Debug.Log("적용");
                    if (item.TryGetComponent<slow_eligibility>(out slow_eligibility target))
                    {
                        Debug.Log("적용");
                        target.Slow_apply();
                    }
                    slow_Obstacle.Add(item);
                    item.gameObject.layer = 11;
                }
            }
            //이거 장애물 자체에 컴포넌트를 넣을지 아니면 플레이어에게 놔둘지 고민중인데 
            //플레이어에게 놔둘거면 Getcomponent하지말고 다른 방법 어디 리스트에 넣어서 그 리스트 안에 있는 것들에게만 속도 감속 시키는거?
            //장애물에게 넣으면 Physics2D가 너무 많아질수도 있다는걸 명심해야됨
        }
        /*else if
        {

        }*/
    }
    public void Key_Press()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Test2.SetActive(true);
            q_down = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            Test2.SetActive(false);
            q_down = false;
        }
    }
}
