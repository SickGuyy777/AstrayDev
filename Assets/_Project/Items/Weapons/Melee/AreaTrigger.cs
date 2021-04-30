using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaTrigger : MonoBehaviour
{
    private List<GameObject> detectedObjects = new List<GameObject>();
    private Collider2D col;

    public event Action<GameObject> OnNewObjectEntered = delegate {  };
    public event Action<GameObject> OnNewObjectLeft = delegate {  };


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        
        detectedObjects.Add(obj);
        OnNewObjectEntered.Invoke(obj);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        
        detectedObjects.Remove(other.gameObject);
        OnNewObjectLeft.Invoke(obj);
    }

    public T[] GetObjects<T>()
    {
        List<T> detectedTypes = new List<T>();
        
        foreach (GameObject obj in detectedObjects)
        {
            T detectedType = obj.GetComponent<T>();
            
            if(detectedType != null)
                detectedTypes.Add(detectedType);
        }

        return detectedTypes.ToArray();
    }
}
