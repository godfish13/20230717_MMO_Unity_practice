using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_Btn : UI_Base   // Bind, Get~~ 등 기본 UI베이스 상속
{ 
    enum enum_Button        // UI로 사용할 버튼/텍스트/게임오브젝트 등의 이름을 동일하게 선언해두고 유니티 엔진 내각 항목과 자동으로 연동하고 사용하기 위한 enum
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
