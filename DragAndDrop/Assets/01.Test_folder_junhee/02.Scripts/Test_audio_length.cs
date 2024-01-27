using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_audio_length : MonoBehaviour
{
    public AudioClip clip;
    public GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        test.transform.localScale = new Vector2(clip.length, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.left * Time.fixedDeltaTime;
    }

}