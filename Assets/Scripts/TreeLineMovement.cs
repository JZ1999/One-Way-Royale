using UnityEngine;

public class TreeLineMovement : MonoBehaviour
{

	#region Variables
	public Transform disappearPoint;
	public Collider collider;
    #endregion

    #region Unity Methods    

    void Start()
    {
        
    }

    void Update()
    {
		if (GameManager._canMove)
		{
			transform.position -= Vector3.forward * (GameManager._gameSpeed * Time.deltaTime);
		}

		if(transform.position.z < disappearPoint.position.z)
		{
			transform.position += new Vector3(0f, 0f, collider.bounds.size.z * 2);
		}
	}

    #endregion
}
