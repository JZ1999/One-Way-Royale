using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class IsMasterRoom : MonoBehaviour
{
    // Start is called before the first frame update

    public int players;
    public int actuallyReady;
    public GameObject UIRoom;
    public TMPro.TextMeshProUGUI textObject;
   
    void Start()
    {
        UIRoom.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = PhotonNetwork.CountOfPlayers + "/" + " 2";
    }
}
