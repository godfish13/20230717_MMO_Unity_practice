using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr
{
    public T Load<T>(string path) where T : Object  // ������Ʈ ���� �޼ҵ���� ���������� �갳�Ͽ� �ۼ��ϸ� ��� ������ ��� ������ ã�� ���� �������
    {                                               // ���� �����ϱ� �����ϱ����� ResourceMgr�� ���ϵ��� Load, Destroy�� �� �޼ҵ�� Wrapping �ص�
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.Log($"Failed to Load prefab : {path}");
        }

        GameObject go = Object.Instantiate(prefab, parent);
        int index = go.name.IndexOf("(Clone)");     // ������ ������Ʈ�� (Clone)���� index��ġ ã�� Clone ����
        if(index > 0)
            go.name = go.name.Substring(0, index);

        return go;
    }

    public void Destroy(GameObject gameObject) 
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject);
    }

    public void Destroy(GameObject gameObject, float time)
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject, time);
    }
}
