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
    //public List<Vector3> powder_keg_pos = new List<Vector3>();
    //public GameObject obj;
    //public int num;

    //public GameObject[] push;
    private void Awake()
    {
        Managers.Pool.Pop(new GameObject()).transform.parent = transform;
        Rigidbody2D rb = new Rigidbody2D();
        GameObject temp = rb.gameObject;
    }
    public void FixedUpdate()
    {

    }
    public void Update()
    {
        Debug.Log(Random.Range(1, 8));
    }
}


