using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] bool moveLeft = true;

    [Header("Score")]
    [SerializeField] int scoreValue = 300;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // 左右の移動速度を計算
        float direction = moveLeft ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // ★【追加】x座標が -10 未満になったら自壊する
        if (transform.position.x < -10f)
        {
            Debug.Log($"{gameObject.name} が画面外（x < -10）に出たため、自動消滅しました。");
            Destroy(gameObject);
        }
    }

    // プレイヤー自身にぶつかった時の判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }

    // AttackHitBoxから呼び出される死亡処理
    public void Die()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }
}