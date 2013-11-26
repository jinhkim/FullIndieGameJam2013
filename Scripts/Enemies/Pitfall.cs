using UnityEngine;
using System.Collections;

public class Pitfall : MonoBehaviour {

	public float touchTime;
	public float armTime;
	public bool playerTouching = false;
	
	public enum PitfallState {
		alert,
		arming,
		fire
	}
	
	public PitfallState pitfallState;

	public Material[] materials;
		
	// Use this for initialization
	void Start () {
		touchTime = 0;
		pitfallState = PitfallState.alert;		
	}
	
	// Update is called once per frame
	void Update () {

		switch (pitfallState) {	
		case PitfallState.alert:
			renderer.material = materials[0];
			if (playerTouching) {
				pitfallState = PitfallState.arming;
				touchTime = armTime + Time.realtimeSinceStartup;
			}
			
			break;
		case PitfallState.arming:
			renderer.material = materials[1];
			
			if (!playerTouching) {
				pitfallState = PitfallState.alert;
			}
			if (Time.realtimeSinceStartup >= touchTime) {
				pitfallState = PitfallState.fire;
			}
			break;
		case PitfallState.fire:			
			renderer.material = materials[2];
			
			GameObject player = GameObject.FindWithTag("Player");

			if (playerTouching) {
				if (player != null) {
					player.renderer.material = GameObject.FindWithTag("Player").GetComponent<CubeMover>().materials [1];
				}
			} else {
				if (player != null) {
					player.renderer.material = GameObject.FindWithTag("Player").GetComponent<CubeMover>().materials [0];
				}
				pitfallState = PitfallState.alert;
			}
			break;
		}
	}
		
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			playerTouching = true;
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			playerTouching = false;
		}
	}


}
