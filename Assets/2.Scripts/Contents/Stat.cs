using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _Level;
    [SerializeField]
    protected int _HP;
    [SerializeField]
    protected int _MaxHP;
    [SerializeField]
    protected int _Attack;
    [SerializeField]
    protected int _Defence;
    [SerializeField]
    protected float _MoveSpeed;

    public int Level { get { return _Level; } set { _Level = value; } }   // ���� SerializeField ������ϰ� ������Ƽ�θ� �ϸ� ����Ƽ �� inspector���� �Ⱥ���
    public int HP { get { return _HP; } set { _HP = value; } }              // �׷��Ƿ� SerializeField�� �� ���� �����صΰ� ������Ƽ�� ���������ϰ� �Ѵ� ����
    public int MaxHP { get { return _MaxHP;} set { _MaxHP = value; } }
    public int Attack { get { return _Attack; } set { _Attack = value; } }
    public int Defence { get { return _Defence; } set { _Defence = value; } }
    public float MoveSpeed { get { return _MoveSpeed; } set { _MoveSpeed = value; } }

    private void Start()
    {
        _Level = 1;
        _HP = 100;
        _MaxHP = 100;
        _Attack = 10;
        _Defence = 5;
        _MoveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat AttackerStat)
    {
        int Damage = Mathf.Max(0, AttackerStat._Attack - Defence);
        HP -= Damage;
        if (HP <= 0)
        {
            HP = 0;
            OnDead(AttackerStat);
        }
    }

    protected virtual void OnDead(Stat AttackerStat)
    {
        PlayerStat playerStat = AttackerStat as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Exp += 150;
        }
        Managers.gameMgr.DeSpawn(gameObject);
    }
}
