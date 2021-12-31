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
		PhotonNetwork.Disconnect();
		PhotonNetwork.ConnectUsingSettings();		
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
		PhotonNetwork.JoinRandomRoom();
	}

	public void QuickCancel()
	{
		quickCancelButton.SetActive(false);
		quickStartButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
