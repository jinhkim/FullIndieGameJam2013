using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

	public Turret turret;

	public GameObject origin;
	public GameObject end;

	public Vector3 originScaleInit;
	public Vector3 endScaleInit;
	public Vector3 laserShotPosInit;
	public Vector3 laserShotScaleInit;

	public float fireTime;
	public float armTime;
	public float fireSpeed;
	public float maxScale;

	public enum ShootState {
		alert,
		arming,
		fire,
		shrink
	}

	public ShootState shootState;

	private Vector3 delta;

	// Use this for initialization
	void Start () {
		turret = transform.parent.GetComponent<Turret>();		
		Physics.IgnoreCollision(turret.collider, collider); // ignore turret collider
		Physics.IgnoreCollision(turret.GetComponentInChildren<Detector>().collider, collider); // ignore detector collider
		transform.parent = turret.transform; // attach to the turret

		originScaleInit = origin.transform.localScale;

		laserShotPosInit = transform.localPosition;
		laserShotScaleInit = transform.localScale;

		fireTime = 0;
		armTime = turret.armTime;
		fireSpeed = turret.fireSpeed;
		maxScale = turret.detectorLength/turret.halfTurretLength*originScaleInit.x + originScaleInit.x;

		
		if (turret.direction == Turret.Direction.up || turret.direction == Turret.Direction.down) {
			delta = new Vector3(0, 0, 1);
		} else {
			delta = new Vector3(1, 0, 0);
		}

		// set end point scale to max!		
		endScaleInit = end.transform.localScale;
		end.transform.localScale += (maxScale - Vector3.Dot (endScaleInit, delta)) * delta;
		endScaleInit = end.transform.localScale;

		shootState = ShootState.alert;

	}
	
	// Update is called once per frame
	void Update () {
		
		float len = Vector3.Dot (transform.parent.localScale, delta); // take the right scale

		switch (shootState) {	
		case ShootState.alert:
			transform.parent = origin.transform; // attach the shot to the origin point

			if (turret.AlarmOn) {
				shootState = ShootState.arming;
				fireTime = armTime + Time.realtimeSinceStartup;
			}
		
			break;
		case ShootState.arming:
			transform.parent = origin.transform; // attach the shot to the origin point

			if (!turret.AlarmOn) {
				shootState = ShootState.alert;
			}
			if (Time.realtimeSinceStartup >= fireTime) {
				shootState = ShootState.fire;
			}
			break;
		case ShootState.fire:			

			if (len < maxScale - Time.deltaTime * fireSpeed) {
				transform.parent.localScale += Time.deltaTime * fireSpeed * delta;
			} else {
				transform.parent.localScale += (maxScale - len) * delta;
				shootState = ShootState.shrink;
			}
			break;
		case ShootState.shrink:
			transform.parent = end.transform; // switch anchor to end

			if (len > 0 + Time.deltaTime * fireSpeed) {
				transform.parent.localScale += -Time.deltaTime * fireSpeed * delta;
			} else {
				transform.parent.localScale += -len * delta;
				Reset();
			}
			break;
		}
	}

	void Reset() {
		// deattach shot
		transform.parent = turret.transform;

		// reset scales of anchor points
		origin.transform.localScale = originScaleInit;
		end.transform.localScale = endScaleInit;

		// reset location of laser shot
		transform.localPosition = laserShotPosInit;
		transform.localScale = laserShotScaleInit;

		if (turret.AlarmOn) {
			shootState = ShootState.arming; // if still in range, fire immediately
		} else {
			shootState = ShootState.alert; // if out of range, disarm.
		}

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
