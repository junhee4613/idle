using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lightning_obj : MonoBehaviour
{
    //경고판은 a가 0부터 시작
    bool pattern_start = false;
    public SpriteRenderer warning_sprite;
    public SpriteRenderer lightning_image;
    public GameObject lightning_obj;
    public GameObject warring_obj;
    public float fade_set_time;
    bool lightning_fade_out;
    float time;
    private void OnEnable()
    {
        pattern_start = false;
        time = 0;
        lightning_image.color = new Color(1f, 1f, 1f, 1f);
        warning_sprite.color = new Color(1f, 0.078f, 0f, 1f);
        warring_obj.SetActive(true);
        lightning_fade_out = false;
    }
    private void FixedUpdate()
    {
        if (warring_obj.activeSelf)
        {
            if (!pattern_start)
            {

                pattern_start = true;
                warning_sprite.DOFade(0, fade_set_time).SetLoops(3, LoopType.Yoyo).OnComplete(() =>
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
            if (!lightning_fade_out)
            {
                lightning_fade_out = true;
                Managers.Main_camera.Moving();
                lightning_image.DOFade(0, 0.4f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
                    {
                        lightning_obj.SetActive(false);
                        Managers.Pool.Push(this.gameObject);
                    }); 
            }
        }
    }
}
