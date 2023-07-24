using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr
{
    public Action KeyAction = null;

    public Action<Define.MouseEvent> MouseAction = null;
    bool pressed = false;

    public void UpdateWhenanyKey()      // Managers�� Update���� anyKey�� ������ KeyAction �̺�Ʈ�� ��������� �Լ�
    {
        /*Debug.Log("keydowning");
        KeyAction.Invoke();*/       // %%%% 1) ver.Rx inputMgr Move


        if (Input.anyKey)
        {
            //Debug.Log("keydowning");
            KeyAction.Invoke();     // Invoke�� Action�� ȣ������ ���⼭�� Input.anyKey�� ������ KeyAction�۵���
        }                           // KeyAction�� PlayerCtrl���� Move�� �������ѳ����Ƿ� Input.anyKey�� ������ Move�ϰ� ��
    

        if (Input.GetMouseButton(0))
        {
            MouseAction.Invoke(Define.MouseEvent.Press);
            pressed = true;
        }
        else
        {
            if (pressed)
                MouseAction.Invoke(Define.MouseEvent.Click);
            pressed = false;
        }
    }
}
