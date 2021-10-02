using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour
{

	#region Variables
	public string levelToLoad;
	public GameObject mainScreen;
	public GameObject switchingScreen;
	public Transform mainCamera;
	public Transform charSwitchHolder;
	private Vector3 camTarPosition;
	public float cameraSpeed;

	public GameObject[] theChars;
	public int currentChar;

	public GameObject switchPlayButton;
	public GameObject switchUnlockButton;
	public GameObject switchGetCoinsButton;
	public GameObject charLockedImage;

	public int coinsCollected;

	public Text coinsText;

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
	}

    void Update()
    {
		mainCamera.position = Vector3.Lerp(mainCamera.position, camTarPosition, cameraSpeed * Time.deltaTime);

		coinsText.text = "Coins: " + coinsCollected;

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

    #endregion

	public void PlayGame()
	{
		SceneManager.LoadScene(levelToLoad);
	}

	public void ChooseChar()
	{
		mainScreen.SetActive(false);
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

				if (coinsCollected < 500)
				{
					switchGetCoinsButton.SetActive(true);
					switchUnlockButton.SetActive(false);
				} else
				{
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
		coinsCollected -= 500;

		PlayerPrefs.SetInt(theChars[currentChar].name, 1);
		PlayerPrefs.SetInt("CoinsCollected", coinsCollected);

		UnlockedCheck();
	}

	public void SelectChar()
	{
		PlayerPrefs.SetString("SelectedChar", theChars[currentChar].name);

		PlayGame();
	}

	public void GetCoins()
	{
		ShowRewardedVideo();
	}

	public bool IsVideoAdLoaded()
	{
		return Advertisement.isInitialized && Advertisement.IsReady(videoAdZone);
	}

	void ShowRewardedVideo()
	{
		if(IsVideoAdLoaded())
		{
			ShowOptions options = new ShowOptions();
			//options.resultCallback = HandleShowResult;

			//TODO check if all this code regarding ads can be deleted

			Advertisement.Show(videoAdZone, options);
		}
		else
		{
			Debug.LogWarning("Ad video not loaded yet.");
		}
	}

	void HandleShowResult(ShowResult result)
	{
		if (result == ShowResult.Finished)
		{
			Debug.Log("Video completed - Offer a reward to the player");
			// Reward your player here.
			coinsCollected += 100;
			PlayerPrefs.SetInt("CoinsCollected", coinsCollected);
			rewardText.text = "Thanks for watching. You receive 100 coins!";
		}
		else if (result == ShowResult.Skipped)
		{
			Debug.LogWarning("Video was skipped - Do NOT reward the player");
			rewardText.text = "You didn't watch the ad. Watch another to earn coins";
		}
		else if (result == ShowResult.Failed)
		{
			Debug.LogError("Video failed to show");
			rewardText.text = "Unable to show ad. Please try again.";
		}

		adRewardPanel.SetActive(true);
	}

	public void CloseAdPanel()
	{
		adRewardPanel.SetActive(false);
	}
}

