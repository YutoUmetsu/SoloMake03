using UnityEngine;
// 1. 新しいInput Systemを使うための宣言を追加
using UnityEngine.InputSystem;

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
        // 2. 新しいインプットシステムの書き方に変更（スペースキー）
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            animator.SetTrigger("Jump");
        }
    }

    void Attack()
    {
        // 3. 新しいインプットシステムの書き方に変更（左Shiftキー）
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");
        }
    }

    void AnimatorUpdate()
    {
        animator.SetBool("IsGround", isGround);
    }

    public void AttackStart()
    {
        attackHitBox.SetActive(true);
    }

    public void AttackEnd()
    {
        attackHitBox.SetActive(false);
    }
}