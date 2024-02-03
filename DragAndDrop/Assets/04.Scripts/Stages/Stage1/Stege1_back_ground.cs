using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stage_FSM;
using DG.Tweening;
using UnityEngine.UI;
public class Stege1_back_ground : Background_controller
{
    public Middle_effect_objs middles;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Trail();
    }
    public void Trail()
    {
        middles.trail_image1.position = new Vector3(middles.trail_image1.position.x + Time.fixedDeltaTime, middles.trail_image1.position.y, middles.trail_image1.position.z);
        middles.trail_image2.position = new Vector3(middles.trail_image2.position.x + Time.fixedDeltaTime, middles.trail_image2.position.y, middles.trail_image2.position.z);
        middles.trail_image3.position = new Vector3(middles.trail_image3.position.x + Time.fixedDeltaTime, middles.trail_image3.position.y, middles.trail_image3.position.z);
       if (middles.trail_image1.position.x >= 15)
        {
            middles.trail_image1.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 1, 0);
            middles.trail_image1.position = new Vector3(-15, middles.trail_image1.position.y, transform.position.z);
        }
        if (middles.trail_image2.position.x >= 15)
        {
            middles.trail_image2.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 1, 0);
            middles.trail_image2.position = new Vector3(-15, middles.trail_image2.position.y, transform.position.z);
        }
        if(middles.trail_image3.position.x >= 15)
        {
            middles.trail_image3.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 1, 0);
            middles.trail_image3.position = new Vector3(-15, middles.trail_image3.position.y, transform.position.z);
        }
    }
    [System.Serializable]
    public class Middle_effect_objs
    {
        public Transform trail_image1;
        public Transform trail_image2;
        public Transform trail_image3;
        public GameObject clouds;
        public Sprite fade_in_out_image;
    }
}
