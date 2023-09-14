using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCtrl : MonoBehaviour
{
    [SerializeField] protected Vector3 DestinationPos;
    [SerializeField] protected Define.State _State = Define.State.Idle;
    [SerializeField] protected GameObject LockTarget;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public virtual Define.State State  // �ִϸ��̼� set�� ������� set�� ���ÿ��ϵ��� ������Ƽ ����
    {
        get { return _State; }
        set
        {
            _State = value;
            Animator Anim = GetComponent<Animator>();
            switch (_State)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    Anim.CrossFade("WAIT", 0.1f);       // �ִϸ��̼� �ڵ忡�� ����(�ִϸ����� �� parameter ��� x)
                    break;                              // CrossFade("�ִϸ��̼Ǹ�", normalizedTransitionDuration, layer, normalizedTimeOffset)
                case Define.State.Moving:                // normalizedTransitionDuration : �� �����ϸ� ���� Ÿ�Ӹ�ŭ �ִϸ��̼� �Ѿ�� ������ �༭ ���(�ִϸ������� �ִϸ��̼� ������ ����)                  
                    Anim.CrossFade("RUN", 0.1f);        // normalizedTimeOffset : ���� �־��ָ� �ش� �ð����� �ִϸ��̼��� �ٽ� �����(0�����ϸ� loop�� ����)
                    break;                              // layer�� ���� ��� ���ϸ� �׳� -1�� �θ� ��
                case Define.State.Skill:
                    Anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }

    private void Start()
    {
        init();
    }

    private void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    protected abstract void init();
    protected virtual void UpdateDie() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateSkill() { }
}
