#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

//Defines state and behaviour of enemy cameras

public class EnemyCamera : MonoBehaviour {

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
	 	  coneRotateRange = 90f, deltaRot = 0f;
//	GameObject coneVision;
	

	// Use this for initialization
	void Start () {
		camPos = transform.position;
		camUp = new Vector3(0f, camPos.y, 0f);
		targetSpotted = new Vector3(0f, 0f, 0f);
		detectedPlayer = false;
		
		mRenderer = transform.FindChild("ConeVision").GetComponentInChildren<Renderer>();
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
		
		
		//detected player presence
//		else if(detectedPlayer) {
////			float currentTime = Time.time;
//			
//			backToSafeDuration -= 1f * Time.deltaTime;
//			
//			if(backToSafeDuration > 0f && intruderAlert) {
//				mRenderer.material.color = warningColor;
//			}
//			else {
//				//left the coneVision, but havent triggered red alarm
//				if(detectedPlayer && !intruderAlert){
//					detectedPlayer = false;
//					intruderAlert = false;
//					mRenderer.material.color = safeColor;
//				}
//				
//				//left the coneVision and HAVE triggered red alarm
////				else if(detectedPlayer && intruderAlert){
////					detectedPlayer = false;
////					intruderAlert = false;
////					mRenderer.material.color = safeColor;
////				}
//				backToSafeDuration = 3f;
//			}
//		}
	}
	
	//camera detects an intruder
	void OnTriggerEnter(Collider collision){
//		Debug.Log("Collision Entered!");
		if(collision.CompareTag("Player")){
//			collision.gameObject.SendMessage("changeMaterialColor",
//					Color.red, SendMessageOptions.DontRequireReceiver);
			mRenderer = transform.FindChild("ConeVision").GetComponentInChildren<Renderer>();
			mRenderer.material.color = warningColor;
			nextTime = Time.time + warningDuration;
			scaleVec = transform.FindChild("ConeVision").localScale;
			scanEnabled = false;
		}
//		mRenderer.material.color = safeColor;
//		lineOfSight.origin = camPos;
//		lineOfSight.direction.Set(0f, 0f, 1f);
	}
	
	//camera slowly focuses view
	// i.e. narrows field of view
	void OnTriggerStay(Collider collisionInfo){
		Vector3 temp = transform.FindChild("ConeVision").localScale;
		if(collisionInfo.CompareTag("Player")){
			detectedPlayer = true;
			if(nextTime > Time.time){
				//set color yellow and narrow view
	//			if(collisionInfo.bounds.IntersectRay(lineOfSight))
	//				targetSpotted = collisionInfo.bounds.center;
	//			else {
	//				targetSpotted = lineOfSight.origin + lineOfSight.direction;
	//			}
				targetSpotted = collider.bounds.center;
	//			if(!mRenderer.material.color.Equals(warningColor))
				mRenderer.material.color = warningColor;
				
	//			if(temp.magnitude >= 0.1f)
	//				transform.FindChild("ConeVision").localScale *= 0.9f;
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
//		Debug.Log("Collision Exited!!");
		
//		collisionInfo.gameObject.SendMessage("changeMaterialColor",
//		                                 Color.blue, SendMessageOptions.DontRequireReceiver);
//		Renderer renderer = transform.FindChild("ConeVision").GetComponentInChildren<Renderer>();
//		nextSafeTime = Time.time + backToSafeDuration;
		if(collisionInfo.CompareTag("Player")){
			mRenderer.material.color = warningColor;
			scanEnabled = true;
		}
	}
	
	void focusOnIntruder(){
		//shrink x-z of coneVision
	}
	
	void unfocusOnIntruder(){
		//grow x-z back to normal
	}
}
