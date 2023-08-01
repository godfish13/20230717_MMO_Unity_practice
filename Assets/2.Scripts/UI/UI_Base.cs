using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> D_objects = new Dictionary<Type, UnityEngine.Object[]>(); // Hashtable �Ϲ�ȭ ����

    protected void Bind<T>(Type type) where T : UnityEngine.Object        // Button, Text���� ����Ƽ���� �� UI�� �� ��ũ��Ʈ�� ������ UI�̸��� �����ϰ� ������ ���
    {                                                           // �ش� UI�� �� ��ũ��Ʈ�� ������ ������ ����������
        string[] Names = Enum.GetNames(type);       // type enum���� �׸�� ���� string���� �̸���ȯ�ؼ� �迭�� ������

        UnityEngine.Object[] obj = new UnityEngine.Object[Names.Length];
        D_objects.Add(typeof(T), obj);

        for (int i = 0; i < Names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))     // GameObject�� Monobehavior, ... �� ��ӹ��� ���� ���̶� ���� �����϶�� ������ �׷��Ƿ� �Ϲ�ȭ�ƴѹ��� ���
                obj[i] = Utils.FindChild(gameObject, Names[i], true);           // Utils script�� ������ ���� ������Ʈ Ž�� �� �迭�� ���� �޼ҵ�
            else
                obj[i] = Utils.FindChild<T>(gameObject, Names[i], true);

            if (obj[i] == null)
                Debug.Log($"Failed to Bind(Can't Find '{Names[i]}')");
        }
    }

    protected T Get_UI<T>(int idx) where T : UnityEngine.Object       // Bind�� ������ UI ���� enum�� ���� int�� ������ ��ȯ�Ͽ� ȣ���� �� �ְ�����
    {
        UnityEngine.Object[] objects = null;
        if (D_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    // Text, Button, Image ���پ��� ���ϰ� Get_UI�Ἥ �ܾ���� �޼ҵ� ������
    protected Text GetText(int idx) { return Get_UI<Text>(idx); }
    protected Button GetButton(int idx) { return Get_UI<Button>(idx); }
    protected Image GetImage(int idx) { return Get_UI<Image>(idx); }
    protected GameObject GetGameObject(int idx) { return Get_UI<GameObject>(idx); }

    // UI�� ���콺 ���� Event������Ű�� �޼ҵ�
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}