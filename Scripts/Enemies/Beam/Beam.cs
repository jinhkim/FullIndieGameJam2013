using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

	public BeamTurret beamTurret;


	void Start () {
		beamTurret = transform.parent.GetComponent<BeamTurret>();		
		Physics.IgnoreCollision(beamTurret.collider, collider); // ignore turret collider

	}
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			other.renderer.material = other.GetComponent<CubeMover>().materials [1];
		}
	}
	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			other.renderer.material = other.GetComponent<CubeMover>().materials [0];
		}
	}

}
