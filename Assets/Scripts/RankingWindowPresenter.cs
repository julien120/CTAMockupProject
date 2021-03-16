using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class RankingWindowPresenter : MonoBehaviour
{
    [SerializeField] private RankingWindowView rankingWindowView;
   // private readonly ReactiveProperty<int> rankingScore = new ReactiveProperty<int>();
   // public IObservable<int> OnRankingScore => rankingScore;
    //rankingWindowView.SetRankingScore(rankingScore.Value);

    // Start is called before the first frame update
    public void Initialize()
    {
        
        
    }

    public void SetRankingScore(int score)
    {
        rankingWindowView.SetRankingScore(score);
    }
}
