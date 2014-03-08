using UnityEngine;
using System.Collections;

public class ExplosionBehavior : MonoBehaviour {

	public float radius = 1.0f;
	public float power = 0.1f;

	int elapsedTime;

	// Use this for initialization
	void Start () {

		Vector3 explosionPos = this.gameObject.transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

		Debug.Log("Exp start, colliders.Length: " + colliders.Length);
		foreach (Collider hit in colliders) {
			if (hit && hit.rigidbody){
				hit.rigidbody.AddExplosionForce(power, explosionPos, radius, 1.0f);
			}
			
		}


		elapsedTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (++elapsedTime > 120)
			Destroy (gameObject);
	}
}
