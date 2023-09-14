using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : BaseCtrl
{
    // GameObject (Player) 부착
    // 마우스 이벤트, 플레이어 상태에 따른 행동 및 모션

    PlayerStat _Stat;
    bool StopSkill = false;
    LayerMask GroundMonsterLayerMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);

    protected override void init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _Stat = GetComponent<PlayerStat>();
        Managers.inputMgr.MouseAction -= OnMouseEvent;
        Managers.inputMgr.MouseAction += OnMouseEvent;
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UIMgr.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리 내에 존재하면 공격
        if(LockTarget != null)
        {
            DestinationPos = LockTarget.transform.position;
            float distance = (DestinationPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = DestinationPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            //NavMeshAgent Nma =  gameObject.GetComponent<NavMeshAgent>();
            //Nma.Move(dir.normalized * MoveDist);
            // NavMeshAgent.Move(float offset) : offset 방향으로 이동하되 NavMesh 내에서만 움직임 
            // 아래에서 레이캐스팅으로 이동가능한 범위 계산중이므로 nav 삭제

            // 캐릭터 앞으로 레이캐스팅하여 장애물과 가까울 경우 정지하도록 설정
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            float MoveDist = Mathf.Clamp(_Stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            // Clamp(float value, min, max) : value값이 min과 max 범위 이외의 값을 넘지 않도록 함 -> 정확히 이동할 거리 계산
            transform.position += dir.normalized * MoveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }      
    }

    protected override void UpdateSkill()
    {
        if(LockTarget != null)
        {
            Vector3 dir = LockTarget.transform.position - transform.position;
            Quaternion MonsterLook = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, MonsterLook, 20 * Time.deltaTime);
        }
    }

    public void OnHitEvent()
    {
        //Debug.Log("Hit!");

        if(LockTarget != null)
        {
            Stat TargetStat =  LockTarget.GetComponent<Stat>();
            Stat MyStat = gameObject.GetComponent<PlayerStat>();
            int Damage = Mathf.Max(0, MyStat.Attack - TargetStat.Defence);
            //Debug.Log(Damage);
            TargetStat.HP -= Damage;
        }
    }

    public void OnHitRoutineEvent()
    {
        if (StopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
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
                            DestinationPos = new Vector3(hit.point.x, 0, hit.point.z);
                        }
                        else
                        {
                            LockTarget = null;
                            DestinationPos = hit.point;                         
                        }
                        State = Define.State.Moving;
                        StopSkill = false;
                        //Debug.Log(MouseClickDestination);
                    }
                }
                break;
            case Define.MouseEvent.Press:       // 마우스 잠깐 딸깍 클릭
                {
                    if (LockTarget == null && RayCastHitTarget)
                        DestinationPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:   // 마우스 꾹 누르다가 놓기
                StopSkill = true;
                break;
        }
    }
}
