using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_Btn : UI_Base   // Bind, Get~~ �� �⺻ UI���̽� ���
{ 
    enum enum_Button        // UI�� ����� ��ư/�ؽ�Ʈ/���ӿ�����Ʈ ���� �̸��� �����ϰ� �����صΰ� ����Ƽ ���� ���� �׸�� �ڵ����� �����ϰ� ����ϱ� ���� enum
    {
        PointBtn,
    }

    enum enum_Text
    {
        PointTxt,
        ScoreTxt,
    }

    enum enum_GameObject
    {
        TestObj,
    }

    private void Start()
    {
        Bind<Button>(typeof(enum_Button));
        Bind<Text>(typeof(enum_Text));
        Bind<GameObject>(typeof(enum_GameObject));

        GetText((int)enum_Text.PointTxt).text = "Testing";
        Get_UI<Text>((int)enum_Text.ScoreTxt).text = "Binding test";
    }

    public void OnBtnClicked()
    {

    }
}
