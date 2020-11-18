using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;

    //_instanceに一意のインスタンスを格納し、これのみ参照する構造
    public static SceneController Instance
    {

        get
        {
            if (instance == null)
            {

                GameObject single = new GameObject();
                //_instanceに格納されてる値を管理する
                instance = single.AddComponent<SceneController>();
                //scene跨いでもインスタンスが残るのでnull処理に行かない
                DontDestroyOnLoad(instance);

            }
            return instance;

        }
    }

    public void LoadResultScene()
    {
        SceneManager.LoadScene(SceneName.ResultScene);
    }

    public void LoadInGameScene()
    {
        SceneManager.LoadScene(SceneName.InGameScene);
    }
}