using Photon.Pun;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{

	public int multiplayerSceneIndex;
	public MainMenuOnline mainMenuOnline;

	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		Debug.Log("Joined room");
		if (!PhotonNetwork.IsMasterClient)
		{
			StartGame();
			mainMenuOnline.PlayerIsReady();
		}
			
	}

	public void StartGame()
	{
		Debug.Log("Starting Game");
		PhotonNetwork.LoadLevel(multiplayerSceneIndex);	
	}
}
