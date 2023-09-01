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

    public void UpdateWhenanyKey()      // Managers�� Update���� anyKey�� ������ KeyAction �̺�Ʈ�� ��������� �Լ�
    {
        /*Debug.Log("keydowning");
        KeyAction.Invoke();*/       // %%%% 1) ver.Rx inputMgr Move

        /*if (Input.anyKey)
        {
            //Debug.Log("keydowning");
            KeyAction.Invoke();     // Invoke�� Action�� ȣ������ ���⼭�� Input.anyKey�� ������ KeyAction�۵���
        }                           // KeyAction�� PlayerCtrl���� Move�� �������ѳ����Ƿ� Input.anyKey�� ������ Move�ϰ� ��*/
    
        if(EventSystem.current.IsPointerOverGameObject())       // UI�� Ŭ���Ѱ��� �Ǻ�����
        {
            return;             // UI Ŭ���ѰŸ� �̵� ���ϰ� ���ƹ���
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
