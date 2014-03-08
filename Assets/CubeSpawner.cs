using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour {
	public Rigidbody body;

	List<Rigidbody> bodyList;

	// Use this for initialization
	void Start () {
		bodyList = new List<Rigidbody> ();

		SpawnCube ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			SpawnCube ();
		}
	}

	void SpawnCube(){
		foreach(Rigidbody body in bodyList)
		{
			Destroy(body.gameObject);
		}

		bodyList.Clear ();

		for(int x = -5; x < 5; ++x){
			for(int y = -5; y < 5; ++y){
				bodyList.Add(Instantiate(body, new Vector3(x, y, 30), Quaternion.identity) as Rigidbody);
			}
		}

	}
}
