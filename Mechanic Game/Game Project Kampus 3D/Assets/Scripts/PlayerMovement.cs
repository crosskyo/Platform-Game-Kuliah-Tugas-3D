using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	[Range(0,10)][SerializeField] private float jumpPower;
	[Range(0,20)][SerializeField] private float moveSpeed;
	[SerializeField] private float maxSpeedPlayerCanMove;

	[HideInInspector] public bool stopMovementInput;
	[HideInInspector] public bool stopJumpInput;

	// ------------------------------------------
	public static int moveState = Animator.StringToHash ("Base Layer.Move");
	public static int idleState = Animator.StringToHash ("Base Layer.Idle");
	// ------------------------------------------


	protected Animator anim;
	protected Rigidbody rigid;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		playerMoving ();

		if (Input.GetButtonUp ("Jump")) 
		{
			Debug.Log ("ASU");
			rigid.velocity = new Vector3 (rigid.velocity.x, jumpPower, rigid.velocity.z);
			anim.SetBool ("startJump", true);
		}
	}

	void playerMoving()
	{
		float h;
		if (!stopMovementInput) 
		{

			h = Input.GetAxis ("Horizontal");
			anim.SetFloat ("Supido", h);

			if(rigid.velocity.magnitude < maxSpeedPlayerCanMove)
			{
				rigid.velocity = new Vector3(moveSpeed * h,rigid.velocity.y,rigid.velocity.z);

			}
			else
			{
				rigid.velocity = new Vector3(0,rigid.velocity.y,rigid.velocity.z);
			}

			if(h < 0)
			{
				transform.rotation = (Quaternion.AngleAxis(270, Vector3.up));
			}
			else if (h > 0)
			{
				transform.rotation = (Quaternion.AngleAxis(90, Vector3.up));
			}
			else
			{
				rigid.velocity = new Vector3(0,rigid.velocity.y,rigid.velocity.z);
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == ("Ground")) 
		{
			if (anim.GetBool ("startJump"))
			{
				anim.SetBool ("startJump", false);
			}
		}
	}
}
