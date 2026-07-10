using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] Transform spawnPointA; // 通常確率1
    [SerializeField] Transform spawnPointB; // 通常確率2
    [SerializeField] Transform spawnPointC; // 低確率（レア用など）
    [SerializeField] float lowProbability = 0.1f; // ポイントCの出現確率 (0.1 = 10%)

    [Header("Enemy Prefabs Lists")]
    [SerializeField] List<GameObject> normalEnemies = new List<GameObject>(); // A, B用の敵リスト
    [SerializeField] List<GameObject> rareEnemies = new List<GameObject>();   // C用の敵リスト

    [Header("Spawn Limits")]
    [SerializeField] int maxEnemiesOnScreen = 3; // 画面上の最大数

    [Header("Spawn Intervals (Seconds)")]
    [SerializeField] float minSpawnInterval = 2.0f; // 最小の出現間隔
    [SerializeField] float maxSpawnInterval = 5.0f; // 最大の出現間隔
    [SerializeField] float speedUpPer1000Score = 0.2f; // スコア1000ごとに短縮する秒数
    [SerializeField] float limitMinInterval = 0.5f;   // これ以上は早くならない限界値

    private float timer;
    private float currentInterval;
    private int lastCheckedScore = 0;
    private float difficultyOffset = 0f; // スコアによる短縮時間の合計

    void Start()
    {
        SetNextSpawnInterval();
    }

    void Update()
    {
        // スコアによる難易度（出現速度）の更新
        UpdateDifficulty();

        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            timer = 0f;
            SetNextSpawnInterval();

            // 画面上のエネミーの数を数えて、上限未満なら生成する
            if (GetCurrentEnemyCount() < maxEnemiesOnScreen)
            {
                SpawnEnemy();
            }
        }
    }

    // 次の出現までの時間をランダムに決める（スコアによる高速化も反映）
    void SetNextSpawnInterval()
    {
        float min = Mathf.Max(minSpawnInterval - difficultyOffset, limitMinInterval);
        float max = Mathf.Max(maxSpawnInterval - difficultyOffset, limitMinInterval);
        currentInterval = Random.Range(min, max);
    }

    // スコアが1000増えるごとにスピードアップを計算
    void UpdateDifficulty()
    {
        if (ScoreManager.Instance == null) return;

        int currentScore = ScoreManager.Instance.CurrentScore;
        // スコアが1000の倍数をまたいだかチェック
        if (currentScore / 1000 > lastCheckedScore / 1000)
        {
            int levelsUp = (currentScore / 1000) - (lastCheckedScore / 1000);
            difficultyOffset += levelsUp * speedUpPer1000Score;
            Debug.Log($"スコアが1000増加！ 出現速度がアップしました（短縮合計: {difficultyOffset}秒）");
        }
        lastCheckedScore = currentScore;
    }

    // 画面上に存在する「Enemy」タグのオブジェクト数を数える
    int GetCurrentEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }

    // 敵を生成するメイン処理
    void SpawnEnemy()
    {
        Transform chosenPoint = null;
        GameObject chosenPrefab = null;

        // 確率で場所と敵のリストを決める
        float randomValue = Random.value; // 0.0 〜 1.0 のランダムな値

        if (randomValue < lowProbability)
        {
            // 低確率：ポイントCから、レア敵リストから生成
            chosenPoint = spawnPointC;
            chosenPrefab = GetRandomPrefabFromList(rareEnemies);
            Debug.Log("低確率のポイントCからエネミーが生成されます！");
        }
        else
        {
            // 通常確率：ポイントAかBを50%ずつの確率で選ぶ
            chosenPoint = (Random.value < 0.5f) ? spawnPointA : spawnPointB;
            chosenPrefab = GetRandomPrefabFromList(normalEnemies);
        }

            if (chosenPoint != null && chosenPrefab != null)
            {
                Instantiate(chosenPrefab, chosenPoint.position, chosenPrefab.transform.rotation);
            }
    }

    // リストの中からランダムに1つのプレハブを返す関数
    GameObject GetRandomPrefabFromList(List<GameObject> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("エネミーのリストが空っぽです！");
            return null;
        }
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
}