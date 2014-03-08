using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	Vector3[] shakePath;

	// Use this for initialization
	void Start () {
		shakePath = iTweenPath.GetPath ("move");

		Hashtable args = new Hashtable(){
			{"looptype", "loop"},
			{"path", shakePath},
			{"time", 5},
			{"easetype", iTween.EaseType.linear},
			{"looktarget", new Vector3(25, 10, 30)}
		};

		//iTween.MoveTo(this.gameObject, args);


	}
	
	// Update is called once per frame
	void Update () {
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");


		//iTween.MoveUpdate(this.gameObject, new Vector3(h, 0, v), 1);



		//iTween.LookUpdate (this.gameObject, new Vector3 (0, 0, 0), 0.1f);
	}

	void FixedUpdate(){
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");
		this.rigidbody.velocity = new Vector3 (h, 0, v) * 30.0f;

	}
}
