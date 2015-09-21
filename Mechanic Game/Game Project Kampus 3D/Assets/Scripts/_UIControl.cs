using UnityEngine;
using System.Collections;

public class _UIControl : MonoBehaviour {

	[SerializeField] private GameObject HPBar;
	[SerializeField] private GameObject MagnetBar;

	protected PlayerAttribut atribP;
	// Use this for initialization
	void Start () 
	{
		atribP = _GControl.playerGO.GetComponent<PlayerAttribut> ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBar ();
	}

	void UpdateBar()
	{
		var tempHP = _GControl.HPPlayer / atribP.HPMax;
		var tempMagnet = _GControl.MagnetPW / atribP.MagnetPower;

		HPBar.transform.localScale = new Vector3 (tempHP, 1, 1);
		MagnetBar.transform.localScale = new Vector3 (tempMagnet, 1, 1);
	}
}
