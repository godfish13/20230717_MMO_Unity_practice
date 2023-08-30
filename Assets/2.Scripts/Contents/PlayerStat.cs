using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _Exp;
    [SerializeField]
    protected int _Gold;

    public int Exp { get { return _Exp; } set { _Exp = value; } }
    public int Gold { get { return _Gold;} set { _Gold = value; } }

    private void Start()
    {
        _Level = 1;
        _HP = 100;
        _MaxHP = 100;
        _Attack = 10;
        _Defence = 5;
        _MoveSpeed = 5.0f;
        _Exp = 0;
        _Gold = 0;
    }
}
