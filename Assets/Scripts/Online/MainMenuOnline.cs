using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOnline : MonoBehaviourPun, IPunObservable
{
    public PhotonView customPhotonView;
    public GameObject UICapacity;
    public int players;
    public int actuallyReady;
    public QuickStartRoomController QuickStartRoomController;
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
        Debug.Log(string.Format("{0} {1} {2} {3} {4} {5}", sender.IsLocal, sender.UserId, sender.IsMasterClient, sender.NickName, sender.HasRejoined, sender.ActorNumber));
        Debug.LogFormat("{0} {1}", type, json);
        switch (type)
        {
            case "player_ready":
                QuickStartRoomController.StartGame();
                break;
        }
    }
}
