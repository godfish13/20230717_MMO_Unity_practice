using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    //GameObject (Player) ����
    // ���콺 Ŀ�� ����, ���콺 �̺�Ʈ, �÷��̾� ���¿� ���� �ൿ �� ���

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
        /*Managers.inputMgr.KeyAction -= Move; // �ٸ��κп��� Move�� ������������ �׼��̺�Ʈ�� ������ �߻��ϴ� ���׹����� ���� �ʱ�ȭ
        Managers.inputMgr.KeyAction += Move; // inputMgr�� KeyAction�� Move ����*/ // ���ۿ����� �ϴ� ���콺 ������ ����

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
        if (Input.GetMouseButton(0))    // ���콺 Ŭ���߿��� Ŀ�� ��ȭ x
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
                    _cursorType = CursorType.Attack;    // SetCursor(texture, hotspot, cursorMode) hotspot : ���콺 �����Ͱ� Ŭ���Ǵ� ��ġ (0,0) == ������ ����
                }                                                               // cursorMode : auto == �ϵ��� ���� ����ȭ �� auto����
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
        // �ƹ��͵� ���ϴ� �������
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
            // Clamp(float value, min, max) : value���� min�� max ���� �̿��� ���� ���� �ʵ��� �� -> ��Ȯ�� �̵��� �Ÿ� ���
            NavMeshAgent Nma =  gameObject.GetComponent<NavMeshAgent>();
            Nma.Move(dir.normalized * MoveDist);
            // NavMeshAgent.Move(float offset) : offset �������� �̵��ϵ� NavMesh �������� ������

            // ĳ���� ������ ����ĳ�����Ͽ� ��ֹ��� ����� ��� �����ϵ��� ����
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    _Statue = PlayerStatue.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // ���� �ִϸ��̼� ó��
        Animator Anim = GetComponent<Animator>();
        // ���� ���� ���¿� ���� ������ �Ѱ���
        Anim.SetFloat("Speed", _Stat.MoveSpeed);
    }

    void UpdateIdle()
    {

        // ���� �ִϸ��̼� ó��
        Animator Anim = GetComponent<Animator>();
        Anim.SetFloat("Speed", 0);
    }

    int GroundMonsterLayerMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);
                                // 0000 0000 0010 0000              // 0000 0000 0100 0000
                                // ���� Define���� 6, 7�� ���� ���� �ش� ���鸸ŭ ��Ʈ������ �̵�
                                // �� ���� ���ϸ� 0000 0000 0110 0000 �̹Ƿ� �̴� ���̾� ����ũ���� 6, 7�� ���̾ �ǹ��ϰԵ�
                                // LayerMask.GetMask("String Name")�� �ᵵ �ǳ� ����ӵ��鿡�� ��Ʈ �÷��װ� �ξ� ����
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
            case Define.MouseEvent.PointerDown:     // ���콺 �� ������ �ֱ�
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
            case Define.MouseEvent.Press:       // ���콺 ��� ���� Ŭ��
                {
                    if (LockTarget!= null)
                        MouseClickDestination = LockTarget.transform.position;                  
                    else if (RayCastHitTarget)
                        MouseClickDestination = hit.point;                  
                }
                break;
            case Define.MouseEvent.PointerUp:   // ���콺 �� �����ٰ� ����
                break;
        }
    }
}
