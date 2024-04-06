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
    public List<Vector3> powder_keg_pos = new List<Vector3>();
    public GameObject obj;
    public int num;

    public GameObject[] push;
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
            num = Random.Range(0, powder_keg_pos.Count - 1);
            Instantiate(obj).transform.position = powder_keg_pos[num];
            powder_keg_pos.RemoveAt(num);
        }
    }
}


