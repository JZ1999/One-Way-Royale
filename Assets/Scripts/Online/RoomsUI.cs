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
        Debug.Log(42);
        
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log(level);
    }

    override public void OnLeftLobby()
    {
        Debug.Log(1);
        
    }

    override public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GeneratorRoomButtons(roomList);
    }

    override public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log(23);
        
    }

    override public void OnConnected()
    {
        Debug.Log(17);
        
    }

    override public void OnConnectedToMaster()
    {
        Debug.Log(16);
        
    }

    override public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(15);
        
    }

    override public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log(14 + " " +regionHandler.EnabledRegions);
        
    }

    override public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log(13);
        
    }

    override public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log(12);
        
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
