﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviour
{

	#region Variables
	static public bool canMove = false;
	static public bool _canMove;

	static public float gameSpeed = 10;
	static public float gameSpeedStore;
	static public float _gameSpeed;

	static public int coinsCollect = 0;
	static public bool collectedCoin = false;

	static private bool gameStarted = false;

	//speeding up the game
	static public float timeToIncreaseSpeed = 5;
	static public float increaseSpeedTimer;
	static public float speedMultiplier = 1f;
	static public float targetSpeedMultiplier;
	static public float acceleration = 0.75f;
	static public float accelerationStore;
	static public float speedIncreaseAmount = 1.5f;

	public GameObject tapMessage;

	public string menuScene;
	static public string _menuScene;

	// UI screen variables
	public Text coinsText;
	public Text distanceText;

	static public Text _coinsText;
	static public Text _distanceText;

	static private float distanceCovered;

	//player online
	public GameObject[] _playersOnline;
	static public GameObject[] playersOnline;
	public GameObject mud;

	[SerializeField]
	private Vector3 rightLaneGenerator;

	//Death screen variables
	public GameObject deathScreen;
	static public GameObject _deathScreen;
	public Text deathScreenCoins;
	static public Text _deathScreenCoins;
	public Text deathScreenDistance;
	static public Text _deathScreenDistance;

	public float deathScreenDelay;
	static  float _deathScreenDelay;

	static public bool dead = false;

	public GameObject[] models;
	public GameObject defaultChar;
	public PlayerMovement thePlayer;

	static public AudioManager theAM;
	#endregion

	#region Unity Methods    

	void Start()
	{
		ResetVariables();

		theAM = FindObjectOfType<AudioManager>();
		playersOnline = _playersOnline;
		_coinsText = coinsText;
		_distanceText = distanceText;
		_canMove = canMove;
		_gameSpeed = gameSpeed;
		_deathScreen = deathScreen;
		_deathScreenCoins = deathScreenCoins;
		_deathScreenDistance = deathScreenDistance;
		_deathScreenDelay = deathScreenDelay;
		_menuScene = menuScene;

		

		gameSpeedStore = gameSpeed;
		accelerationStore = acceleration;

		if (PlayerPrefs.HasKey("CoinsCollected")) { 
			coinsCollect = PlayerPrefs.GetInt("CoinsCollected");
			_coinsText.text = "Coins: " + coinsCollect;
		}

		increaseSpeedTimer = timeToIncreaseSpeed;

		targetSpeedMultiplier = speedMultiplier;
		_distanceText.text = distanceCovered + "m";
		if (playersOnline.Length == 0)
		{
			for (int i = 0; i < models.Length; i++)
			{
				if (models[i].name == PlayerPrefs.GetString("SelectedChar"))
				{
					GameObject clone = Instantiate(models[i], thePlayer.modelHolder.transform);
					Destroy(clone.GetComponent<Rigidbody>());
					//defaultChar.SetActive(false);
				}
			}
		}
    }
    void Update()
    {	
		_canMove = canMove;
		_gameSpeed = GameManager.gameSpeed;

		if (PhotonNetwork.CurrentRoom?.PlayerCount == null && !gameStarted && Input.GetMouseButtonDown(0))
		{
			gameStarted = true;
			canMove = true;
			tapMessage.SetActive(false);

		}

		//Increase speed over time	
		if (canMove)
		{
			increaseSpeedTimer -= Time.deltaTime;
			if (increaseSpeedTimer < 0)
			{
				increaseSpeedTimer = timeToIncreaseSpeed;

				targetSpeedMultiplier *= speedIncreaseAmount;

				timeToIncreaseSpeed *= .97f;
			}
			acceleration = accelerationStore * speedMultiplier;

			speedMultiplier = Mathf.MoveTowards(speedMultiplier, targetSpeedMultiplier, acceleration * Time.deltaTime);
			gameSpeed = gameSpeedStore * speedMultiplier;

			//Updating UI
			distanceCovered += Time.deltaTime * gameSpeed;
			_distanceText.text = Mathf.Floor(distanceCovered)+ "m";
		}

		collectedCoin = false;

		if (dead && !PhotonNetwork.IsConnected)
		{
			
			if (deathScreenDelay <= 0)
			{
				deathScreen.SetActive(true);
			}
			else
			{
				deathScreenDelay -= Time.deltaTime;
			}
		}
	}

	#endregion




	public void StartOnlineGame()
	{
		gameStarted = true;
		canMove = true;
	}


	public void ResetVariables()
	{
		dead = false;
		gameStarted = false;
		canMove = false;
		gameSpeed = 10;
		coinsCollect = 0;
		collectedCoin = false;
		timeToIncreaseSpeed = 5;
		speedMultiplier = 1f;
		acceleration = 0.5f;
		speedIncreaseAmount = 1.1f;
		distanceCovered = 0;
	}

	static public void HazardHit()
	{
		_canMove = false;
		canMove = false;
		dead = true;
		
		
		PlayerPrefs.SetString("distance", Mathf.Floor(distanceCovered) + "");
		PlayerPrefs.SetInt("CoinsCollected", coinsCollect);

		_deathScreenCoins.text = coinsCollect + " coins!";
		_deathScreenDistance.text = Mathf.Floor(distanceCovered) + "m";

		theAM.StopMusic();
		theAM.gameOverMusic.Play();
	}

	static public void AddCoin()
	{
		if (!collectedCoin)
		{
			coinsCollect++;
			collectedCoin = true;

			_coinsText.text = "Coins: " + coinsCollect;
		}
	}

	static public void ApplyDebuff(string nameDebuff) {
		if(nameDebuff != "none")
		{
			int index = UnityEngine.Random.Range(0, playersOnline.Length);
			GameObject focusPlayer = playersOnline[index];
			FindObjectOfType<GameSetupController>().SendMessage(nameDebuff);
			//FindObjectOfType<GameManager>().ReceivedDebuff(nameDebuff);

#if UNITY_EDITOR
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
				FindObjectOfType<GameManager>().ReceivedDebuff(nameDebuff);
#endif
		}
    }

	public void ReceivedDebuff(string nameDebuff)
    {
		Debug.Log(nameDebuff);
		switch (nameDebuff)
		{
			case "mud":
				FindObjectOfType<HazardGenerator>().InstantiateObject(mud);
				break;
			case "coins":
				FindObjectOfType<CoinGeneration>().setShouldApplyDebuff(true);
				break;
			case "object":
				FindObjectOfType<HazardGenerator>().timer = 0;
				break;
		}
	}

	public float GetDistance()
    {
		return distanceCovered;

	}

	public IEnumerator GoToMainMenu()
	{
		PhotonNetwork.LeaveRoom();
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(0);
	}
}
