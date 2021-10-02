using UnityEngine;

public class CoinGeneration : MonoBehaviour
{

	#region Variables
	[SerializeField]
	private float timer_cp;
	public float timer;
	public GameObject[] coinGroups;
	public Transform topPosition;
    #endregion

    #region Unity Methods    

    void Start()
    {
		timer_cp = timer;
	}

	void Update()
    {
		if (GameManager._canMove)
		{
			timer -= Time.deltaTime;

			if (timer <= 0)
			{

				bool goTop = Random.value > 0.5f;

				int random = Random.Range(0, coinGroups.Length);
				if(goTop)
					Instantiate(coinGroups[random], topPosition);
				else
					Instantiate(coinGroups[random], transform);

				timer = Random.Range(timer_cp * 0.75f, timer_cp * 1.25f);
			}
		}
	}


	public void UpdateTimeRespawn(float fix, bool div)
    {	if(div)
			timer /= fix;
		else
			timer *= fix;
	}
    #endregion
}
