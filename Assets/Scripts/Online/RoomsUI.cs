using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsUI : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    public void GeneratorRoomButtons()
    {
        
    }

    override public void OnJoinedLobby()
    {
        Debug.Log(42);
        
    }

    override public void OnLeftLobby()
    {
        Debug.Log(1);
        
    }

    override public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //
        Debug.Log(2);
        Debug.Log(roomList);
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
}
