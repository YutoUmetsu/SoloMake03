using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // どこからでも ScoreManager.Instance.AddScore() のように呼べるようにする（シングルトン構造）
    public static ScoreManager Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] float scorePerSecond = 10f; // 1秒間あたりの時間経過スコア

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    private float scoreAccumulator = 0f;
    private const string HighScoreKey = "HighScore"; // 保存用のキーワード

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移してもスコアを維持したい場合
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ゲーム起動時に保存されているハイスコアを読み込む
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    void Update()
    {
        // 時間経過でのスコア加算
        scoreAccumulator += scorePerSecond * Time.deltaTime;
        if (scoreAccumulator >= 1f)
        {
            int amountToAdd = Mathf.FloorToInt(scoreAccumulator);
            AddScore(amountToAdd);
            scoreAccumulator -= amountToAdd;
        }
    }

    // スコアを増やす関数（時間経過や、敵を倒したとき、プレイヤーから呼ぶ）
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        Debug.Log($"現在のスコア: {CurrentScore}");

        // 現在のスコアがハイスコアを超えたら更新
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            // ハイスコアを即座に保存
            SaveHighScore();
        }
    }

    // ハイスコアを保存する関数
    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save(); // 確実にディスクに書き込む
    }

    // ゲームオーバー時などにスコアをリセットする関数
    public void ResetScore()
    {
        CurrentScore = 0;
        scoreAccumulator = 0f;
    }
}