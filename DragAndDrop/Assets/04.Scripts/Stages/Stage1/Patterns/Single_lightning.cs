using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Single_lightning : MonoBehaviour
{
    //경고판은 a가 0부터 시작
    bool pattern_start = false;
    public SpriteRenderer warning_sprite;
    public SpriteRenderer lightning_image;
    public GameObject lightning_obj;
    public GameObject warring_obj;
    float time;
    private void FixedUpdate()
    {
        if (warring_obj.activeSelf)
        {
            if (!pattern_start)
            {

                pattern_start = true;
                warning_sprite.DOFade(0, 0.25f).SetLoops(3, LoopType.Yoyo).OnComplete(() =>
                {
                    warring_obj.SetActive(false);
                });
            }
        }
        else
        {
            if (!lightning_obj.activeSelf)
            {
                lightning_obj.SetActive(true);
            }
            time += Time.fixedDeltaTime;
            if (time < 0.2f)
            {
                lightning_obj.transform.localScale = new Vector3(lightning_obj.transform.localScale.x * -1, lightning_obj.transform.localScale.y, 0);
            }
            lightning_image.DOFade(0, 0.4f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
            {
                lightning_obj.SetActive(false);
                Managers.Pool.Push(this.gameObject);
            });
        }
    }
    private void OnEnable()
    {
        time = 0;
        warring_obj.SetActive(true);
    }

}
