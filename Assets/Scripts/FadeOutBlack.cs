using UnityEngine;
using UnityEngine.UI;
// Creado por JZ1999

public class FadeOutBlack : MonoBehaviour
{
	#region Variables
	public Image blackScreen;

	public float waitToFade;

	public float fadeSpeed;
    #endregion

    #region Unity Methods
    void Start()
    {
        
    }

    void Update()
    {
        if(waitToFade > 0)
		{
			waitToFade -= Time.deltaTime;
		} else
		{
			blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

			if(blackScreen.color.a <= 0)
			{
				gameObject.SetActive(false);
			}
		}
    }
    #endregion

    #region Custom methods

    #endregion
}
