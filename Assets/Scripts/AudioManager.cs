using UnityEngine;
// Creado por JZ1999

public class AudioManager : MonoBehaviour
{
    #region Variables
	public AudioSource menuMusic;
	public AudioSource gameMusic;
	public AudioSource gameOverMusic;

	public AudioSource sfxCoin;
	public AudioSource sfxJump;
	public AudioSource sfxHit;

	public bool soundMuted;

	public GameObject mutedImage;
    #endregion

    #region Unity Methods
    void Start()
    {
        if(PlayerPrefs.HasKey("SoundMuted"))
		{
			if(PlayerPrefs.GetInt("SoundMuted") == 1)
			{
				MuteAll();
				mutedImage.SetActive(true);
				soundMuted = true;
			}
		} else
		{
			PlayerPrefs.SetInt("SoundMuted", 0);
			soundMuted = false;
		}
    }

    void Update()
    {
        
    }
    #endregion

    #region Custom methods
	public void SoundOnOff()
	{
		if(soundMuted)
		{
			mutedImage.SetActive(false);
			soundMuted = false;
			UnmuteAll();
		} else
		{
			mutedImage.SetActive(true);
			soundMuted = true;
			MuteAll();
		}
	}

	public void MuteAll()
	{
		menuMusic.gameObject.SetActive(false);
		gameMusic.gameObject.SetActive(false);
		gameOverMusic.gameObject.SetActive(false);
		sfxHit.gameObject.SetActive(false);
		sfxJump.gameObject.SetActive(false);
		sfxCoin.gameObject.SetActive(false);

		PlayerPrefs.SetInt("SoundMuted", 1);
	}

	public void UnmuteAll()
	{
		menuMusic.gameObject.SetActive(true);
		gameMusic.gameObject.SetActive(true);
		gameOverMusic.gameObject.SetActive(true);
		sfxHit.gameObject.SetActive(true);
		sfxJump.gameObject.SetActive(true);
		sfxCoin.gameObject.SetActive(true);

		PlayerPrefs.SetInt("SoundMuted", 0);
	}
    #endregion
}
