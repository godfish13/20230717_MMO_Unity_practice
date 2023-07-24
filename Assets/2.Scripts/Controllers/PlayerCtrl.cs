using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float M_speed = 10.0f;

    bool isMovingToDest = false;
    Vector3 MouseClickDestination;

    private void Start()
    {
        Managers.inputMgr.KeyAction -= Move; // 다른부분에서 Move가 연동되있을시 액션이벤트가 여러번 발생하는 버그방지를 위해 초기화
        Managers.inputMgr.KeyAction += Move; // inputMgr의 KeyAction에 Move 연동

        Managers.inputMgr.MouseAction -= OnMouseClicked;
        Managers.inputMgr.MouseAction += OnMouseClicked;

    }

    //GameObject (Player)
    // Transform
    // PlayerCtrl (*)

    void Update()
    {
        if(isMovingToDest)
        {
            Vector3 dir = MouseClickDestination - transform.position;
            if(dir.magnitude < 0.0001f)
            {
                isMovingToDest = false;
            }
            else
            {
                float MoveDist = Mathf.Clamp(M_speed * Time.deltaTime, 0, dir.magnitude);
                // Clamp(float value, min, max) : value값이 min과 max 범위 이외의 값을 넘지 않도록 함 -> 정확히 이동할 거리 계산
                transform.position += dir.normalized * MoveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            }
        }


        if (isMovingToDest)
        {
            Animator Anim = GetComponent<Animator>();
            Anim.Play("RUN");
        }
        else
        {
            Animator Anim = GetComponent<Animator>();
            Anim.Play("WAIT");
        }       
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

        isMovingToDest = false;
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        /*if (evt != Define.MouseEvent.Click)     // 마우스 클릭하고 뗏을때만 작동하도록 함, inputMgr의 Mouse클릭 이벤트 액션 호출 참조
            return;*/

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Wall");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            MouseClickDestination = hit.point;
            isMovingToDest = true;
            //Debug.Log($"RayCast Camera : {hit.collider.gameObject.name}");
        }
    }
}
