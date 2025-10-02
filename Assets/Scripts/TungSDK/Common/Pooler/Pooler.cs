using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolComponent<T> where T : Component
{
    Queue<T> mPool = new Queue<T>(10);
    Transform mParent { get; set; }
    T prefab { get; set; }
    Action OnRecycleAll { get; set; }
    public PoolComponent(T prefab, Transform parent = null, int capacity = 1)
    {
        OnRecycleAll = () => { };
        mParent = parent;
        this.prefab = prefab;
        for (int i = 0; i < capacity; i++)
        {
            CreateItem();
        }
    }

    T CreateItem()
    {
        var go = GameObject.Instantiate(prefab.gameObject);
        SetDefaultItem(go);
        go.TryGetComponent(out TickBehaviour tick);
        go.TryGetComponent(out T result);
        if (tick != null)
        {
            tick.OnRegisterCallback += _ =>
            {
                if (_ == false)
                {
                    Recycle(result.gameObject);
                }
            };
        }
        OnRecycleAll += () => Recycle(result);
        mPool.Enqueue(result);
        return result;
    }
    void SetDefaultItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(mParent);
        item.transform.position = Vector3.zero;
        item.transform.rotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
    }
    public T Spawn()
    {
        T result = null;
        if (mPool.Count > 0)
        {
            while (mPool.Count > 0 && result == null)
            {
                result = mPool.Peek();

                if (result.gameObject.activeSelf)
                {
                    mPool.Dequeue();
                    result = null;
                }
            }
        }
        if (result == null)
        {
            result = CreateItem();
        }

        result.gameObject.SetActive(true);

        return result;
    }
    public void Recycle(GameObject go)
    {
        if (go == null) return;
        go.TryGetComponent(out T component);
        if (component == null) return;
        SetDefaultItem(component.gameObject);
        if (mPool.Contains(component) == false)
        {
            mPool.Enqueue(component);
        }
    }
    public void Recycle(T component)
    {
        if (component == null) return;
        SetDefaultItem(component.gameObject);
        if (mPool.Contains(component) == false)
        {
            mPool.Enqueue(component);
        }
    }

    public void RecycleAll()
    {
        OnRecycleAll?.Invoke();
    }
}

public class PoolGameObject
{
    Queue<GameObject> mPool = new Queue<GameObject>(10);
    Transform mParent { get; set; }
    GameObject prefab { get; set; }
    Action OnRecycleAll { get; set; }
    public PoolGameObject(GameObject prefab, Transform parent = null, int capacity = 1)
    {
        OnRecycleAll = () => { };
        mParent = parent;
        this.prefab = prefab;
        for (int i = 0; i < capacity; i++)
        {
            CreateItem();
        }
    }

    GameObject CreateItem()
    {
        var go = GameObject.Instantiate(prefab.gameObject);
        SetDefaultItem(go);
        go.TryGetComponent(out TickBehaviour tick);
        if (tick != null)
        {
            tick.OnRegisterCallback += _ =>
            {
                if (_ == false)
                {
                    Recycle(go);
                }
            };
        }
        OnRecycleAll += () => Recycle(go);
        mPool.Enqueue(go);
        return go;
    }
    void SetDefaultItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(mParent);
        item.transform.position = Vector3.zero;
        item.transform.rotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
    }
    public GameObject Spawn()
    {
        GameObject result = null;
        if (mPool.Count > 0)
        {
            while (mPool.Count > 0 && result == null)
            {
                result = mPool.Peek();

                if (result.gameObject.activeSelf)
                {
                    mPool.Dequeue();
                    result = null;
                }
            }
        }
        if (result == null)
        {
            result = CreateItem();
        }

        result.gameObject.SetActive(true);

        return result;
    }
    public void Recycle(GameObject go)
    {
        if (go == null) return;
        SetDefaultItem(go);
        if (mPool.Contains(go) == false)
        {
            mPool.Enqueue(go);
        }
    }
    public void RecycleAll()
    {
        OnRecycleAll?.Invoke();
    }
}