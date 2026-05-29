using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float jumpPower = 10f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Attack")]
    [SerializeField] GameObject attackHitBox;

    Rigidbody2D rb;
    Animator animator;

    bool isGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackHitBox.SetActive(false);
    }

    void Update()
    {
        GroundCheck();

        Jump();

        Attack();

        AnimatorUpdate();
    }

    void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

            animator.SetTrigger("Jump");
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("Attack");
        }
    }

    void AnimatorUpdate()
    {
        animator.SetBool("IsGround", isGround);
    }

    // AnimationEvent‚©‚çŒÄ‚Ô
    public void AttackStart()
    {
        attackHitBox.SetActive(true);
    }

    // AnimationEvent‚©‚çŒÄ‚Ô
    public void AttackEnd()
    {
        attackHitBox.SetActive(false);
    }
}