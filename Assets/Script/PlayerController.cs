using UnityEngine;
// 1. 新しいInput Systemを使うための宣言を追加
using UnityEngine.InputSystem;
using System.Collections; // ★コルーチン（秒数カウント）を使うために必要です

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

    [Header("Damage Settings")]
    [SerializeField] float invincibleDuration = 1.0f; // ★無敵時間（秒）

    Rigidbody2D rb;
    Animator animator;

    bool isGround;
    bool isInvincible = false; // ★現在無敵状態かどうかを管理するフラグ

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
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            animator.SetTrigger("Jump");
        }
    }

    void Attack()
    {
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
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

    // 敵から呼ばれるダメージ処理
    public void TakeDamage()
    {
        // ★無敵時間中なら、これ以降のダメージ処理を完全に無視（スルー）する
        if (isInvincible) return;

        Debug.Log("プレイヤーがダメージを受けました！");

        // 食らったら少しノックバック（上に跳ねる）
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * -0.5f, 5f);

        // ★無敵化コルーチンを開始
        StartCoroutine(InvincibleRoutine());
    }

    // 時間をカウントして無敵状態を解除する特別な関数（コルーチン）
    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        Debug.Log("無敵状態スタート（物理的にもすり抜けます）");

        // ★「Player」レイヤーと「Enemy」レイヤーの衝突判定を【無効】にする
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true
        );

        // 指定された秒数（invincibleDuration）だけ待つ
        yield return new WaitForSeconds(invincibleDuration);

        // ★「Player」レイヤーと「Enemy」レイヤーの衝突判定を【有効】に戻す
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            false
        );

        isInvincible = false;
        Debug.Log("無敵状態が解除されました（衝突判定が戻りました）");
    }
}