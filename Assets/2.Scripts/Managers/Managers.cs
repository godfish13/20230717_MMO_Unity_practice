using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;     // 이 스크립트가 존재하지않을 때 다른곳에서 먼저 호출 시 init()한번 실행하여 singleton패턴의 instance 만들어줌
    public static Managers Instance { get { init(); return s_Instance; } }  // 이미 존재할 경우 init()내에서 생성 스킵됨

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

    static void init()          //singleton 패턴
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
