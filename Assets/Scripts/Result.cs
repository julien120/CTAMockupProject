using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    private void Start()
    {
        resultText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.ScoreData).ToString();
    }

    public void OnClickRetryButton()
    {
        SceneController.Instance.LoadInGameScene();
    }
}
