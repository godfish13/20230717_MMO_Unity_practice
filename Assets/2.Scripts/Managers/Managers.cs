using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;     // �� ��ũ��Ʈ�� ������������ �� �ٸ������� ���� ȣ�� �� init()�ѹ� �����Ͽ� singleton������ instance �������
    public static Managers Instance { get { init(); return s_Instance; } }  // �̹� ������ ��� init()������ ���� ��ŵ��

    InputMgr _inputMgr = new InputMgr();
    ResourceMgr _resourceMgr = new ResourceMgr();
    UIMgr _UIMgr = new UIMgr();

    public static InputMgr inputMgr { get { return Instance._inputMgr; } }
    public static ResourceMgr resourceMgr { get { return Instance._resourceMgr; } }
    public static UIMgr UIMgr { get { return Instance._UIMgr; } }

    void Start()
    {
        init();
        /*this.UpdateAsObservable()
            .Select(_ => Input.anyKey)
            .Where(anyKey => anyKey != false)
            .Subscribe(anyKey => _inputMgr.UpdateWhenanyKey());*/ // %%% 1) ver.Rx inputMgr Move - inputMgr 1)���̶� �Բ��ϱ�
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
