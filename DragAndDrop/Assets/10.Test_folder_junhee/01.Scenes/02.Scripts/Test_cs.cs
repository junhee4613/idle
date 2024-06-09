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
    float rot_speed = 2;
    public sbyte num = 1;
    HashSet<int> test = new HashSet<int>();

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
            transform.DOMoveX(rot_speed * num, 1).SetEase(Ease.OutQuad);
            num *= -1;
        }
    }
    
}


