using UnityEngine;
using System.Collections;

public class MinionFastBehaviour : BaseMinion {

	public override void initMinion(){
//		Debug.Log("FastMinion.initMinion Called!");
		minionName = "Slinky";
		speed = 0.15f;
		type = MinionType.FAST;
		isActive = false;
		pos = new Vector2 (transform.position.x, transform.position.z);
		smooth = 10f;
		minionColor = new Color(0.5f, 0.5f, 1.0f);
		gameObject.renderer.material.color = minionColor;
//		Instantiate(selectedGlow, pos, new Quaternion(0,0,0,1));
	}
	
	public override void specialAbility(){
	
	}
	
	public override void selected(){
//		selectedGlow.GetComponent<Renderer>().material.color = Color.white;
//		Instantiate(selectedGlow, new Vector3(pos.x, 0, pos.y), new Quaternion(0,0,0,1));
		gameObject.renderer.material.color = new Color(0.7f, 0.7f, 1.0f);
	}
	
	public override void unselected(){
		gameObject.renderer.material.color = minionColor;
	}
	
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}
}
