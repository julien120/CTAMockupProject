using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingWindowModel : MonoBehaviour
{
    public UserData UserInfo = new UserData();

    //TODO:viewに後ほど移す予定
   [SerializeField] private GameObject rankElementUI;
   [SerializeField] private Transform verticalScoreUI;
   [SerializeField] private RankElementView rankElementView;


    //TODO
    //https://pacific-anchorage-24826.herokuapp.com /ranking をPOSTする
    //requestBodyは以下の通り
    //"user_id": "hogehoge",
    //"user_name": "user_name",
    //"score": highScore

    //帰ってきた内容をforeachで一個ずつ取り出す。取り出した個数に合わせてelementを生成し、それぞれのtextに描画。
    //intiateの第二引数で親要素を選択できた気がする。それでveatualGroupの空オブジェクト以下に設置する
    //string userId, string userName, int highScoreを引数にする？
    public void PostRankingScore(int score)
    {

        UserInfo.user_id = UserAccountData.UserId;//InputField.textを格納した変数を代入する
        UserInfo.user_name = UserAccountData.UserName;
        UserInfo.score = score;

        Debug.Log(UserAccountData.UserName);
        //var obj = new highScoreData インスタンスしなくてもいけるっぽい？なんで？
        //{
        //    DataHighScore = highScore
        //};

        string userJson = JsonUtility.ToJson(UserInfo);
        //UnityWebRequest request = UnityWebRequest.Post(APIName.URI + APIName.RankingQuery);

        StartCoroutine(GetEventsInformation(userJson));
    }

    IEnumerator GetEventsInformation(string myjson)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
        var request = new UnityWebRequest(APIName.URI+APIName.RankingQuery, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //以上構文だと思っていた内容をメソッド分割し、またトークンを用意しているのがハッカソンのやつ
        //ユーザー登録の処理は思ったよりベクトルが違うのかな?

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
            //Debug.Log(request.downloadHandler.text);
            //デシリアライズ処理
            RankingData rankingData = JsonUtility.FromJson<RankingData>(request.downloadHandler.text);

            //TODO:0番目のインデックスをとるときだけ過去の叩いたjsonになったり、88888のままになったりするランクはずっと1
            int count = 0;
            foreach (UserRankingData i in rankingData.ranking)
            {
                //後ほどMVPの分離と生成したelementへのアタッチをやる
                Instantiate(rankElementUI, verticalScoreUI,false);
                rankElementView.RankScoreText.text = rankingData.ranking[count].score.ToString();
                rankElementView.RankNameText.text = rankingData.ranking[count].name.ToString();
                rankElementView.RankRankText.text = rankingData.ranking[count].rank.ToString();
                count++;
            }

        }
    }
}
