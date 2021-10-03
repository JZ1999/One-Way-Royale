using Photon.Pun;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameSetupController : MonoBehaviourPun, IPunObservable
{

	public PhotonView photonView;

	public IDictionary<int, GameObject> players;

	public Transform spawn;

	public GameObject prefab;

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log(gameObject);
		players = new Dictionary<int, GameObject>();
		photonView = gameObject.AddComponent<PhotonView>();
		photonView.ViewID = 1;
		CreatePlayer();
		//Send message
		photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "test");

	}

	public void SendMessage(string message)
	{
		photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, message);
	}

	[PunRPC]
	void SendChat(Photon.Realtime.Player sender, string debuff)
	{
		
		if (sender.IsLocal)
			return;
		Debug.Log(string.Format("{0} {1} {2} {3} {4} {5}", sender.IsLocal, sender.UserId, sender.IsMasterClient, sender.NickName, sender.HasRejoined, sender.ActorNumber));
		Debug.Log(debuff);
		FindObjectOfType<GameManager>().RecievedDebuff(debuff);
	}

	private void CreatePlayer()
	{
		Debug.Log("Creating Player");
		int playersInRoom = PhotonNetwork.CurrentRoom.Players.Keys.Count;
		//string prefab = PlayerPrefs.GetString("SelectedChar");
		Debug.Log(prefab);
		GameObject player = Instantiate(prefab, spawn.position, Quaternion.identity);
		player.transform.parent = spawn;
		Destroy(player.GetComponent<Rigidbody>());
		Debug.Log(player);

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log(stream);
		Debug.Log(info);
	}
}
