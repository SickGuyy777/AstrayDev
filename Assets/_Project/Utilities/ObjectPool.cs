using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolObject<T>
{
    public readonly T objPrefab;
    public int size { get; private set; }
    public readonly Queue<T> objects = new Queue<T>();


    public ObjectPool(T objPrefab, int poolSize)
    {
        this.objPrefab = objPrefab;
        this.size = poolSize;

        PreCreateObjects();
    }

    public T Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if(objects.Count <= 0)
            IncreaseSize();

        T createdObj = objects.Dequeue();

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
        objectToDestroy.gameObject.SetActive(false);
        objects.Enqueue(objectToDestroy);
    }

    public IEnumerator Destroy(T objectToDestroy, float seconds = 0)
    {
        if (seconds > 0);
            yield return new WaitForSeconds(seconds);
        
        objectToDestroy.gameObject.SetActive(false);
        objects.Enqueue(objectToDestroy);
    }

    public void DestroyPool()
    {
        foreach (T poolObject in objects)
        {
            if(poolObject == null || poolObject.gameObject == null)
                continue;
            
            GameObject.Destroy(poolObject.gameObject);
        }
        
        objects.Clear();
    }
    

    private void PreCreateObjects()
    {
        for (int i = 0; i < size; i++)
        {
            PrecreateObject();
        }
    }
    
    private T PrecreateObject()
    {
        T createdObject = GameObject.Instantiate(objPrefab, new Vector3(-9999f, -9999f, -9999f), Quaternion.identity, null);
        createdObject.CurrentPool = this;
        createdObject.OnPreStarted();
        objects.Enqueue(createdObject);
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

