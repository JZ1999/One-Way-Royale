using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	#region Variables
	public Camera camera;
	public Rigidbody rb;
	public float jumpForce;
	public Transform modelHolder;
	public LayerMask whatIsGround;
	private bool onGround;
	public Animator anim;

	private Vector3 startPosition;
	private Quaternion startRotation;

	private bool wantJump = false;

	public float invincibleTime;
	private float invincibleTimer;

	[Range(1, 10)]
	public float mudDebuffSpeed;

	//Generador Scripts
	public CoinGeneration coinGeneration;
	public HazardGenerator hazardGenerator;

	//Api Script
	public ApiService api = new ApiService();

	public AudioManager theAM;
	public GameObject coinEffect;

	public UIBarManager BarOnlineManager;
	#endregion

	public float timeForUsePowerUpSaved;
	public float timeForUsePowerUp;
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
				string Power = BarOnlineManager.UsedPowerUp();
				GameManager.ApplyDebuff(Power);
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
			if (wantJump)
			{
				wantJump = false;
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
#if !UNITY_EDITOR
				
				api.RegisterScore();
#endif

				rb.constraints = RigidbodyConstraints.None;

				float rand_x = Random.Range(GameManager.gameSpeed / 2, -GameManager.gameSpeed / 2);
				rb.velocity = new Vector3(rand_x, 2.5f, -GameManager.gameSpeed / 2f);

				theAM.sfxHit.Play();
			}
			else if (other.CompareTag("Coins"))
			{
				GameManager.AddCoin();
				if (BarOnlineManager)
				{
					BarOnlineManager.FillBar(0.2f);
				}
				Destroy(other.gameObject);
				theAM.sfxCoin.Play();
				Instantiate(coinEffect, gameObject.transform.position, gameObject.transform.rotation);
			}
			else if (other.CompareTag("Mud"))
			{
				GameManager.gameSpeed /= mudDebuffSpeed;
				GameManager._gameSpeed /= mudDebuffSpeed;
				GameManager.gameSpeedStore /= mudDebuffSpeed;
				coinGeneration.UpdateTimeRespawn(mudDebuffSpeed, false);
				hazardGenerator.UpdateTimeRespawn(mudDebuffSpeed, false);
			}
		}
			
	}

	private void OnTriggerExit(Collider other)
	{
		if (invincibleTimer <= 0)
		{
			if (other.CompareTag("Mud"))
			{
				GameManager.gameSpeed *= mudDebuffSpeed;
				GameManager._gameSpeed *= mudDebuffSpeed;
				GameManager.gameSpeedStore *= mudDebuffSpeed;
				coinGeneration.UpdateTimeRespawn(mudDebuffSpeed, true);
				hazardGenerator.UpdateTimeRespawn(mudDebuffSpeed, true);
			}
		}
	}

	public void RaiseDebuff()
    {
		string Power = BarOnlineManager.UsedPowerUp();
		GameManager.ApplyDebuff(Power);
	}

    public void ResetPlayer()
	{
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		transform.rotation = startRotation;
		transform.position = startPosition;

		invincibleTimer = invincibleTime;
	}
	public void setJump()
    {
			wantJump = true;
    }
	#endregion
}
