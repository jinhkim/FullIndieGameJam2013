using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	public enum Direction {
		up,   // + z
		right,// + x
		down, // - z
		left  // - x
	}

	public Direction direction;
	public float detectorLength;
	public float fireSpeed;
	public float armTime;

	public float halfTurretLength;

	public Material[] materials;

	public Transform detector;
	public Transform laser;
		
	public bool AlarmOn = false;

	Vector3 detectorPosition;
	Vector3 laserPosition;

	// Use this for initialization
	void Start () {
		Vector3 turretPos = transform.localPosition;
		halfTurretLength = transform.localScale.x/2;

		float halfDetectorLength = detectorLength/2;

		// Create detector beam
		detector = (Transform) Instantiate(detector);

		// Create laser beam
		laser = (Transform) Instantiate(laser);

		// Create origin and end points
		GameObject origin = new GameObject("Laser Shot Origin");
		GameObject end = new GameObject("Laser Shot End");

		switch (direction) {
			case Direction.up :
			detectorPosition = turretPos + new Vector3 (0, 0, halfDetectorLength + halfTurretLength);
			detector.RotateAround(detectorPosition, Vector3.up, 90);

			laserPosition = turretPos + new Vector3 (0, 0, halfTurretLength/2);
			laser.RotateAround(laserPosition, Vector3.up, 90);

			origin.transform.localPosition = turretPos;// + new Vector3 (0, 0, halfTurretWidth);
			end.transform.localPosition = turretPos + new Vector3 (0, 0, halfDetectorLength * 2 + halfTurretLength);
			break;

			case Direction.right :
			detectorPosition = turretPos + new Vector3 (halfDetectorLength + halfTurretLength, 0, 0);
			
			laserPosition = turretPos + new Vector3 (halfTurretLength/2, 0, 0);

			origin.transform.localPosition = turretPos;// + new Vector3 (halfTurretWidth, 0, 0);			
			end.transform.localPosition = turretPos + new Vector3 (halfDetectorLength * 2 + halfTurretLength, 0, 0);
			break;

			case Direction.down :
			detectorPosition = turretPos + new Vector3 (0, 0, - halfDetectorLength - halfTurretLength);
			detector.RotateAround(detectorPosition, Vector3.up, 90);

			laserPosition = turretPos + new Vector3 (0, 0, -halfTurretLength/2);
			laser.RotateAround(laserPosition, Vector3.up, 90);

			origin.transform.localPosition = turretPos;// + new Vector3 (0, 0, - halfTurretWidth);			
			end.transform.localPosition = turretPos + new Vector3 (0, 0, - halfDetectorLength * 2 - halfTurretLength);
			break;

			case Direction.left :
			detectorPosition = turretPos + new Vector3 (- halfDetectorLength - halfTurretLength, 0, 0);

			laserPosition = turretPos + new Vector3 (-halfTurretLength/2, 0, 0);

			origin.transform.localPosition = turretPos;// + new Vector3 (- halfTurretWidth, 0, 0);			
			end.transform.localPosition = turretPos + new Vector3 (- halfDetectorLength * 2 - halfTurretLength, 0, 0);
			break;
		}
		detector.localScale = new Vector3 (1, halfDetectorLength, 1);
		detector.localPosition = detectorPosition;
		laser.localScale = new Vector3 (2, halfTurretLength/2, 2);
		laser.localPosition = laserPosition;
		laser.GetComponent<LaserShot>().origin = origin;
		laser.GetComponent<LaserShot>().end = end;

		// set all things as a child
		detector.parent = transform; 
		laser.parent = transform;
		origin.transform.parent = transform;
		end.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (AlarmOn) {
			renderer.material = materials[1];
		} else {
			renderer.material = materials[0];
		}
	}

	public void SetAlarm (bool b) {
		AlarmOn = b;
	}

}
