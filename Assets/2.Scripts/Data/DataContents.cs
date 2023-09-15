using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat

    [Serializable]      // 메모리에 들고있는 정보를 파일로 변환시키기 위해 필요한 선언
    public class Stat
    {
        public int Level;       // public or [SerializeField] 선언해야지 JSON에서 데이터 받아올 수 있음
        public int MaxHP;          // 각 항목의 이름이랑 JSON 파일 내 항목의 이름이 꼭 같아야 데이터 받아올 수 있음
        public int Attack;      // 자료형 또한 주의!
        public int TotalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> Stats = new List<Stat>();     //!!!!!!중요!!!!!! JSON파일에서 받아오려는 list와 이름이 꼭!!! 같아야함

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