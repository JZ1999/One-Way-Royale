using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Text;

public class ApiService
{
    public string url = "https://one-way-game.herokuapp.com/api/v1/";

    public void RegisterTemporaryAccount()
    {
        AccountModel Acc = new AccountModel() {
            name = PlayerPrefs.GetString("name"),
            uuid = PlayerPrefs.GetString("uuid"),
            device = SystemInfo.deviceType.ToString(),
        };
        _ = RegisterRequest(Acc);
    }

    // Uses UnityWebRequest, not System.Net.Http.HttpClient: on WebGL, HttpClient's HTTPS path
    // goes through Mono's TLS provider init, which hits an IL2CPP generic-sharing bug
    // (Marshal.PtrToStructure<T>) that crashes with "function signature mismatch" under
    // Minimal stripping. UnityWebRequest is the platform's supported networking API and
    // avoids that path entirely.
    private async Task RegisterRequest(AccountModel Acc)
    {
        var json = JsonUtility.ToJson(Acc);

        Debug.LogWarning(String.Format("Request: {0} {1} -> {2}", "POST", url + "account/temporary", json));

        using (var request = new UnityWebRequest(url + "account/temporary", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var op = request.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"RegisterRequest failed: {request.error}");
        }
    }

    public async Task<LeaderboardEntriesModel> GetLeaderboard()
    {
        Debug.LogWarning(String.Format("Request: {0} {1}", "GET", url + "game/leaderboard/offline"));

        using (var request = UnityWebRequest.Get(url + "game/leaderboard/offline"))
        {
            var op = request.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GetLeaderboard failed: {request.error}");
                return null;
            }
            return JsonUtility.FromJson<LeaderboardEntriesModel>(request.downloadHandler.text);
        }
    }

    // async void (not void + .Result): WebGL is single-threaded, so blocking on
    // Task.Result to wait for the response deadlocks the browser tab.
    public async void RegisterScore()
    {
        LeadetBoardModel LeaderBoard = new LeadetBoardModel()
        {
            name = PlayerPrefs.GetString("name"),
            account_uuid = PlayerPrefs.GetString("uuid"),
            distance = PlayerPrefs.GetString("distance"),
        };

        var json = JsonUtility.ToJson(LeaderBoard);

        var authenticationString = $"{PlayerPrefs.GetString("uuid")}:{PlayerPrefs.GetString("uuid")}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

        Debug.LogWarning(String.Format("Request: {0} {1} -> {2}", "POST", url + "game/leaderboard/offline/", json));

        using (var request = new UnityWebRequest(url + "game/leaderboard/offline/", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Basic " + base64EncodedAuthenticationString);

            var op = request.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"RegisterScore failed: {request.error}");
        }
    }
}
