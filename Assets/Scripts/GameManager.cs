using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    private int[,] stageState = new int[4, 4];

    private void Start()
    {
        // ステージの初期状態を生成
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                stageState[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, 4), Random.Range(0, 4));
        var posB = new Vector2((posA.x + Random.Range(1, 3)) % 4, (posA.y + Random.Range(1, 3))% 4);
        stageState[(int)posA.x, (int)posA.y] = 2;
        stageState[(int) posB.x, (int) posB.y] = Random.Range(0, 1.0f) < 0.5f ? 2 : 4;
        
        // ステージの初期状態をViewに反映
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                cells[i * 4 + j].SetText(stageState[i, j]);
            }
        }
    }

    private void Update()
    {
        // 入力検知
        if (Input.GetKeyDown(KeyCode.RightArrow)) { }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { }

        if (Input.GetKeyDown(KeyCode.DownArrow)) { }
    }

    private bool IsGameOver(int[][] stageState)
    {
        // 空いている場所があればゲームオーバーにはならない
        for (var i = 0; i < stageState.Length; i++)
        {
            for (var j = 0; j < stageState[i].Length; j++)
            {
                if (stageState[i][j] <= 0)
                {
                    return false;
                }
            }
        }

        // 合成可能なマスが一つでもあればゲームオーバーにはならない
        for (var i = 0; i < stageState.Length; i++)
        {
            for (var j = 0; j < stageState[i].Length; j++)
            {
                var state = stageState[i][j];
                var canMerge = false;
                if (i > 0)
                {
                    canMerge |= state == stageState[i - 1][j];
                }

                if (i < stageState.Length - 1)
                {
                    canMerge |= state == stageState[i + 1][j];
                }

                if (j > 0)
                {
                    canMerge |= state == stageState[i][j - 1];
                }

                if (j < stageState[i].Length - 1)
                {
                    canMerge |= state == stageState[i][j + 1];
                }

                if (canMerge)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void LoadResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
}