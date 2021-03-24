using System.Collections.Generic;

[System.Serializable]
public class UserRankingData
{
    public int score; 
    public string name;
    public int rank;
}

[System.Serializable]
public class RankingData
{
        public List<UserRankingData>  ranking;
}

[System.Serializable]
public class UserData
{
    public string user_id;
    public string user_name;
    public int score;
}