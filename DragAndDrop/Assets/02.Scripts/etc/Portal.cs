using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteraction_obj
{
    public GameObject barricade;
    public Transform portals;
    byte index = 0;
    public void Awake()
    {
        //Managers.GameManager.portal_init += this.Setting;
        portals = gameObject.transform.parent;
        Setting();
        gameObject.GetOrAddComponent< BoxCollider2D >().isTrigger = true;
        if (gameObject.layer != 8)
        {
            gameObject.layer = 8;
        }
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
        /*if(barricade == null)
        {
            barricade = GameObject.Find(this.name + "_next_barricade");
        }*/

    }
    private void Start()
    {
        
    }
    public void Setting()
    {
        Debug.Log(1);
        if (Managers.GameManager.stage_clear[this.gameObject.name])
        {
            if (index + 1 < portals .childCount)
            {
                portals.GetChild(index + 1).gameObject.SetActive(true);
                portals.GetChild(index).gameObject.SetActive(false);
            }
            //barricade.SetActive(false); FIX : 나중에 챕터별로 클리어 확인 후 그 챕터의 바리케이트를 여는 식으로 변경
        }
        //Managers.GameManager.portal_init -= this.Setting; //이건 나중에 챕터들만 존재하는 씬이 생기면 거기로 갈 때마다 이벤트 핸들러를 다시 빼주기로
    }
    public void practice()
    {
        Managers.UI_jun.Fade_out_next_in("Black", 0, 1, gameObject.name, 1);
    }
}
