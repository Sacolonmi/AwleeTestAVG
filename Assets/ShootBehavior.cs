using UnityEngine;
using System.Collections;

public class ShootBehavior : MonoBehaviour {

	public Rigidbody ammo;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && ammo != null) {
			Shoot ();
		}

	}

	void Shoot(){
		Transform trans = this.gameObject.transform;
		Vector3 firePos = trans.position + trans.forward * 2.0f;

		Rigidbody newAmmo = Instantiate (ammo, firePos, trans.rotation) as Rigidbody;

		newAmmo.velocity = trans.forward * 25.0f;
	}
}
