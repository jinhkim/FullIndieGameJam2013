﻿using UnityEngine;
using System.Collections;

public class fadeFartCloud : MonoBehaviour {

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		
		Destroy (gameObject, 1.5f);
	}
}
