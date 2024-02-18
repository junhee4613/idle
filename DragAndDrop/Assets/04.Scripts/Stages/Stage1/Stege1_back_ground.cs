using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stage_FSM;
using DG.Tweening;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Stege1_back_ground : Background_controller
{
    public Middle_effect_objs middles;
    public Back_ground_pattern back_ground_pattern;
    protected override void Awake()
    {
        base.Awake();
        back_ground_pattern.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage_1_back_ground_data").text);
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
        if (!back_ground_pattern.pattern_ending)
        {
            if (back_ground_pattern.pattern_data[back_ground_pattern.pattern_count].time <= Managers.Sound.bgSound.time)
            {
                Pattern_action_num();
            } 
        }
    }
    void Pattern_action_num()
    {
        switch (back_ground_pattern.pattern_data[back_ground_pattern.pattern_count].action_num)
        {
            case 0:
                back_ground_pattern.dark_clouds.SetActive(true);
                back_ground_pattern.dark_clouds.GetComponent<SpriteRenderer>().DOFade(1, 7).OnComplete(() => 
                { middles.clouds.SetActive(false); });
                break;
            case 1:
                foreach (var item in back_ground_pattern.thunders)
                {
                    item.gameObject.SetActive(true);
                    item.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
                }
                break;
            default:
                break;
        }
        back_ground_pattern.pattern_count++;
        if(back_ground_pattern.pattern_data.Count == back_ground_pattern.pattern_count)
        {
            back_ground_pattern.pattern_ending = true;
        }
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
    [System.Serializable]
    public class Back_ground_pattern : Pattern_base_data
    {
        public GameObject dark_clouds;
        public GameObject[] thunders;
    }
}
