using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{

	public Animator anim{ get; set; }

	private Hashtable args;

	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator> ();

		args = new Hashtable ();

		args.Add ("easeType", iTween.EaseType.easeOutCubic);

		args.Add ("time", 1f);

		args.Add ("amount", new Vector3(1, 1, 0));


	}

	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Z)) {
			args["easeType"] = iTween.EaseType.linear;
			iTween.ShakePosition (this.gameObject, args);
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			args["easeType"] = iTween.EaseType.punch;
            iTween.PunchPosition(this.gameObject, args);
		}
	}


}
