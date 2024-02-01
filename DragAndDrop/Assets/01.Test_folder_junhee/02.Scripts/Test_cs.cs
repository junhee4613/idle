using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_cs : MonoBehaviour
{
    public float test;
    private void Start()
    {
    }
    private void Update()
    {
        test += Time.deltaTime;
        Debug.Log(test);
    }
}


