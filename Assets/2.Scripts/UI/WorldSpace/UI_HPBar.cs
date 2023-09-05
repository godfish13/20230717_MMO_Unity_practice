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
                                        // ���� ũ�Ⱑ �ٸ� ĳ���͸��� �ڿ������� ��ġ�� HPBarǥ�õǵ��� �ݶ��̴� y�� ũ�� + 0.3f�� ��ġ
        transform.rotation = Camera.main.transform.rotation;    // UI�� ī�޶� �Ĵٺ�����(billboard) ���� rotation�� ��ġ������
        float ratio = _stat.HP / (float)_stat.MaxHP;    //�Ѵ� int�� �س��� ������� int�� �����Ƿ� �ϳ� float���� ĳ����
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float ratio)
    {
        GetGameObject((int)UIGameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
