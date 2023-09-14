using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class InputMgr
{
    public Action KeyAction = null;

    public Action<Define.MouseEvent> MouseAction = null;
    bool pressed = false;
    float pressedTime = 0;

    public void UpdateWhenanyKey()      // Managers의 Update에서 anyKey가 눌리면 KeyAction 이벤트를 실행시켜줄 함수
    {
        /*Debug.Log("keydowning");
        KeyAction.Invoke();*/       // %%%% 1) ver.Rx inputMgr Move

        /*if (Input.anyKey)
        {
            //Debug.Log("keydowning");
            KeyAction.Invoke();     // Invoke는 Action을 호출해줌 여기서는 Input.anyKey가 들어오면 KeyAction작동함
        }                           // KeyAction에 PlayerCtrl에서 Move를 연동시켜놨으므로 Input.anyKey가 들어오면 Move하게 됨*/
    
        if(EventSystem.current.IsPointerOverGameObject())       // UI를 클릭한건지 판별해줌
        {
            return;             // UI 클릭한거면 이동 못하게 막아버림
        }

        if (Input.GetMouseButton(0))
        {
            if (MouseAction == null)
                return;
            if (pressed == false)
            {
                MouseAction.Invoke(Define.MouseEvent.PointerDown);
                pressedTime = Time.time;
            }
            MouseAction.Invoke(Define.MouseEvent.Press);
            pressed = true;
        }
        else
        {
            if (MouseAction == null)
                return;
            if (pressed)
            {
                if(Time.time < pressedTime * 0.2f)              
                    MouseAction.Invoke(Define.MouseEvent.Click);
                MouseAction.Invoke(Define.MouseEvent.PointerUp);
            }
            pressed = false;
            pressedTime = 0;
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
