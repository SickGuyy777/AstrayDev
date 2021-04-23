using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalkAnimation(bool moving)
    {
        animator.SetBool("Moving", moving);
    }
}
