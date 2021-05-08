using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] private Transform head;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float sensitivity = 2;

    private CinemachinePOV cameraPov;
    

    [Header("Movement")]
    [SerializeField] [Range(0, 1)] private float inAirControl = .1f;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float acceleration = 8;
    [SerializeField] private float deceleration = 8;
    [SerializeField] private float jumpForce = 4;
    [SerializeField] private float gravityScale = 10;

    [Header("GroundCheck")] 
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = .8f;

    private Camera cam;
    private Rigidbody rb;
    private Vector3 velocity;
    private float xRot;
    private float yRot;
    
    private bool grounded;
    private bool IsGrounded
    {
        get => grounded;

        set
        {
            if (value == true && grounded == false)
                OnEnteredGround.Invoke();
            else if (value == false && grounded)
                OnLeftGround.Invoke();

            grounded = value;
        }
    }
    
    public event Action OnEnteredGround = delegate { };
    public event Action OnLeftGround = delegate { };


    private void Awake()
    {
        cam = Camera.main;
        cameraPov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        cameraPov.m_VerticalAxis.m_MaxSpeed = sensitivity;
        cameraPov.m_HorizontalAxis.m_MaxSpeed = sensitivity;
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        
        ApplyExtraGravity();
        
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    public void Move(Vector3 direction, float deltaTime)
    {
        direction.y = 0;
        bool moving = direction != Vector3.zero;
        Vector3 moveDirection = moving ? transform.right * direction.x + transform.forward * direction.z : Vector3.zero;

        float speed = moving ? acceleration : deceleration;
        float airControl = IsGrounded ? 1 : inAirControl;

        velocity = Vector3.Lerp(velocity, moveDirection * maxSpeed, speed * airControl * deltaTime);
    }

    public void UpdateLook()
    {
        transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
        head.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0, 0);
    }

    public void Jump()
    {
        if(!IsGrounded)
            return;
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ApplyExtraGravity()
    {
        if(IsGrounded)
            return;
        
        rb.AddForce(Vector3.down * gravityScale);
    }
    
    private void OnDrawGizmos()
    {
        if(groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
