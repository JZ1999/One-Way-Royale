using UnityEngine;

public class ObjectMovement : MonoBehaviour
{

	#region Variables
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

		if (transform.position.z < EndPoint.zPoint)
		{
			Destroy(gameObject);
		}
	}

    #endregion
}
