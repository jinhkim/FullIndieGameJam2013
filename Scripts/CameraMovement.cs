using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	float deltaX = 0, deltaZ = 0, shiftSpeed, speed = 0;

	// Use this for initialization
	void Start () {
		speed = 0.5f;
		shiftSpeed = speed * 2;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey){
			if(Input.GetKey(KeyCode.LeftShift)){
				if (Input.GetKey (KeyCode.W)) {
					deltaZ = shiftSpeed;
				}
				else if(Input.GetKey(KeyCode.S)){
					deltaZ = -shiftSpeed;
				}
				else {
					deltaZ = 0f;
				}
				
				if (Input.GetKey (KeyCode.D)) {
					deltaX = shiftSpeed;
				}
				else if(Input.GetKey(KeyCode.A)){
					deltaX = -shiftSpeed;
				}
				else{
					deltaX = 0f;
				}
			}
			else {
				if (Input.GetKey (KeyCode.W)) {
					deltaZ = speed;
				}
				else if(Input.GetKey(KeyCode.S)){
					deltaZ = -speed;
				}
				else {
					deltaZ = 0f;
				}
				
				if (Input.GetKey (KeyCode.D)) {
					deltaX = speed;
				}
				else if(Input.GetKey(KeyCode.A)){
					deltaX = -speed;
				}
				else{
					deltaX = 0f;
				}
			}
			
		}
		else {
			deltaX = deltaZ = 0f;
		}
		
		transform.Translate (new Vector3(deltaX,deltaZ,0f));
	}
}
