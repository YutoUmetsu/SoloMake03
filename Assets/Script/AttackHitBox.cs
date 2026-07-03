using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // デバッグ用ログ（何かに当たったら必ず出す）
        Debug.Log($"[HitBox] 衝突検知! 相手の名前: {collision.gameObject.name}, タグ: {collision.gameObject.tag}");

        // 敵のタグ「Enemy」に当たったか判定
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("→ 敵のタグと一致しました！死亡処理を試みます。");

            // 衝突したオブジェクト自身、またはその親オブジェクトから NormalEnemy を探す
            NormalEnemy enemy = collision.GetComponentInParent<NormalEnemy>();

            if (enemy == null)
            {
                // もし親にもなければ、衝突したオブジェクト自体から探す
                enemy = collision.GetComponent<NormalEnemy>();
            }

            if (enemy != null)
            {
                enemy.Die();
                Debug.Log("→ 敵の死亡処理（Die）を実行しました！");
            }
            else
            {
                Debug.LogError("⚠️警告: タグはEnemyですが、NormalEnemyスクリプトが見つかりません！");
            }
        }
    }
}