using UnityEngine;

public class RandomObjectsGenerator : MonoBehaviour
{

	#region Variables
	public GameObject[] objects;
	public float timer;
	private float timer_cp;

	public Transform minPoint;
	public Transform maxPoint;
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
				int random = Random.Range(0, objects.Length);

				Vector3 genPoint = new Vector3(Random.Range(minPoint.position.x, maxPoint.position.x), transform.position.y, transform.position.z);
				Instantiate(objects[random], genPoint, Quaternion.Euler(0, Random.Range(0, 360), 0));

				timer = Random.Range(timer_cp * 0.75f, timer_cp * 1.25f);
			
				timer /= GameManager.speedMultiplier;

			}
		}
	}

    #endregion
}
