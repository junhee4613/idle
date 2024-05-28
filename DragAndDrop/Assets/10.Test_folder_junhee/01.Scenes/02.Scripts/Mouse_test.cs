using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("누르는 중");
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("누름");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("뗌");
        }
    }
}
