using UnityEngine;
using System.Collections;

public class EnemyDrone : MonoBehaviour
{

	public bool detectedPlayer = false, intruderAlert = false, scanEnabled = true;
	
	public static float PI = 3.141592f;
	public static float DEG2RAD = PI/180f;
	public static float RAD2DEG = 1f/DEG2RAD;
	
	Vector3 camPos, camUp, targetSpotted, scaleVec;
	Renderer mRenderer;
	volatile float warningDuration = 3f, backToSafeDuration = 3f, 
	nextTime = 0f, nextSafeTime = 0f, timer;
	Color safeColor, warningColor, alertColor, prevColor;
	Ray lineOfSight;
	float coneFOV = 45f, rotAccum = 0f, degrees = 0f,
	coneRotateRange = 45f, deltaRot = 0f;

	// Use this for initialization
	void Start () {
		camPos = transform.position;
		camUp = new Vector3(0f, camPos.y, 0f);
		targetSpotted = new Vector3(0f, 0f, 0f);
		detectedPlayer = false;
		
		mRenderer = transform.FindChild("DroneConeVision").GetComponentInChildren<Renderer>();
		safeColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);		//green
		warningColor =  new Color(1.0f, 1.0f, 0.0f, 0.5f);	//yellow
		alertColor =  new Color(1.0f, 0.0f, 0.0f, 0.5f);	//red
		mRenderer.material.color = safeColor;
		deltaRot = 0.3f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(scanEnabled){
			if(rotAccum > coneRotateRange/2 || rotAccum < -coneRotateRange/2){
				deltaRot = -deltaRot;
			}
			transform.RotateAround(camPos, camUp, deltaRot);
			rotAccum += deltaRot;
			if(mRenderer.material.color != safeColor)
				mRenderer.material.color = safeColor;
		}
		
	}
	
	//camera detects an intruder
	void OnTriggerEnter(Collider collision){
		Debug.Log("Collision Entered!");
		if(collision.CompareTag("Player")){
			mRenderer = transform.FindChild("DroneConeVision").GetComponentInChildren<Renderer>();
			mRenderer.material.color = warningColor;
			nextTime = Time.time + warningDuration;
			scaleVec = transform.FindChild("DroneConeVision").localScale;
			scanEnabled = false;
		}
	}
	
	//camera slowly focuses view
	// i.e. narrows field of view
	void OnTriggerStay(Collider collisionInfo){
//		Vector3 temp = transform.FindChild("DroneConeVision").localScale;
		if(collisionInfo.CompareTag("Player")){
			detectedPlayer = true;
			if(nextTime > Time.time){

				targetSpotted = collider.bounds.center;
				mRenderer.material.color = warningColor;
				
				//			if(temp.magnitude >= 0.1f)
				//				transform.FindChild("DroneConeVision").localScale *= 0.9f;
			}
			else {
				//			if(!mRenderer.material.color.Equals(alertColor))
				mRenderer.material.color = alertColor;
				intruderAlert = true;
			}
		}
	}
	
	//no intruder in range detected
	// i.e. slowly widen field of view to normal
	void OnTriggerExit(Collider collisionInfo){
		Debug.Log("Collision Exited!!");
		if(collisionInfo.CompareTag("Player")){
			mRenderer.material.color = warningColor;
			scanEnabled = true;
		}
	}
}

