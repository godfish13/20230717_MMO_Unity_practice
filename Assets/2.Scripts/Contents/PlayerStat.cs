using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _Exp;
    [SerializeField]
    protected int _Gold;

    public int Exp 
    { 
        get { return _Exp; }
        set
        { 
            _Exp = value;
            //������ üũ
            int leveltmp = Level;
            while (true)            // �ѹ��� �뷮�� ����ġ�� ���͵� �ѹ��� ������ ������ ����
            {
                Data.Stat stat;
                if (Managers.dataMgr.StatDictionary.TryGetValue(leveltmp + 1, out stat) == false)
                    break;
                if (_Exp < stat.TotalExp) 
                    break;
                leveltmp++;
            }

            if(leveltmp != Level)  // ���� ������ �ӽ� ������ �ٸ� == ������ ������ ���� == ��������
            {
                Debug.Log("Level Up!!");
                Level = leveltmp;
                SetLevel(Level);
            }
        }
    }
    public int Gold { get { return _Gold;} set { _Gold = value; } }

    private void Start()
    {
        _Level = 1;
        SetLevel(1);

        _Defence = 5;
        _MoveSpeed = 5.0f;
        _Exp = 0;
        _Gold = 0;
    }

    public void SetLevel(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.dataMgr.StatDictionary;
        Data.Stat StatData = dict[level];

        _HP = StatData.MaxHP;
        _MaxHP = StatData.MaxHP;
        _Attack = StatData.Attack;
    }

    protected override void OnDead(Stat AttackerStat)
    {
        Managers.gameMgr.DeSpawn(gameObject);
    }
}
