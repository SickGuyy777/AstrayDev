using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolObject<T>
{
    private readonly T objPrefab;
    private readonly Queue<T> pool = new Queue<T>();

    private int size;
    private bool destroyed;
    
    
    public ObjectPool(T objPrefab, int poolSize)
    {
        this.objPrefab = objPrefab;
        this.size = poolSize;

        PreCreateObjects();
    }

    public T Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if(pool.Count <= 0)
            IncreaseSize();

        T createdObj = pool.Dequeue();

        createdObj.gameObject.SetActive(true);
        createdObj.OnStart();
        
        createdObj.transform.SetPositionAndRotation(position, rotation);
        createdObj.transform.parent = parent;
        
        return createdObj;
    }

    private void IncreaseSize()
    {
        size++;
        Debug.LogError("Pool size was too small, It has been increased to: " + size);
        
        PrecreateObject();
    }
    
    public void Destroy(T objectToDestroy)
    {
        if (!destroyed)
        {
            objectToDestroy.gameObject.SetActive(false);
            pool.Enqueue(objectToDestroy);
        }
        else
        {
            if(objectToDestroy == null || objectToDestroy.gameObject == null)
                return;
            
            GameObject.Destroy(objectToDestroy.gameObject);
        }
    }

    public IEnumerator Destroy(T objectToDestroy, float seconds = 0)
    {
        if (seconds > 0);
            yield return new WaitForSeconds(seconds);

        Destroy(objectToDestroy);
    }

    public void DestroyPool()
    {
        foreach (T poolObject in pool)
        {
            if(poolObject == null || poolObject.gameObject == null)
                continue;
            
            GameObject.Destroy(poolObject.gameObject);
        }
        
        destroyed = true;
        pool.Clear();
    }
    
    private List<T> PreCreateObjects()
    {
        List<T> createdObjs = new List<T>();
        
        for (int i = 0; i < size; i++)
        {
            T obj = PrecreateObject();
            createdObjs.Add(obj);
        }

        return createdObjs;
    }
    
    private T PrecreateObject()
    {
        T createdObject = GameObject.Instantiate(objPrefab, new Vector3(-9999f, -9999f, -9999f), Quaternion.identity, null);
        createdObject.CurrentPool = this;
        createdObject.OnPreStarted();
        pool.Enqueue(createdObject);
        createdObject.gameObject.SetActive(false);
        
        return createdObject;
    }
}

public interface IPoolObject<T> where T: MonoBehaviour, IPoolObject<T>
{
    ObjectPool<T> CurrentPool { get; set; }
    void OnPreStarted();
    void OnStart();
}

