using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToast : UIBase
{
   
    enum Images
    {
        BackgroundImage
    }

    enum Texts
    {
        ToastMessageValueText,
    }
    public void OnEnable()
    {
        PopupOpenAnimation(gameObject);
    }
   
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        Refresh();
        return true;
    }

    public void SetInfo(string msg)
    {
        // 메시지 변경
        transform.localScale = Vector3.one;
        GetText((int)Texts.ToastMessageValueText).text = msg;
        Refresh();
    }

    void Refresh()
    {


    }

}
