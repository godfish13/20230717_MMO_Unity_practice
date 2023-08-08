using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Managers : MonoBehaviour
{
    static Managers Mgr_Instance;     // �� ��ũ��Ʈ�� ������������ �� �ٸ������� ���� ȣ�� �� init()�ѹ� �����Ͽ� singleton������ instance �������
    public static Managers Instance { get { init(); return Mgr_Instance; } }  // �̹� ������ ��� init()������ ���� ��ŵ��

    InputMgr _inputMgr = new InputMgr();
    ResourceMgr _resourceMgr = new ResourceMgr();
    SceneMgrEx _sceneMgrEx = new SceneMgrEx();
    SoundMgr _soundMgr = new SoundMgr();
    UIMgr _UIMgr = new UIMgr();

    public static InputMgr inputMgr { get { return Instance._inputMgr; } }
    public static ResourceMgr resourceMgr { get { return Instance._resourceMgr; } }
    public static SceneMgrEx sceneMgrEx { get { return Instance._sceneMgrEx; } }
    public static SoundMgr soundMgr { get { return Instance._soundMgr; } }
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
        if(Mgr_Instance == null)
        {
            GameObject MgrObject = GameObject.Find("@Managers");
            if(MgrObject == null)
            {
                MgrObject = new GameObject { name = "@Managers" };
                MgrObject.AddComponent<Managers>();
            }

            DontDestroyOnLoad(MgrObject);
            Mgr_Instance = MgrObject.GetComponent<Managers>();

            Mgr_Instance._soundMgr.init();
        }      
    }

    public static void Clear()
    {
        inputMgr.Clear();
        sceneMgrEx.Clear();
        soundMgr.Clear();
        UIMgr.Clear();
    }
}
