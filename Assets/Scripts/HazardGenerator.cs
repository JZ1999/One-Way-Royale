﻿using UnityEngine;

public class HazardGenerator : MonoBehaviour
{

	#region Variables
	public GameObject[] hazards;
	public float timer;
	[SerializeField]
	private float timer_cp;
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
				int random = Random.Range(0, hazards.Length);
				InstantiateObject(hazards[random]);

				timer = Random.Range(timer_cp * 0.75f, timer_cp * 1.25f);

				//increase difficulty
				timer /= GameManager.speedMultiplier;
			}
		}
    }
	public void InstantiateObject(GameObject obj)
    {
		Instantiate(obj, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
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
