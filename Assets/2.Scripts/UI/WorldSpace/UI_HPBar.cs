using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum UIGameObjects
    {
        HPBar,
    }

    Stat _stat;

    public override void init()
    {
        Bind<GameObject>(typeof(UIGameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y + Vector3.up.y * 0.3f); 
                                        // 각자 크기가 다른 캐릭터마다 자연스러운 위치에 HPBar표시되도록 콜라이더 y축 크기 + 0.3f에 위치
        transform.rotation = Camera.main.transform.rotation;    // UI가 카메라를 쳐다보도록(billboard) 둘의 rotation을 일치시켜줌
        float ratio = _stat.HP / (float)_stat.MaxHP;    //둘다 int로 해놔서 결과값이 int로 나오므로 하나 float으로 캐스팅
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float ratio)
    {
        GetGameObject((int)UIGameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
