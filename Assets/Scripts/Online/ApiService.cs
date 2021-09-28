using System.Net.Http;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Text;

public class ApiService
{
    public string url = "https://7ce0-186-32-36-54.ngrok.io/api/v1/";
    public static HttpClient client = new HttpClient();

    public void RegisterTemporaryAccount()
    {
           AccountModel Acc = new AccountModel() { 
            name = PlayerPrefs.GetString("name"),
            uuid = PlayerPrefs.GetString("uuid"),
            device = SystemInfo.deviceType.ToString(),
           };
        _ = RegisterRequest(Acc);
    }

    private async Task<Uri> RegisterRequest(AccountModel Acc)
    {
        
        var json = JsonUtility.ToJson(Acc);
        var jsonString = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        Debug.Log(Acc.name + Acc.device + Acc.uuid );
        Debug.Log(json.ToString());
        HttpResponseMessage response = await client.PostAsync( this.url +
            "account/temporary", jsonString);
        response.EnsureSuccessStatusCode();

        // return URI of the created resource.
        return response.Headers.Location;
    }

    public async Task<Uri> RegisterScore()
    {
        LeadetBoardModel LeaderBoard = new LeadetBoardModel()
        {
            name = PlayerPrefs.GetString("name"),
            account_uuid = PlayerPrefs.GetString("uuid"),
            distance = PlayerPrefs.GetString("distance"),

        };
        var json = JsonUtility.ToJson(LeaderBoard);
        var jsonString = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(this.url +
            "game/leaderboard/offline/", jsonString);
        response.EnsureSuccessStatusCode();

        // return URI of the created resource.
        return response.Headers.Location;
    }
}
