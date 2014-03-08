using UnityEngine;
using System.Collections;

public class ParticlePaperBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		Vector3 rot = camera.transform.eulerAngles;

		//iTween.LookUpdate (this.gameObject, camera.transform.position, 0.5f);
		iTween.RotateUpdate(this.gameObject, camera.transform.eulerAngles, 0.5f);
	}
}
