using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using DG.Tweening;
using System;
public class Test_cs : MonoBehaviour
{
    public float angle = 0.5f;
    public float speed = 5;
    public Transform left_hand;
    private void Awake()
    {

    }
    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            left_hand.DOJump(Vector3.zero, 5, 1, 1f).SetEase(Ease.InSine);
        }
        /*if(left_hand.position.y >= -3f)
        {
            Mathf.Clamp(angle -= Time.fixedDeltaTime, -5, 0.5f);
            left_hand.position = new Vector3(left_hand.position.x + Time.fixedDeltaTime * 3, left_hand.position.y + angle * Time.fixedDeltaTime * 18);
            left_hand.Rotate(new Vector3(0, 0, 360) * Time.fixedDeltaTime);
        }*/
    }
    public void Update()
    {
       
    }
}


