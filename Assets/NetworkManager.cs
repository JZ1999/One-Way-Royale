using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	// Start is called before the first frame update
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
		PhotonNetwork.ConnectToRegion("us");
		Debug.LogWarning("Connected " + PhotonNetwork.CloudRegion);
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	// Self-heal if Photon drops while sitting in the menu (eg. socket errors) instead
	// of leaving "Play Online" stuck unusable until the player reloads the scene.
	public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		Debug.LogWarning("Photon disconnected (" + cause + "). Reconnecting.");
		PhotonNetwork.ConnectUsingSettings();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
