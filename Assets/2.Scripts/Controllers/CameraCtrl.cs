using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private Define.CameraMode _mode = Define.CameraMode.QuaterView;      //Define�� �����ص� CameraMode
    [SerializeField] private Vector3 _delta = new Vector3(0, 0, 0); // ī�޶� ĳ���� �ٶ󺸴� ����
    [SerializeField] private GameObject Player = null;

    public void SetPlayer(GameObject _player) { Player = _player; }

    void Start()
    {
        SetQuaterView(_delta);
    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuaterView)
        {
            if(Player.isValid() == false)       // �÷��̾ ���»��¸� return
            {
                return;
            }

            RaycastHit hit;
            if(Physics.Raycast(Player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - Player.transform.position).magnitude * 0.8f;
                transform.position = Player.transform.position + Vector3.up * 1.0f + _delta.normalized * dist;
            }
            else
            {
                transform.position = Player.transform.position + _delta;
                transform.LookAt(Player.transform.position + Vector3.up * 1.0f);
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
