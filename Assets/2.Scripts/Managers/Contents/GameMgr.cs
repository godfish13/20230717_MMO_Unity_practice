using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    GameObject Player;

    // int <-> GameObject ������ ���̽� �� �� ���ӿ�����Ʈ�� int�� ������ �����ϴ� ���� ���߿� �غ��� ������ �ϴ� key������ HashSet���� ����
    HashSet<GameObject> Monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return Player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.resourceMgr.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                Monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1); // ���� ���� �þ��ٴ� �ǹ̷� +1
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
                    {
                        Monsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);    // ���� ���� �پ��ٴ� �ǹ̷� -1
                    }
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
