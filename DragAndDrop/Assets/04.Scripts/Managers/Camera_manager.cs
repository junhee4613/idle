using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Camera_manager
{
    float time;
    Camera main_camera;
    public Camera Main_camera { 
        get 
        {
            if (main_camera == null)
            {
                main_camera = GameObject.FindObjectOfType<Camera>();
            }
            return main_camera; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Moving()
    {
        Main_camera.transform.DOShakePosition(0.3f, 0.3f, 100, 90, false, true);
    }
    public void Move_y()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Main_camera.transform.DOMoveY(0.3f, 0.1f));
        sequence.Append(Main_camera.transform.DOMoveY(0, 0.1f));
    }
    public void Puch(float expansion, float reduction)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(Main_camera.DOOrthoSize(expansion, 0.1f));
        sequence.Append(Main_camera.DOOrthoSize(reduction, 0.1f));
    }
}
