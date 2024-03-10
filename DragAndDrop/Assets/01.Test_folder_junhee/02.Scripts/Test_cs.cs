using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Test_cs : MonoBehaviour
{
    public ParticleSystem test_particle;

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
            test_particle.Play();
        }
    }

}


