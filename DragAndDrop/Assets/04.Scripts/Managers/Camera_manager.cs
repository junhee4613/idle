using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Camera_manager
{
    float time;
    public Camera main_camera;
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
        if (main_camera != null)
        {
            main_camera.transform.DOShakePosition(0.5f, 0.5f, 1, 90, false, true);
        }
        else
        {
            Debug.Log("Ä«¸Þ¶ó°¡ ºö");
        }
    }
}
