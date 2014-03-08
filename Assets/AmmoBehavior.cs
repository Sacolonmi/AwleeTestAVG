using UnityEngine;
using System.Collections;

public class AmmoBehavior : MonoBehaviour {

	public float radius = 10.0F;
	public float power = 10.0F;

	public GameObject HitEffect;
	public int liveTime = 120;
	
	int elaspedTime;

	bool isHit;

	// Use this for initialization
	void Start () {
		elaspedTime = 0;
		isHit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (elaspedTime > liveTime) {
			Destroy (this.gameObject);
		}

		elaspedTime++;
	}

	void FixedUpdate(){

	}

	void OnCollisionEnter(){
		Debug.Log("Ammo collision");
		if(HitEffect != null && !isHit)
		{
			Instantiate(HitEffect, this.transform.position, this.transform.rotation);
			isHit = true;
			Debug.Log("Create Hit effect");
		}

		Destroy (this.gameObject);
	}
}
