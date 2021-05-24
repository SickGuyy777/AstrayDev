using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minDistance;
    
    private PathFinder pathFinder;
    private Rigidbody2D rb;
    private Vector2 mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);


    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
        rb = GetComponent<Rigidbody2D>();
        
        StartCoroutine(MoveToPath());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            pathFinder.UpdatePath(mousePos);
    }

    private IEnumerator MoveToPath()
    {
        while (true)
        {
            yield return null;
            Queue<Vector2> path = pathFinder.GetPath();
            
            bool checkPath = path != null && path.Count > 0;

            if (checkPath)
            {
                Vector2 direction = (path.Peek() - (Vector2) transform.position).normalized;
                rb.velocity = direction * moveSpeed;

                float distance = Vector2.Distance(transform.position, path.Peek());

                if (distance <= minDistance)
                {
                    path.Dequeue();
                    rb.velocity = Vector2.zero;
                }
            }
            else
                rb.velocity = Vector2.zero;
        }
    }
}
