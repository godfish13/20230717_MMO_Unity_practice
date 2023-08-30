using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    //GameObject (Player) 부착

    PlayerStat _Stat;
    Vector3 MouseClickDestination;

    public enum PlayerStatue
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    [SerializeField] PlayerStatue _Statue = PlayerStatue.Idle;

    private void Start()
    {
        /*Managers.inputMgr.KeyAction -= Move; // 다른부분에서 Move가 연동되있을시 액션이벤트가 여러번 발생하는 버그방지를 위해 초기화
        Managers.inputMgr.KeyAction += Move; // inputMgr의 KeyAction에 Move 연동*/ // 본작에서는 일단 마우스 조종만 구현

        _Stat = GetComponent<PlayerStat>();

        Managers.inputMgr.MouseAction -= OnMouseClicked;
        Managers.inputMgr.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        switch (_Statue)
        {
            case PlayerStatue.Die:
                UpdateDie();
                break;
            case PlayerStatue.Idle:
                UpdateIdle();
                break;
            case PlayerStatue.Moving:
                UpdateMoving();
                break;
        }    
    }

    void UpdateDie()
    {
        // 아무것도 못하는 사망상태
    }

    void UpdateMoving()
    {
        Vector3 dir = MouseClickDestination - transform.position;
        if (dir.magnitude < 0.1f)
        {
            _Statue = PlayerStatue.Idle;
        }
        else
        {
            float MoveDist = Mathf.Clamp(_Stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            // Clamp(float value, min, max) : value값이 min과 max 범위 이외의 값을 넘지 않도록 함 -> 정확히 이동할 거리 계산
            NavMeshAgent Nma =  gameObject.GetComponent<NavMeshAgent>();
            Nma.Move(dir.normalized * MoveDist);
            // NavMeshAgent.Move(float offset) : offset 방향으로 이동하되 NavMesh 내에서만 움직임

            // 캐릭터 앞으로 레이캐스팅하여 장애물과 가까울 경우 정지하도록 설정
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _Statue = PlayerStatue.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 이하 애니메이션 처리
        Animator Anim = GetComponent<Animator>();
        // 현재 게임 상태에 대한 정보를 넘겨줌
        Anim.SetFloat("Speed", _Stat.MoveSpeed);
    }

    /*void OnRunEvent()     // AnimationEvent 실험
    {
        Debug.Log("뚜벅뚜벅")
    }*/

    void UpdateIdle()
    {

        // 이하 애니메이션 처리
        Animator Anim = GetComponent<Animator>();
        Anim.SetFloat("Speed", 0);
    }

    int GroundMonsterLayerMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);
                                // 0000 0000 0010 0000              // 0000 0000 0100 0000
                                // 각각 Define에서 6, 7의 값을 가짐 해당 값들만큼 비트연산자 이동
                                // 두 값을 합하면 0000 0000 0110 0000 이므로 이는 레이어 마스크에서 6, 7번 레이어를 의미하게됨
                                // LayerMask.GetMask("String Name")을 써도 되나 연산속도면에서 비트 플래그가 훨씬 빠름
    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_Statue == PlayerStatue.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, GroundMonsterLayerMask))
        {
            MouseClickDestination = hit.point;
            _Statue = PlayerStatue.Moving;

            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Debug.Log("Monster!!");
            }
            else
            {
                Debug.Log("Ground!!");
            }
        }
    }
}
