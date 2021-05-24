using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class Room : MonoBehaviour
{
    public UnityEvent<GameObject> OnObjectEnter;
    public UnityEvent<GameObject> OnObjectExit;

    public static event Action<Room, GameObject> OnRoomEntered;
    public static event Action<Room, GameObject> OnRoomExited;

    [HideInInspector]
    public List<GameObject> objectsInsideRoom;

    [SerializeField]
    private bool showGizmos;

    private void Start() => TickClock.OnTick += UpdateLoop;

    private void UpdateLoop()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, transform.eulerAngles.z);
        if (hits.Length <= 0)
        {
            foreach (GameObject obj in objectsInsideRoom.ToList())
                RemoveFromRoom(obj);

            return;
        }

        if (hits.Length == 1)
        {
            var hit = hits[0].gameObject;

            bool clearObjects = objectsInsideRoom.Count > 1 || objectsInsideRoom.Count > 0 && objectsInsideRoom[0] != hit;
            if (clearObjects)
            {
                foreach (GameObject obj in objectsInsideRoom.ToList())
                {
                    bool isHit = hit == obj;
                    if (!isHit)
                        RemoveFromRoom(obj);
                }
            }

            AddToRoom(hit.gameObject);

            return;
        }

        // hits.Lenght > 1
        var hitObjects = new List<GameObject>();
        foreach (Collider2D hitCollider in hits)
        {
            var hit = hitCollider.gameObject;
            AddToRoom(hit);

            hitObjects.Add(hit);
        }

        foreach (GameObject obj in objectsInsideRoom.ToList())
        {
            bool isNoLongerInRoom = !hitObjects.Contains(obj);
            if (isNoLongerInRoom)
                RemoveFromRoom(obj);
        }

        void RemoveFromRoom(GameObject obj)
        {
            if (!IsInRoom(obj))
                return;

            OnObjectExit.Invoke(obj);
            OnRoomExited?.Invoke(this, obj);
            objectsInsideRoom.Remove(obj);
        }

        void AddToRoom(GameObject obj)
        {
            if (IsInRoom(obj))
                return;

            OnObjectEnter.Invoke(obj);
            OnRoomEntered?.Invoke(this, obj);
            objectsInsideRoom.Add(obj);
        }

        bool IsInRoom(GameObject obj) => objectsInsideRoom.Contains(obj);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}