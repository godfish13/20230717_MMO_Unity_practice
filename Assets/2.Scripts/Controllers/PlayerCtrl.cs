using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    //GameObject (Player) 부착
    // 마우스 이벤트, 플레이어 상태에 따른 행동 및 모션

    PlayerStat _Stat;
    Vector3 MouseClickDestination;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    [SerializeField] PlayerState _State = PlayerState.Idle;

    int GroundMonsterLayerMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);
    // 0000 0000 0010 0000              // 0000 0000 0100 0000
    // 각각 Define에서 6, 7의 값을 가짐 해당 값들만큼 비트연산자 이동
    // 두 값을 합하면 0000 0000 0110 0000 이므로 이는 레이어 마스크에서 6, 7번 레이어를 의미하게됨
    // LayerMask.GetMask("String Name")을 써도 되나 연산속도면에서 비트 플래그가 훨씬 빠름
    GameObject LockTarget;

    public PlayerState State  // 애니메이션 set과 현재상태 set을 동시에하도록 프로퍼티 설정
    {
        get { return _State; }
        set
        {
            _State = value;
            Animator Anim = GetComponent<Animator>();
            switch (_State)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    Anim.CrossFade("WAIT", 0.1f);       // 애니메이션 코드에서 관리(애니메이터 내 parameter 사용 x)
                    break;                              // CrossFade("애니메이션명", normalizedTransitionDuration, layer, normalizedTimeOffset)
                case PlayerState.Moving:                // normalizedTransitionDuration : 값 설정하면 블렌딩 타임만큼 애니메이션 넘어가는 딜레이 줘서 재생(애니메이터의 애니메이션 블랜딩과 동일)                  
                    Anim.CrossFade("RUN", 0.1f);        // normalizedTimeOffset : 값을 넣어주면 해당 시간부터 애니메이션이 다시 재생됨(0으로하면 loop와 동일)
                    break;                              // layer는 따로 사용 안하면 그냥 -1로 두면 됨
                case PlayerState.Skill:
                    Anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;              
            }
        }
    }

    void Start()
    {
        /*Managers.inputMgr.KeyAction -= Move; // 다른부분에서 Move가 연동되있을시 액션이벤트가 여러번 발생하는 버그방지를 위해 초기화
        Managers.inputMgr.KeyAction += Move; // inputMgr의 KeyAction에 Move 연동*/ // 본작에서는 일단 마우스 조종만 구현

        _Stat = GetComponent<PlayerStat>();

        Managers.inputMgr.MouseAction -= OnMouseEvent;
        Managers.inputMgr.MouseAction += OnMouseEvent;
    }

    void Update()
    {
        switch (State)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;
        }    
    }

    void UpdateDie()
    {
        // 아무것도 못하는 사망상태
    }

    void UpdateMoving()
    {
        // 몬스터가 내 사정거리 내에 존재하면 공격
        if(LockTarget != null)
        {
            float distance = (MouseClickDestination - transform.position).magnitude;
            if (distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = MouseClickDestination - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
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
                if (Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }      
    }

    void UpdateIdle()
    {

    }

    void UpdateSkill()
    {
        if(LockTarget != null)
        {
            Vector3 dir = LockTarget.transform.position - transform.position;
            Quaternion MonterLook = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, MonterLook, 20 * Time.deltaTime);
        }
    }

    public void OnHitEvent()
    {
        Debug.Log("Hit!");

        if (StopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }

    bool StopSkill = false;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        StopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool RayCastHitTarget = Physics.Raycast(ray, out hit, 100.0f, GroundMonsterLayerMask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:     // 마우스 꾹 누르고 있기
                {
                    if (RayCastHitTarget)
                    {
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            LockTarget = hit.collider.gameObject;
                            MouseClickDestination = new Vector3(hit.point.x, 0, hit.point.z);
                        }
                        else
                        {
                            LockTarget = null;
                            MouseClickDestination = hit.point;                         
                        }
                        State = PlayerState.Moving;
                        StopSkill = false;
                        Debug.Log(MouseClickDestination);
                    }
                }
                break;
            case Define.MouseEvent.Press:       // 마우스 잠깐 딸깍 클릭
                {
                    if (LockTarget == null && RayCastHitTarget)
                        MouseClickDestination = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:   // 마우스 꾹 누르다가 놓기
                StopSkill = true;
                break;
        }
    }
}
