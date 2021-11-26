using System.Collections;
using UnityEngine;

public class CoinGeneration : MonoBehaviour
{

	#region Variables
	[SerializeField]
	private float timer_cp;
	public float timer;
	private bool shouldApplyDebuff = false;
	public GameObject[] coinGroups;
	public Transform topPosition;
	[SerializeField]
	private RuntimeAnimatorController debuffAnimation;
	[SerializeField]
	private RuntimeAnimatorController normalAnimation;
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

				spawnCoins(random, goTop, shouldApplyDebuff);
				
				timer = Random.Range(timer_cp * 0.75f, timer_cp * 1.25f);
			}
		}
	}
	#endregion

	private void spawnCoins(int random, bool goTop, bool withDebuff)
	{
		GameObject coins;
		if (goTop)
		{
			coins = Instantiate(coinGroups[random], topPosition);

		}
		else
		{
			coins = Instantiate(coinGroups[random], transform);
		}

		if (withDebuff)
			foreach (Transform obj in coins.transform)
			{
				obj.GetComponentInChildren<Animator>().runtimeAnimatorController = debuffAnimation;
			}

	}

	public void UpdateTimeRespawn(float fix, bool div)
	{
		if (div)
			timer /= fix;
		else
			timer *= fix;
	}

	public IEnumerator setShouldApplyDebuff(bool newValue)
	{
		shouldApplyDebuff = newValue;

		yield return new WaitForSeconds(5);

		shouldApplyDebuff = false;
	}
}
