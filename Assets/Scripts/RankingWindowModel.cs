using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingWindowModel : MonoBehaviour
{
    public Ranking UserInfo = new Ranking();
    [SerializeField] private Ranking ranking;

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

        UserInfo.user_id = "name";
        UserInfo.user_name = "ジュリジュリ";
        UserInfo.score = 10;
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
        yield return request.Send();



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
            Debug.Log(UserInfo.user_name);
            
        }
    }
}
