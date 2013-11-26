using UnityEngine;
using System.Collections;

public class BeamSwitch : MonoBehaviour {

	public bool playerTouching = false;

	public Material[] materials;
		
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
		
	void OnTriggerEnter () {
		playerTouching = true;
		renderer.material = materials[1];
	}
	
	void OnTriggerExit () {
		playerTouching = false;
		renderer.material = materials[0];
	}


}
