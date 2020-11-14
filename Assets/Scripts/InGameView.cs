using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public event Action inputRihgt;
    public event Action inputLeft;
    public event Action inputUp;
    public event Action inputDown;


    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            inputRihgt();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            inputLeft();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            inputUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            inputDown();
        }


    }


}
