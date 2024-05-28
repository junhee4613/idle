using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class Test_cs : MonoBehaviour
{
    public GameObject target;
    public float rot_speed;
    HashSet<int> test = new HashSet<int>();

    private void Awake()
    {
    }
    public void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            int num = Random.Range(0, 4);
            test.Add(num);
            Debug.Log(test.Count);
            Debug.Log(num);

        }
    }
    public void Update()
    {
    }
    
}


