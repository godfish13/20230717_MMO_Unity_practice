using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr
{
    public Action KeyAction = null;

    public void UpdateWhenanyKey()      // Managers�� Update���� anyKey�� ������ KeyAction �̺�Ʈ�� ��������� �Լ�
    {
        /*Debug.Log("keydowning");
        KeyAction.Invoke();*/       // %%%% 1) ver.Rx inputMgr Move

        if (Input.anyKey == false)
            return;

        if (Input.anyKey != false)
        {
            //Debug.Log("keydowning");
            KeyAction.Invoke();     // Invoke�� Action�� ȣ������ ���⼭�� Input.anyKey�� ������ KeyAction�۵���
        }                           // KeyAction�� PlayerCtrl���� Move�� �������ѳ����Ƿ� Input.anyKey�� ������ Move�ϰ� ��
    }
}
