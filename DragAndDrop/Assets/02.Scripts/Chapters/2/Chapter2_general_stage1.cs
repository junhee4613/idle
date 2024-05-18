using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class Chapter2_general_stage1 : BossController
{
    public Cactus_climb_up cactus_climb_up;
    List<GameObject> general_warning_obj = new List<GameObject>();
    public Color warning_color;
    public Cactus_Barrage cactus_barrage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Pattern_processing()
    {
        Pattern_function(ref cactus_climb_up.pattern_data, ref cactus_climb_up.pattern_ending, ref cactus_climb_up.duration,ref cactus_climb_up.pattern_count, Cactus_climb_up_pattern);
        Pattern_function(ref cactus_barrage.pattern_data, ref cactus_barrage.pattern_ending, ref cactus_barrage.duration,ref cactus_barrage.pattern_count, Cactus_barrage_pattern);
    }
    void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:             //선인장 하나 생성 둥 때 나옴
                //먼저 경고 장판 생성한뒤
                cactus_climb_up.index_num = (sbyte)Random.Range(1, 4);
                general_warning_obj[general_warning_obj.Count] = General_warning_box(new Vector3(cactus_climb_up.cactus_size[cactus_climb_up.index_num], 0.1f, 0), new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0), warning_color);
                break;
            case 1:
                general_warning_obj[general_warning_obj.Count].SetActive(false);
                general_warning_obj.RemoveAt(0);
                GameObject cactus_obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("여기에 나중에 선인장"));
                cactus_obj.transform.position = new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0);
                cactus_obj.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
                {
                    cactus_obj.transform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                    {
                        //선인장 팔 부분 dotween으로 효과 준 후 그 효과가 끝나면 List 안에 넣기
                        cactus_obj.transform.DOMoveY(8, cactus_climb_up.speed).SetEase(Ease.Linear).OnComplete(() => 
                        {
                            cactus_obj.transform.localScale = Vector2.zero;
                            //여기에 팔도 스케일 값 조정해주는 로직 넣어주기
                            Managers.Pool.Push(cactus_obj);
                        });
                    });
                });
                break;
        }
    }
    void Cactus_barrage_pattern()
    {
        switch (cactus_barrage.pattern_data[cactus_barrage.pattern_count].action_num)
        {
            case 0:             //선인장 등장
                cactus_barrage.cactus = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("탄막 선인장"));
                cactus_barrage.cactus.transform.position = new Vector3(0, 0, 0);
                cactus_barrage.cactus.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                cactus_barrage.cactus.transform.DOScaleY(10, 0).OnComplete(() =>
                {
                    cactus_barrage.cactus.transform.DOScaleY(10, 0);
                });
                break;
            case 1:     //열매 떨어트림
                 Managers.Pool.Pop(Managers.Resource.Load<GameObject>("선인장 열매")).transform.position = 
                    cactus_barrage.fruit_drop_ready[Random.Range(0, cactus_barrage.fruit_drop_ready.Length)].transform.position;
                break;
            case 2:     //선인장 사라짐
                cactus_barrage.cactus.transform.DOScaleY(10, 0).OnComplete(() =>
                {
                    cactus_barrage.cactus.transform.DOScaleY(10, 0);
                });
                break;
        }
    }
    [Serializable]
    public class Cactus_climb_up : Pattern_base_data
    {
        public float speed;         //선인장 움직이는 속도
        public float[] cactus_size;     //움직이는 선인장 사이즈들
        public int appearance_dir = 0;      //선인장 등장하는 방향
        public int appearance_height = 0;      //선인장 등장하는 높이
        public sbyte index_num;             //몇번째 선인장을 가져올건지 고를 변수
    }
    [Serializable]
    public class Cactus_Barrage : Pattern_base_data 
    {
        public GameObject[] cactus_fruit;
        public GameObject[] fruit_drop_ready;
        public float barrage_bullet_num;
        public GameObject cactus;

    }

}
