﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class RankingWindowView : MonoBehaviour
{
    [SerializeField] private GameObject window;
    //[SerializeField] private Text scoreText;

    private readonly Subject<Unit> drawRanking = new Subject<Unit>();
    public IObservable<Unit> DrawRanking => drawRanking;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 1.menuを表示
    /// 2.ランキングデータを描画
    /// </summary>
    public void OpenWindow()
    {
        window.SetActive(true);
        drawRanking.OnNext(Unit.Default);
    }

    //public void SetRankingScore(int rankingScore)
    //{

    //    scoreText.text = $" {rankingScore}";

    //}

    public void ClosedWindow()
    {
        window.SetActive(false);
    }

    //TODO
    //https://pacific-anchorage-24826.herokuapp.com /ranking をPOSTする
    //requestBodyは以下の通り
    //"user_id": "hogehoge",
    //"user_name": "user_name",
    //"score": highScore


}
