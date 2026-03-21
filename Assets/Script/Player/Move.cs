using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [Header("移动参数")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("跳跃参数")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("相机设置")]
    [SerializeField] private Camera mainCamera;

    protected Animator animator;
    private Attack attack;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpPressed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        rb = GetComponent<Rigidbody>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        if (attack.isAttacking)
            return;
        HandleMovement();
        HandleJump();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            jumpPressed = true;
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        
        if (moveDirection.magnitude > 0.1f)
        {
            //获取相机方向
            Vector3 cameraForward = mainCamera != null ? mainCamera.transform.forward : Vector3.forward;
            Vector3 cameraRight = mainCamera != null ? mainCamera.transform.right : Vector3.right;
            
            //去除Y轴分量，保持水平
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();
            
            //根据相机方向计算实际移动方向
            Vector3 actualMoveDirection = cameraRight * moveInput.x + cameraForward * moveInput.y;
            actualMoveDirection.Normalize();
            
            //计算目标旋转角度
            Quaternion targetRotation = Quaternion.LookRotation(actualMoveDirection);
            
            //平滑旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            //应用移动
            Vector3 targetVelocity = actualMoveDirection * maxSpeed;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;
        }
        else
        {
            Vector3 stopVelocity = new Vector3(0f, rb.velocity.y, 0f);
            rb.velocity = stopVelocity;
        }
    }

    private void HandleJump()
    {
        if (jumpPressed)
        {
            float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
            jumpPressed = false;
            
            OnJumpStart();
        }
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    #region 动画接口

    private void HandleAnimation()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        
        if (isMoving)
        {
            OnWalk();
        }
        else
        {
            OnIdle();
        }

        if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                OnJumpUp();
            }
            else
            {
                OnJumpDown();
            }
        }
        else if (isGrounded && rb.velocity.y <= 0)
        {
            OnLand();
        }
    }

    protected virtual void OnIdle()
    {
        animator.SetBool("isWalk", false);
    }
    protected virtual void OnWalk()
    {
        animator.SetBool("isWalk", true);
    }
    protected virtual void OnJumpStart()
    {
        animator.SetBool("isJump", true);
    }
    protected virtual void OnJumpUp()
    {
    }
    protected virtual void OnJumpDown()
    {
    }
    protected virtual void OnLand()
    {
        animator.SetBool("isJump", false);
    }
    #endregion
}
