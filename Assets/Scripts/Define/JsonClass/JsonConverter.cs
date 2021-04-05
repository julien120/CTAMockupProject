using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonConverter : MonoBehaviour
{
    public static Events Deserialize(string json)
    {
        Events eventInfo = JsonUtility.FromJson<Events>(json);
        return eventInfo;
    }

    public static string Serialize(UserData model)
    {
        string json = JsonUtility.ToJson(model);
        return json;
    }
}
