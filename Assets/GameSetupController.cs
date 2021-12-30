﻿using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class GameSetupController : MonoBehaviourPun, IPunObservable
{

	public PhotonView customPhotonView;

	public List<GameObject> players;

	public Transform spawn;

	//public GameObject prefab;

	public GameObject otherPlayerLeft;
	public GameObject otherPlayerRight;
	public GameObject playersObjectPool;

	public HazardGenerator hazardGenerator;

	private int playersReady = 1;

	public GameObject prefabMud;

	[SerializeField]
	private GameManager gameManager;

	[SerializeField]
	private GameObject rightLaneGenerator;


	[SerializeField]
	private float readyTimer = 10;

	[SerializeField]
	private GameObject uiReadyTimer;

	private int initialTime = 0;

	private bool startedOnlineGame = false;

    void Start()
    {
		customPhotonView.RPC("Loaded", RpcTarget.All, PhotonNetwork.LocalPlayer, true);
	}

    void Update()
    {
		if (playersReady == PhotonNetwork.CurrentRoom?.PlayerCount)
        {
			if (initialTime == 0)
				initialTime = PhotonNetwork.ServerTimestamp;
			
			if(readyTimer <= 0 && !startedOnlineGame) {
				uiReadyTimer.SetActive(false);
				gameManager.StartOnlineGame();
				startedOnlineGame = true;
			}
            else
            {
				readyTimer = 10 - ((PhotonNetwork.ServerTimestamp - initialTime) / 1_000);
				uiReadyTimer.GetComponentsInChildren<TMP_Text>()[0].text = readyTimer + "";
			}
        }
    }

    // Start is called before the first frame update
    void Awake()
	{
		players = new List<GameObject>();
		customPhotonView = gameObject.AddComponent<PhotonView>();
		customPhotonView.ViewID = 1;
		CreatePlayer();
		Player player = new Player() {
			name = PlayerPrefs.GetString("name"),
			charName = PlayerPrefs.GetString("SelectedChar"),
			isMe = false,
			actorNumber = PhotonNetwork.LocalPlayer.ActorNumber
		};
		//Send message
		customPhotonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "spawn", JsonUtility.ToJson(player));
	}

	public void SendMessage(string message)
	{
		customPhotonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, message);
	}

	public void SendMessage(string type, string json)
	{
		customPhotonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, type, json);
	}

	[PunRPC]
	void Loaded(Photon.Realtime.Player sender, bool loaded)
    {
		Debug.Log("request");

		if (sender.IsLocal)
			return;
		playersReady += 1;
	}

	[PunRPC]
	void SendChat(Photon.Realtime.Player sender, string debuff)
	{
		
		if (sender.IsLocal)
			return;
		FindObjectOfType<GameManager>().ReceivedDebuff(debuff);
	}

	[PunRPC]
	void SendChat(Photon.Realtime.Player sender, string type, string json)
	{
		if (sender.IsLocal)
			return;
		Player player;
		switch (type)
		{
			case "spawn":
				
				player = JsonUtility.FromJson<Player>(json);
				foreach (GameObject p in players)
				{
					if (p.GetComponentInParent<PlayerMovement>().playerData.name == player.name)
					{
						return;
					}
				}

				Transform otherPlayer = DecideLeftOrRight(player.actorNumber);

				GameObject playerObject = Instantiate(Resources.Load(Path.Combine("PhotonPrefabs", player.charName)), otherPlayer.position, otherPlayer.transform.rotation) as GameObject;

				players.Add(playerObject);
				playerObject.transform.parent = otherPlayer;
				Destroy(playerObject.GetComponent<Rigidbody>());

				PlayerMovement playerMovement = playerObject.GetComponentInParent<PlayerMovement>();

				playerMovement.playerData = player;

				player = new Player()
				{
					name = PlayerPrefs.GetString("name"),
					charName = PlayerPrefs.GetString("SelectedChar"),
					actorNumber = PhotonNetwork.LocalPlayer.ActorNumber
				};
				Debug.Log(player.actorNumber);
				//Send message
				customPhotonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "spawn", JsonUtility.ToJson(player));
				Destroy(playerObject.GetComponent<PlayerMovement>());

				if(otherPlayer == playersObjectPool.transform)
				{
					playerObject.SetActive(false);
				}
				break;
			case "jump":
				player = JsonUtility.FromJson<Player>(json);
				foreach(GameObject p in players) {
					if(p.GetComponentInParent<PlayerMovement>().playerData.name == player.name)
					{
						p.GetComponentInParent<PlayerMovement>().setJumpOffline();
					}
				}
				break;
			case "spawn_hazard":
				Hazard hazard = JsonUtility.FromJson<Hazard>(json);
				hazard.position.z -= 8.45f; // send to neighbor;
				Instantiate(hazardGenerator.hazards[hazard.id], hazard.position, Quaternion.identity);
				break;
			case "mud_debuff":
				Vector3 laneTranform = rightLaneGenerator.transform.position;
				laneTranform.z -= 8.45f;
				Debug.Log(Instantiate(prefabMud, laneTranform, prefabMud.transform.rotation));
				break;
		}	
		
	}

	private Transform DecideLeftOrRight(int playerActorNumber)
	{
		if((PhotonNetwork.LocalPlayer.ActorNumber - playerActorNumber) == 1)
		{
			return otherPlayerLeft.transform;
		}
		else if ((PhotonNetwork.LocalPlayer.ActorNumber -  playerActorNumber) == -1)
		{
			return otherPlayerRight.transform;
		}

		return playersObjectPool.transform;
	}

	private void CreatePlayer()
	{
		Debug.Log("Creating Player");
		int playersInRoom = PhotonNetwork.CurrentRoom.Players.Keys.Count;
		
		Player playerData = new Player()
		{
			name = PlayerPrefs.GetString("name"),
			charName = PlayerPrefs.GetString("SelectedChar"),
			isMe = true,
			actorNumber = PhotonNetwork.LocalPlayer.ActorNumber
		};
		GameObject player = Instantiate(Resources.Load(Path.Combine("PhotonPrefabs", playerData.charName)), spawn.position, Quaternion.identity) as GameObject;


		players.Add(player);
		player.transform.parent = spawn;
		Destroy(player.GetComponent<Rigidbody>());

		player.GetComponentInParent<PlayerMovement>().playerData = playerData;

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		
	}
}
