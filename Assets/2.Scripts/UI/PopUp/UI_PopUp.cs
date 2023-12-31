using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : UI_Base
{
    public override void init()      // Start로 부르고 싶으나 이 스크립트를 쓰는게 아닌 UI_Btn에서 UI_PopUp클래스를 상속받아 사용하므로
    {                                                   // 가상함수로 선언, UI_PopUp클래스에서 override 후 Start에서 사용해줌
        Managers.UIMgr.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopUpUI()
    {
        Managers.UIMgr.ClosePopUpUI(this);
    }
}
