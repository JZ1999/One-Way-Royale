using UnityEngine;
using Photon.Pun;

public class HazardGenerator : MonoBehaviour
{

	#region Variables
	public GameObject[] hazards;
	public float timer;
	[SerializeField]
	private float timer_cp;
	[SerializeField]
	private GameObject gameSetup;
	private PhotonView photonView;
	[SerializeField]
	private Transform rightLaneGenerator;
	#endregion

	#region Unity Methods    

	void Start()
    {
		timer_cp = timer;
		photonView = gameSetup.GetComponent<PhotonView>();
	}

    void Update()
    {
		if (GameManager._canMove)
		{
			timer -= Time.deltaTime;

			if (timer <= 0)
			{
				int random = Random.Range(0, hazards.Length);
				InstantiateObject(hazards[random]);
				Hazard hazard = new Hazard()
				{
					id = random,
					position = rightLaneGenerator.position
					
				};
				if(photonView)
					photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "spawn_hazard", JsonUtility.ToJson(hazard));

				timer = Random.Range(timer_cp * 0.75f, timer_cp * 1.25f);

				//increase difficulty
				timer /= GameManager.speedMultiplier;
			}
		}
    }
	public void InstantiateObject(GameObject obj)
    {
		Instantiate(obj, transform.position, obj.transform.rotation);
		if(obj.name == "mud")
			photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "mud_debuff");
	}

	public void UpdateTimeRespawn(float fix, bool div)
	{
		if (div)
			timer /= fix;
		else
			timer *= fix;
	}
	#endregion
}
