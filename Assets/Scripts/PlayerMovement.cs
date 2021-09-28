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

	public AudioManager theAM;
	public GameObject coinEffect;

	public UIBarManager BarOnlineManager;
	#endregion

	public float timeForUsePowerUpSaved;
	private float timeForUsePowerUp;
	#region Unity Methods    

	void Start()
    {
		startPosition = transform.position;
		startRotation = transform.rotation;
		timeForUsePowerUp = timeForUsePowerUpSaved;
    }

    void Update()
    {
		onGround = Physics.OverlapSphere(modelHolder.position, 0.2f, whatIsGround).Length > 0;

#if UNITY_EDITOR
		if (Input.GetMouseButton(1))
		{
			timeForUsePowerUp -= Time.deltaTime;
			if(timeForUsePowerUp <= 0)
			{
				bool canUsePower = BarOnlineManager.UsedPowerUp(1);
				timeForUsePowerUp = timeForUsePowerUpSaved;
			}
        }
        else
        {
			timeForUsePowerUp = timeForUsePowerUpSaved;
		}
#endif

		if (GameManager.canMove && onGround)
		{
			if (Input.GetMouseButtonDown(0))
			{
				rb.velocity = new Vector3(0, jumpForce, 0);
				theAM.sfxJump.Play();
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
			if (other.CompareTag("Hazards"))
			{
				GameManager.HazardHit();

				rb.constraints = RigidbodyConstraints.None;

				float rand_x = Random.Range(GameManager.gameSpeed / 2, -GameManager.gameSpeed / 2);
				rb.velocity = new Vector3(rand_x, 2.5f, -GameManager.gameSpeed / 2f);

				theAM.sfxHit.Play();
			}
			else if (other.CompareTag("Coins"))
			{
				GameManager.addCoin();
                if (BarOnlineManager)
                {
					BarOnlineManager.FillBar(0.2f);
                }
				Destroy(other.gameObject);
				theAM.sfxCoin.Play();
				Instantiate(coinEffect, gameObject.transform.position, gameObject.transform.rotation);
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
