using UnityEngine;

public class BasicMutantAI : MonoBehaviour, IPoolObject<BasicMutantAI>
{
    private Movement movement;
    private CharacterAnimator animator;
    private AIVision vision;
    private HealthManager healthManager;

    public HealthManager HealthManager => healthManager;
    
    public ObjectPool<BasicMutantAI> CurrentPool { get; set; }

    
    public void OnPreStarted()
    {
        movement = GetComponent<Movement>();
        animator = GetComponent<CharacterAnimator>();
        vision = GetComponent<AIVision>();
        healthManager = GetComponent<HealthManager>();

        healthManager.OnDied += Die;
    }

    public void OnStart()
    {
        healthManager.Health = healthManager.MaxHealth;
    }

    private void Update()
    {
        Vector2 movementDirection = vision?.Target != null ? (Vector2)(vision.Target.transform.position - transform.position).normalized : Vector2.zero;
        Vector2 lookDirection = vision?.Target != null ? movementDirection : (Vector2)transform.right;
        
        movement.Move(movementDirection, Time.deltaTime);
        movement.LookInDirection(lookDirection, Time.deltaTime);
        animator?.SetWalkAnimation(movement.Velocity.magnitude > .2f);
    }

    private void Die()
    {
        if(CurrentPool == null)
            Destroy(this.gameObject);
        else
            CurrentPool.Destroy(this);
    }
}
