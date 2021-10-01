using System.Net.Http;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;

public class ApiService
{
    public string url = "http://onewayapi.herokuapp.com/api/v1/";
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

		Debug.LogWarning(String.Format("Request: {0} {1} -> {2}", "POST", url + "account/temporary", jsonString));

		HttpResponseMessage response = await client.PostAsync(url +
            "account/temporary", jsonString);
        response.EnsureSuccessStatusCode();


        // return URI of the created resource.
        return response.Headers.Location;
    }

	public async Task<LeaderboardEntriesModel> GetLeaderboard()
	{
		Debug.LogWarning(String.Format("Request: {0} {1}", "GET", url + "game/leaderboard/offline"));

		string response = await client.GetStringAsync(url +
			"game/leaderboard/offline");
		// return URI of the created resource.
		return JsonUtility.FromJson<LeaderboardEntriesModel>(response);
	}


	public void RegisterScore()
    {

        LeadetBoardModel LeaderBoard = new LeadetBoardModel()
        {
            name = PlayerPrefs.GetString("name"),
            account_uuid = PlayerPrefs.GetString("uuid"),
            distance = PlayerPrefs.GetString("distance"),

        };

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.ConnectionClose = true;

        var json = JsonUtility.ToJson(LeaderBoard);
        var jsonString = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        var authenticationString = $"{PlayerPrefs.GetString("uuid")}:{PlayerPrefs.GetString("uuid")}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url +
            "game/leaderboard/offline/");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        requestMessage.Content = jsonString;

		Debug.LogWarning(String.Format("Request: {0} {1} -> {2}", "POST", url + "game/leaderboard/offline/", jsonString));

		var task = client.SendAsync(requestMessage);
        var response = task.Result;
        response.EnsureSuccessStatusCode();
        _ = response.Content.ReadAsStringAsync().Result;
    }
}
