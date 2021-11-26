using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidedDistanceOnline : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    [SerializeField]
    private float distance;

    public GameObject winText;
    public GameObject loseText;

    [Range(100,1500)]
    public float objective;

    void Start()
    {
        distance = gameManager.GetDistance();
    }

    // Update is called once per frame
    void Update()
    {
        distance = gameManager.GetDistance();
        if (distance >= objective)
        {
            Time.timeScale = 0f;
            winText.SetActive(true);
			endGame();
		}
    }

    public void YouLose()
    {
        loseText.SetActive(true);
		endGame();
    }

	public void endGame()
	{
		StartCoroutine(gameManager.GoToMainMenu());
	}
}
