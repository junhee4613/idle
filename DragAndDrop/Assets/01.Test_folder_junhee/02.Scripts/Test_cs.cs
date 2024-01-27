using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_cs : MonoBehaviour
{
    public RaycastHit2D note_hit;
    public LayerMask hit_layers;
    public GameObject target;
    float beat_time;
    public AudioSource au;
    float beat;
    public AudioClip clip;
    bool pattern_start;
    private void Update()
    {
        if (au.clip != clip)
        {
            au.clip = clip;
            au.Play();
        }
        beat_time += Time.fixedDeltaTime;
        note_hit = Physics2D.Raycast(transform.position, Vector2.up, 5f, hit_layers);
        if (!pattern_start)
        {
            pattern_start = true;
            if (note_hit)
            {
                Debug.Log("ÀÎ½Ä");
            }
            if (note_hit && beat <= beat_time)
            {
                if (note_hit.collider.gameObject.layer == 20)
                {
                    Pattern1();
                }
                else if (note_hit.collider.gameObject.layer == 21)
                {
                    Pattern2();
                }
                else if (note_hit.collider.gameObject.layer == 22)
                {
                    Pattern3();
                }
            } 
        }
        if (!note_hit)
        {
            pattern_start = false;
        }
    }
    public void Pattern1()
    {
        target.transform.localScale = Vector2.one * 0.5f;
    }
    public void Pattern2()
    {
        target.transform.localScale = Vector2.one;
    }
    public void Pattern3()
    {
        target.transform.Rotate(0, 0, 15);
    }
}


