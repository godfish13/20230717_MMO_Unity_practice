using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCtrl : MonoBehaviour
{
    [SerializeField] protected Vector3 DestinationPos;
    [SerializeField] protected Define.State _State = Define.State.Idle;
    [SerializeField] protected GameObject LockTarget;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public virtual Define.State State  // 애니메이션 set과 현재상태 set을 동시에하도록 프로퍼티 설정
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
                    Anim.CrossFade("WAIT", 0.1f);       // 애니메이션 코드에서 관리(애니메이터 내 parameter 사용 x)
                    break;                              // CrossFade("애니메이션명", normalizedTransitionDuration, layer, normalizedTimeOffset)
                case Define.State.Moving:                // normalizedTransitionDuration : 값 설정하면 블렌딩 타임만큼 애니메이션 넘어가는 딜레이 줘서 재생(애니메이터의 애니메이션 블랜딩과 동일)                  
                    Anim.CrossFade("RUN", 0.1f);        // normalizedTimeOffset : 값을 넣어주면 해당 시간부터 애니메이션이 다시 재생됨(0으로하면 loop와 동일)
                    break;                              // layer는 따로 사용 안하면 그냥 -1로 두면 됨
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
