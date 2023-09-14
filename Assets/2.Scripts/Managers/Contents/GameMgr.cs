using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    GameObject Player;

    // int <-> GameObject 데이터 베이스 상 각 게임오브젝트가 int의 값으로 저장하는 것은 나중에 해보고 지금은 일단 key값없는 HashSet으로 제작
    HashSet<GameObject> Monsters = new HashSet<GameObject>();

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.resourceMgr.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                Monsters.Add(go);
                break;
            case Define.WorldObject.Player:
                Player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseCtrl bs = go.GetComponent<BaseCtrl>();
        if (bs == null)
            return Define.WorldObject.Unknown;

        return bs.WorldObjectType;
    }

    public void DeSpawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if(Monsters.Contains(go))
                        Monsters.Remove(go);
                }
                break;
            case Define.WorldObject.Player:
                {
                    if (Player == go)
                        Player = null;
                }
                break;
        }

        Managers.resourceMgr.Destroy(go);
    }
}
