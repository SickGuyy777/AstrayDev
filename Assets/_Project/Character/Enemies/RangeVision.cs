using UnityEngine;

public class RangeVision : AIVision
{
    [SerializeField] private float radius = 5f;
    
    
    protected override PlayerController DetectPlayer()
    {
        Debug.Log("yooo");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider2d in colliders)
        {
            PlayerController detectedPlayer = collider2d.GetComponent<PlayerController>();

            if (detectedPlayer != null)
                return detectedPlayer;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
