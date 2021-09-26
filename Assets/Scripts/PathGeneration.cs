using UnityEngine;

public class PathGeneration : MonoBehaviour
{

	#region Variables
	public GameObject[] pathPieces;
	public Transform threshold;
    #endregion

    #region Unity Methods    

    void Start()
    {
        
    }

    void Update()
    {
        if(transform.position.z < threshold.position.z)
		{
			GameObject pathPiece = pathPieces[Random.Range(0, pathPieces.Length)];
			Instantiate(pathPiece, transform.position, transform.rotation);
			transform.position += Vector3.forward * 3.2f;
		}
		
    }

    #endregion
}
