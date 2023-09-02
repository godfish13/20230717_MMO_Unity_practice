using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    //GameObject (Player) ����
    // ���콺 �̺�Ʈ, �÷��̾� ���¿� ���� �ൿ �� ���

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
    // ���� Define���� 6, 7�� ���� ���� �ش� ���鸸ŭ ��Ʈ������ �̵�
    // �� ���� ���ϸ� 0000 0000 0110 0000 �̹Ƿ� �̴� ���̾� ����ũ���� 6, 7�� ���̾ �ǹ��ϰԵ�
    // LayerMask.GetMask("String Name")�� �ᵵ �ǳ� ����ӵ��鿡�� ��Ʈ �÷��װ� �ξ� ����
    GameObject LockTarget;

    public PlayerState State  // �ִϸ��̼� set�� ������� set�� ���ÿ��ϵ��� ������Ƽ ����
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
                    Anim.CrossFade("WAIT", 0.1f);       // �ִϸ��̼� �ڵ忡�� ����(�ִϸ����� �� parameter ��� x)
                    break;                              // CrossFade("�ִϸ��̼Ǹ�", normalizedTransitionDuration, layer, normalizedTimeOffset)
                case PlayerState.Moving:                // normalizedTransitionDuration : �� �����ϸ� ���� Ÿ�Ӹ�ŭ �ִϸ��̼� �Ѿ�� ������ �༭ ���(�ִϸ������� �ִϸ��̼� ������ ����)                  
                    Anim.CrossFade("RUN", 0.1f);        // normalizedTimeOffset : ���� �־��ָ� �ش� �ð����� �ִϸ��̼��� �ٽ� �����(0�����ϸ� loop�� ����)
                    break;                              // layer�� ���� ��� ���ϸ� �׳� -1�� �θ� ��
                case PlayerState.Skill:
                    Anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;              
            }
        }
    }

    void Start()
    {
        /*Managers.inputMgr.KeyAction -= Move; // �ٸ��κп��� Move�� ������������ �׼��̺�Ʈ�� ������ �߻��ϴ� ���׹����� ���� �ʱ�ȭ
        Managers.inputMgr.KeyAction += Move; // inputMgr�� KeyAction�� Move ����*/ // ���ۿ����� �ϴ� ���콺 ������ ����

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
        // �ƹ��͵� ���ϴ� �������
    }

    void UpdateMoving()
    {
        // ���Ͱ� �� �����Ÿ� ���� �����ϸ� ����
        if(LockTarget != null)
        {
            float distance = (MouseClickDestination - transform.position).magnitude;
            if (distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        // �̵�
        Vector3 dir = MouseClickDestination - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
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
            case Define.MouseEvent.PointerDown:     // ���콺 �� ������ �ֱ�
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
            case Define.MouseEvent.Press:       // ���콺 ��� ���� Ŭ��
                {
                    if (LockTarget == null && RayCastHitTarget)
                        MouseClickDestination = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:   // ���콺 �� �����ٰ� ����
                StopSkill = true;
                break;
        }
    }
}
