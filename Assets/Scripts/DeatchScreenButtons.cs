using UnityEngine;
using UnityEngine.SceneManagement;

public class DeatchScreenButtons : MonoBehaviour
{

	#region Variables
	public GameObject ContinueScreen;
	public GameObject GameOverScreen;
	public GameObject PauseScreen;

	public PlayerMovement player;
	public AudioManager theAM;

	#endregion

	#region Unity Methods    

	void Start()
    {
        
    }

    void Update()
    {
        
    }

	#endregion

	public void ContinueGame()
	{
		if(GameManager.coinsCollect >= 100)
		{
			GameOverScreen.SetActive(false);
			GameManager.dead = false;
			GameManager.coinsCollect -= 100;
			GameManager.canMove = true;
			GameManager._canMove = true;

			player.ResetPlayer();
			theAM.StopMusic();
			theAM.gameMusic.Play();
		}
		else
		{
			ContinueScreen.SetActive(true);
		}
	}

	public void CloseContinueScreen()
	{
		ContinueScreen.SetActive(false);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GetCoins()
	{

	}

	public void MainMenu()
	{
		SceneManager.LoadScene(GameManager._menuScene);

		Time.timeScale = 1f;
	}

	public void PauseGame()
	{

		if(Time.timeScale == 1f)
		{
			PauseScreen.SetActive(true);

			Time.timeScale = 0f;
		}
		else
		{
			Resume();
		}
		
	}
	public void Resume()
	{
		PauseScreen.SetActive(false);

		Time.timeScale = 1f;
	}
}
