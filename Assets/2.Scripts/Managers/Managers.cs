using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;     // �� ��ũ��Ʈ�� ������������ �� �ٸ������� ���� ȣ�� �� init()�ѹ� �����Ͽ� singleton������ instance �������
    public static Managers Instance { get { init(); return s_Instance; } }  // �̹� ������ ��� init()������ ���� ��ŵ��

    InputMgr _inputMgr = new InputMgr();
    public static InputMgr inputMgr { get { return Instance._inputMgr; } }

    void Start()
    {
        init();
    }

    void Update()
    {
        _inputMgr.OnUpdate();
    }

    static void init()          //singleton ����
    {
        if(s_Instance == null)
        {
            GameObject MgrObject = GameObject.Find("@Managers");
            if(MgrObject == null)
            {
                MgrObject = new GameObject { name = "@Managers" };
                MgrObject.AddComponent<Managers>();
            }
            DontDestroyOnLoad(MgrObject);
            s_Instance = MgrObject.GetComponent<Managers>();
        }      
    }
}
