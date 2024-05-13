using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteraction_obj
{
    public GameObject barricade;
    public void Awake()
    {
        gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D bc);
        if (bc == null)
        {
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        }
        if (gameObject.layer != 8)
        {
            gameObject.layer = 8;
        }
        if(barricade == null)
        {
            barricade = GameObject.Find(this.name + "_next_barricade");
        }
        if (Managers.GameManager.stage_clear[this.gameObject.name])
        {
            barricade.SetActive(false);
        }
    }
    private void Start()
    {
        
    }

    public void practice()
    {
        Managers.UI_jun.Fade_out_next_in("Black", 0, 1, gameObject.name, 1);
    }
}
