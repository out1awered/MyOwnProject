using System.Collections;
using System.Collections.Generic;



using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 lastMoveDirection;
    private Vector3 moveDirection;
    public float walkAcceleration = 8f;
    public float sprintAcceleration = 14f;
    public float deceleration = 12f;

    private float currentSpeed = 0f;
    public float moveSpeed = 5f;
    public float sprintSpeed = 9f;
    public float jumpForce = 5f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Animator animator;
    private bool isMoving = false;
    private bool isSprinting = false;

    public float stepHeight = 0.3f;
    public float stepCheckDistance = 0.5f;

    private bool canStepUp = false;

    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;

    public float maxSlopeAngle = 45f;

    private bool jumpRequested = false;

    private RaycastHit slopeHit;
    private bool onSlope;

    private float jumpLockTimer = 0f;
    public float jumpLockTime = 0.15f;

    public float stepUpSpeed = 2.5f;

    private bool OnSlope()
    {
        if (Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            out slopeHit,
            0.3f,
            groundLayer
        ))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle > 0f && angle <= maxSlopeAngle;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private bool CheckStep()
    {
        if (!isMoving)
        {
            return false;
        }

        Vector3 lowerOrigin = groundCheck.position + Vector3.up * 0.05f;

        Vector3 upperOrigin =
            groundCheck.position +
            Vector3.up * stepHeight;

        bool lowerHit = Physics.Raycast(
            lowerOrigin,
            moveDirection,
            stepCheckDistance,
            groundLayer
        );

        bool upperHit = Physics.Raycast(
            upperOrigin,
            moveDirection,
            stepCheckDistance,
            groundLayer
        );

        return lowerHit && !upperHit;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        onSlope = OnSlope();

        isGrounded = Physics.Raycast(
              groundCheck.position,
              Vector3.down,
              0.3f,
              groundLayer);

        if (jumpLockTimer > 0f)
        {
            jumpLockTimer -= Time.deltaTime;
        }





        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            8f * Time.deltaTime
        );

        Vector3 movement = cameraForward * z + cameraRight * x;

        isMoving = movement.magnitude > 0.1f;

        isSprinting = isMoving && Input.GetKey(KeyCode.LeftShift);


        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isGrounded", isGrounded);

        if (isMoving)
        {
            lastMoveDirection = movement;
        }

        movement = Vector3.ClampMagnitude(movement, 1f);

        float targetSpeed = 0f;

        if (isMoving)
        {
            targetSpeed = moveSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetSpeed = sprintSpeed;
            }
        }
        if (isMoving)
        {
            float currentAcceleration = walkAcceleration;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentAcceleration = sprintAcceleration;
            }

            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                targetSpeed,
                currentAcceleration * Time.deltaTime
            );
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }



        if (isMoving)
        {
            moveDirection = movement;
        }
        else
        {
            moveDirection = lastMoveDirection;
        }

        canStepUp = CheckStep();
        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
{
      
            jumpRequested = true;

        }
    }
    void FixedUpdate()
    {
        if (jumpRequested)
        {
            rb.useGravity = true;

            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );

            jumpLockTimer = jumpLockTime;

            jumpRequested = false;
            return;
        }
        if (canStepUp && isGrounded)
        {
            rb.MovePosition(
                rb.position + Vector3.up * stepUpSpeed * Time.fixedDeltaTime
            );
        }
        // 站在斜坡上不动
        if (onSlope && isGrounded && !isMoving && jumpLockTimer <= 0f)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            return;
        }

        rb.useGravity = true;

        // 在斜坡上移动
        if (onSlope && isGrounded && isMoving && jumpLockTimer <= 0f)
        {
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();

            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);

            float slopeSpeedMultiplier = 1f;

            // 只有上坡时减速
            if (Vector3.Dot(slopeMoveDirection, Vector3.up) > 0f)
            {
                slopeSpeedMultiplier = Mathf.Lerp(
                    1f,
                    0.6f,
                    slopeAngle / maxSlopeAngle
                );
            }

            rb.velocity =
                slopeMoveDirection *
                currentSpeed *
                slopeSpeedMultiplier;

            return;
        }

        // 普通平地移动
        Vector3 velocity = rb.velocity;

        velocity.x = moveDirection.x * currentSpeed;
        velocity.z = moveDirection.z * currentSpeed;

        rb.velocity = velocity;
    }
}