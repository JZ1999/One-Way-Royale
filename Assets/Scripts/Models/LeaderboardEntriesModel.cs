using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LeaderboardEntriesModel
{
	public int count;
	public string next;
	public string previous;
	public LeaderboardEntryModel[] results;
}
