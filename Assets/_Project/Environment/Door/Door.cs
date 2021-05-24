using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public UnityEvent OnOpen;
    public UnityEvent OnClose;

    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;

    private Animator animator;

    private readonly int openendHash = Animator.StringToHash("Opened");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (_isOpen == value)
                return;

            _isOpen = value;

            if (value)
                OnOpen.Invoke();
            else
                OnClose.Invoke();

            spriteRenderer.enabled = !value;
            doorCollider.enabled = !value;

            animator.SetBool(openendHash, value);
        }
    }

    public void OnEnterRoom(GameObject obj)
    {
        Debug.Log($"{obj.name} has entered the room.", obj);
        
        if (obj.CompareTag("Player"))
            IsOpen = true;
    }

    public void OnExitRoom(GameObject obj)
    {
        Debug.Log($"{obj.name} has exited the room.", obj);

        if (obj.CompareTag("Player"))
            IsOpen = false;
    }
}
