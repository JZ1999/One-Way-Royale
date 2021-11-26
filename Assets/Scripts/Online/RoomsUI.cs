using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomsUI : MonoBehaviourPunCallbacks, IPunCallbacks
{
    // Start is called before the first frame update
    public GameObject lobbyGrid;
    public GameObject lobbyButton;
    public GameObject NetworkManager;

    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    public void GeneratorRoomButtons(List<RoomInfo> roomList)
    {
        foreach (Transform child in lobbyGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (RoomInfo room in roomList) {
            if (!room.IsOpen || !room.IsVisible || room.MaxPlayers == 0)
                continue;

            GameObject newbutton  = Instantiate(lobbyButton, lobbyGrid.transform);
            TMPro.TextMeshProUGUI[] buttonsText = newbutton.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            buttonsText[1].text = room.Name.Split('@')[0];
            buttonsText[0].text = room.PlayerCount +"/" + room.MaxPlayers;
            newbutton.GetComponent<Button>().onClick.AddListener(delegate { PhotonNetwork.JoinRoom(room.Name); });
        }
    }

    override public void OnJoinedLobby()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
    }

    override public void OnLeftLobby()
    {
    }

    override public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GeneratorRoomButtons(roomList);
    }

    override public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    override public void OnConnected()
    {
    }

    override public void OnConnectedToMaster()
    {
    }

    override public void OnDisconnected(DisconnectCause cause)
    {
    }

    override public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    override public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    override public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }
    
    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Photon.Realtime.Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }
    
}
