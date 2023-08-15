using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    protected override void init()
    {
        base.init();
        SceneType = Define.Scene.InGame;

        //Managers.UIMgr.ShowSceneUI<UI_Inven>();
        //Managers.UIMgr.ShowPopUpUI<UI_Btn>();   

        for (int i = 0; i < 4; i++)
        {
            Managers.resourceMgr.Instantiate("UnityChan");
        }
    }

    public override void Clear()
    {

    }

}
