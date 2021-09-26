using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	#region Variables
	public string levelToLoad;
	public GameObject mainScreen;
	public GameObject switchingScreen;
	public Transform camera;
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
    #endregion

    #region Unity Methods    

    void Start()
    {
		camTarPosition = camera.position;

		if (!PlayerPrefs.HasKey(theChars[currentChar].name))
		{

			PlayerPrefs.SetInt(theChars[currentChar].name, 1);
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
		camera.position = Vector3.Lerp(camera.position, camTarPosition, cameraSpeed * Time.deltaTime);
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

		camTarPosition = camera.position + new Vector3(0f, charSwitchHolder.position.y);

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
}

