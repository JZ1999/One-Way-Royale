using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
	[SerializeField]
	private ApiService api = new ApiService();

	[SerializeField]
	private GameObject leaderboardEntry;
	[SerializeField]
	private Transform leaderboard;

	private LeaderboardEntriesModel entries;

    async void Start()
    {
		entries = await api.GetLeaderboard();

        for (int i = 0; (i < entries.results.Length) && i != 10; i++)
		{
            GameObject instance = Instantiate(leaderboardEntry, leaderboard);

			// Should only have 2 children
			instance.GetComponentsInChildren<TMP_Text>()[0].text = entries.results[i].name;
			instance.GetComponentsInChildren<TMP_Text>()[1].text = entries.results[i].distance.ToString() + "m";
		}
    }

	public void CloseLeaderBoard()
	{
		gameObject.SetActive(false);
	}
	public void OpenLeaderBoard()
	{
		gameObject.SetActive(true);
	}
}
