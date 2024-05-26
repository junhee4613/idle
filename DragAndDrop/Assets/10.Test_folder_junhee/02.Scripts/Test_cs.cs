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
    private void Awake()
    {
        
    }
    public void FixedUpdate()
    {

    }
    public void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Max(transform.rotation.z + Time.deltaTime, Look_at_target(target.transform.position))));
    }
    public float Look_at_target(Vector3 target)
    {
        float temp_rot = Mathf.Atan2(transform.position.y - target.y, transform.position.x - target.x) * Mathf.Rad2Deg + 90;
        return temp_rot;
    }
}


