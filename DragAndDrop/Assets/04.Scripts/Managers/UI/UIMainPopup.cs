using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainPopup : UIPopup
{
    enum GameObjects
    {
        ContentObject,
    }

    enum Buttons
    {
        Test_001,
        Test_002,
        Test_003,
        Close
    }

    enum Texts
    {
        Test_Text,
    }

    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        //PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
       
        //BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.Test_001).gameObject.BindEvent(OnButtonTest_001);
        GetButton((int)Buttons.Test_002).gameObject.BindEvent(OnButtonTest_002);
        GetButton((int)Buttons.Test_003).gameObject.BindEvent(OnButtonTest_003);

        GetButton((int)Buttons.Close).gameObject.BindEvent(OnClickClose);

        GetText((int)Texts.Test_Text).text = $"¹öÀü : {Application.version}";

        return true;
    }

    void OnButtonTest_001()
    {

    }

    void OnButtonTest_002()
    {

    }

    void OnButtonTest_003()
    {

    }

    void OnClickClose()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
