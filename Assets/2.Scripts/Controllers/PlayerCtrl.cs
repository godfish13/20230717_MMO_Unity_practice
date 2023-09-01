using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    //GameObject (Player) 부착
    // 마우스 커서 변경, 마우스 이벤트, 플레이어 상태에 따른 행동 및 모션

    PlayerStat _Stat;
    Vector3 MouseClickDestination;

    Texture2D AttackCursor;
    Texture2D HandCursor;

    enum CursorType
    {
        None,
        Hand,
        Attack,
    }

    CursorType _cursorType = CursorType.None;

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

        AttackCursor = Managers.resourceMgr.Load<Texture2D>("Textures/Cursors/Attack");
        HandCursor = Managers.resourceMgr.Load<Texture2D>("Textures/Cursors/Hand");

        _Stat = GetComponent<PlayerStat>();

        Managers.inputMgr.MouseAction -= OnMouseEvent;
        Managers.inputMgr.MouseAction += OnMouseEvent;
    }

    void Update()
    {
        UpdateMouseCursor();

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

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))    // 마우스 클릭중에는 커서 변화 x
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, GroundMonsterLayerMask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if(_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(AttackCursor, new Vector2(AttackCursor.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;    // SetCursor(texture, hotspot, cursorMode) hotspot : 마우스 포인터가 클릭되는 위치 (0,0) == 왼쪽위 구석
                }                                                               // cursorMode : auto == 하드웨어에 따라 최적화 걍 auto쓰자
            } 
            else
            {
                if(_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(HandCursor, new Vector2(HandCursor.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
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
                if (Input.GetMouseButton(0) == false)
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
    GameObject LockTarget;

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_Statue == PlayerStatue.Die)
            return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool RayCastHitTarget = Physics.Raycast(ray, out hit, 100.0f, GroundMonsterLayerMask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch(evt)
        {
            case Define.MouseEvent.PointerDown:     // 마우스 꾹 누르고 있기
                {
                    if (RayCastHitTarget)
                    {
                        MouseClickDestination = hit.point;
                        _Statue = PlayerStatue.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)                       
                            LockTarget = hit.collider.gameObject;                      
                        else                    
                            LockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:       // 마우스 잠깐 딸깍 클릭
                {
                    if (LockTarget!= null)
                        MouseClickDestination = LockTarget.transform.position;                  
                    else if (RayCastHitTarget)
                        MouseClickDestination = hit.point;                  
                }
                break;
            case Define.MouseEvent.PointerUp:   // 마우스 꾹 누르다가 놓기
                break;
        }
    }
}
