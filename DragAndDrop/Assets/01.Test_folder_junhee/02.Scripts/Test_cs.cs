using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Test_cs : MonoBehaviour
{
    public Transform test_target;
    float test_float = 0f;
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
        test_float += Time.deltaTime * 15f;

        transform.position = new Vector3(test_target.position.x, test_target.position.y, test_target.position.z).normalized * 5f;
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(test_float) * 5f, transform.position.z);
    }

}


