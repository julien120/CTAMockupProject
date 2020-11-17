using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    private void Start()
    {
        resultText.text = PlayerPrefs.GetInt(InGamePresenter.ScoreData, 0).ToString();
    }

    public void OnClickRetryButton()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
