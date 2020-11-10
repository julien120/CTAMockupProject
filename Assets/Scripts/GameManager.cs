using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
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
}