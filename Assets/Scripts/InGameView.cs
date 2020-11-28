using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private Text scoreText;
    public readonly int[,] stageState = new int[RowStage, ColStage];

    //行列の数
    public const int RowStage = 4;
    public const int ColStage = 4;

    public event Action<int,int,int,int> CheckCell;

    private InGameModel inGameModel;

    private void Start()
    {
        inGameModel = GetComponent<InGameModel>();

        // ステージの初期状態を生成
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                stageState[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, RowStage), Random.Range(0, ColStage));
        var posB = new Vector2((posA.x + Random.Range(1, RowStage - 1)) % RowStage, (posA.y + Random.Range(1, ColStage - 1)) % ColStage);
        stageState[(int)posA.x, (int)posA.y] = 2;
        stageState[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < InGameModel.GenerationRate ? 2 : 4;

        // ステージの初期状態をViewに反映
        ReflectStage();
    }

    private void Update()
    {
        InputKey();
    }
    


        public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ReflectStage()
    {
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                //これだけViewのaction型変数に置き換えるのが正しい粒度？
                cells[i * RowStage + j].SetText(stageState[i, j]);
            }
        }
    }



    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (var col = ColStage; col >= 0; col--)
            {
                for (var row = 0; row < RowStage; row++)
                {
                    CheckCell(row, col, 1, 0);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (var row = 0; row < RowStage; row++)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    CheckCell(row, col, -1, 0);
                }
            }

        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (var row = 0; row < RowStage; row++)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    CheckCell(row, col, 0, -1);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (var row = RowStage; row >= 0; row--)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    CheckCell(row, col, 0, 1);
                }
            }
        }
    }


}
