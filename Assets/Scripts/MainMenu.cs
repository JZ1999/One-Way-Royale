using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Advertisements;
using System.IO;

public class MainMenu : MonoBehaviour
{

	#region Variables
	public string levelToLoad;
	public GameObject selectMode;
	public GameObject onlineUI;
	public GameObject masterUI;
	public GameObject availableRooms;
	public GameObject switchingScreen;
	public Transform charHolder;
	public Transform mainCamera;
	public Transform charSwitchHolder;
	private Vector3 camTarPosition;
	public float cameraSpeed;

	public GameObject[] theChars;
	public int currentChar;

	public GameObject switchPlayButton;
	public GameObject switchUnlockButton;
	public GameObject switchGetCoinsButton;
	public GameObject coinCounter;
	public GameObject coinCounterHolder;
	public GameObject profileButton;
	public GameObject charLockedImage;

	public int coinsCollected;
	public Text coinsText;
	public int characterPrice = 20;

	public GameObject adRewardPanel;
	public Text rewardText;
	private readonly string videoAdZone;
	#endregion

	#region Unity Methods    

	void Start()
    {
		
		camTarPosition = mainCamera.position;

		if (!PlayerPrefs.HasKey(theChars[currentChar].name))
		{

			PlayerPrefs.SetInt(theChars[currentChar].name, 1);
		}

		if (!PlayerPrefs.HasKey("SelectedChar"))
		{
			PlayerPrefs.SetString("SelectedChar", "chr_costume1");
		}

		if (PlayerPrefs.HasKey("CoinsCollected"))
		{
			coinsCollected = PlayerPrefs.GetInt("CoinsCollected");
		} else
		{
			PlayerPrefs.SetInt("CoinsCollected", 0);
			coinsCollected = 0;
		}

		MainScreen();

	}

    void Update()
    {
		mainCamera.position = Vector3.Lerp(mainCamera.position, camTarPosition, cameraSpeed * Time.deltaTime);

		coinsText.text = "Cost: 20";

#if UNITY_EDITOR

		if(Input.GetKeyDown(KeyCode.L))
		{
			for(int i = 1; i < theChars.Length; i++)
			{
				PlayerPrefs.SetInt(theChars[i].name, 0);
			}

			UnlockedCheck();
		}
#endif
	}

    private void Awake()
    {
		int counter = 0;
        foreach(GameObject t in theChars)
        {
			GameObject character = Instantiate(t, charSwitchHolder);
			character.transform.position += Vector3.left * counter;
			counter+=2;
        }
    }

    #endregion

    public void PlayGame()
	{
		SceneManager.LoadScene(levelToLoad);
	}
	public void MainScreen()
    {
		coinCounter.GetComponent<TMPro.TextMeshProUGUI>().text = coinsCollected.ToString();

		coinCounterHolder.SetActive(true);
		profileButton.SetActive(true);

		string charName = PlayerPrefs.GetString("SelectedChar");
		foreach (Transform child in charHolder)
		{
			Destroy(child.gameObject);
		}
		Debug.Log(Path.Combine("PhotonPrefabs", charName));
		Instantiate(Resources.Load(Path.Combine("PhotonPrefabs", charName)), charHolder.transform);
		selectMode.SetActive(true);
		switchingScreen.SetActive(false);

		camTarPosition = new Vector3(charHolder.position.x, charHolder.position.y, 3.1f);
		currentChar = 0;
	}


	public void BackToSelectMode()
    {
		Debug.Log("BackToSelectMode");
		profileButton.SetActive(true);
		onlineUI.SetActive(false);
		selectMode.SetActive(true);
	}
	public void ChooseChar()
	{
		profileButton.SetActive(false);

		selectMode.SetActive(false);
		switchingScreen.SetActive(true);

		camTarPosition = mainCamera.position + new Vector3(0f, charSwitchHolder.position.y);

		UnlockedCheck();
	}

	public void moveLeft()
	{
		if(currentChar > 0)
		{
			camTarPosition -= new Vector3(-2f, 0, 0);

			currentChar--;
		}

		UnlockedCheck();
	}

	public void moveRight()
	{
		if (currentChar < theChars.Length - 1)
		{
			camTarPosition += new Vector3(-2f, 0, 0);

			currentChar++;
		}

		UnlockedCheck();
	}

	public void UnlockedCheck()
	{
		if(PlayerPrefs.HasKey(theChars[currentChar].name))
		{

			if(PlayerPrefs.GetInt(theChars[currentChar].name) == 0)
			{
				switchPlayButton.SetActive(false);

				charLockedImage.SetActive(true);

				if (coinsCollected < characterPrice)
				{
					switchGetCoinsButton.SetActive(true);
					switchUnlockButton.SetActive(false);
				} else
				{
					//switchUnlockButton.GetComponent<Text>().text = "Unlock";
					switchUnlockButton.SetActive(true);
					switchGetCoinsButton.SetActive(false);
				}
			} else
			{
				switchPlayButton.SetActive(true);

				switchGetCoinsButton.SetActive(false);
				switchUnlockButton.SetActive(false);
				charLockedImage.SetActive(false);
			}
		}else
		{
			PlayerPrefs.SetInt(theChars[currentChar].name, 0);

			UnlockedCheck();
		}
	}

	public void UnlockCharacter()
	{
		coinsCollected -= characterPrice;

		PlayerPrefs.SetInt(theChars[currentChar].name, 1);
		PlayerPrefs.SetInt("CoinsCollected", coinsCollected);

		coinCounter.GetComponent<TMPro.TextMeshProUGUI>().text = coinsCollected.ToString();

		UnlockedCheck();
	}

	public void SelectChar()
	{
		PlayerPrefs.SetString("SelectedChar", theChars[currentChar].name);

		MainScreen();
	}

	public void GetCoins()
	{
		
	}

	
	public void CloseAdPanel()
	{
		adRewardPanel.SetActive(false);
	}

	public void OnlineUI()
    {
		profileButton.SetActive(false);
		selectMode.SetActive(false);
		onlineUI.SetActive(true);
	}

	public void SearchRoomUI()
    {
		profileButton.SetActive(false);
		PhotonNetwork.JoinLobby();
		onlineUI.SetActive(false);
		availableRooms.SetActive(true);
	}

	public void BackToUIOnline()
	{
		profileButton.SetActive(false);
		//PhotonNetwork.JoinLobby(); 
		onlineUI.SetActive(true);
		availableRooms.SetActive(false);
		masterUI.SetActive(false);
	}
	public void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene("MainMenu");
	}
}

