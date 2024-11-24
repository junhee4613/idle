using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteraction_obj
{
    public SceneName sceneName;

    public void Awake()
    {
        if (!Managers.instance.invincibility)
        {
            Setting();
        }
        gameObject.GetOrAddComponent< BoxCollider2D >().isTrigger = true;
        if (gameObject.layer != 8)
        {
            gameObject.layer = 8;
        }
    }
    private void Start()
    {
        
    }
    public void Setting()
    {
        for (int i = 0; i < Managers.GameManager.Portals.childCount; i++)
        {
            if(i == Managers.GameManager.clear_stage_count)
            {
                Managers.GameManager.Portals.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Managers.GameManager.Portals.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void practice()
    {
        Managers.UI_jun.Fade_out_next_in("Black", 0, 1, sceneName, 1);
        Managers.GameManager.InitPos = transform.position;
        Managers.Main_camera.camera_pos = Managers.Main_camera.Main_camera.transform.position;
    }
}
