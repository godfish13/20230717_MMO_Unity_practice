using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr
{
    public Action KeyAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
            return;

        if (Input.anyKey != false)
        {
            KeyAction.Invoke();     // Invoke�� Action�� ȣ������ ���⼭�� Input.anyKey�� ������ KeyAction�۵���
        }                           // KeyAction�� PlayerCtrl���� Move�� �������ѳ����Ƿ� Input.anyKey�� ������ Move�ϰ� ��
    }
}
