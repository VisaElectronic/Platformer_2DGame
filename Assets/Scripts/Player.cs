using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    public Vector2 wallJumpDirection;

    private float defaultJumpForce;

    private bool canDoubleJump = true;
    private bool canMove;

    private bool readyToLand;


    [SerializeField] private float bufferJumpTime;
    private float bufferJumpCounter;

    [SerializeField] private float cayoteJumpTime;
    private float cayoteJumpCounter;
    private bool canHaveCayoteJump;

    private float defaultGravityScale;

    private bool isKnocked;
    private bool canBeKnocked = true;
    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    private RaycastHit2D isGrounded;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;

    private bool facingRight = true;
    private int facingDirection = 1;

    [Header("Controlls info")]
    private float movingInput;
    private float vInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        defaultJumpForce = jumpForce;
        defaultGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControllers();
        FlipController();
        CollisionChecks();
        InputChecks();


        bufferJumpCounter -= Time.deltaTime;
        cayoteJumpCounter -= Time.deltaTime;

        if (isGrounded)
        {
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }

            canMove = true;

            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }

            canHaveCayoteJump = true;

            if (readyToLand)
            {
                readyToLand = false;
            }
        }
        else
        {
            if (!readyToLand)
                readyToLand = true;

            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpCounter = cayoteJumpTime;
            }
        }


        if (canWallSlide)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.1f);
        }


        Move();
    }

    private void AnimationControllers()
    {
        bool isMoving = rb.linearVelocity.x != 0;

        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void InputChecks()
    {
        movingInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");


        if (vInput < 0)
            canWallSlide = false;

        if (Input.GetButtonDown("Jump"))
            JumpButton();
    }

    public void ReturnControll()
    {
        rb.gravityScale = defaultGravityScale;
    }

    public void JumpButton()
    {
        if (!isGrounded)
            bufferJumpCounter = bufferJumpTime;

        if (isWallSliding)
        {
            WallJump();
            canDoubleJump = true;
        }
        else if (isGrounded || cayoteJumpCounter > 0)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
        }

        canWallSlide = false;
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canBeKnocked = true;
    }

    private void Move()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(moveSpeed * movingInput, rb.linearVelocity.y);
    }

    private void WallJump()
    {
        canMove = false;
        rb.linearVelocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void Push(float pushForce)
    {

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, pushForce);
    }

    private void FlipController()
    {
        if (facingRight && rb.linearVelocity.x < 0)
            Flip();
        else if (!facingRight && rb.linearVelocity.x > 0)
            Flip();

    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsWall);

        if (isWallDetected && rb.linearVelocity.y < 0)
            canWallSlide = true;

        if (!isWallDetected)
        {
            isWallSliding = false;
            canWallSlide = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
    }
}
