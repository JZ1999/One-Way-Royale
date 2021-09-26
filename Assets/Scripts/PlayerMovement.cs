using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	#region Variables
	public Rigidbody rb;
	public float jumpForce;
	public Transform modelHolder;
	public LayerMask whatIsGround;
	private bool onGround;
	public Animator anim;

	private Vector3 startPosition;
	private Quaternion startRotation;

	public float invincibleTime;
	private float invincibleTimer;
	#endregion

	#region Unity Methods    

	void Start()
    {
		startPosition = transform.position;
		startRotation = transform.rotation;
    }

    void Update()
    {
		onGround = Physics.OverlapSphere(modelHolder.position, 0.2f, whatIsGround).Length > 0;

		if (GameManager.canMove && onGround)
		{
			if (Input.GetMouseButtonDown(0))
			{
				rb.velocity = new Vector3(0, jumpForce, 0);
			}
		}

		//control invincibility
		if(invincibleTimer > 0)
		{
			invincibleTimer -= Time.deltaTime;
		}

		anim.SetBool("walking", GameManager.canMove);
		anim.SetBool("jumping", !onGround);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(invincibleTimer <= 0)
		{
			if (other.tag.Equals("Hazards"))
			{
				GameManager.HazardHit();

				rb.constraints = RigidbodyConstraints.None;

				float rand_x = Random.Range(GameManager.gameSpeed / 2, -GameManager.gameSpeed / 2);
				rb.velocity = new Vector3(rand_x, 2.5f, -GameManager.gameSpeed / 2f);
			}
			else if (other.tag.Equals("Coins"))
			{
				GameManager.addCoin();
				Destroy(other.gameObject);
			}
		}
			
	}

	public void ResetPlayer()
	{
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		transform.rotation = startRotation;
		transform.position = startPosition;

		invincibleTimer = invincibleTime;
	}

	#endregion
}
