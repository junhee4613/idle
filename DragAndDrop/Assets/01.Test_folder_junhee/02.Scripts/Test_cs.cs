using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using DG.Tweening;
public class Test_cs : MonoBehaviour
{
    public Transform test_target;
    public float test_float = 0f;
    public Vector3 target_diretion;
    public GameObject criteria_obj;
    public float criteria_x;
    public float criteria_y;
    bool transform_action = false;
    bool action_end = false;
    Vector3 start_pos;
    public float angle;
    public float criteria_dir_x = 1;
    public float criteria_dir_y = 1;
    /*float Test_float
    {
        get { return test_float; }
        set 
        {
            if(test_float > 2)
            {
                value -= 2;
                //여기선 -1이 찍히고
            }
            test_float = value;
            //모든 코드가 실행돼야 값이 적용됨

        }
    }*/
    private void Awake()
    {
        

    }
    public void FixedUpdate()
    {
        
        
    }
    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform_action = !transform_action;
            if (transform_action)
            {
                start_pos = transform.position;
                transform.DOLocalMove(test_target.position, 1);
                action_end = true;
            }
            else
            {
                transform.DOLocalMove(start_pos, 1).OnComplete(() => action_end = false);
            }
        }
        if(!action_end)
        {
            if (transform.localPosition.x == criteria_x || transform.localPosition.x == -criteria_x)
            {
                criteria_dir_x = -criteria_dir_x;
            }
            if (transform.localPosition.y == criteria_y || transform.localPosition.y == -criteria_y)
            {
                criteria_dir_y = -criteria_dir_y;
            }
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x + Time.deltaTime * Mathf.Sin(45 * Mathf.Deg2Rad) * criteria_dir_x, -criteria_x, criteria_x),
                Mathf.Clamp(transform.localPosition.y + Time.deltaTime * Mathf.Cos(315 * Mathf.Deg2Rad) * criteria_dir_y, -criteria_y, criteria_y));
        }
    }
        
    public void Test_1()
    {

    }
}


