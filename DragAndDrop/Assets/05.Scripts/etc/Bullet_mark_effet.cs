using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet_mark_effet : MonoBehaviour
{
    public float fade_time;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //FIX : 추후에 인터페이스 만들어서 효과 처리하는 걸로 변경
        sr.DOFade(0, fade_time).OnComplete(() => gameObject.SetActive(false));
    }
    private void OnEnable()
    {
        sr.DOFade(0, 0);
    }
}
