using UnityEngine;
using System.Collections;

public class SelectedGlow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision(this.collider, gameObject.collider);
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameObject.GetComponent<BaseMinion>().isActive)
			Destroy(this);
	}
}
