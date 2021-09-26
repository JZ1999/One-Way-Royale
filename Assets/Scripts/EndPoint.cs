using UnityEngine;

public class EndPoint : MonoBehaviour
{

	#region Variables
	static public float zPoint;
    #endregion

    #region Unity Methods    

    void Start()
    {
		zPoint = transform.position.z;
    }

    void Update()
    {
		zPoint = transform.position.z;
	}

    #endregion
}
