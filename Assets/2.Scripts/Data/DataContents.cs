using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat

    [Serializable]      // �޸𸮿� ����ִ� ������ ���Ϸ� ��ȯ��Ű�� ���� �ʿ��� ����
    public class Stat
    {
        public int Level;       // public or [SerializeField] �����ؾ��� JSON���� ������ �޾ƿ� �� ����
        public int MaxHP;          // �� �׸��� �̸��̶� JSON ���� �� �׸��� �̸��� �� ���ƾ� ������ �޾ƿ� �� ����
        public int Attack;      // �ڷ��� ���� ����!
        public int TotalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> Stats = new List<Stat>();     //!!!!!!�߿�!!!!!! JSON���Ͽ��� �޾ƿ����� list�� �̸��� ��!!! ���ƾ���

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in Stats)
                dict.Add(stat.Level, stat);
            return dict;
        }
    }

    #endregion
}