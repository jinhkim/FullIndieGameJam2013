using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {

	public Turret turret;

	public Material[] materials;

	// Use this for initialization
	void Start () {
		turret = transform.parent.GetComponent<Turret>();
		Physics.IgnoreCollision(turret.collider, collider); // ignore turret collider		
		Physics.IgnoreCollision(turret.GetComponentInChildren<LaserShot>().collider, collider); // ignore laser collider
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			Start ();
			turret.SetAlarm(true);
			renderer.material = materials[1];
		}
	}

	void OnTriggerExit (Collider other) {		
		if (other.tag == "Player") {
			turret.SetAlarm(false);
			renderer.material = materials[0];
		}
	}

}
