using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;     // �� ��ũ��Ʈ�� ������������ �� �ٸ������� ���� ȣ�� �� init()�ѹ� �����Ͽ� singleton������ instance �������
    public static Managers Instance { get { init(); return s_Instance; } }  // �̹� ������ ��� init()������ ���� ��ŵ��

    InputMgr _inputMgr = new InputMgr();
    ResourceMgr _resourceMgr = new ResourceMgr();

    public static InputMgr inputMgr { get { return Instance._inputMgr; } }
    public static ResourceMgr resourceMgr { get { return Instance._resourceMgr; } }

    void Start()
    {
        init();
    }

    void Update()
    {
        _inputMgr.UpdateWhenanyKey();   // anyKey�� ������ Update �۵�
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
