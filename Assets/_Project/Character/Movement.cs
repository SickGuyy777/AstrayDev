using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float acceleration = 12;
    [SerializeField] private float deceleration = 12;
    [SerializeField] private float lookSpeed = 10f;
    
    
    public Vector2 Velocity { get; private set; }
    
    private Rigidbody2D rb;


    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate() => rb.velocity = Velocity;

    public void Move(Vector2 direction, float deltaTime)
    {

        

        direction.Normalize();

            bool isMoving = direction != Vector2.zero;
            Vector2 targetVelocity = isMoving ? direction * maxSpeed : Vector2.zero;
            float accelerationType = isMoving ? acceleration : deceleration;

            Velocity = Vector2.Lerp(Velocity, targetVelocity, accelerationType * deltaTime);
        
    }

    public void LookInDirection(Vector2 direction, float deltaTime)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), lookSpeed * deltaTime);
    }
}
