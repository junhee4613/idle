using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteraction_obj
{
    public Transform portals;
    byte index = 0;
    public void Awake()
    {
        portals = gameObject.transform.parent;
        if (!Managers.instance.invincibility)
        {
            Setting();
        }
        gameObject.GetOrAddComponent< BoxCollider2D >().isTrigger = true;
        if (gameObject.layer != 8)
        {
            gameObject.layer = 8;
        }
        if (!Managers.instance.invincibility)
        {
            for (byte i = 0; i < portals.childCount; i++)
            {
                if (portals.transform.GetChild(i).gameObject == this.gameObject)
                {
                    index = i;
                    if (index != 0)
                    {
                        portals.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            } 
        }
    }
    private void Start()
    {
        
    }
    public void Setting()
    {
        if (Managers.GameManager.stage_clear[this.gameObject.name])
        {
            if (index + 1 < portals .childCount)
            {
                portals.GetChild(index + 1).gameObject.SetActive(true);
                portals.GetChild(index).gameObject.SetActive(false);
            }
        }
    }
    public void practice()
    {
        Managers.UI_jun.Fade_out_next_in("Black", 0, 1, gameObject.name, 1);
    }
}
