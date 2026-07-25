using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{

	public GameObject quickStartButton;
	public GameObject quickCancelButton;
	public int roomSize;
	public GameObject masterObject;
	public GameObject UIOnline;

	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
		Debug.Log("Connected " + PhotonNetwork.CloudRegion);
		PhotonNetwork.AutomaticallySyncScene = true;
		quickStartButton.GetComponent<Button>().interactable = true;
	}

	// Start is called before the first frame update
	void Start()
    {
		// NetworkManager already connects on menu load; forcing a Disconnect()
		// immediately followed by ConnectUsingSettings() here raced with that
		// connection tearing down and caused ObjectDisposedException crashes on
		// the socket's receive thread. Only (re)connect if we're not already.
		if (!PhotonNetwork.IsConnectedAndReady)
		{
			PhotonNetwork.ConnectUsingSettings();
		}
		quickStartButton.GetComponent<Button>().interactable = PhotonNetwork.IsConnectedAndReady;

	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		base.OnJoinRandomFailed(returnCode, message);
		Debug.Log("Failed to join room returnCode: "+returnCode+" message: "+message);
		CreateRoom();
	}

	public void CreateRoom()
	{
		Debug.Log("Creating room");
		int randomRoomNumber = Random.Range(0, 10_000);
		RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) roomSize };
		string masterName = PlayerPrefs.GetString("name");
		string nameRoom = masterName + "@" + randomRoomNumber;
		masterObject.SetActive(true);
		UIOnline.SetActive(false);
		PhotonNetwork.CreateRoom(nameRoom, roomOptions);
		Debug.Log(nameRoom);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		CreateRoom();
	}

	public void JoinRandomOrCreateRoom()
    {
		UIOnline.SetActive(false);
		masterObject.SetActive(true);

		// Atomic join-or-create: avoids the race where two clients that both
		// search for a room at nearly the same time (eg. Multiplayer Play Mode's
		// Main + Clone starting together) find none and each create their own room.
		int randomRoomNumber = Random.Range(0, 10_000);
		string masterName = PlayerPrefs.GetString("name");
		string nameRoom = masterName + "@" + randomRoomNumber;
		RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) roomSize };

		PhotonNetwork.JoinRandomOrCreateRoom(roomName: nameRoom, roomOptions: roomOptions);
	}

	public void QuickCancel()
	{
		quickCancelButton.SetActive(false);
		quickStartButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
