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
        // 左Shiftから「J」キーに変更しました
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");

            // 前回のスコア加算テストを残す場合はここ
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(100);
            }
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
    // 敵から呼ばれるダメージ処理
    public void TakeDamage()
    {
        Debug.Log("プレイヤーがダメージを受けました！");

        // 【今後拡張可能】HPを減らす、ダメージアニメーションを再生するなど
        // 今回は仮に、食らったら少しノックバック（上に跳ねる）させてみます
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * -0.5f, 5f);
    }
}