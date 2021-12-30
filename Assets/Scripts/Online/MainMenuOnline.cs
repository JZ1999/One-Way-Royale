using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOnline : MonoBehaviourPun, IPunObservable
{
    public PhotonView customPhotonView;
    public GameObject UICapacity;
    public int players;
    public int actuallyReady;
    public QuickStartRoomController QuickStartRoomController;
    public QuickStartLobbyController quickStartLobbyController;
    private bool isStartingGame = false;
	private int playerCount = -1;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStartingGame && PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
        {

			if(playerCount != PhotonNetwork.CurrentRoom.PlayerCount)
			{
				playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
				UICapacity.GetComponentInChildren<Text>().text = playerCount + "/" + quickStartLobbyController.roomSize;
			}

            if (PhotonNetwork.CurrentRoom.PlayerCount == quickStartLobbyController.roomSize)
            {
                isStartingGame = true;
                QuickStartRoomController.StartGame();
            }
        }
    }


    public void PlayerIsReady()
    {
        customPhotonView.RPC("MessageReceived", RpcTarget.All, PhotonNetwork.LocalPlayer, "player_ready", "");
    }

    [PunRPC]
    void MessageReceived(Photon.Realtime.Player sender, string type, string json)
    {
        if (sender.IsLocal)
            return;
        switch (type)
        {
            case "player_ready":
                
                break;
        }
    }
}
