using UnityEngine;
using System.Collections;

public class CubeMover : MonoBehaviour {

	public float speed;

	public Material[] materials;

	public bool destroyOnHit;

	float dx = 0;
	float dz = 0;




	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	/*
		if (Input.anyKey) {
			if (Input.GetKeyDown ("up")) {
				dz = speed;
			} else if (Input.GetKeyDown ("down")) {
				dz = -speed;
			} else if (Input.GetKeyUp ("up") || Input.GetKeyUp ("down") ) {
				dz = 0;
			}
			if (Input.GetKeyDown ("left")) {
				dx = -speed;
			} else if (Input.GetKeyDown ("right")) {
				dx = speed;
			} else if (Input.GetKeyUp ("left") || Input.GetKeyUp ("right") ) {
				dx = 0;
			}

		} else {
			dx = dz = 0f;
			rigidbody.velocity = Vector3.zero;
		}

		transform.Translate(new Vector3(dx, 0f, dz));
	*/
		if (destroyOnHit && renderer.sharedMaterial == materials[1]) {
			Destroy (gameObject); // DESTROY
			//gameObject.SetActive(false); // DISABLE
		}

	}

}
