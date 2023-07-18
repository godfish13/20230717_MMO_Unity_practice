using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float M_speed = 10.0f;
    //[SerializeField] private float R_speed = 100.0f;

    private void Start()
    {
        Managers.inputMgr.KeyAction -= Move; // 다른부분에서 Move가 연동되있을시 액션이벤트가 여러번 발생하는 버그방지를 위해 초기화
        Managers.inputMgr.KeyAction += Move; // inputMgr의 KeyAction에 Move 연동
    }

    //GameObject (Player)
    // Transform
    // PlayerCtrl (*)

    void Update()
    {
        
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
            transform.position += Vector3.forward * Time.deltaTime * M_speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.1f);
            transform.position += Vector3.back * Time.deltaTime * M_speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
            transform.position += Vector3.left * Time.deltaTime * M_speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
            transform.position += Vector3.right * Time.deltaTime * M_speed;
        }
    }
}
