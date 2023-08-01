using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr
{
    int _order = 0; // UI���� ȭ�鿡 ����Ǵ� ���� (Canvas�� order)

    Stack<UI_PopUp> _PopUpStack = new Stack<UI_PopUp>();

    public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
    {
        if(string.IsNullOrEmpty(name))      // �̸� ���� ���ϰ� �׳� ����ϸ� T �˾���Ŵ
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

        if(_PopUpStack.Peek() != popUp)         // Peek() : Stack�� ���� �����ִ� ��� Ȯ�ο�(Pop���� ���� �׳� ��������)
        {
            Debug.Log("Close PopUp failed");
            return;
        }
        ClosePopUpUI();
    }

    public void ClosePopUpUI()      // ���� �������� ���� �����Ͽ� ���� �������� �˾���(���� �����ִ�) UI �ݱ�
    {
        if (_PopUpStack.Count == 0)
            return;

        UI_PopUp popUp = _PopUpStack.Pop();
        Managers.resourceMgr.Destroy(popUp.gameObject);
        popUp = null;   // Ȥ�� �ٽ� ������ ������ ���ֱ� ���� popUp�� null�� �ʱ�ȭ
        _order--;
    }

    public void CloseAllPopUpUI()
    {
        while(_PopUpStack.Count > 0)
            ClosePopUpUI();
    }
}
