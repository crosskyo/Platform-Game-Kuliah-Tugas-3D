using UnityEngine;
using System.Collections;

public class _GControl : MonoBehaviour {

	public static float HPPlayer;
	public static float MagnetPW;
	public static GameObject playerGO;

	// Use this for initialization
	void Awake () 
	{
		playerGO = GameObject.Find ("Player");
		HPPlayer = playerGO.GetComponent<PlayerAttribut> ().HPMax;
		MagnetPW = playerGO.GetComponent<PlayerAttribut> ().MagnetPower;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
