using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameSetupController : MonoBehaviourPun, IPunObservable
{

	public PhotonView customPhotonView;

	public List<GameObject> players;

	public Transform spawn;

	public GameObject prefab;

	public GameObject otherPlayer;

	public HazardGenerator hazardGenerator;

	private int playersReady = 1;

	[SerializeField]
	private GameManager gameManager;


	[SerializeField]
	private float readyTimer = 10;

	private int initialTime = 0;

	private bool startedOnlineGame = false;

    void Start()
    {
		customPhotonView.RPC("Loaded", RpcTarget.All, PhotonNetwork.LocalPlayer, true);
	}

    void Update()
    {
		if (playersReady == PhotonNetwork.CurrentRoom.PlayerCount)
        {
			if (initialTime == 0)
				initialTime = PhotonNetwork.ServerTimestamp;
			
			if(readyTimer <= 0 && !startedOnlineGame) {
				gameManager.StartOnlineGame();
				startedOnlineGame = true;
			}
            else
            {
				readyTimer = 10 - ((PhotonNetwork.ServerTimestamp - initialTime) / 1_000);
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
			charName = PlayerPrefs.GetString("SelectedChar")
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
		FindObjectOfType<GameManager>().RecievedDebuff(debuff);
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

				GameObject playerObject = Instantiate(Resources.Load(Path.Combine("PhotonPrefabs", player.charName)), otherPlayer.transform.position, otherPlayer.transform.rotation) as GameObject;
				players.Add(playerObject);
				playerObject.transform.parent = otherPlayer.transform;
				Destroy(playerObject.GetComponent<Rigidbody>());

				PlayerMovement playerMovement = playerObject.GetComponentInParent<PlayerMovement>();

				playerMovement.playerData = player;

				player = new Player()
				{
					name = PlayerPrefs.GetString("name"),
					charName = PlayerPrefs.GetString("SelectedChar")
				};
				//Send message
				customPhotonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "spawn", JsonUtility.ToJson(player));
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
				Debug.Log(json);
				Hazard hazard = JsonUtility.FromJson<Hazard>(json);
				hazard.position.z -= 8.45f;
				Instantiate(hazardGenerator.hazards[hazard.id], hazard.position, Quaternion.identity);
				break;
		}			
	}

	private void CreatePlayer()
	{
		Debug.Log("Creating Player");
		int playersInRoom = PhotonNetwork.CurrentRoom.Players.Keys.Count;
		
		Player playerData = new Player()
		{
			name = PlayerPrefs.GetString("name"),
			charName = PlayerPrefs.GetString("SelectedChar")
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
