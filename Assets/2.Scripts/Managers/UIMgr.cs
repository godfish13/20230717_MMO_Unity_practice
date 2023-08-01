using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr
{
    int _order = 0; // UI들이 화면에 노출되는 순서 (Canvas의 order)

    Stack<UI_PopUp> _PopUpStack = new Stack<UI_PopUp>();

    public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
    {
        if(string.IsNullOrEmpty(name))      // 이름 지정 안하고 그냥 사용하면 T 팝업시킴
            name = typeof(T).Name;

        GameObject go = Managers.resourceMgr.Instantiate($"UI/PopUp/{name}");
        T PopUp = Utils.GetOrAddComponent<T>(go);
        _PopUpStack.Push(PopUp);
        return PopUp;
    }

    public void ClosePopUpUI(UI_PopUp popUp)
    {
        if (_PopUpStack.Count == 0)
            return;

        if(_PopUpStack.Peek() != popUp)         // Peek() : Stack의 제일 위에있는 요소 확인용(Pop되지 않음 그냥 관찰만함)
        {
            Debug.Log("Close PopUp failed");
            return;
        }
        ClosePopUpUI();
    }

    public void ClosePopUpUI()      // 스택 형식으로 순서 관리하여 제일 마지막에 팝업된(제일 위에있는) UI 닫기
    {
        if (_PopUpStack.Count == 0)
            return;

        UI_PopUp popUp = _PopUpStack.Pop();
        Managers.resourceMgr.Destroy(popUp.gameObject);
        popUp = null;   // 혹시 다시 접근할 위험을 없애기 위해 popUp을 null로 초기화
        _order--;
    }

    public void CloseAllPopUpUI()
    {
        while(_PopUpStack.Count > 0)
            ClosePopUpUI();
    }
}
