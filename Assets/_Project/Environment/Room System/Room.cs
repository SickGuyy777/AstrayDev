using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public UnityEvent<GameObject> OnObjectEnter;
    public UnityEvent<GameObject> OnObjectExit;

    public string[] tagList;

    [HideInInspector]
    public List<GameObject> objectsInsideRoom;

    [HideInInspector]
    public bool useBlackList;

    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private Vector2Int scale;    

    private void Start()
    {
        TickClock.OnTick += UpdateLoop;
    }

    private void UpdateLoop()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, transform.eulerAngles.z);
        if (hits.Length <= 0)
        {
            for (int i = 0; i < objectsInsideRoom.Count; i++)
            {

            }

            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var contains = tagList.Contains(collision.tag);
        if (tagList.Length == 0
                || contains && useBlackList || !contains && !useBlackList)
        {
            OnObjectEnter.Invoke(collision.gameObject);

            if (!objectsInsideRoom.Contains(collision.gameObject))
                objectsInsideRoom.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var contains = tagList.Contains(collision.tag);
        if (tagList.Length == 0
                || contains && useBlackList || !contains && !useBlackList)
        {
            OnObjectExit.Invoke(collision.gameObject);
            objectsInsideRoom.Remove(collision.gameObject);
        }
    }
}