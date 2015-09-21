using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] public done playerMode;
	[SerializeField] public MGMode magnetMode;
	[Range(0,10)][SerializeField] private float jumpPower;
	[Range(0,20)][SerializeField] private float moveSpeed;
	//[SerializeField] private float maxSpeedPlayerCanMove;

	[HideInInspector] public bool stopMovementInput;
	[HideInInspector] public bool stopJumpInput;
	[SerializeField] private GameObject magnetObject;
	[SerializeField] private GameObject objected;

	// ------------------------------------------
	public static int moveState = Animator.StringToHash ("Base Layer.BasicMove");
	public static int jumpState = Animator.StringToHash ("Base Layer.JumpStart");
	// ------------------------------------------

	public enum done
	{
		Normal,Events,Magnet,Pause
	}
	public enum MGMode
	{
		Search,Pull,Push
	}

	protected Animator anim;
	protected Rigidbody rigid;
	protected AnimatorStateInfo currentBaseState;
	public float jumpCount = 2;
	protected bool touchTheWall;
	protected float h;
	protected Vector3 inversePoint;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();
		playerMode = done.Normal;
//		radiusMagnet.SetActive (false);
		magnetMode = MGMode.Search;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentBaseState = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
		if (playerMode == done.Normal) 
		{
			playerMoving ();
			jumpPlayer (jumpPower);

		} 

		else if (playerMode == done.Magnet) 
		{
//			if(!radiusMagnet.activeInHierarchy)
//			{
//				radiusMagnet.SetActive (true);
//			}

			_GControl.MagnetPW -= 2 * Time.deltaTime;
			
			if(_GControl.MagnetPW <= 0)
			{
				playerMode = done.Normal;
//				radiusMagnet.SetActive (false);
				_GControl.MagnetPW = GetComponent<PlayerAttribut>().MagnetPower;
			}
		} 

		else if (playerMode == done.Events) 
		{

		} 

		else if (playerMode == done.Pause) 
		{

		}

		if (Input.GetButton ("Magnet")) 
		{
			if(playerMode != done.Magnet)
			{
				playerMode = done.Magnet;
				anim.SetFloat ("Supido", 0);
				rigid.velocity = new Vector3(0,0,0);
			}
		}
		else
		{
			playerMode = done.Normal;
//			radiusMagnet.SetActive (false);
			_GControl.MagnetPW = GetComponent<PlayerAttribut>().MagnetPower;
		}
	}

	void playerMoving()
	{
		if(!stopMovementInput)
		{
			h = Input.GetAxis ("Horizontal");
			if(touchTheWall)
			{
				if(inversePoint.x < 0)
				{
					if(h > 0)
					{
						h = 0;
					}
				}
				else if (inversePoint.x > 0)
				{
					if(h < 0)
					{
						h = 0;
					}
				}
			}
			anim.SetFloat ("Supido", h);
			rigid.velocity = new Vector3(moveSpeed * h, rigid.velocity.y, 0f);

			if(h > 0)
			{
				transform.rotation = (Quaternion.AngleAxis(90, Vector3.up));
				//rightPosition = true;
			}
			else if (h < 0)
			{
				transform.rotation = (Quaternion.AngleAxis(270, Vector3.up));
				//rightPosition = false;
			}
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Ground") 
		{
			if(jumpCount < 2)
			{
				jumpCount = 2;
			}
		}

		if (col.gameObject.tag == "Wall") 
		{
			touchTheWall = true;
			inversePoint = transform.InverseTransformPoint (col.transform.position);
			if(inversePoint.x < 0)
			{
				Debug.Log ("Right");
			}
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if(col.gameObject.tag == "Wall")
		{
			touchTheWall = false;
		}
	}
	void jumpPlayer(float j)
	{
		// Check State;
		if (currentBaseState.fullPathHash == moveState ||
		    currentBaseState.fullPathHash == jumpState) 
		{
			//check jumpCount
			if(jumpCount > 0)
			{
				//PRESS BUTTON to jump
				if(Input.GetButtonDown ("Jump"))
				{
					jumpCount --; 		// decrease jumpCount
					var tempVelX = rigid.velocity.x; // save Temporary Velocity X
					var tempVelZ = rigid.velocity.z; // save Temporary Velocity Z
					rigid.velocity = new Vector3 (tempVelX, j, tempVelZ); // JUMP the Player
					if(currentBaseState.fullPathHash == jumpState)
					{
						anim.SetTrigger ("StartJump");
					}
					else
					{
						anim.SetTrigger ("StartJump"); // Trigger the Jump Animation
					}
				}
			}
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Magnet") 
		{
			if (playerMode == done.Magnet) 
			{
				if(magnetMode == MGMode.Search)
				{
					var wantedX = magnetObject.transform.position.x;
					var wantedY = magnetObject.transform.position.y;
					var currentX = col.transform.position.x;
					var currentY = col.transform.position.y;
					currentX = Mathf.Lerp (currentX,wantedX, 5 * Time.deltaTime);
					currentY = Mathf.Lerp (currentY,wantedY, 5 * Time.deltaTime);

					col.transform.position = new Vector3(currentX,currentY,
					                                     col.transform.position.z);

					if(col.transform.position == magnetObject.transform.position)
					{
						magnetMode = MGMode.Pull;
						objected = col.gameObject;
					}
				}
				else if (magnetMode == MGMode.Pull)
				{
					if(Input.GetButtonDown ("Cancel"))
					{
						testMagnet ();
						magnetMode = MGMode.Search;
						playerMode = done.Normal;
					}
				}
			}
		}
	}

	void testMagnet()
	{
		objected.GetComponent<Rigidbody> ().velocity = new Vector3 (15, objected.GetComponent<Rigidbody> ().velocity.y,
		                                                          objected.GetComponent<Rigidbody> ().velocity.z);
	}
}
