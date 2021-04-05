using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Title : MonoBehaviour
{
    [SerializeField] private Text eventsText;
    [SerializeField] private Text startText;
    [SerializeField] private Text endText;

    [SerializeField] private InputField userIdInputField;
    [SerializeField] private InputField userNameInputField;


    private void Start()
    {
        userIdInputField.onEndEdit.AddListener(AddUserIdQuery);
        userNameInputField.onEndEdit.AddListener(SetUserData);
    }


    /// <summary>
    ///  検索クエリーを元にAPIを叩く
    ///  エンコードしたURIはコレ↓
    ///  https://pacific-anchorage-24826.herokuapp.com/event?user_id=0
    ///  叩いたAPIをJSONにパースし、第一階層にあるname、start_time、end_timeを受け取る
    ///  Debug.Logに一旦描画する
    /// </summary>
    //TODO:UniTaskで実装したい
    IEnumerator GetEventsInformation(string userId)
    {
        //rubyだとwww_encodeみたいなのでqueryのハードコーディングを防ぐプロパティがあるけどunityはないんかな
        using (UnityWebRequest request = UnityWebRequest.Get(APIName.URI + APIName.EventQuery + "?user_id=" + userId)) {

            //URLに接続して結果が戻ってくるまで待機
            yield return request.SendWebRequest();

        //エラーが出ていないかチェック
        if (request.isNetworkError)
        {
            //通信失敗
            Debug.Log(request.error);
        }
        else if (request.isHttpError)
        {
            //通信失敗:HTTPステータスがエラーを示している場合
            Debug.Log(request.error);
        }
        else
        {
            //通信成功
            Debug.Log(request.downloadHandler.text);
            var response = JsonConverter.Deserialize(request.downloadHandler.text);
            DrawText(response);
        }
       }
    }

   private void DrawText(Events events)
    {
        eventsText.text = events.name;
        startText.text = events.start_time;
        endText.text = events.end_time;
    }

    public void LoadInGameScene()
    {
        SceneController.Instance.LoadInGameScene();
    }

    public void AddUserIdQuery(string text)
    {
        StartCoroutine(GetEventsInformation(text));
    }

    public void SetUserData( string username)
    {
        UserAccountData.UserId = userNameInputField.text;
        UserAccountData.UserName = username;
    }

}
