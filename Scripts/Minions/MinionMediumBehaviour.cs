using UnityEngine;
using System.Collections;

public class MinionMediumBehaviour : BaseMinion {

	
	public override void initMinion(){
//		Debug.Log("MediumMinion.initMinion Called!");
		minionName = "Ugg";
		speed = 0.1f;
		type = MinionType.MEDIUM;
		isActive = false;
		pos = new Vector2 (transform.position.x, transform.position.z);
		smooth = 5f;
		minionColor = new Color(1.0f, 0.0f, 1.0f);
		gameObject.renderer.material.color = minionColor;
//		Instantiate(selectedGlow, pos, new Quaternion(0,0,0,1));
	}
	
	public override void specialAbility(){
	
	}
	
	public override void selected(){
//		selectedGlow.GetComponent<Renderer>().material.color = Color.white;
//		Instantiate(selectedGlow, new Vector3(pos.x, 0, pos.y), new Quaternion(0,0,0,1));
		gameObject.renderer.material.color = new Color(1.0f,0.5f,1.0f);
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

