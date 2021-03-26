using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class RankingWindowPresenter : MonoBehaviour
{
    [SerializeField] private RankingWindowView rankingWindowView;
    private readonly Subject<Unit> postRanking = new Subject<Unit>();
    public IObservable<Unit> PostRanking => postRanking;

    // Start is called before the first frame update
    public void Initialize()
    {
        
        rankingWindowView.DrawRanking.Subscribe(_ => postRanking.OnNext(Unit.Default));
    }

}
