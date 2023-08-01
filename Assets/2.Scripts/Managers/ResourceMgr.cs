using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr
{
    public T Load<T>(string path) where T : Object  // 오브젝트 관련 메소드들을 여러곳에서 산개하여 작성하면 어디서 무엇을 어떻게 쓰는지 찾기 점점 어려워짐
    {                                               // 따라서 추적하기 쉽게하기위해 ResourceMgr을 통하도록 Load, Destroy등 각 메소드들 Wrapping 해둠
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.Log($"Failed to Load prefab : {path}");
        }

        return Object.Instantiate(prefab, parent);
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
