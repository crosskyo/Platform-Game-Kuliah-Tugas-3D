using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	[Range(0,10)][SerializeField] private float howSmoothCameraMove;
	[Range(0,10)][SerializeField] private float distance;
	[SerializeField] private GameObject targetFollow;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (transform.position.x < targetFollow.transform.position.x + distance) 
		{
			transform.position = new Vector3((Mathf.Lerp(transform.position.x, targetFollow.transform.position.x,
			                                             howSmoothCameraMove * Time.deltaTime)),transform.position.y,transform.position.z);
		}
	}
}
