using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr
{
    public Action KeyAction = null;

    public void UpdateWhenanyKey()      // Managers의 Update에서 anyKey가 눌리면 KeyAction 이벤트를 실행시켜줄 함수
    {                                   
        if (Input.anyKey == false)
            return;

        if (Input.anyKey != false)
        {
            KeyAction.Invoke();     // Invoke는 Action을 호출해줌 여기서는 Input.anyKey가 들어오면 KeyAction작동함
        }                           // KeyAction에 PlayerCtrl에서 Move를 연동시켜놨으므로 Input.anyKey가 들어오면 Move하게 됨
    }
}
