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
    public Fruit_Barrage fruit_barrage;
    public Cactus_thorn cactus_thorn;

    // Start is called before the first frame update
    void Start()
    {
        fruit_barrage.cactus = Managers.Resource.Load<GameObject>("Fruit_cactus");
        Debug.Log(fruit_barrage.cactus.name);
        for (int i = 0; i < fruit_barrage.cactus.transform.childCount; i++)
        {
            for (int j = 0; j < fruit_barrage.cactus.transform.GetChild(i).childCount; j++)
            {
                fruit_barrage.fruit_spawner_pos[] = fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject;
                Debug.Log(fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject.name);
            }
        }
    }

    // Update is called once per frame
    
    public override void Pattern_processing()
    {
        Pattern_function(ref cactus_climb_up.pattern_data, ref cactus_climb_up.pattern_ending, ref cactus_climb_up.duration,ref cactus_climb_up.pattern_count, Cactus_climb_up_pattern);
        Pattern_function(ref fruit_barrage.pattern_data, ref fruit_barrage.pattern_ending, ref fruit_barrage.duration,ref fruit_barrage.pattern_count, Fruit_barrage_pattern);
        if(fruit_barrage.fruit == null/*여기에 열매가 날 자리들 중 빈자리가 있는지 확인*/)
        {
            fruit_barrage.fruit = Managers.Resource.Load<GameObject>("선인장 열매");
        }
        fruit_barrage.fruit.transform.position = fruit_barrage.fruit_spawner_pos[Random.Range(0, fruit_barrage.fruit_spawner_pos.Length)].transform.position;
        Managers.Pool.Pop(fruit_barrage.fruit);
    }
    void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:             //선인장 하나 생성 둥 때 나옴
                cactus_climb_up.index_num = (sbyte)Random.Range(1, 4);
                general_warning_obj[general_warning_obj.Count] = General_warning_box(new Vector3(cactus_climb_up.cactus_size[cactus_climb_up.index_num], 0.1f, 0), new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0), warning_color);
                break;
            case 1:
                //랜덤한 선인장 하나 소환해서 위로 이동시키기
                general_warning_obj[general_warning_obj.Count].SetActive(false);
                general_warning_obj.RemoveAt(0);
                GameObject cactus_obj = Managers.Resource.Load<GameObject>("여기에 나중에 선인장");
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
    void Fruit_barrage_pattern()
    {
        switch (fruit_barrage.pattern_data[fruit_barrage.pattern_count].action_num)
        {
            case 0:             //선인장 등장

                fruit_barrage.cactus = Managers.Resource.Load<GameObject>("탄막 선인장");
                fruit_barrage.cactus.transform.position = new Vector3(0, 0, 0);
                fruit_barrage.cactus.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                Managers.Pool.Pop(fruit_barrage.cactus);
                fruit_barrage.cactus.transform.DOScaleY(10, 0).OnComplete(() =>
                {
                    fruit_barrage.cactus.transform.DOScaleY(10, 0);
                });
                break;
            case 1:     //열매 떨어트림
                fruit_barrage.random_num = Random.Range(0, fruit_barrage.fully_grown.Count);

                if (fruit_barrage.fully_grown[fruit_barrage.random_num].TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.gravityScale = 1;
                    fruit_barrage.fully_grown.Remove(rb.gameObject);
                }
                break;
            case 2:
                // 열매 터짐
                Managers.Pool.Push(fruit_barrage.fully_grown[fruit_barrage.random_num]);
                fruit_barrage.fully_grown.RemoveAt(fruit_barrage.random_num);
                //여기에 탄막 생성하는 로직 추가
                break;
            case 3:     //선인장 사라짐
                fruit_barrage.cactus.transform.DOScaleY(10, 0).OnComplete(() =>
                {
                    fruit_barrage.cactus.transform.DOScaleY(10, 0);
                });
                break;
        }
    }
    void Cactus_thorn_pattern()
    {

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
    public class Fruit_Barrage : Pattern_base_data 
    {
        public List<GameObject> fruit_spawner_pos = new List<GameObject>();
        public float barrage_bullet_num;
        public GameObject cactus;
        public List<GameObject> fully_grown = new List<GameObject>();
        public int random_num;
        public GameObject fruit;
    }
    [Serializable]
    public class Cactus_thorn : Pattern_base_data
    {

    }

}
