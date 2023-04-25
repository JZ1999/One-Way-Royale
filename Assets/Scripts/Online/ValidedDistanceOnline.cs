using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidedDistanceOnline : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    [SerializeField]
    private float distance;

    public bool win = false;
    public GameObject winText;
    public GameObject loseText;

    [Range(100,1500)]
    public float objective;

    void Start()
    {
        distance = gameManager.GetDistance();
    }

    // Update is called once per frame


    public IEnumerator YouWin(GameObject player)
    {
        win = true;
        yield return new WaitForSeconds(3);
        Destroy(player.GetComponent<Rigidbody>());
        Destroy(player.GetComponent<Collider>());
        winText.SetActive(true);
        endGame();
    }


    public void YouLose()
    {
        if (!win)
        {
            loseText.SetActive(true);
            endGame();
        }
    }

	public void endGame()
	{
		StartCoroutine(gameManager.GoToMainMenu());
	}
}
