using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingWindowModel : MonoBehaviour
{
    public UserData UserInfo = new UserData();
   [SerializeField] private GameObject rankElementUI;

    // Start is called before the first frame update
    void Start()
    {
        PostRankingScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //TODO
    //https://pacific-anchorage-24826.herokuapp.com /ranking をPOSTする
    //requestBodyは以下の通り
    //"user_id": "hogehoge",
    //"user_name": "user_name",
    //"score": highScore

    //帰ってきた内容をforeachで一個ずつ取り出す。取り出した個数に合わせてelementを生成し、それぞれのtextに描画。
    //intiateの第二引数で親要素を選択できた気がする。それでveatualGroupの空オブジェクト以下に設置する
    //string userId, string userName, int highScoreを引数にする？
    public void PostRankingScore()
    {

        UserInfo.user_id = "name";//InputField.textを格納した変数を代入する
        UserInfo.user_name = "ジュリジュリ";
        UserInfo.score = 10;

        //var obj = new highScoreData インスタンスしなくてもいけるっぽい？なんで？
        //{
        //    DataHighScore = highScore
        //};

        string myjson = JsonUtility.ToJson(UserInfo);
        //UnityWebRequest request = UnityWebRequest.Post(APIName.URI + APIName.RankingQuery);

        StartCoroutine(GetEventsInformation(myjson));
    }

    IEnumerator GetEventsInformation(string myjson)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
        var request = new UnityWebRequest(APIName.URI+APIName.RankingQuery, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //URLに接続して結果が戻ってくるまで待機
        yield return request.SendWebRequest();



        //エラーが出ていないかチェック
        if (request.isNetworkError)
        {
            //通信失敗
            Debug.Log(request.error);
        }
        else
        {
            //通信成功
            Debug.Log(request.downloadHandler.text);
            //デシリアライズ処理
            RankingData rankingData = JsonUtility.FromJson<RankingData>(request.downloadHandler.text);

            int count = 0;
            foreach (UserRankingData i in rankingData.ranking)
            {
                
                Debug.Log(rankingData.ranking[count].name);
                Debug.Log(rankingData.ranking[count].rank);
                Debug.Log(rankingData.ranking[count].score);
                count++;

                //後ほどMVPの分離と生成したelementへのアタッチをやる
                //Instantiate(rankElementUI,);
            }
 
        }
    }
}
