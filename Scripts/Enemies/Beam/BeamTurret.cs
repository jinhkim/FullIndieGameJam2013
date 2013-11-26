using UnityEngine;
using System.Collections;

public class BeamTurret : MonoBehaviour {
	public enum Direction {
		up,   // + z
		right,// + x
		down, // - z
		left  // - x
	}

	public Direction direction;
	public float beamLength;

	public float halfTurretLength;

	public Material[] materials;

	public Transform beam;
		
	public bool beamOn;
	private bool canToggle = true;

	Vector3 beamPosition;

	// Use this for initialization
	void Start () {
		Vector3 turretPos = transform.localPosition;
		halfTurretLength = transform.localScale.x/2;

		float halfBeamLength = beamLength/2;

		// Create detector beam
		beam = (Transform) Instantiate(beam);

		switch (direction) {
			case Direction.up :
			beamPosition = turretPos + new Vector3 (0, 0, halfBeamLength + halfTurretLength);
			beam.RotateAround(beamPosition, Vector3.up, 90);
			break;

			case Direction.right :
			beamPosition = turretPos + new Vector3 (halfBeamLength + halfTurretLength, 0, 0);
			break;

			case Direction.down :
			beamPosition = turretPos + new Vector3 (0, 0, - halfBeamLength - halfTurretLength);
			beam.RotateAround(beamPosition, Vector3.up, 90);
			break;

			case Direction.left :
			beamPosition = turretPos + new Vector3 (- halfBeamLength - halfTurretLength, 0, 0);
			break;
		}
		beam.localScale = new Vector3 (2, halfBeamLength, 2);
		beam.localPosition = beamPosition;

		// set all things as a child
		beam.parent = transform; 
		SetState ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponentInChildren<BeamSwitch>().playerTouching) {
			if (canToggle) {
				canToggle = false;
				Toggle();
			}
		} else {
			canToggle = true;
		}
	}

	void Toggle () {
		if (beamOn) {
			beamOn = false;
			SetState();
		} else {
			beamOn = true;			
			SetState();
		}
	}

	void SetState () {
		if (beamOn) {
			beam.gameObject.SetActive (true); // enable beam
			renderer.material = materials [1];
		} else {	
			beam.gameObject.SetActive (false); // disable beam
			renderer.material = materials [0];
		}
	}

}
