using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawns : GapiMonoBehaviour
{
    [SerializeField] public List<Transform> objPool;
    [SerializeField] public List<Transform> prefabs;
    [SerializeField] protected Transform prefab;

    [SerializeField] public Transform holder;

    protected override void Reset()
    {
        base.Reset();

        this.prefab = transform.Find("Prefab");
        foreach (Transform t in this.prefab)
        {
            this.prefabs.Add(t);
        }

        this.holder = transform.Find("Holder");
    }

    public virtual Transform Spawn(string nameObj, Vector3 pos, Quaternion rot)
    {
        Transform objPrefab = GetObjInPrefab(nameObj);
        if (objPrefab == null)
        {
            Debug.LogError($"❌ Không tìm thấy prefab có tên: {nameObj}");
            return null;
        }

        Transform objPool = GetObjInPool(objPrefab);
        if (objPool == null)
        {
            Debug.LogError($"❌ Không tạo được object từ prefab: {objPrefab.name}");
            return null;
        }

        objPool.name = objPrefab.name;
        objPool.SetPositionAndRotation(pos, rot);
        objPool.parent = this.holder;
        objPool.gameObject.SetActive(true);

        return objPool;
    }

    public virtual void AddObjPool(Transform obj)
    {
        obj.gameObject.SetActive(false);
        this.objPool.Add(obj);
    }

    protected virtual Transform GetObjInPrefab(string nameObj)
    {
        foreach (Transform child in this.prefabs)
        {
            if (child.name.Contains(nameObj)) // dùng Contains để tránh lỗi tên có "(Clone)" hoặc khoảng trắng
            {
                return child;
            }
        }

        Debug.LogWarning($"⚠️ Không tìm thấy prefab với tên gần giống: {nameObj}");
        return null;
    }

    protected virtual Transform GetObjInPool(Transform obj)
    {
        for (int i = 0; i < this.objPool.Count; i++)
        {
            Transform child = this.objPool[i];
            if (child.name == obj.name)
            {
                this.objPool.RemoveAt(i);
                return child;
            }
        }

        Transform objNew = Instantiate(obj);
        return objNew;
    }
}
