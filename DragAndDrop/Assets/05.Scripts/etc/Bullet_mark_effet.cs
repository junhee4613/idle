using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet_mark_effet : MonoBehaviour
{
    float fade_time = 0.7f;
    SpriteRenderer sr;
    SpriteRenderer SR 
    {
        get
        {
            if(sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            return sr;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        /*SR.DOFade(0, 0.7f).OnComplete(() =>
        {
            Managers.Pool.Push(this.gameObject);
        }).SetDelay(0.3f);*/
    }

    // Update is called once per frame
    void Update()
    {
        //FIX : 추후에 인터페이스 만들어서 효과 처리하는 걸로 변경
        
    }
    private void OnEnable()
    {
        SR.color = new Color(0 ,0, 0, 1);
        Debug.Log(fade_time);
        SR.DOFade(0, fade_time).OnComplete(() =>
        {
            Managers.Pool.Push(this.gameObject);
        }).SetDelay(0.3f);
    }
}
