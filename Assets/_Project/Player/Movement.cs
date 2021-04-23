using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float acceleration = 12;
    [SerializeField] private float deceleration = 12;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 velocity;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayWalkAnimation();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    public void Move(Vector2 direction, float deltaTime)
    {
        direction.Normalize();

        bool isMoving = direction != Vector2.zero;
        Vector2 targetVelocity = isMoving ? direction * maxSpeed : Vector2.zero;
        float accelerationType = isMoving ? acceleration : deceleration;

        velocity = Vector2.Lerp(velocity, targetVelocity, accelerationType * deltaTime);
    }

    public void LookInDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void PlayWalkAnimation()
    {
        bool isMoving = velocity.magnitude > .1f;
        animator.SetBool("Moving", isMoving);
    }
}
