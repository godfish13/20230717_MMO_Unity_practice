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

    public int Level { get { return _Level; } set { _Level = value; } }   // ���� SerializeField ������ϰ� ������Ƽ�θ� �ϸ� ����Ƽ �� inspector���� �Ⱥ��� �׷��� ���� ����
    public int HP { get { return _HP; } set { _HP = value; } }
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

}
