using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Test_cs : MonoBehaviour
{
    public Transform test_target;
    public float test_float = 0f;
    float Test_float
    {
        get { return test_float; }
        set 
        {
            if(test_float > 360)
            {
                test_float -= 360f;
            }
            test_float = value;
        }
    }
    private void Awake()
    {
    }
    public void FixedUpdate()
    {
        
        
    }
    public void Update()
    {
        Test_float += Time.deltaTime;
        transform.position = Target_diraction(transform.position, test_target.position);
        Debug.Log(Mathf.Sin(Test_float));


    }
    public Vector3 Target_diraction(Vector3 target, Vector3 follow_target)
    {
        target = new Vector3(Mathf.Clamp(target.x + follow_target.normalized.x, -Mathf.Abs(follow_target.x), Mathf.Abs(follow_target.x))
            , Mathf.Clamp(target.y + (follow_target.normalized.y + Mathf.Sin(Test_float) * 2) * Time.deltaTime, follow_target.normalized.y - 1, target.y + follow_target.normalized.y + 1), target.z);
        return target;
    }

}


