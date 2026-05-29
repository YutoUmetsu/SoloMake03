using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] string ChengeScene; // 遷移先シーン
    [SerializeField] bool isQuitButton=false;   // ゲーム終了ボタンにするならチェック

    public void Onclick()
    {
        // ゲーム終了フラグが真なら終了処理を実行
        if (isQuitButton)
        {
            QuitGame();
            return;
        }

        // ChengeSceneが設定済みなら遷移
        if (!string.IsNullOrEmpty(ChengeScene))
        {
            SceneManager.LoadScene(ChengeScene);
        }
        else
        {
            Debug.Log("遷移先が未登録");
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
            // Unityエディタの再生モードを停止
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドしたゲームを終了
        Application.Quit();
#endif
    }
}
