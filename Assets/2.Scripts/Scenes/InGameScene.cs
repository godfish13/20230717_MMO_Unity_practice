using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    Coroutine co;

    protected override void init()
    {
        base.init();
        SceneType = Define.Scene.InGame;

        Managers.UIMgr.ShowSceneUI<UI_Inven>();
        //Managers.UIMgr.ShowPopUpUI<UI_Btn>();   

        /*for (int i = 0; i < 4; i++)
        {
            Managers.resourceMgr.Instantiate("UnityChan");
        }*/

        /*co = StartCoroutine("CoExplode", 4.0f);     // 이런모양으로도 사용가능
        StopCoroutine(CoStopExplode(2.0f));*/

        Dictionary<int, Data.Stat> dict = Managers.dataMgr.StatDictionary;

        gameObject.GetOrAddComponent<CursorCtrl>();

        GameObject Player = Managers.gameMgr.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraCtrl>().SetPlayer(Player);

        Managers.gameMgr.Spawn(Define.WorldObject.Monster, "DogPBR");
    }

    IEnumerator CoStopExplode(float seconds)
    {
        Debug.Log("Stop Enter...");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Exlode Stopped");
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    IEnumerator CoExplode(float seconds)
    {
        Debug.Log("Explode Enter...");
        yield return new WaitForSeconds(seconds);
        Debug.Log("KABOOM!");
    }

    public override void Clear()
    {

    }

}
