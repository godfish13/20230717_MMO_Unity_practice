using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : BaseCtrl
{
    Stat _Stat;
    Stat TargetStat;

    [SerializeField] float ScanRange = 10.0f;
    [SerializeField] float AttackRange = 2.0f;

    protected override void init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _Stat = GetComponent<Stat>();

        if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UIMgr.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        GameObject Player = Managers.gameMgr.GetPlayer();
        if (Player == null)
            return;

        float distance = (Player.transform.position - transform.position).magnitude;
        if (distance <= ScanRange)
        {
            LockTarget = Player;
            if(TargetStat == null)
                TargetStat = LockTarget.GetComponent<Stat>();
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateDie()
    {

    }

    protected override void UpdateMoving()
    {
        // �÷��̾ �� �����Ÿ� ���� �����ϸ� ����
        if (LockTarget != null)
        {
            DestinationPos = LockTarget.transform.position;
            float distance = (DestinationPos - transform.position).magnitude;
            if (distance <= AttackRange)
            {
                NavMeshAgent Nma = gameObject.GetComponent<NavMeshAgent>();
                Nma.SetDestination(transform.position); // ���� ��Ÿ� ���� ��� �̵��������� �ڱ��ڸ��� ���߱�
                State = Define.State.Skill;
                return;
            }
        }

        // �̵�
        Vector3 dir = DestinationPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent Nma = gameObject.GetComponent<NavMeshAgent>();
            Nma.SetDestination(DestinationPos); // NavMeshAgent.SetDestination(target.position) : target������ ���� ����
            Nma.speed = _Stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        if (LockTarget != null)
        {
            Vector3 dir = LockTarget.transform.position - transform.position;
            Quaternion MonsterLook = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, MonsterLook, 20 * Time.deltaTime);
        }
    }


    public void OnHitEventMonster()
    {
        if (LockTarget != null)
        {
            TargetStat.OnAttacked(_Stat);        
        }
        else
        {
            State = Define.State.Idle;
        }
    }

    public void OnHitRoutineEventMonster()
    {
        if (TargetStat.HP <= 0)
        {
            Managers.gameMgr.DeSpawn(TargetStat.gameObject);
        }
        if (TargetStat.HP > 0)
        {
            float Distance = (LockTarget.transform.position - transform.position).magnitude;
            if (Distance <= AttackRange)
                State = Define.State.Skill;
            else
                State = Define.State.Moving;
        }
        else
        {
            State = Define.State.Idle;
        }
    }
}
